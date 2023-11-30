using UnityEngine;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration.Attributes;
using System.Linq;
using Mirror;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using CsvHelper.Configuration;

public class MapLoader : NetworkBehaviour
{
    [SerializeField] private TextAsset ontologyCsv;
    [SerializeField] private GameObject ontologyNodePrefab;
    [SerializeField] private Vector3 origin;  // The origin for the root nodes.
    [SerializeField] private float scale;  // The origin for the root nodes.
    [SerializeField] private float layerHeight;
    [SerializeField] private float radius;
    [SerializeField] private float radiusPower;
    [SerializeField] private bool invertTree;
    public Dictionary<Node, GameObject> nodeGameObjects = new Dictionary<Node, GameObject>();


    // This seems to not sync well over the network due to the large size
    // Perhaps break every node into it's own Network RPC message to contstruct starting from the root and send a message per child for every node?
    // Mirror should batch the messages and KCP transport protocol garuntees it will be in-order processing.
    // Next group should do this as their goal.
    // For how to sync custom data types, see the custom serialization class at the end of this script.
    [SyncVar]
    private Node RootNode;

    //The server should read the file
    public override void OnStartServer()
    {
        //Loads the CSV data into the RootNode object
        LoadOntologyFromCSV();
    }

    //Clients layout based on the root node
    public override void OnStartClient()
    {
        LayoutTree(RootNode, origin, 0, layerHeight, radius, radiusPower, invertTree);
        base.OnStartClient();
    }

    private void LoadOntologyFromCSV()
    {
        if (!isServer)
            return;

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            PrepareHeaderForMatch = args => args.Header.ToLower(),
        };

