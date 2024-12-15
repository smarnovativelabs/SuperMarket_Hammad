using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Drone : MonoBehaviour
{
    public Transform[] waypoints; 
    public float speed = 5f; 
    public float waitTimeAtEnd = 5f; 
    public bool isMoving = true;
    private int currentPointIndex = 0; 
    private bool isReversing = false; 
    public GameObject briefcase;
    public GameObject Safe;
    private void Update()
    {
        if (waypoints.Length == 0 || !isMoving) return; 
        MoveToNextPoint();
    }

    private void MoveToNextPoint()
    {
    
        Transform targetPoint = waypoints[currentPointIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            if (isReversing)
            {
                currentPointIndex--;
                if (currentPointIndex < 0)
                {
                    currentPointIndex = 0;
                    isReversing = false;
                    isMoving = false;
                    MonetizationManager.instance.totalDroneArrived++;
                    MonetizationManager.instance.StartNextDroneDeliveryTimer();
                    gameObject.GetComponent<AudioSource>().Stop();
                    gameObject.SetActive(false);
               
                }
            }
            else
            {
                currentPointIndex++;
                if (currentPointIndex >= waypoints.Length)
                {
                    currentPointIndex = waypoints.Length - 1;
                    isReversing = true;
                    MonetizationManager.instance.SpawnMoneyBox(waypoints[currentPointIndex]);
                    briefcase.gameObject.SetActive(false);
                    Safe.gameObject.SetActive(false);
              
                }
            }
        }
    }   
    public void SetWaypoints(Transform[] newWaypoints)
    {
        waypoints = newWaypoints;
        currentPointIndex = 0; 
        isReversing = false; 
    }
    public void SetMovement(bool move)
    {
        isMoving = move;
        gameObject.GetComponent<AudioSource>().Play();
    }
}
