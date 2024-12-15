using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerInteraction : ItemPickandPlace, InteractableObjects
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
        UIController.instance.DisplayHoverObjectName(itemName, true);
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
            SetObjectToCam();
            print(gameObject.name + " Interacted");
            GameController.instance.UpdateCurrentPickedItem(gameObject);

            if (indicator[0] != null)
            {
                indicator[currentIndicator].GetComponent<Indicator>().ResetIndicator(gameObject);
                indicator[currentIndicator].GetComponent<Indicator>().EnableIndicator();
            }

        }
        else
        {
            UIController.instance.DisplayInstructions("Item is already picked");
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
