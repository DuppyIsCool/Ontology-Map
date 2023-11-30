using UnityEngine;

public class ResetPlayerPosition : MonoBehaviour
{
    public Transform referenceObject; // The object to compare the player's y position with
    public float resetHeight = 1f; // The height above the reference object to reset the player to
    public float threshold = 10f; // Threshold for checking if the player is too far below

    void Update()
    {
        // Calculate the difference in y position
        float difference = transform.position.y - referenceObject.position.y;

        if (difference < -threshold)
        {
            // Print an error message
            Debug.LogError("Player fell through the platform!");
        }
        else if (difference < 0)
        {
            transform.position = new Vector3(transform.position.x, referenceObject.position.y + resetHeight, transform.position.z);
        }
    }
}
