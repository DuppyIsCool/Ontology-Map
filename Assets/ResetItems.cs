using UnityEngine;

public class ResetItems : MonoBehaviour
{
    public Transform spawnTransform; // Assign this in the Inspector with the spawn object's Transform
    public float maxDistance = 10.0f; // Set the maximum allowed distance from the spawn point
    private Rigidbody myRigidbody;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 initialScale;

    // Start is called before the first frame update
    void Start()
    {
        // If we have a rigidbody attached to our gameobj, then we want to use it to reset velocity
        if (gameObject.GetComponent<Rigidbody>() != null)
        {
            myRigidbody = gameObject.GetComponent<Rigidbody>();
        }

        if (spawnTransform != null)
        {
            // Store the initial transform of the spawn object
            initialPosition = spawnTransform.position;
            initialRotation = spawnTransform.rotation;
            initialScale = spawnTransform.localScale;
        }
        else
        {
            Debug.LogError("Spawn Transform is not set. Please assign it in the inspector.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check the distance from the spawn point
        if (spawnTransform != null && Vector3.Distance(transform.position, spawnTransform.position) > maxDistance)
        {
            ResetToSpawn();
        }
    }

    // Reset the GameObject's transform to the spawn object's transform
    private void ResetToSpawn()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        transform.localScale = initialScale;
        // Reset velocity, or else the object will keep flying off the platform. 
        if(myRigidbody != null)
            myRigidbody.velocity = Vector3.zero;
    }
}
