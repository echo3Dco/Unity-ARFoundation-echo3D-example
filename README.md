# Unity-ARFoundation-echo3D-example
Example Unity project with AR Foundation and echo3D integrated.

## Setup
* Built with Unity 2020.3.25.  _(Note: The echo3D Unity SDK is supported in 2020.3.25 and higher.)_
* Register for FREE at [echo3D](https://console.echo3D.co/#/auth/register).
* [Add the Unity SDK](https://medium.com/r/?url=https%3A%2F%2Fdocs.echo3d.co%2Funity%2Finstallation). Troubleshoot [here](https://docs.echo3d.com/unity/troubleshooting#im-getting-a-newtonsoft.json.dll-error-in-unity).
* Clone this repo. 

## Steps
* Open the _SimpleAR_ scene in Unity.
* [Set the API key](https://docs.echo3d.co/quickstart/access-the-console) for the 'echo3DTest' object from within the Inspector. <br>
![APIKeyandEntryId](https://user-images.githubusercontent.com/99516371/195749269-f7a43477-b67a-49e8-a212-6abdb9c948fd.png)<br>
![NEWAPIKeyandEntryID](https://user-images.githubusercontent.com/99516371/205407613-b746840f-8e8a-4ec8-b056-a680395dfab4.png)<br>
* [Type your Secret Key](https://docs.echo3d.co/web-console/deliver-pages/security-page#secret-key) as the value for the parameter secKey in the file Packages/co.echo3D.unity/Runtime/Echo3DHologram.cs. _(Note: Secret Key only matters if you have the Security Key enabled). You can turn it off in the Security options in your echo3D console._
![NEWSecKey2](https://user-images.githubusercontent.com/99516371/195749308-b2349a3b-7e43-4d3c-8f09-fbfa9d3cb0be.png)<br>
* (Recommended) To move, resize or edit the assets live in your Scene view, check the boxes for “Editor Preview” and “Ignore Model Transforms”. To enable this, click Echo3D > Load Editor Holograms in your Unity toolbar. <br>
![EditorPreviewAndIgnoreModelTransforms](https://user-images.githubusercontent.com/99516371/195749348-dc0b06ad-efa6-4dbd-962f-0119b5c33ea0.png)<br>
![LoadHolograms](https://user-images.githubusercontent.com/99516371/195749354-b2295183-f877-444a-af22-ed87ffb17705.png) <br>

## Run
In Unity, go into Play mode. 

## Learn More
Refer to our [documentation](https://docs.echo3D.co/unity/) to learn more about how to use Unity and echo3D.

## Troubleshooting
* Visit our troubleshooting guide [here](https://docs.echo3d.co/unity/troubleshooting#im-getting-a-newtonsoft.json.dll-error-in-unity).
* Troubleshoot [AR Foundation](https://docs.echo3D.co/unity/adding-ar-capabilities#4-build-and-run-the-ar-application).

## Support
Feel free to reach out at [support@echo3D.co](mailto:support@echo3D.co) or join our [support channel on Slack](https://go.echo3D.co/join). 



