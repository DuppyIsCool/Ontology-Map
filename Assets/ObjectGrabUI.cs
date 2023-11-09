using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class ObjectGrabUI : MonoBehaviour
{
    public XRGrabInteractable grabInteractable; // Reference to the XRGrabInteractable
    public GameObject infoPanel; // Reference to the InfoPanel which follows the player
    public OntologyNode myNode; // Reference to the OntologyNode script

    // References to the Text components within the InfoPanel hierarchy
    private Text nodeValText;
    private Text parentValText;
    private Text definitionValText;

    void Awake()
    {
        infoPanel = GameObject.Find("InfoPanel");
        // Find the Text components within the InfoPanel hierarchy
        nodeValText = infoPanel.transform.Find("PanelStart/NodeVal").GetComponent<Text>();
        parentValText = infoPanel.transform.Find("PanelStart/ParentVal").GetComponent<Text>();
        definitionValText = infoPanel.transform.Find("PanelStart/DefinitionVal").GetComponent<Text>();

        // Subscribe to the select entered and exited events
        grabInteractable.onSelectEntered.AddListener(ShowInformation);
    }

    private void ShowInformation(XRBaseInteractor interactor)
    {
        // Check if the myNode variable is assigned and the infoPanel is available
        if (myNode != null && infoPanel != null)
        {
            // Update the UI Text components with information from the myNode variable
            nodeValText.text = myNode.myNode.Label;
            if (myNode.myNode.Parent != null)
                if(!string.IsNullOrWhiteSpace(myNode.myNode.Parent.Label))
                    parentValText.text = myNode.myNode.Parent.Label;
            else
                parentValText.text = "(No Parent)";
            if (!string.IsNullOrWhiteSpace(myNode.myNode.Definition))
                definitionValText.text = myNode.myNode.Definition;
            else
                definitionValText.text = "(No Definition Provided)";

            // Show the infoPanel
            infoPanel.SetActive(true);
        }
    }

    void OnDestroy()
    {
        // Unsubscribe from the events when the script is destroyed
        grabInteractable.onSelectEntered.RemoveListener(ShowInformation);
    }
}
