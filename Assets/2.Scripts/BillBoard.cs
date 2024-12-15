using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    public Transform targetCam;
    Vector3 targetAngle;
    // Start is called before the first frame update
   

    // Update is called once per frame
    void Update()
    {
        if (targetCam == null)
        {
            targetCam = Camera.main.transform;
            return;
        }
        transform.LookAt(targetCam);
        targetAngle = transform.eulerAngles;
        targetAngle.x = 0;
        targetAngle.z = 0;
        transform.eulerAngles = targetAngle;
    }
}
