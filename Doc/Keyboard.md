# Keyboard Functionality
Welcome to the basic document that covers how the keyboard works and where everything is located. 

## GETTING STARTED
There are a few things to know about the keyboard, currently in the project there are two Unity prefabs associated with the Keyboard. One Keyboard is called the Physical Keyboard Rotated and the other is the normal Physical Keyboard. With the current state of the project, all instances of the keyboard use the Physical Keyboard Rotated Prefab. 

### Keyboard Controls
There are a few hotkeys that are used to manipulate the position of the keyboard for the user. WASD will move the keyboard Forward, Back, Left, or Right. The Up and Down Arrow Keys will adjust the height of the entire XR Rig and associated UI. Q and E are used to rotate the Keyboard Counterclockwise and Clockwise, respectively. 

### How does it work? 
The keyboard that appears in the Unity scene is linked to the UI manager object in the scene. When you click on the UI manager, in the Inspector you will see a list of objects. The Physical Keyboard Prefab R is the current keyboard. If you want to change which keyboard prefab is used, simply change the "Prefab R" and "Parent R". This will be the prefab that the UI manager creates when an instance of a keyboard is needed. 
NOTE: The Physical Keyboard is the original Keyboard and it is not called anywhere. This version is meant to be a clean slate in the case the R version became unusable. 

## Associated Scripts
All of the scripts that interact with the Keyboard can be found in the Assets/Scripts/UI/Keyboard folder. The scripts that are most likely to be edited are PhysicalKeyboard.cs and PhsyicalKeyboardKey.cs.
PhysicalKeyboard.cs contains the bulk of the functionality of the keyboard. There you will find the functions that determine the behavior of each Keypress. Functions in the Keyboard scripts are commented explaining their functionality. 

## TODO: Voice Activated Typing
For some users the style of typing with the VR keyboard proves to be a bit difficult. Other users may find ease in typing using their voice instead. 
