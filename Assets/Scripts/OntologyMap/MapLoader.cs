using UnityEngine;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration.Attributes;
using System.Linq;
using System.Collections;
using System.Text;

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
            newNodeComponent.InitializeNode(node.Label, node.Parent != null ? nodeGameObjects[node.Parent].transform : null, GetColorForLevel((int)level));
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
                childScale = 0.15f * scale;
                break;
            case > 15:
                childScale = 0.3f * scale; // Adjust the scale factor as needed
                break;
            case > 8:
                childScale = 0.5f * scale; // Adjust the scale factor as needed
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
            childNodeComponent.InitializeNode(child.Label, newNodeObject.transform, GetColorForLevel((int)(level + 1)));

            nodeGameObjects[child] = childNodeObject;

            Debug.Log($"Instantiated child node {child.Label} at scale {childScale}.");

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

    public class OntologyNodeData
    {
        [Name("Class ID")]
        public string ClassID { get; set; }

        [Name("Preferred Label")]
        public string PreferredLabel { get; set; }

        [Name("Parents")]
        public string Parents { get; set; }

        [Name("definition")]
        public string definition { get; set; }
    }

}

public class Node
{
    public string Id;
    public string Label;
    public string Definition;
    public Node Parent;
    public List<Node> Children = new List<Node>();

    public Node(string id, string label, string definiton)
    {
        Id = id;
        Label = label;
        Definition = definiton;
    }
}
