# Unity Ontology Visualization

## Overview

This Unity project contains scripts for visualizing ontology data in a 3D space. The primary scripts are `MapLoader.cs` and `OntologyNode.cs`. These scripts work together to read ontology data from a CSV file and create a 3D representation of the ontology in the Unity scene.

## Files

- `MapLoader.cs`: Responsible for loading ontology data from a CSV file and laying out the ontology tree in 3D space.
- `OntologyNode.cs`: Manages the visual representation of individual ontology nodes, including text labels and connecting lines.

## Getting Started

1. **CSV Data**: Prepare your ontology data in CSV format. Ensure that the CSV file includes columns for "Class ID," "Preferred Label," "Parents," and "definition."

2. **Unity Setup**:
    - Import the provided scripts into your Unity project.
    - Attach the `MapLoader` script to a GameObject in your scene.

3. **Configure MapLoader**:
    - In the Unity Editor, select the GameObject with the `MapLoader` script.
    - Assign the `vroom.csv` file to the `ontologyCsv` field in the inspector.
    - Configure other parameters such as the ontology node prefab, origin, scale, and layout options.

4. **Run the Scene**: Press the Play button in the Unity Editor to run the scene.

## Understanding the Scripts

### `MapLoader.cs`

- **LoadOntologyFromCSV()**: Reads ontology data from the specified CSV file, creates `Node` objects, and establishes parent-child relationships.

- **LayoutTree()**: Recursively lays out the ontology tree in 3D space. It instantiates GameObjects for each node and positions them based on parent-child relationships.

- **GetColorForLevel()**: Assigns colors to nodes based on their level in the ontology tree.

### `OntologyNode.cs`

- **InitializeNode()**: Initializes an ontology node by setting the label text, parent transform, and updating the line renderer for visualization.

- **UpdateLine()**: Updates the line renderer positions to match the current node's position and its parent transform.

## Example Usage

1. **CSV File**: Use the provided `vroom.csv` file as an example or replace it with your own ontology data in CSV format.

2. **Unity Editor**:
    - Attach the `MapLoader` script to a GameObject.
    - Assign the CSV file to the `ontologyCsv` field.
    - Configure other parameters as needed.

3. **Run the Scene**: Press Play in the Unity Editor to visualize the ontology in 3D space.

## Dependencies

- Unity Engine (developed and tested on Unity version X.X.X)
- TextMeshPro package (ensure it is installed in your project)

## Notes

- This project assumes a specific CSV format. Ensure your ontology data follows the required structure.
- Customize prefab, layout parameters, and colors as needed for your specific visualization requirements.


