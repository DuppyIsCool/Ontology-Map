using TMPro;
using UnityEngine;

public class OntologyNode : MonoBehaviour
{
    [SerializeField] private TextMeshPro labelField;
    [SerializeField] private LineRenderer lineRenderer;
    public void InitializeNode(string label, Transform parentTransform, Color lineColor)
    {
        labelField.text = label;

        if (parentTransform != null)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, parentTransform.position);

            // Set the color of the LineRenderer
            lineRenderer.startColor = lineColor;
            lineRenderer.endColor = lineColor;

            lineRenderer.material.color = lineColor;
            
        }
        else
        {
            lineRenderer.positionCount = 0; // No line if this node is the root
        }
    }
}
