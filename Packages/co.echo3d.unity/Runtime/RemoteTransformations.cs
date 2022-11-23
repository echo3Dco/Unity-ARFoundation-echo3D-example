/**************************************************************************
* Copyright (C) echoAR, Inc. (dba "echo3D") 2018-2021.                    *
* echoAR, Inc. proprietary and confidential.                              *
*                                                                         *
* Use subject to the terms of the Terms of Service available at           *
* https://www.echo3D.co/terms, or another agreement                       *
* between echoAR, Inc. and you, your company or other organization.       *
***************************************************************************/
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using System.Globalization;
using UnityEngine.Video;

public class RemoteTransformations : MonoBehaviour
{
    [HideInInspector]
    public Entry entry;

    /// <summary>
    /// EXAMPLE BEHAVIOUR
    /// Queries the database and transforms the object based on the result.
    /// </summary>

    // Initial transformation
    private Vector3 initialWorldSpacePosition;
    private Quaternion initialWorldSpaceRotation;
    private Vector3 initialScale;

    // Use this for initialization
    void Start()
    {
        try
        {
            // Set initial transfomation
            initialWorldSpacePosition = (this.gameObject.transform.parent) ? this.gameObject.transform.parent.transform.position : this.gameObject.transform.position;
            initialWorldSpaceRotation = this.gameObject.transform.rotation;
            initialScale = this.gameObject.transform.localScale;
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }

    }

    public void Update()
    {
        string value = "";

        //Note: Disabled this, the object is assigned to the parent when instantiated.
        //      and the initial position is already stored on Start()
        // Repeatedly updating position relative to parent here is problematic
        //// Attach to parent
        //if (this.gameObject.transform.parent)
        //{
        //    initialWorldSpacePosition = this.gameObject.transform.parent.transform.position;
        //}

        // Handle translation
        Vector3 positionOffset = Vector3.zero;
        bool applyPositionOffset = false;
        if (entry.getAdditionalData().TryGetValue("x", out value))
        {
            applyPositionOffset = true;
            positionOffset.x = float.Parse(value, CultureInfo.InvariantCulture);
        }
        if (entry.getAdditionalData().TryGetValue("y", out value))
        {
            applyPositionOffset = true;
            positionOffset.y = float.Parse(value, CultureInfo.InvariantCulture);
        }
        if (entry.getAdditionalData().TryGetValue("z", out value))
        {
            applyPositionOffset = true;
            positionOffset.z = float.Parse(value, CultureInfo.InvariantCulture);
        }
        if (applyPositionOffset)
        {
            this.gameObject.transform.localPosition = positionOffset;
        }

        // Handle spinning
        float speed = 150;
        if (entry.getAdditionalData().TryGetValue("speed", out value))
        {
            speed *= float.Parse(value, CultureInfo.InvariantCulture);
        }
        float offset = 0;
        if (entry.getAdditionalData().TryGetValue("direction", out value))
        {
            if (value.Equals("right"))
                offset += Time.time % 360 * speed;
            else
                offset -= Time.time % 360 * speed;
        }

        // Handle rotation
        Quaternion targetQuaternion = initialWorldSpaceRotation;
        float x = 0, y = 0, z = 0;
        bool applyRotation = false;
        if (entry.getAdditionalData().TryGetValue("xAngle", out value))
        {
            applyRotation = true;
            x = float.Parse(value, CultureInfo.InvariantCulture);
        }
        if (entry.getAdditionalData().TryGetValue("yAngle", out value))
        {
            applyRotation = true;
            y = float.Parse(value, CultureInfo.InvariantCulture);
        }
        if (entry.getAdditionalData().TryGetValue("zAngle", out value))
        {
            applyRotation = true;
            z = float.Parse(value, CultureInfo.InvariantCulture);
        }
        if (applyRotation || offset != 0)
        {
            this.gameObject.transform.rotation = Quaternion.Euler(x, y + offset, z);
        }

        // Handle Height and Width
        float height = (entry.getAdditionalData() != null && entry.getAdditionalData().TryGetValue("height", out value)) ? float.Parse(value) * 0.01f : 1;
        float width = (entry.getAdditionalData() != null && entry.getAdditionalData().TryGetValue("width", out value)) ? float.Parse(value) * 0.01f : 1;
        if (height != 1 || width != 1)
        {
            this.gameObject.transform.localScale = initialScale = new Vector3(width, height, height);
        }

        // Handle Scale
        float scaleFactor = 1f;
        if (entry.getAdditionalData().TryGetValue("scale", out value))
        {
            scaleFactor = float.Parse(value, CultureInfo.InvariantCulture);
            this.gameObject.transform.localScale = initialScale * scaleFactor;
        }

        // Mute
        bool mute = false;
        if (entry.getAdditionalData() != null && entry.getAdditionalData().TryGetValue("mute", out value))
        {
            mute = value.Equals("true") ? true : false;
            VideoPlayer videoPlayer = this.GetComponent<VideoPlayer>();
            for (ushort i = 0; videoPlayer != null && i < videoPlayer.controlledAudioTrackCount; ++i)
                videoPlayer.SetDirectAudioMute(i, mute);
        }

    }
}