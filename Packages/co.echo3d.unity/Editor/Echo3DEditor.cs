using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GLTFast;
using UnityEditor;

public class Echo3DEditor : MonoBehaviour
{
    void OnDestroy()
    {
        EditorApplication.playModeStateChanged -= HandlePlayStateChange;
    }
    static void HandlePlayStateChange(PlayModeStateChange state)
    {
        Debug.Log("State change: " + state);
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            GetAndClearEditorHolograms();
        }
    }
    [MenuItem("Echo3D/Load Editor Holograms")]
    public static void LoadEditorHolograms()
    {
        EditorApplication.playModeStateChanged += HandlePlayStateChange;
        Echo3DService[] service = GameObject.FindObjectsOfType<Echo3DService>();
        service[0].Awake();
        Echo3DHologram[] sceneHolograms = GameObject.FindObjectsOfType<Echo3DHologram>();
        ClearEditorHolograms(sceneHolograms);
        foreach (Echo3DHologram holoObj in sceneHolograms)
        {
            if (holoObj.editorPreview)
            {
                holoObj.EditorLoad();
            }
        }

    }

    [MenuItem("Echo3D/Clear Editor Holograms")]
    static void GetAndClearEditorHolograms()
    {
        ClearEditorHolograms(GameObject.FindObjectsOfType<Echo3DHologram>());
    }


    static void ClearEditorHolograms(Echo3DHologram[] sceneHolograms)
    {
        GameObject gltfSpawner = GameObject.Find("glTF-StableFramerate");
        if (gltfSpawner != null)
        {
            DestroyImmediate(gltfSpawner);
        }
        if (sceneHolograms == null) { return; }
        foreach (Echo3DHologram holoObj in sceneHolograms)
        {
            if (holoObj.gameObject.GetComponent<WClient>() != null)
            {
                DestroyImmediate(holoObj.gameObject.GetComponent<WClient>());
            }

            if (holoObj.gameObject.transform.childCount > 0)
            {
                List<GameObject> childrenToDestroy = new List<GameObject>();
                foreach (Transform child in holoObj.gameObject.transform)
                {
                    if (child.gameObject.GetComponent<CustomBehaviour>() != null)
                    {
                        childrenToDestroy.Add(child.gameObject);
                    }
                }
                foreach (GameObject child in childrenToDestroy)
                {
                    DestroyImmediate(child);
                }
            }

        }
    }
}
