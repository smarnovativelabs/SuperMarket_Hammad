using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotatereward : MonoBehaviour
{

    public float rotationSpeed = 50f; // Speed of rotation (degrees per second)

    void Update()
    {
        // Rotate the GameObject around its Y-axis
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}
