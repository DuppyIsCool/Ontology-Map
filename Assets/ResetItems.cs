using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetItems : MonoBehaviour
{
    public Vector3 spawnLocation;
    public float resetDistance = 15.0f; 

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the spawn location to the current position at the time of awakening
        spawnLocation = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Check the distance from the current position to the spawn location
        if (Vector3.Distance(transform.position, spawnLocation) > resetDistance)
        {
            //If it's greater than resetDistance, reset the position
            transform.position = spawnLocation;
        }
    }
}
