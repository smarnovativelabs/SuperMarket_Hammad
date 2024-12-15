using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetecter : MonoBehaviour
{
    public GameObject door;

    // Start is called before the first frame update
    void Start()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Customer")
        {
            door.GetComponent<DoorOpen>().OnCustomerOpenDoor();
        }
        else if (other.gameObject.tag == "Player")
        {
            print(" triggered");
            door.GetComponent<DoorOpen>().OnPlayerEnterTrigger(true);
        }
        else if (other.gameObject.tag == "Employee")
        {
            print("Employee triggered");
            door.GetComponent<DoorOpen>().OnCustomerOpenDoor();
        }

        else if (other.gameObject.tag == "trolly")
        {
            print("trolly triggered");
            door.GetComponent<DoorOpen>().OnCustomerOpenDoor();
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.tag == "Customer")
        {
            door.GetComponent<DoorOpen>().OnCustomerCloseDoor();
        }else if (other.gameObject.tag == "Player")
        {
            door.GetComponent<DoorOpen>().OnPlayerEnterTrigger(false);
        }
        else if (other.gameObject.tag == "Employee")
        {
            door.GetComponent<DoorOpen>().OnCustomerCloseDoor();
        }
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Customer" || other.gameObject.tag == "Player")
        {
            door.GetComponent<DoorOpen>().OnStayInTrigger();
        }
    }

}
