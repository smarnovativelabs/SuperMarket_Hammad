using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterTrigger : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            UIController.instance.customerRequestPanel.SetActive(true);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            UIController.instance.customerRequestPanel.SetActive(false);
        }
    }
}
