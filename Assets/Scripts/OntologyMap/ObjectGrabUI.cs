using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class ObjectGrabUI : MonoBehaviour
{
    public XRGrabInteractable grabInteractable; // Reference to the XRGrabInteractable
    public GameObject infoPanel; // Reference to the InfoPanel which follows the player
    public OntologyNode myNode; // Reference to the OntologyNode script
    public GameObject Map;
    public GameObject highlight;
    // References to the Text components within the InfoPanel hierarchy
    private Text nodeValText;
    private Text parentValText;
    private Text definitionValText;
    private Rigidbody rb;
    void Awake()
    {
        infoPanel = GameObject.Find("InfoPanel");
        Map = GameObject.Find("Map");
        // Find the Text components within the InfoPanel hierarchy
        nodeValText = infoPanel.transform.Find("PanelStart/NodeVal").GetComponent<Text>();
        parentValText = infoPanel.transform.Find("PanelStart/ParentVal").GetComponent<Text>();
        definitionValText = infoPanel.transform.Find("PanelStart/DefinitionVal").GetComponent<Text>();
        rb = GetComponent<Rigidbody>();
        // Subscribe to the select entered and exited events
        grabInteractable.onSelectEntered.AddListener(GrabShowInformation);
    }

    

    //Function to show information when grabbed
    private void GrabShowInformation(XRBaseInteractor interactor)
    {
        rb.useGravity = false;
        rb.isKinematic = true;
        //this.transform.parent = Map.transform;
        ShowInformation();
    }

    //Shows information on the panel, this is called on grab and on hover.
    public void ShowInformation() 
    {
        // Check if the myNode variable is assigned and the infoPanel is available
        if (myNode != null && infoPanel != null)
        {
            //If we are grabbing a new node, disable the previous node's highlight and toggle ours on
            if (infoPanel.GetComponent<PanelInfo>().selectedNode != null)
            {
                Debug.Log("A");
                if (!infoPanel.GetComponent<PanelInfo>().selectedNode.GetComponent<OntologyNode>().myNode.Label.Equals(myNode.myNode.Label))
                {
                    Debug.Log("C");
                    infoPanel.GetComponent<PanelInfo>().selectedNode.GetComponent<ObjectGrabUI>().SetHighlight(false);
                    SetHighlight(true);
                    infoPanel.GetComponent<PanelInfo>().selectedNode = this.gameObject;
                }
            }
            else 
            {
                SetHighlight(true);
                Debug.Log("B");
                infoPanel.GetComponent<PanelInfo>().selectedNode = this.gameObject;             
            }

            // Update the UI Text components with information from the myNode variable
            nodeValText.text = myNode.myNode.Label;
            if (myNode.myNode.Parent != null)
                if (!string.IsNullOrWhiteSpace(myNode.myNode.Parent.Label))
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

    void OnTriggerEnter(Collider other)
    {
        // Enable gravity when entering the trigger
        if (other.CompareTag("BookTrigger")) // Replace with your specific trigger tag
        {
            if (rb != null)
            {
                rb.useGravity = true;
                rb.isKinematic = false;
                //this.transform.parent = other.transform;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Disable gravity when exiting the trigger
        if (other.CompareTag("BookTrigger")) // Replace with your specific trigger tag
        {
            if (rb != null)
            {
                rb.useGravity = false;
                rb.isKinematic = true;
                //rb.transform.parent = Map.transform;
            }
        }
    }

    public void SetHighlight(bool val) 
    {
        highlight.SetActive(val);
    }

    void OnDestroy()
    {
        // Unsubscribe from the events when the script is destroyed
        grabInteractable.onSelectEntered.RemoveListener(GrabShowInformation);
    }
}
