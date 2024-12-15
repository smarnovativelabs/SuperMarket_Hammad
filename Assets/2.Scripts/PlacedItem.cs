using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedItem : MonoBehaviour,InteractableObjects
{
    public GameObject placingItem;

    public void OnHoverItems()
    {
        if(GameController.instance.currentPicketItem==null && GameController.instance.currentPickedTool == null)
        {
            UIController.instance.DisplayHoverObjectName("Tap To Pick " + placingItem.GetComponent<ItemPickandPlace>().itemName, true, HoverInstructionType.General);
            if (GetComponent<Outline>())
            {
                GetComponent<Outline>().enabled = true;
            }
        }
       
    }

    public void OnInteract()
    {
        var _toolPicked = GameController.instance.currentPickedTool;
        var _itemPick = GameController.instance.currentPicketItem;

        if(_toolPicked == null && _itemPick == null)
        {
            placingItem.GetComponent<InteractableObjects>().OnInteract();
            UIController.instance.DisplayHoverObjectName("", false);

            TurnOffOutline();
            gameObject.SetActive(false);
        }
    }

    public void TurnOffOutline()
    {
        if (GetComponent<Outline>())
        {
            GetComponent<Outline>().enabled = false;
        }
    }
}
