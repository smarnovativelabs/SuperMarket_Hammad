using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Testing : MonoBehaviour
{
    public float moveSpeed;
    public float rotationSpeed;
    public Transform targetPoint;
    // Start is called before the first frame update
    void Start()
    {
        //For In Hand Delivery
        //GetComponent<RectTransform>().sizeDelta = new Vector2(0, -80);
        //GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -40);

        //For Cart System
        //GetComponent<RectTransform>().sizeDelta = new Vector2(-300, -80);
        //GetComponent<RectTransform>().anchoredPosition = new Vector2(-150, -40);
        
        print(SystemInfo.deviceUniqueIdentifier);
    }

    // Update is called once per frame
    void Update()
    {
       // MoveTowardsTarget();
    }
}
