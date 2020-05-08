/**************************************************************************
* Copyright (C) echoAR, Inc. 2018-2020.                                   *
* echoAR, Inc. proprietary and confidential.                              *
*                                                                         *
* Use subject to the terms of the Terms of Service available at           *
* https://www.echoar.xyz/terms, or another agreement                      *
* between echoAR, Inc. and you, your company or other organization.       *
***************************************************************************/
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using System.Globalization;

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
            initialWorldSpaceRotation = this.gameObject.transform.rotation * Quaternion.AngleAxis(90, transform.worldToLocalMatrix * Camera.main.transform.right) * Quaternion.AngleAxis(-180, transform.worldToLocalMatrix * Camera.main.transform.forward);
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
        float positionFactor = 1f;

        // Attach to parent
        if (this.gameObject.transform.parent) {
            initialWorldSpacePosition =  this.gameObject.transform.parent.transform.position;
            positionFactor = this.gameObject.transform.parent.transform.localScale.magnitude;
        }

        // Handle Interaction
        if (entry.getAdditionalData().TryGetValue("direction", out value))
        {
            if (value.Equals("right"))
                this.gameObject.transform.rotation *= Quaternion.AngleAxis(-Time.deltaTime * 150, transform.worldToLocalMatrix *
                Camera.main.transform.up);
            //this.transform.Rotate(this.transform.worldToLocalMatrix * Vector3.up, -Time.deltaTime * 150);
            else
                this.gameObject.transform.rotation *= Quaternion.AngleAxis(Time.deltaTime * 150, transform.worldToLocalMatrix *
                Camera.main.transform.up);
            //this.transform.Rotate(this.transform.worldToLocalMatrix * Vector3.up, Time.deltaTime * 150);
        }

        // Handle translation
        Vector3 positionOffest = Vector3.zero;
        if (entry.getAdditionalData().TryGetValue("x", out value))
        {
            positionOffest.x = float.Parse(value, CultureInfo.InvariantCulture);
        }
        if (entry.getAdditionalData().TryGetValue("y", out value))
        {
            positionOffest.y = float.Parse(value, CultureInfo.InvariantCulture);
        }
        if (entry.getAdditionalData().TryGetValue("z", out value))
        {
            positionOffest.z = float.Parse(value, CultureInfo.InvariantCulture);
        }
        this.gameObject.transform.position = initialWorldSpacePosition + positionOffest * positionFactor;

        // Handle rotation
        Quaternion targetQuaternion = initialWorldSpaceRotation;
        if (entry.getAdditionalData().TryGetValue("xAngle", out value))
        {
            targetQuaternion *= Quaternion.AngleAxis(float.Parse(value, CultureInfo.InvariantCulture), transform.worldToLocalMatrix *
                Camera.main.transform.right);
            this.gameObject.transform.rotation = targetQuaternion;

        }
        if (entry.getAdditionalData().TryGetValue("yAngle", out value))
        {
            targetQuaternion *= Quaternion.AngleAxis(float.Parse(value, CultureInfo.InvariantCulture), transform.worldToLocalMatrix *
                Camera.main.transform.up);
            this.gameObject.transform.rotation = targetQuaternion;
        }
        if (entry.getAdditionalData().TryGetValue("zAngle", out value))
        {
            targetQuaternion *= Quaternion.AngleAxis(float.Parse(value, CultureInfo.InvariantCulture), transform.worldToLocalMatrix *
                Camera.main.transform.forward);
            this.gameObject.transform.rotation = targetQuaternion;
        }

        // Handle Scale
        float scaleFactor = 1f;
        if (entry.getAdditionalData().TryGetValue("scale", out value))
        {
            scaleFactor = float.Parse(value, CultureInfo.InvariantCulture);
        }
        this.gameObject.transform.localScale = initialScale * scaleFactor;

    }
}
