//using KinematicCharacterController.Examples;
//using System.Collections.Generic;
//using UnityEngine;

//public class TrolleyCollision : MonoBehaviour
//{
//    public bool isBlocked = false;
//    bool isTilting = false;
//    bool isResetting = false;

//    public GameObject trolley;
//    Vector3 trolleyInitialPos;
//    Quaternion trolleyInitialRotation;
//    Vector3 tiltedPosition;
//    Quaternion tiltedRotation;

//    public float tiltDuration = 0.5f; // Time to tilt
//    private float tiltProgress = 0f;

//    private Vector3 originalScale;
//    private Vector3 scaledDown = new Vector3(0.6f, 0.6f, 0.6f);
//    private float scaleProgress = 0f;
//    public int collisionCount = 0;
//    public List<Collider> invalidColliders;

//    private void Start()
//    {
//        trolleyInitialPos = trolley.transform.localPosition;
//        trolleyInitialRotation = trolley.transform.localRotation;

//        // Define the tilted position and rotation
//        tiltedPosition = new Vector3(trolleyInitialPos.x, 1f, trolleyInitialPos.z);
//        tiltedRotation = Quaternion.Euler(-45f, 0, 0);

//        // Store the original scale
//        originalScale = trolley.transform.localScale;
//    }

//    //private void OnCollisionEnter(Collision collision)
//    //{

//    //    if (collision.gameObject.CompareTag("Player"))
//    //    {
//    //        return;
//    //    }

//    //    // Check if the trolley collides with a wall or obstacle
//    //    if (collision.gameObject.GetComponent<Collider>())
//    //    {
//    //        collisionCount++;
//    //        isBlocked = true;
//    //        Debug.Log("Trolley hit a wall. Stopping movement.");
//    //    }
//    //}
//    private void OnCollisionEnter(Collision collision)
//    {
//        Debug.Log("Collision Done 10");
//        if (!GameController.instance.IsMountedOnTrolley())
//        {
//            Debug.Log("Collision Done 22210");
//            return;
//        }

//        if (collision.gameObject.GetComponent<Moneyboxes>())
//        {
//            Debug.Log("Collision Done 12");
//            return;
//        }

//        if (collision.gameObject.CompareTag("trolly"))
//        {
//            Debug.Log("Collision Done 13");
//            return;
//        }
//        if (collision.gameObject.CompareTag("Player"))
//        {
//            Debug.Log("Collision Done 14");
//            return;
//        }
//        if (collision.gameObject.GetComponent<ItemPickandPlace>())
//        {
//            Debug.Log("Collision Done 15");
//            return;
//        }
//        if (!invalidColliders.Contains(collision.collider))
//        {
//            Debug.Log("Collision Done 16");
//            invalidColliders.Add(collision.collider);
//        }

//    }

//    //private void OnCollisionExit(Collision collision)
//    //{

//    //    if (collision.gameObject.GetComponent<Collider>())
//    //    {
//    //        collisionCount--;
//    //        if(collisionCount<0) collisionCount = 0;
//    //        isBlocked = false;
//    //        Debug.Log("Trolley no longer blocked.");
//    //    }
//    //}
//    private void OnCollisionExit(Collision collision)
//    {

//        if (collision.gameObject.CompareTag("trolly"))
//        {
//            return;
//        }
//        if (collision.gameObject.CompareTag("Player"))
//        {
//            return;
//        }
//        if (collision.gameObject.GetComponent<ItemPickandPlace>())
//        {
//            return;
//        }
//        if (invalidColliders.Contains(collision.collider))
//        {
//            invalidColliders.Remove(collision.collider);
//        }
//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        if (isTilting || !other.CompareTag("tilttrolly"))
//            return;

//        Debug.Log("Tilting trolley.");
//        isTilting = true;
//        isResetting = false;
//        tiltProgress = 0f;
//        scaleProgress = 0f; // Reset scale progress
//    }

//    private void OnTriggerExit(Collider other)
//    {
//        if (isResetting || !other.CompareTag("tilttrolly"))
//            return;

//        Debug.Log("Resetting trolley.");
//        isResetting = true;
//        isTilting = false;
//        tiltProgress = 0f;
//        scaleProgress = 0f; // Reset scale progress
//    }

//    private void Update()
//    {
//        if (isTilting)
//        {
//            SmoothTilt(tiltedPosition, tiltedRotation);
//            SmoothScale(scaledDown);
//        }
//        else if (isResetting)
//        {
//            SmoothTilt(trolleyInitialPos, trolleyInitialRotation);
//            SmoothScale(originalScale);
//        }

//        if (isBlocked)
//        {

//            Controlsmanager.instance.StopPlayer();
//        }
//        else
//        {

//            // Dragging trolley will reduce player speed
//            if (Controlsmanager.instance.playervehicleInteraction.ridingVehicle != null && Controlsmanager.instance.playervehicleInteraction.VechicleType == PlayerVechicle.VechicleType.Trolly)
//            {
//                Controlsmanager.instance.charactercontroller.setMaxSpeed(2.5f);
//            }
//        }
//    }

//    private void SmoothTilt(Vector3 targetPosition, Quaternion targetRotation)
//    {
//        // Increment progress
//        tiltProgress += Time.deltaTime / tiltDuration;

//        trolley.transform.localPosition = Vector3.Lerp(trolley.transform.localPosition, targetPosition, tiltProgress);
//        trolley.transform.localRotation = Quaternion.Lerp(trolley.transform.localRotation, targetRotation, tiltProgress);

