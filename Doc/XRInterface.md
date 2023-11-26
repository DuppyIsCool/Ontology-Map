# XR Interface with Mirror Networking

## Overview

This Unity project demonstrates an XR interface with Mirror networking for multiplayer functionality. The project includes scripts for object grabbing in VR (`ObjectGrabUI.cs`), resetting items within a specific range (`ResetItems.cs`), and synchronizing player colors across the network in an additive scene environment (`SetPlayerColor.cs`).

## ObjectGrabUI.cs

### Description

The `ObjectGrabUI.cs` script is designed to be used in a VR environment, where users can interact with objects using XR interactions. It showcases the following features:

- Object grabbing using the XRGrabInteractable component.
- Displaying information about the grabbed object on a UI panel (`InfoPanel`).
- Utilizing the OntologyNode script for storing information about the object.

### Usage

1. Attach the script to an XR grabbable object.
2. Assign the `XRGrabInteractable` component to the `grabInteractable` variable.
3. Assign the `InfoPanel` GameObject to the `infoPanel` variable.
4. Attach the `OntologyNode` script to the object and assign it to the `myNode` variable.

## ResetItems.cs

### Description

The `ResetItems.cs` script is designed to reset the position and velocity of a GameObject if it moves beyond a specified distance from a spawn point. Key features include:

- Utilizing a designated spawn position (assigned to the `spawnPosition` variable).
- Resetting the object's position and rotation when it moves beyond a specified distance (`maxDistance`).
- Optionally resetting the velocity if the object has a Rigidbody component.

### Usage

1. Attach the script to the GameObject you want to reset.
2. Assign the spawn position to the `spawnPosition` variable in the Inspector.
3. Set the maximum allowed distance from the spawn point using the `maxDistance` variable.

## SetPlayerColor.cs

### Description

The `SetPlayerColor.cs` script is part of a multiplayer environment using the Mirror networking framework. It synchronizes the color of player body parts across the network. Key features include:

- Randomly generating a color on the server using `OnStartServer`.
- Synchronizing the color across the network using the `[SyncVar]` attribute.
- Updating the appearance of player body parts based on the synchronized color.

### Usage

1. Attach the script to the player GameObject.
2. Assign head, left hand, and right hand GameObjects to the respective public variables.
3. Ensure that the head and hand GameObjects have Renderer components with materials that support color changes.
