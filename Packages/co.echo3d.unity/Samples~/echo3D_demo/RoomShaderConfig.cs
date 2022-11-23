using UnityEngine;

public class RoomShaderConfig : MonoBehaviour
{

    //Quick hack to adjust a shader setting on an imported model
    //A future SDK update will introduce events as a much more reliable pattern to perform
    // such modifications
    private const string roomModelName = "white-round-exhibition-gallery.glb";
    private bool shaderAdjusted = false;

    // Update is called once per frame
    void Update()
    {
        if (!shaderAdjusted)
        {
            GameObject modelGO = GameObject.Find(roomModelName);
            if (modelGO != null)
            {
                if (modelGO.transform.childCount > 0)
                {
                    Transform rootNode = modelGO.transform.GetChild(0);
                    if (rootNode != null)
                    {
                        foreach (Transform child in rootNode)
                        {
                            MeshRenderer meshRenderer = child.gameObject.GetComponent<MeshRenderer>();
                            if (meshRenderer != null)
                            {
                                child.gameObject.GetComponent<MeshRenderer>().material.SetFloat("_CullMode", 0);
                                shaderAdjusted = true;
                                Debug.Log("Shader adjusted.");
                            }

                        }
                    }
                }

            }
        }
    }
}
