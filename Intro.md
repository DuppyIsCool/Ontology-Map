# Introduction to Virtual Medical Ontology
This guide will help you get started with development for the Virtual Medical Ontology. 
This software provides a convenient process of displaying and manipulating large data structures in a virtual reality environment. Virtual Medical Ontology reads from a file and creates a graphical structure using the data. Virtual Medical Ontology grants solutions to many technical problems limited to a conceived screen real estate. A Virtual Reality headset is required in order to use the software. 

# GETTING STARTED

FIRST THINGS FIRST MAKE SURE YOU  SET THE SCENCE TO THE PROPER SCENCE IN Assets/Scenes UpdatedScene

Almost every file used is location in Assets/Scripts and all UI and prefabs are located in the Assets/Prefab folder. CSV files are located in Assets/Resources

This project starts with the tree game object in the scene. This file contains a TreeObject which then calls the OntologyLoader file. In the Tree object you can edit the file name 
and set which CSV file for the project to load. There are 2 main types of files in this project. Theres the code representation of Nodes, Edges, and the Tree, and finally the 3d object which are NodeObject, EdgeObject, TreeObject. 

Controls - Left controller manages all movement. Right controller hold B button to activate the ray cast controllers so you can get access to the node menu.

## Saving
The save function is located at `$"Assets/Scripts/Graph/TreeObj.cs`. Here is a list of tasks that might be necessary to test in order to ensure the validity of the function:  

 1. Have a clean `.CSV` file to ensure that each lines are properly read by the CSV reader. There is a copy of `CVDO.csv` called `CVDO1.csv` that was quickly cleaned up that you can use to test your code. You can also try cleaning and using `CIDO.csv`.  
   ![csvdiff.png](https://mail.google.com/mail/u/0?ui=2&ik=bf0bfe8373&attid=0.0.1&permmsgid=msg-a:r-4355650480607951434&th=17ae49e41ba79e7b&view=fimg&sz=s0-l75-ft&attbid=ANGjdJ-S7UUAGwTlcbgKWyEq37e5xC295R85JrEH1V9e6N3hMb4-RIlDoFAsIBNsPkySyNN2DVDLpysBv9Qy1znrGeQHKt9XmxL2DowyVrPTWnQSxJwdfXm2IpXDDC4&disp=emb&realattid=ii_krl3lyl70)
 2. We managed to get most of the columns from the original `.csv` file, however, not all columns are being printed in the new save file. The process of printing it in terms of code is quite lengthy and probably inefficient, this might need a revision. Here's an example:

```cs
if (dictionary.ContainsKey(nodeUN))
{
  if (dictionary[nodeUN][1] != null)
  {
      syn = dictionary[nodeUN][1];
  }
  else { syn = string.Empty; }
  if (dictionary[nodeUN][2] != null)
  {
      def = dictionary[nodeUN][2];
  }
  else { def = string.Empty; }
```

 3. Test your code by comparing the original `.csv` file to the new save file using a [CSV Comparison Tool](https://extendsclass.com/csv-diff.html#result)
 
 4. It might be better to have the dictionary populated at the start of the game session. You can add this on `$"Assets/Scripts/Ontology/OntologyLoader.cs` and call the function under `private void Start()`

## Tips

 - Navigating through an already existing project might be daunting for others, however, you can use Microsoft Visual Studio and use their Unity debugger to add breakpoints in your code to see if it's hitting the code you're expecting it to hit.
 - If you don't have any VR headset, you can use the XR Device Simulator on Unity to navigate through the game. `Note: This might cause some Scene errors when you merge all your branches to test your progress.`


[Get started!](features/index.md)