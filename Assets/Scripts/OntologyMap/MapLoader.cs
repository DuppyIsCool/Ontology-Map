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
    private Dictionary<Node, GameObject> nodeGameObjects = new Dictionary<Node, GameObject>();

    public List<Node> Roots { get; private set; } = new List<Node>();

    void Start()
    {
        LoadOntology();

        float rootSpacing = 5.0f;
        for (int i = 0; i < Roots.Count; i++)
        {
            PositionNodes(Roots[i], origin + new Vector3(i * rootSpacing, 0, 0), 5.0f, scale, 0);
            Debug.Log("EEASesferg");
        }
    }

    void LoadOntology()
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
                    Roots.Add(node);
                }
            }
        }
    }

    void PositionNodes(Node node, Vector3 position, float radius, float scale, int level)
    {
        GameObject newNodeObject = Instantiate(ontologyNodePrefab, position, Quaternion.identity);
        newNodeObject.transform.localScale = Vector3.one * scale;
        OntologyNode newNodeComponent = newNodeObject.GetComponent<OntologyNode>();
        newNodeComponent.InitializeNode(node.Label, node.Parent != null ? nodeGameObjects[node.Parent].transform : null, GetColorForLevel(level));
        nodeGameObjects[node] = newNodeObject;

        float angleStep = 360f / Mathf.Max(1, node.Children.Count);  // Avoid division by zero
        float currentAngle = 0;

        foreach (var child in node.Children)
        {
            float childRadius = radius * (1 + 0.5f * child.Children.Count);  // Dynamic radius based on children count
            Vector3 childPosition = position + new Vector3(Mathf.Cos(currentAngle * Mathf.Deg2Rad), Mathf.Sin(currentAngle * Mathf.Deg2Rad), 0) * childRadius;
            PositionNodes(child, childPosition, radius * 0.8f, scale * 0.8f, level + 1);  // Increment level for child nodes
            currentAngle += angleStep;
        }
    }

    Color GetColorForLevel(int level)
    {
        switch (level)
        {
            case 0: return Color.red;
            case 1: return Color.blue;
            case 2: return Color.green;
            case 3: return Color.yellow;
            // Add more colors/levels if needed
            default: return Color.white; // Default color if level is not explicitly handled
        }
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


