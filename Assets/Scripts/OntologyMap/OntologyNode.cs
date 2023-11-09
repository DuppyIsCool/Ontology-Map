using TMPro;
using UnityEngine;

public class OntologyNode : MonoBehaviour
{
    [SerializeField] private TextMeshPro labelField;
    [SerializeField] private LineRenderer lineRenderer;
    public Node myNode;
    public void InitializeNode(string label, Transform parentTransform, Color lineColor)
    {
        labelField.text = label;

        if (parentTransform != null)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, parentTransform.position);

            // Correctly setting the color of the child object’s material
            if (transform.GetChild(0).GetComponent<Renderer>() != null)
            {
                transform.GetChild(0).GetComponent<Renderer>().material.color = lineColor;
            }

            lineRenderer.material.color = lineColor;
        }
        else
        {
            lineRenderer.positionCount = 0; // No line if this node is the root
        }
    }
}