        using (var reader = new StringReader(ontologyCsv.text))
        using (var csv = new CsvReader(reader, config))
        {
            var ontologyData = csv.GetRecords<OntologyNodeData>().ToList();

            Dictionary<string, Node> nodes = new Dictionary<string, Node>();

            foreach (var dataRow in ontologyData)
            {
                Node node = new Node(dataRow.ClassID, dataRow.PreferredLabel, dataRow.definition);
                nodes[dataRow.ClassID] = node;
            }

            foreach (var dataRow in ontologyData)
            {
                Node node = nodes[dataRow.ClassID];
                string parentId = dataRow.Parents;

                if (!string.IsNullOrEmpty(parentId) && nodes.ContainsKey(parentId))
                {
                    Node parentNode = nodes[parentId];
                    node.Parent = parentNode;
                    parentNode.Children.Add(node);
                }
                else if (string.IsNullOrEmpty(parentId))
                {
                    if (RootNode != null)
                    {
                        RootNode.Children.Add(node);
                        node.Parent = RootNode;
                    }
                    else
                    {
                        RootNode = node;
                    }
                }
            }
        }
    }

    private void LayoutTree(Node node, Vector3 position, float level, float layerHeight, float radius, float radiusPower, bool invertTree, float parentAngle = 0f)
    {
        // Prevent duplicate node instantiation
        GameObject newNodeObject;
        if (!nodeGameObjects.ContainsKey(node))
        {
            newNodeObject = Instantiate(ontologyNodePrefab, position, Quaternion.identity, this.transform);
            newNodeObject.transform.localScale = Vector3.one * scale;

            OntologyNode newNodeComponent = newNodeObject.GetComponent<OntologyNode>();
            newNodeComponent.InitializeNode(node, node.Parent != null ? nodeGameObjects[node.Parent].transform : null, GetColorForLevel((int)level));
            nodeGameObjects[node] = newNodeObject;
        }
        else
        {
            Debug.LogWarning($"Node {node.Label} already instantiated, skipping duplicate creation.");
            newNodeObject = nodeGameObjects[node];
        }

        float newZ = invertTree ? (origin.z + layerHeight * level) : (origin.z - layerHeight * level);

        // Normalize the parentAngle
        float normalizedParentAngle = (parentAngle % 360 + 360) % 360;
        float arcStart = normalizedParentAngle - 45f;
        float arcEnd = normalizedParentAngle + 45f;
        float angleStep;
        float currentAngle;

        // Determine if the node is the root or a direct child of the root (level 0 or 1)
        if (level <= 1)
        {
            angleStep = 360f / Mathf.Max(1, node.Children.Count);
            currentAngle = 0;
        }
        else
        {
            angleStep = (arcEnd - arcStart) / Mathf.Max(1, node.Children.Count - 1);
            currentAngle = arcStart;
        }

        // Determine the scale for children based on the number of children using a switch statement
        float childScale;
        int numberOfChildren = node.Children.Count;
        switch (numberOfChildren)
        {
            case > 30:
                childScale = 0.40f * scale;
                break;
            case > 15:
                childScale = 0.6f * scale; // Adjust the scale factor as needed
                break;
            case > 8:
                childScale = 0.8f * scale; // Adjust the scale factor as needed
                break;
            default:
                childScale = scale; // Standard scale
                break;
        }

        // Cap the maximum growth of the radius
        float maxRadius = 2.0f; // Set to the maximum radius you want to allow
        float scaledRadius = Mathf.Min(radius * Mathf.Pow(node.Children.Count, radiusPower), maxRadius);

        for (int i = 0; i < node.Children.Count; i++)
        {
            var child = node.Children[i];
            
            // Skip the instantiation if this child has already been created
            if (nodeGameObjects.ContainsKey(child))
            {
                Debug.LogWarning($"Duplicate instantiation attempt for child node {child.Label} of parent {node.Label}.");
                continue;
            }

            Vector3 direction = Quaternion.Euler(0, 0, currentAngle) * Vector3.right;
            Vector3 childPosition = position + direction * scaledRadius;
            childPosition.z = newZ;

            GameObject childNodeObject = Instantiate(ontologyNodePrefab, childPosition, Quaternion.identity, this.transform);
            childNodeObject.transform.localScale = Vector3.one * childScale;

            OntologyNode childNodeComponent = childNodeObject.GetComponent<OntologyNode>();
            childNodeComponent.InitializeNode(child, newNodeObject.transform, GetColorForLevel((int)(level + 1)));

            nodeGameObjects[child] = childNodeObject;

            // Recursive call to layout the child's subtree
            LayoutTree(child, childPosition, level + 1, layerHeight, scaledRadius, radiusPower, invertTree, currentAngle);

            // Increment the angle
            currentAngle += angleStep;
        }
    }

    private Color GetColorForLevel(int level)
    {
        switch (level)
        {
            case 0: return Color.red;
            case 1: return Color.blue;
            case 2: return Color.green;
            case 3: return Color.yellow;
            case 4: return Color.cyan;
            case 5: return Color.magenta;
            case 6: return Color.grey;
            case 7: return Color.white;
            case 8: return new Color(1, 0.5f, 0); // Orange
            case 9: return new Color(0.5f, 0, 0.5f); // Purple
            case 10: return new Color(0.5f, 0.5f, 0); // Olive
            case 11: return new Color(0, 0.5f, 0.5f); // Teal
            case 12: return new Color(0.5f, 0.5f, 0.5f); // Dark Grey
            case 13: return new Color(0.5f, 0, 0); // Maroon
            case 14: return new Color(0, 0.5f, 0); // Dark Green
            case 15: return new Color(0, 0, 0.5f); // Navy
                                                   // Add more colors/levels if needed
            default: return Color.black; // Default color if level is not explicitly handled
        }
    }

    //We handled this all in lower case per CSV reader
    public class OntologyNodeData
    {
        [Name("class id")]
        public string ClassID { get; set; }

        [Name("preferred label")]
        public string PreferredLabel { get; set; }

        [Name("parents")]
        public string Parents { get; set; }

        [Name("definitions")]
        public string definition { get; set; }
    }


    public Dictionary<Node, GameObject> GetNodeGameObjects()
    {
        return nodeGameObjects;
    }

}

[Serializable]
public class Node
{
    public string Id { get; set; }
    public string Label { get; set; }
    public string Definition { get; set; }
    public Node Parent { get; set; }
    public List<Node> Children { get; set; } = new List<Node>();

    public Node(string id, string label, string definition)
    {
        Id = id;
        Label = label;
        Definition = definition;
    }

    public Node() { }
}

public static class CustomNetworkWriterReader
{
    public static void WriteNode(this NetworkWriter writer, Node node)
    {
        writer.WriteString(node.Id);
        writer.WriteString(node.Label);
        writer.WriteString(node.Definition);

        // Serialize children count
        writer.WriteInt(node.Children.Count);

        // Recursively serialize children
        foreach (var child in node.Children)
        {
            writer.WriteNode(child);
        }
    }

    public static Node ReadNode(this NetworkReader reader)
    {
        var id = reader.ReadString();
        var label = reader.ReadString();
        var definition = reader.ReadString();

        var node = new Node(id, label, definition);

        // Deserialize children
        int childrenCount = reader.ReadInt();
        for (int i = 0; i < childrenCount; i++)
        {
            var child = reader.ReadNode();
            child.Parent = node;
            node.Children.Add(child);
        }

        return node;
    }
}

