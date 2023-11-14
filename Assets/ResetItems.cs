using UnityEngine;

public class ResetItems : MonoBehaviour
{
    public Transform spawnPosition; // Assign this in the Inspector with the spawn object's Transform
    public float maxDistance = 10.0f; // Set the maximum allowed distance from the spawn point
    private Rigidbody myRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        // If we have a rigidbody attached to our gameobj, then we want to use it to reset velocity
        if (gameObject.GetComponent<Rigidbody>() != null)
        {
            myRigidbody = gameObject.GetComponent<Rigidbody>();
        }

        if (spawnPosition == null)
        {
            Debug.LogError("Spawn Transform is not set. Please assign it in the inspector.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //
        // Check the distance from the spawn point
        if (spawnPosition != null && Vector3.Distance(transform.position, spawnPosition.position) > maxDistance)
        {
            ResetToSpawn();
        }
    }

    // Reset the GameObject's transform to the spawn object's transform
    private void ResetToSpawn()
    {
        transform.position = spawnPosition.position;
        transform.rotation = spawnPosition.rotation;
        // Reset velocity, or else the object will keep flying off the platform. 
        if(myRigidbody != null)
            myRigidbody.velocity = Vector3.zero;
    }
}
