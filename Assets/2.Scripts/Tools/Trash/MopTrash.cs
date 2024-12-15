using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CandyCoded.HapticFeedback;
public class MopTrash : ItemPickandPlace, InteractableObjects
{
    public int roomId;

    public void OnHoverItems()
    {
        if (GameController.instance.currentPicketItem == null)
        {
            var currentTool = GameController.instance.currentPickedTool;
            if (currentTool == null)
            {
                UIController.instance.DisplayHoverObjectName("Select Mop To Remove Dust!", true, HoverInstructionType.Tools);
            }
            else if (currentTool.name == "Brush")
            {
                UIController.instance.DisplayHoverObjectName("Tap To Remove Dust!", true, HoverInstructionType.General);
                UIController.instance.OnChangeInteraction(0, true);
                
            }
            else
            {
                UIController.instance.DisplayHoverObjectName("Select Mop To Remove Dust!", true, HoverInstructionType.Tools);
            }
            if (gameObject.GetComponent<Outline>())
            {
                gameObject.GetComponent<Outline>().enabled = true;
            }
        }
    }

    public void OnInteract()
    {
        print(gameObject.name + " Interacted");
        var currentTool = GameController.instance.currentPickedTool;
        if (currentTool == null)
        {
            UIController.instance.DisplayInstructions("Pick Mop to remove Stains");
            //SetObjectToCam();
            //GameController.instance.UpdateCurrentPickedItem(gameObject);
        }
        else if (currentTool.name == "Brush")
        {
           // RoomManager.instance.currentRoomNumber = roomId;
            PlayerInteraction.instance.MoveDustParticle(gameObject);
           // RoomManager.instance.rooms[roomId].inactiveCountofMopTrash++;
           // RoomManager.instance.rooms[roomId].CheckRoomProgress();
            //RoomManager.instance.rooms[roomId].CheckForDustDinTrashPickedCount();
            gameObject.SetActive(false);
            HapticFeedback.LightFeedback();
          //  RoomManager.instance.rooms[roomId].OnRemoveDust(roomId);
           
        }
        else if (currentTool.name != "Brush")
        {
            UIController.instance.DisplayInstructions("Pick Mop to Remove Stains");
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
