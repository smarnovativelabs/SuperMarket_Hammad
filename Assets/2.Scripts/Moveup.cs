using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveup : MonoBehaviour
{// Speed at which the object moves
    float speed = 0.8f;

    // Destination vector for movement (moving upwards in this case)
    Vector3 destination;

    // Start is called before the first frame update
    void Start()
    {
        // Randomize the starting position slightly around the object, but limit to a small range
        Vector3 _pos = UnityEngine.Random.insideUnitSphere * 0.25f;

        // Set the upward movement destination (moving along the Y-axis)
        destination = Vector3.up;

        // Start a coroutine that will handle destruction of the object after a delay
        StartCoroutine(MoveAndDestroyPrice());
    }

    // Update is called once per frame
    void Update()
    {
        // Move the object smoothly towards the destination (upward)
        transform.position = Vector3.Lerp(transform.position, transform.position + destination, speed * Time.deltaTime);
    }

    /// <summary>
    /// Coroutine that waits for a specified time, then destroys the object.
    /// </summary>
    IEnumerator MoveAndDestroyPrice()
    {
        // Wait for 1 second to simulate a floating or displaying effect
        yield return new WaitForSeconds(1f);

        // Destroy the game object after the delay
        Destroy(gameObject);
    }

}
