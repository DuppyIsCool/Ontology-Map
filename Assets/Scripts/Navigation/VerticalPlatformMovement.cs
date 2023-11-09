using UnityEngine;

public class VerticalPlatformMovement : MonoBehaviour
{
    public float speed = 1.0f; // Speed of the vertical movement
    public GameObject targetObject; // The target GameObject to move, set this in the Inspector
    private bool isRaising = false;
    private bool isLowering = false;

    private void Update()
    {
        // Check if the object should be raising
        if (isRaising)
        {
            Move(Vector3.up);
        }

        // Check if the object should be lowering
        if (isLowering)
        {
            Move(Vector3.down);
        }
    }

    // Call this function with true to start raising and false to stop
    public void SetRaising(bool raising)
    {
        isRaising = raising;
        if (raising) isLowering = false; // Prevent both from happening at the same time
    }

    // Call this function with true to start lowering and false to stop
    public void SetLowering(bool lowering)
    {
        isLowering = lowering;
        if (lowering) isRaising = false; // Prevent both from happening at the same time
    }

    // Move the GameObject and the target object in the given direction
    private void Move(Vector3 direction)
    {
        // Move the GameObject this script is attached to
        transform.position += direction * speed * Time.deltaTime;

        // Also move the target object if it's set
        if (targetObject != null)
        {
            targetObject.transform.position += direction * speed * Time.deltaTime;
        }
    }
}
