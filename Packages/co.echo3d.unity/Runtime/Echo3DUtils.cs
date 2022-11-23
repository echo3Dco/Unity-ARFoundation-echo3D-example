using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Echo3DUtils : MonoBehaviour
{
    public IEnumerator UploadModel(string apiKey, string secKey, byte[] objFile, string objFileName, byte[] mtlFile, string mtlFileName)
    {
        return UploadModel(apiKey, secKey, objFile, objFileName, mtlFile, mtlFileName, (List<byte[]>)null, (List<string>)null);
    }

    public IEnumerator UploadModel(string apiKey, string secKey, byte[] objFile, string objFileName, byte[] mtlFile, string mtlFileName, byte[] pngFile, string pngFileName)
    {
        return UploadModel(apiKey, secKey, objFile, objFileName, mtlFile, mtlFileName, new List<byte[]> { pngFile }, new List<string> { pngFileName });
    }

    public IEnumerator UploadModel(string apiKey, string secKey, byte[] objFile, string objFileName, byte[] mtlFile, string mtlFileName, List<byte[]> pngFiles, List<string> pngFileNames)
    {
        // Create form
        WWWForm form = new WWWForm();
        // Set form data
        form.AddField("key", apiKey);      // API Key
        form.AddField("secKey", secKey);    // Secret Key
        form.AddField("src", Echo3DGlobals.src);         // Query source
        form.AddField("target_type", 2);    // Target type is SURFACE
        form.AddField("hologram_type", 2);  // Hologram type is MODEL
        // Set form files
        form.AddBinaryData("file_model", objFile, objFileName, null);   // .obj file
        form.AddBinaryData("file_model", mtlFile, mtlFileName, null);   // .mtl file
        if (pngFiles != null && pngFileNames != null)
            for (int i = 0; i < pngFileNames.Count; i++)                    // Texture files
                form.AddBinaryData("file_model", pngFiles[i], pngFileNames[i], null);
        // Send request
        UnityWebRequest www = UnityWebRequest.Post(Echo3DGlobals.endpointURL + "/upload", form);
        yield return www.SendWebRequest();
        // Check for errors
        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Upload complete!");
        }
    }


}
