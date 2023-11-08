using UnityEngine;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration.Attributes;
using System.Linq;

public class MapLoader : MonoBehaviour
{
    [SerializeField] private TextAsset ontologyCsv;
    [SerializeField] private GameObject ontologyNodePrefab;
    [SerializeField] private Vector3 origin;  // The origin for the root nodes.
    [SerializeField] private float scale;  // The origin for the root nodes.
    [SerializeField] private float layerHeight;
    [SerializeField] private float radius;
    [SerializeField] private float radiusPower;
    [SerializeField] private bool invertTree;
    private Dictionary<Node, GameObject> nodeGameObjects = new Dictionary<Node, GameObject>();

    private Node RootNode { get; set; }

    private void Start()
    {
        LoadOntologyFromCSV();
        LayoutTree(RootNode, origin, 0, layerHeight, radius, radiusPower, invertTree);
    }

    private void LoadOntologyFromCSV()
    {
        using (var reader = new StringReader(ontologyCsv.text))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var ontologyData = csv.GetRecords<OntologyNodeData>().ToList();

            Dictionary<string, Node> nodes = new Dictionary<string, Node>();

            foreach (var dataRow in ontologyData)
            {
                Node node = new Node(dataRow.ClassID, dataRow.PreferredLabel);
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

    private void LayoutTree(Node node, Vector3 position, float level, float layerHeight, float radius, float radiusPower, bool invertTree)
    {
        GameObject newNodeObject = Instantiate(ontologyNodePrefab, position, Quaternion.identity, this.transform);
        newNodeObject.transform.localScale = Vector3.one * scale;

        OntologyNode newNodeComponent = newNodeObject.GetComponent<OntologyNode>();
        newNodeComponent.InitializeNode(node.Label, node.Parent != null ? nodeGameObjects[node.Parent].transform : null, GetColorForLevel((int)level));
        nodeGameObjects[node] = newNodeObject;

        float childLevel = level + 1;

        float angleStep = 360f / Mathf.Max(1, node.Children.Count);  // Avoid division by zero
        float currentAngle = 0;

        if (invertTree)
        {
            currentAngle = 180f; // Start from the opposite direction
        }

        foreach (var child in node.Children)
        {
            float newRadius = radius * Mathf.Pow(child.Children.Count, radiusPower);
            Vector3 childPosition = position + new Vector3(Mathf.Cos(currentAngle * Mathf.Deg2Rad), Mathf.Sin(currentAngle * Mathf.Deg2Rad), 0) * newRadius;

            LayoutTree(child, childPosition, childLevel, layerHeight, radius, radiusPower, invertTree);
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

    public class OntologyNodeData
    {
        [Name("Class ID")]
        public string ClassID { get; set; }

        [Name("Preferred Label")]
        public string PreferredLabel { get; set; }

        [Name("Parents")]
        public string Parents { get; set; }
    }

    public class Node
    {
        public string Id;
        public string Label;
        public Node Parent;
        public List<Node> Children = new List<Node>();

        public Node(string id, string label)
        {
            Id = id;
            Label = label;
        }
    }
}

