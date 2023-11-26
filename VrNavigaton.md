# Unity VR Navigation Scripts

## VRCanvasHUD.cs

### Overview
The `VRCanvasHUD` script manages the UI for hosting, joining, and managing a virtual reality (VR) networked experience. It utilizes the Mirror networking library for network functionality.

### Key Components
- **Network Discovery:** Discovers available servers and automatically starts hosting if none are found.
- **UI Elements:** Buttons for hosting, starting as a server or client, stopping, and auto-starting. Input fields for the network address and player name.
- **Automatic Start:** Optionally, the script can be configured to always auto-start the game.
- **On-Screen Keyboard:** Supports on-screen keyboard input for network address and player name.
- **Canvas Setup:** Manages UI visibility based on the network status (host, client, or none).

### Usage
1. Attach to the XR origin in the Unity canvas under the ontology scene.
2. Configure UI elements and network settings as needed.
3. Use the provided buttons to start as host, server, client, stop, or auto-start.

## VRPlayerRig.cs

### Overview
The `VRPlayerRig` script is associated with a VR player rig, providing synchronization of head and hand transforms. It includes simple movement controls for testing in non-VR environments.

### Key Components
- **Transforms:** References to the right hand, left hand, head, and UI canvas positions.
- **Network Player Script:** References a script (`VRNetworkPlayerScript`) for networked VR players.
- **Update Method:** Synchronizes head and hand transforms with the associated network player script.
- **FixedUpdate Method:** Handles basic movement for testing in PC/Editor environments.

### Usage
1. Attach to the XR player rig in the Unity scene.
2. Assign the appropriate transforms and network player script references.
3. Use the script for testing movement in non-VR environments.

## VRStaticVariables.cs

### Overview
The `VRStaticVariables` script provides a simple class for storing static variables related to player and game information that needs to persist between scenes.

### Key Components
- **Static Variable:** Stores the player name as a static string variable.

### Usage
1. Attach to any GameObject for global access.
2. Access and modify the static variables from other scripts to store and retrieve player and game information.
