/**************************************************************************
* Copyright (C) echoAR, Inc. (dba "echo3D") 2018-2021.                    *
* echoAR, Inc. proprietary and confidential.                              *
*                                                                         *
* Use subject to the terms of the Terms of Service available at 	      *
* https://www.echo3D.co/terms, or another agreement      	              *
* between echoAR, Inc. and you, your company or other organization.       *
***************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Echo3DStream : MonoBehaviour
{

    // Your echo3D API key
    public string APIKey = "<YOUR_API_KEY>";
    // The file resources
    public string sequence = "<SEQUENCE>";
    public string file = "";
    // Stream parameters
    public bool loop = false;
    public bool play = true;
    public bool hideFirstFrame = false;
    public int numberOfFrames = 15;
    public float framerate = 10f;
    public float waitToLoadTime = 5f;
    public int bufferSize = 5;

    // Set server URL
    private string serverURL;
    // Set models download URLs
    private List<string> urls = new List<string>();
    // Set frames
    private Dictionary<string, GameObject> frames = new Dictionary<string, GameObject>();
    private int currentIndex = 0;
    private GameObject currentFrame = null;
    // Set timing parameters
    private float timer = 0.0f;
    private float startTime;
    private float loadTimer;
    // Set buffer parameters
    private volatile int currentIndexBuffer = 0;
    // Set game object
    private GameObject stream3DObject;
    // Callback
    public void callback(bool noError, GameObject frame)
    {
        Debug.Log("Callback function: Buffer reached its size with frame " + frame.name);
        // Do something, e.g., start playing:
        // play = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        // The echo3D server details
        serverURL = "https://api.echo3D.co/volumetric?key=" + APIKey + "&sequence=" + sequence + "&file=" + file;

        // Run the query subroutine followed by assets download subroutine
        try
        {
            StartCoroutine(QueryServer(serverURL));
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
    }

    IEnumerator QueryServer(string serverURL)
    {
        // Create a new request
        UnityWebRequest www = UnityWebRequest.Get(serverURL);

        // Yield for the request
        yield return www.SendWebRequest();

        // Wait for the request to finish
        while (!www.isDone)
        {
            yield return null;
        }

        string responseString = "not found";
        // Check for errors
        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Parse response
            responseString = www.downloadHandler.text;
        }

        // Cleanup
        www.disposeDownloadHandlerOnDispose = true;
        www.disposeUploadHandlerOnDispose = true;
        www.Dispose();
        www = null;

        // Check for valid response
        if (!responseString.ToLower().Contains("not found"))
        {
            // Download Assets
            StartCoroutine(StreamFrames(responseString));
        }
    }

    IEnumerator StreamFrames(string url)
    {
        // Define 3D stream object
        stream3DObject = new GameObject();
        stream3DObject.name = "echo3D_3D_Stream";
        stream3DObject.transform.parent = this.transform;

        // Define frames
        int start = 80;
        int amount = 703;
        for (int n = 0; n < numberOfFrames; ++n)
        {
            urls.Add(url + ((n % amount) + start) + "_echo3D.glb");
        }
        // Set start time
        startTime = Time.time;

        // Download frames based on buffer size
        for (int i = 0; i < bufferSize; ++i)
            StartCoroutine(StreamFrame());

        yield return null;
    }

    IEnumerator StreamFrame()
    {

        // Set URL
        string url = urls[currentIndexBuffer];
        // Set frame
        // Debug.Log("Loading frame " + currentIndexBuffer);
        GameObject frame = new GameObject();
        frame.name = "" + currentIndexBuffer;
        frame.transform.parent = stream3DObject.transform;
        // Download .glb
        var glb = frame.AddComponent<GLTFast.GltfAsset>();
        glb.url = url;
        // Set parameters
        //glb.frame = frame;
        // Update frame on load
        //glb.onLoadComplete += updateFrame;
        //if (currentIndexBuffer == bufferSize) {
        //    glb.onLoadComplete += callback;
        //}
        // Increment current index of the buffer
        ++currentIndexBuffer;
        // Return
        yield return null;
    }

    public void updateFrame(bool noError, GameObject frame)
    {
        StartCoroutine(updateFrameCoroutine(noError, frame));
    }

    IEnumerator updateFrameCoroutine(bool noError, GameObject frame)
    {
        // Update frame
        // Debug.Log("Load complete for " + frame.name + " in " + (Time.time - startTime) + " seconds");
        frames.Add(frame.name, frame);
        if (frame.name == "0")
        {
            currentFrame = frame;
        }
        if (frame.name != "0" || hideFirstFrame)
        {
            // Hide frame
            var renderers = frame.GetComponentsInChildren<MeshRenderer>();
            foreach (var renderer in renderers)
            {
                renderer.enabled = false;
            }
        }
        if (currentIndexBuffer < numberOfFrames)
        {
            StartCoroutine(StreamFrame());
        }
        // Return
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        // Wait to load
        loadTimer += Time.deltaTime;
        if (loadTimer <= waitToLoadTime)
            return;
        // Check frames were loaded
        if (play && currentFrame != null && ((loop && frames.Count >= numberOfFrames) || !loop))
        {
            // Update frame rate
            timer += Time.deltaTime;
            // Advance frame in a specific rate
            if (timer > (1 / framerate))
            {
                // Advance frame
                currentIndex = (loop) ? (currentIndex + 1) % frames.Count : currentIndex + 1;
                // Check if done
                if (loop || currentIndex < numberOfFrames)
                {
                    // Wait for next frame to load
                    if (!frames.ContainsKey("" + currentIndex))
                    {
                        currentIndex = (currentIndex - 1 <= 0) ? 0 : currentIndex - 1;
                    }
                    else
                    {
                        // Hide current frame
                        var renderers = currentFrame.GetComponentsInChildren<MeshRenderer>();
                        foreach (var renderer in renderers)
                        {
                            renderer.enabled = false;
                        }
                        // Save as old frame
                        GameObject oldFrame = currentFrame;
                        // Update current frame
                        currentFrame = frames["" + currentIndex];
                        // Print index and frame
                        // Debug.Log(currentIndex + " " + currentFrame.name);
                        // Show current frame
                        renderers = currentFrame.GetComponentsInChildren<MeshRenderer>();
                        foreach (MeshRenderer renderer in renderers)
                        {
                            renderer.enabled = true;
                        }
                        // Delete old frame
                        if (!loop)
                        {
                            frames.Remove(oldFrame.name);
                            Object.DestroyImmediate(oldFrame);
                            oldFrame = null;
                        }
                    }
                    // Remove the recorded time
                    timer = 0;
                }
            }
        }
    }

    void OnDestroy()
    {
        Object.DestroyImmediate(stream3DObject);
    }
}