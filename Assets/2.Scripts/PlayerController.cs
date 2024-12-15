using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public Transform CameraLockAtCounter;
    public float moveSpeed = 5f; // Speed at which the player moves towards the target

    private bool shouldMove = false; // Flag to start the movement
    private Vector3 startPosition; // To store the initial position when movement starts

    void Start()
    {
        instance = this;
    }

    void Update()
    {
        if (shouldMove)
        {
            // Move the player towards the target position using MoveTowards
            transform.position = Vector3.MoveTowards(transform.position, CameraLockAtCounter.position, moveSpeed * Time.deltaTime);

            // Stop moving when the player is close enough to the target
            if (Vector3.Distance(transform.position, CameraLockAtCounter.position) < 0.1f)
            {
                shouldMove = false; // Stop moving
            }
        }
    }

    public void LockAtCounter()
    {
        // Set the start position and start moving the player towards the target
        startPosition = transform.position;
        shouldMove = true; // Start moving the player towards the target
    }
}
