using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterInteractable : ItemPickandPlace, InteractableObjects
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
        UIController.instance.DisplayHoverObjectName("Tap To Serve Customers!",true, HoverInstructionType.General);
        UIController.instance.OnChangeInteraction(0, true);

        //code for outline object

        if (gameObject.GetComponent<Outline>())
        {
            gameObject.GetComponent<Outline>().enabled = true;
        }
    }

    public void OnInteract()
    {
        var toolPicked = GameController.instance.currentPickedTool;
        var itemPick = GameController.instance.currentPicketItem;

        if (itemPick == null && toolPicked == null)
        {
            PlayerController.instance.LockAtCounter();
            UIController.instance.counterCheckOutContainer.SetActive(true);
        }
        else
        {
            UIController.instance.DisplayInstructions("Drop Item");
        }


    }

    public void TurnOffOutline()
    {
        if (gameObject.GetComponent<Outline>())
        {
            gameObject.GetComponent<Outline>().enabled = false;
        }
    }
}
