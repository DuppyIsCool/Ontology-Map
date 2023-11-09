using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VRPlayerVerticalMovement : MonoBehaviour
{
    public float speed = 1.0f; // Movement speed
    public ActionBasedController rightHandController; // Assign this in the inspector

    private void Update()
    {
        // Check if the right hand controller is assigned
        if (rightHandController == null)
        {
            Debug.LogError("Right hand controller not assigned.");
            return;
        }

        // Read the value of the vertical axis of the right hand joystick
        float verticalInput = rightHandController.moveAction.action.ReadValue<Vector2>().y;

        // Move up or down based on the vertical input from the right hand joystick
        if (Mathf.Abs(verticalInput) > 0.1f) // Deadzone to prevent drift
        {
            MoveVertical(verticalInput * speed);
        }
    }

    private void MoveVertical(float verticalSpeed)
    {
        // Move the player up or down based on the verticalSpeed
        Vector3 moveDirection = new Vector3(0, verticalSpeed, 0);
        transform.position += moveDirection * Time.deltaTime;
    }
}
