using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterPoolTrigger : MonoBehaviour
{
    public bool isExitTrigger;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "CustomerCar")
        {
            gameObject.GetComponent<Animator>().SetTrigger("Open");
            if (isExitTrigger)
            {
                other.gameObject.GetComponent<Collider>().isTrigger = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "CustomerCar")
        {
            gameObject.GetComponent<Animator>().SetTrigger("Close");
            
        }
    }
}
