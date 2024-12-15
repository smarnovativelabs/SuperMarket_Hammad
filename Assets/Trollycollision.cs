using KinematicCharacterController.Examples;
using UnityEngine;

public class TrolleyCollision : MonoBehaviour
{
    public bool isBlocked = false;
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

    private void Start()
    {
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
      
        if (collision.gameObject.CompareTag("Player"))
        {
            return;
        }
       
        // Check if the trolley collides with a wall or obstacle
        if (collision.gameObject.GetComponent<Collider>())
        {
            collisionCount++;
            isBlocked = true;
            Debug.Log("Trolley hit a wall. Stopping movement.");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
       
        if (collision.gameObject.GetComponent<Collider>())
        {
            collisionCount--;
            if(collisionCount<0) collisionCount = 0;
            isBlocked = false;
            Debug.Log("Trolley no longer blocked.");
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

        if (isBlocked)
        {
           
            Controlsmanager.instance.StopPlayer();
        }
        else
        {
         
            // Dragging trolley will reduce player speed
            if (Controlsmanager.instance.playervehicleInteraction.ridingVehicle != null && Controlsmanager.instance.playervehicleInteraction.VechicleType == PlayerVechicle.VechicleType.Trolly)
            {
                Controlsmanager.instance.charactercontroller.setMaxSpeed(2.5f);
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
