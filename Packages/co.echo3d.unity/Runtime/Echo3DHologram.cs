/**************************************************************************
* Copyright (C) echoAR, Inc. (dba "echo3D") 2018-2021.                    *
* echoAR, Inc. proprietary and confidential.                              *
*                                                                         *
* Use subject to the terms of the Terms of Service available at 	      *
* https://www.echo3D.co/terms, or another agreement      	              *
* between echoAR, Inc. and you, your company or other organization.       *
***************************************************************************/
using UnityEngine;
using System.Collections;

public class Echo3DHologram : MonoBehaviour
{
    private string secKey = "<YOUR_SECURITY_KEY>";
    // Your echo3D API key
    [Tooltip("Required - echo3D project API key. Example: 'some-word-1234'")]
    public string apiKey = API_KEY_PLACEHOLDER;

    [Tooltip("Entry IDs separated by comma without spaces. (Optional)")]
    public string entries = "";
    [Tooltip("Filter by by tags separated by comma without spaces. (Optional)")]
    public string tags = "";

    [Tooltip("EXPERIMENTAL - Holograms with this flag enabled will load in the editor via the menu Echo3D -> Load In Editor")]
    public bool editorPreview = false;

    [Tooltip("Optional - If 'Query Only' is checked, manually specify an echo API query here to retrieve and store response data in hologramData script object. Overrides other script config (apiKey, secKey etc) ")]
    public string queryURL = "";

    [Tooltip("When enabled, this script will fetch and store specified hologram data but spawn no holograms.")]
    public bool queryOnly = false;

    [Tooltip("Disable remote transformations - existing metadata will be applied when the hologram is instantiated but runtime metadata changes will not be applied.")]
    public bool disableRemoteTransformations = false;

    [Tooltip("Check this if holograms are instantiating with undesired position, rotation or scale. Instantiated holograms will ignore transform data baked into the model file. This gameobject's transform will define the position, rotation and scale of instantiated Holograms. Note: Transforming hologram metadata (scale,x, y, etc) will still be applied.")]
    public bool ignoreModelTransforms = false;
    // Specified hologram(s) data will be stored in this object
    [HideInInspector]
    public Database queryData;

    private const string API_KEY_PLACEHOLDER = "hidden-silence-7637";

    void Start()
    {
        StartCoroutine(LoadFromEcho3D());
    }

    public void EditorLoad()
    {
        StartCoroutine(LoadFromEcho3D());
    }
    IEnumerator LoadFromEcho3D()
    {
        if (Echo3DService.instance == null)
        {
            Debug.LogError("echo3D Error: Instance of Echo3DService not found! Make sure the Echo3DService prefab is in your scene and active.");
            yield break;
        }
        bool userDefinedQuery = !string.IsNullOrWhiteSpace(queryURL) && queryOnly;

        if (!userDefinedQuery && (string.IsNullOrWhiteSpace(apiKey) || apiKey.Equals(API_KEY_PLACEHOLDER)))
        {
            Debug.LogError("echo3D Error: Loading hologram requires defined 'apiKey'");
            yield break;
        }
        queryURL = userDefinedQuery ? queryURL : Echo3DService.GetQueryURL(apiKey, secKey, entries, tags);

        yield return StartCoroutine(Echo3DService.instance.QueryDatabase(queryURL, (responseDb) =>
        {
            queryData = responseDb;
        }));
        if (queryData == null || queryOnly)
        {
            if (queryData == null) { Debug.LogError("echo3D Error: Failed to receive query data"); }

            yield break;
        }
        //Load entries specified
        foreach (Entry entry in queryData.getEntries())
        {
            Echo3DService.instance.DownloadAndInstantiate(entry, queryURL, this.gameObject, ignoreModelTransforms, disableRemoteTransformations);
        }


#if UNITY_WEBGL
        Debug.Log("WebGL - web socket client will not initialize");
#else
        if (!disableRemoteTransformations)
        // Start Websocket client
        {
            StartCoroutine(Echo3DService.instance.WebsocketClient(this));
        }
#endif
    }



}