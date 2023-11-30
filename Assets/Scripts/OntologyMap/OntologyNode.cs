using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class OntologyNode : MonoBehaviour
{
    [SerializeField] private TextMeshPro labelField;
    [SerializeField] private TextMeshPro labelField2;
    [SerializeField] private LineRenderer lineRenderer;
    public Node myNode;
    private Transform parentTransform; // Store the parent transform

    public void InitializeNode(Node node, Transform parentTransform, Color lineColor)
    {
        myNode = node;
        labelField.text = node.Label;
        labelField2.text = node.Label;
        this.parentTransform = parentTransform; // Assign the parent transform to the member variable

        if (parentTransform != null)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, parentTransform.position);
            lineRenderer.material.color = lineColor;
        }
        else
        {
            lineRenderer.positionCount = 0; // No line if this node is the root
        }
    }

    public void UpdateLine()
    {
        // Only update the line if a parent transform is assigned
        if (parentTransform != null)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, parentTransform.position);
        }
    }
}
