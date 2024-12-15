using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CandyCoded.HapticFeedback;
public class HammerTrash : ItemPickandPlace, InteractableObjects
{
    public int roomId;
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
        var currentTool = GameController.instance.currentPickedTool;
        if (currentTool == null)
        {
            UIController.instance.DisplayInstructions("Pick hammer to remove wood");
            //SetObjectToCam();
            //GameController.instance.UpdateCurrentPickedItem(gameObject);
        }
        else if (currentTool.name == "Hammer")
        {
            //RoomManager.instance.currentRoomNumber = roomId;
            PlayerInteraction.instance.MoveDustParticle(gameObject);
            gameObject.SetActive(false);
            HapticFeedback.LightFeedback();
           // RoomManager.instance.rooms[roomId].OnRemoveWood(roomId);

        }
        else if (currentTool.name != "Hammer")
        {
            UIController.instance.DisplayInstructions("Pick hammer to remove wood");
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
