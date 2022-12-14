# Unity-SDK
Thanks for downloading the echo3D Unity SDK package! This SDK is compatible with Unity 2020 LTS and up.

## Legacy SDK
If you are using an older version of Unity (2019 or earlier), please use the echo3D SDK version provided via the unity asset store [here](https://assetstore.unity.com/packages/tools/network/echo3d-sdk-189301).

## Installing the echo3D SDK

### Embedded Vs Local Packages
A local unity package exists somewhere on your computer outside of your project folder. An embedded package exists within your project `Packages` folder. We recommend installing our package as an embedded package for each project unless you are confident you need a local package installation.

### Installing as an embedded package - RECOMMENDED
1. Within your unity project folder, create a `Packages` folder if one does not exist already.
2. Download the echo3D SDK from echo3D [here](https://console.echo3d.co/#/pages/contentmanager). Unzip the file into your `Packages` directory so that it looks like `YourProject/Packages/co.echo3d.unity/`.
3. Open your project in Unity (or switch focus to Unity if it is already open). Unity will automatically import the package and its dependencies.

### Installing as a local package
1. Download the echo3D SDK from echo3D [here](https://console.echo3d.co/#/pages/contentmanager) (login required). Unzip the file to any directory on your computer.
2. Open your project in Unity and access the package manager via the top menu Window -> Package Manager
3. Click the '+' at the top left of the package manager window and select "Add package from disk"
4. In the location where you unzipped the SDK, select the 'package.json' file
5. The package manager will install the package locally. The project will reference the package files where they are located (the package files will not exist within your Project directory).

## echo3D Demo Scene Sample
A demo scene is included in the package which demostrates loading models, images and video from a public project on the echo3D platform. To add and run this demo scene in your project:
1. Open the package manager and select the echo3D package from the left list
2. Open the "Samples" list on the right and click "Import". The demo scene will be added to your project assets under the `Samples` folder.
3. Open the `Echo3DDemo` scene and click run. The scene will appear empty in the scene viewer - everything is loaded at runtime!

## Setting up your own scene
1. Go to `Packages/echo3D Unity SDK/Prefabs` in the editor project directory.
2. Drag the `Echo3DService` prefab anywhere into your scene. It should remain active in any scene where echo3D content will be loaded. This prefab is set to `DoNotDestroyOnLoad` by default to persist through scene loading.
3. You can spawn holograms by either dragging the `Echo3DHologram` prefab into your scene (a plain old gameobject with the `Echo3DHologram.cs` script already attached) or by adding the `Echo3DHologram.cs` script to any existing gameobject. 

## Configuring Echo3DHologram objects
All required configuration to load holograms can be found in the inspector view for `Echo3DHologram.cs` gameobjects:

1. Set the `Api Key`
2. Within `Echo3DHologram.cs`, set the `secKey` value if applicable (All echo3D accounts have a security key enabled by default, adjust/view the setting [here](https://console.echo3d.co/#/pages/security))
3. By default, the `Echo3DHologram.cs` script will query the `apiKey` and load all holograms that belong to that project. To load one or multiple specific holograms from a project, input the hologram entryID found in the echo3D web console into `Entries`. Separate multiple ids with `,`.
4. You can further filter project holograms by defining `Tags` to load only holograms with specified tags in their metadata.  

### Default Behavior
Any gameobjects with properly configured `Echo3DHologram.cs` scripts will automatically load their holograms when their gameobject is created via the script `Start()`. If [transforming metadata](https://docs.echo3d.co/unity/transforming-content) exists, the modifications will be applied to the spawned object. 

Data that corresponds to the specified holograms (ID, metadata, etc) is stored in `queryData`. This data is fetched prior to fetching the blob data (the actual model, file or image) for the hologram itself.

#### Scripts Added By Default
The `CustomBehavior.cs` script is automatically attached to all instantiated holograms and can be customized with your own logic. Removing this script from holograms can cause unwanted behavior and is not recommended.

The `RemoteTransformations.cs` script is automatically attached to all instantiated holograms and is responsible for listening to and applying changes to holograms at runtime. For example, setting a scale value in the echo3D web console will be automatically reflected in your unity scene hologram. See the following section for more details.

### Inspector Settings
The script provides easy ways to preform common adjustments to hologram loading behavior via the Unity Inspector. 

#### EXPERIMENTAL - Editor Preview
The echo3D SDK allows for holograms to be streamed at design time (ie, before pressing Play) to aid with composing your scene. 

1. Check the `Editor Preview` box in the inspector to designate the object for editor preview
2. From the top menu, go to Echo3D -> Load In Editor. All `Echo3DHologram.cs` objects within your scene with `Editor Preview` checked will load into the scene. 
3. To clear holograms, go to Echo3D -> Clear Holograms. 

Note: Preloaded holograms will clear and re-load on play. More refined controls for behavior is planned for a future update. 

#### Ignore Model Transforms
By default, transform data baked into the model (position, rotation, scale) is applied on load. Enable this setting to ignore baked model transform data, setting all instantiated holograms to default transform values (Vector3.zero for local position, Quaternion.identity for rotation and Vector3.One for scale). The transform values found on the root gameobject (the transform of the gameobject that has the `Echo3DHologram.cs` script component attached) will be used instead.

#### Disable Remote Transformations
By default, a `RemoteTransformations.cs` script is attached to all instantiated holograms. This script is used to apply changes made to model metadata via the echo3D console while your Unity application is running. When enabled, holograms will not respond to live metadata changes at runtime.

#### Advanced - Query Only
Within the 'Advanced' menu on the `Echo3DHologram.cs` script, enable `Query Only` to prevent hologram loading. The script will the query specified hologram data and store it in `queryData` but spawn no holograms.

#### Advanced - Manual API Query
Within the 'Advanced' menu, you can define your own to query via `Query URL` to query the echo3D API directly with whatever request you desire. The script will query the url on Start() and store the data in the `queryData` object within the script, stopping further activity (no holograms will be created). `Query Only` must be enabled. For example, the query URL `https://api.echo3D.co/query?key=<API_KEY>` would result in a list of all project holograms and their data in `queryData` 

### Debug
To enable debug logging for the SDK, define the scripting variable ECHO_DEBUG in your Unity project settings. 

### Known Issues
- WebGL builds will fetch and apply hologram metadata on app start but will not respond to live console metadata changes (Scale, offset, rotation, etc) at runtime.
- Certain GIFs may fail to load / display


