using UnityEngine;

namespace UnityEngine.XR.Interaction.Toolkit
{
    /// <summary>
    /// Highlights objects created by the MapLoader script.
    /// </summary>
    [RequireComponent(typeof(MapLoader))] // Ensure this script is attached to an object with MapLoader.
    public class HighlightMapObjects : MonoBehaviour
    {
        private MapLoader mapLoader;
        private Material highlightMaterial; // Assign a material with a highlight effect in the Unity Editor.

        private void Start()
        {
            mapLoader = GetComponent<MapLoader>();
            
            // Ensure a highlight material is assigned in the Unity Editor.
            if (highlightMaterial == null)
            {
                Debug.LogError("Highlight material is not assigned. Please assign a material with a highlight effect.");
                enabled = false; // Disable the script if the highlight material is not assigned.
                return;
            }

            // Subscribe to the event raised when the ontology tree is loaded and laid out.
            mapLoader.OnOntologyTreeLoaded += OnOntologyTreeLoaded;
        }

        private void OnDestroy()
        {
            // Unsubscribe from events to prevent memory leaks.
            mapLoader.OnOntologyTreeLoaded -= OnOntologyTreeLoaded;
        }

        private void OnOntologyTreeLoaded()
        {
            // Iterate through the objects created by the MapLoader script and highlight them.
            foreach (var nodeGameObject in mapLoader.NodeGameObjects.Values)
            {
                // Check if the game object has a Renderer component.
                Renderer renderer = nodeGameObject.GetComponent<Renderer>();
                if (renderer != null)
                {
                    // Store the original material and apply the highlight material.
                    renderer.material = highlightMaterial;
                }
            }
        }
    }
}