//        if (tiltProgress >= 1f)
//        {
//            if (isTilting)
//            {
//                isTilting = false;
//            }
//            else if (isResetting)
//            {
//                isResetting = false;
//            }
//        }
//    }

//    private void SmoothScale(Vector3 targetScale)
//    {
//        // Increment scale progress
//        scaleProgress += Time.deltaTime / tiltDuration;

//        trolley.transform.localScale = Vector3.Lerp(trolley.transform.localScale, targetScale, scaleProgress);

//        if (scaleProgress >= 1f)
//        {
//            scaleProgress = 1f; // Clamp the value
//        }
//    }
//}

using KinematicCharacterController.Examples;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static PlayerVechicle;
public class TrolleyCollision : MonoBehaviour
{

    bool isTilting = false;
    bool isResetting = false;

    public GameObject trolley;
    Vector3 trolleyInitialPos;
    Quaternion trolleyInitialRotation;
    Vector3 tiltedPosition;
    Quaternion tiltedRotation;

    public float tiltDuration = 0.5f; // Time to tilt
    private float tiltProgress = 0f;

    private Vector3 originalScale;
    private Vector3 scaledDown = new Vector3(0.6f, 0.6f, 0.6f);
    private float scaleProgress = 0f;
    public int collisionCount = 0;
    float recheckTime = 0f;
    public List<Collider> invalidColliders;

    private void Start()
    {
        trolley = Controlsmanager.instance.playervehicleInteraction.trollyParent.gameObject;
        invalidColliders = new List<Collider>();
        trolleyInitialPos = trolley.transform.localPosition;
        trolleyInitialRotation = trolley.transform.localRotation;

        // Define the tilted position and rotation
        tiltedPosition = new Vector3(trolleyInitialPos.x, 1f, trolleyInitialPos.z);
        tiltedRotation = Quaternion.Euler(-45f, 0, 0);

        // Store the original scale
        originalScale = trolley.transform.localScale;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (!GameController.instance.IsMountedOnTrolley())
        {
            print("1");
            return;
        }

        if (collision.gameObject.GetComponent<Moneyboxes>())
        {
            print("2");
            return;
        }

        if (collision.gameObject.CompareTag("trolly"))
        {
            print("3");
            return;
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            print("4");
            return;
        }
        if (collision.gameObject.GetComponent<ItemPickandPlace>())
        {
            print("5");
            return;
        }
        if (!invalidColliders.Contains(collision.collider))
        {
            print("6");
            invalidColliders.Add(collision.collider);
        }

    }

    private void OnCollisionExit(Collision collision)
    {

        if (collision.gameObject.CompareTag("trolly"))
        {
            return;
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            return;
        }
        if (collision.gameObject.GetComponent<ItemPickandPlace>())
        {
            return;
        }
        if (invalidColliders.Contains(collision.collider))
        {
            invalidColliders.Remove(collision.collider);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isTilting || !other.CompareTag("tilttrolly"))
            return;

        Debug.Log("Tilting trolley.");
        isTilting = true;
        isResetting = false;
        tiltProgress = 0f;
        scaleProgress = 0f; // Reset scale progress

    }

    private void OnTriggerExit(Collider other)
    {
        if (isResetting || !other.CompareTag("tilttrolly"))
            return;

        Debug.Log("Resetting trolley.");
        isResetting = true;
        isTilting = false;
        tiltProgress = 0f;
        scaleProgress = 0f; // Reset scale progress
    }

    private void Update()
    {
        recheckTime += Time.deltaTime;
        if (recheckTime > 1.5f)
        {

            int _counter = 0;
            while (_counter < invalidColliders.Count)
            {
                if (invalidColliders[_counter] == null || invalidColliders[_counter].gameObject.activeInHierarchy == false)
                {
                    invalidColliders.Remove(invalidColliders[_counter]);
                }
                else
                {
                    _counter++;
                }
            }

            recheckTime = 0f;
        }

        if (isTilting)
        {
            SmoothTilt(tiltedPosition, tiltedRotation);
            SmoothScale(scaledDown);
        }
        else if (isResetting)
        {
            SmoothTilt(trolleyInitialPos, trolleyInitialRotation);
            SmoothScale(originalScale);
        }

        if (invalidColliders.Count > 0 && GameController.instance.IsMountedOnTrolley())
        {
            Controlsmanager.instance.StopPlayer();
        }
        else
        {
            // Dragging trolley will reduce player speed
            if (Controlsmanager.instance.playervehicleInteraction.ridingVehicle != null && Controlsmanager.instance.playervehicleInteraction.VehicleType == VehicleType.Trolly)
            {
                Controlsmanager.instance.charactercontroller.setMaxSpeed(4.5f);
            }
        }

    }

    private void SmoothTilt(Vector3 targetPosition, Quaternion targetRotation)
    {
        // Increment progress
        tiltProgress += Time.deltaTime / tiltDuration;

        trolley.transform.localPosition = Vector3.Lerp(trolley.transform.localPosition, targetPosition, tiltProgress);
        trolley.transform.localRotation = Quaternion.Lerp(trolley.transform.localRotation, targetRotation, tiltProgress);

        if (tiltProgress >= 1f)
        {
            if (isTilting)
            {
                isTilting = false;
            }
            else if (isResetting)
            {
                isResetting = false;
            }
        }
    }

    private void SmoothScale(Vector3 targetScale)
    {
        // Increment scale progress
        scaleProgress += Time.deltaTime / tiltDuration;

        trolley.transform.localScale = Vector3.Lerp(trolley.transform.localScale, targetScale, scaleProgress);

        if (scaleProgress >= 1f)
        {
            scaleProgress = 1f; // Clamp the value
        }
    }
}
