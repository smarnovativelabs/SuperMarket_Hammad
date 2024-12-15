using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour, InteractableObjects
{
    public void OnHoverItems()
    {
       /* if (gameObject.GetComponent<Outline>())
        {
            if (gameObject.GetComponent<Outline>().enabled == true)
            {
                gameObject.GetComponent<Outline>().enabled = false;
            }
        }*/
        UIController.instance.DisplayHoverObjectName("Tap to Open Game Store!", true, HoverInstructionType.General);
        UIController.instance.OnChangeInteraction(0, true);

        //code for outline object
        
        if (gameObject.GetComponent<Outline>())
        {
            gameObject.GetComponent<Outline>().enabled = true;
        }
    }

    public void OnInteract()
    {
        UIController.instance.OnPressPC();
    }

    public void TurnOffOutline()
    {
        if (gameObject.GetComponent<Outline>())
        {
            gameObject.GetComponent<Outline>().enabled = false;
        }
    }
}
