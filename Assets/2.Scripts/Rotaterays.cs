using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotaterays : MonoBehaviour
{
    public float rotationSpeed = 50f; 

    private RectTransform rectTransform;

    void Start()
    {
      
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
     
        rectTransform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}
