using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheels : MonoBehaviour
{
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void UpdateSpeed(float _speed)
    {
        speed = _speed;
    }
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(transform.right, speed);
    }
}
