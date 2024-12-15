using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CandyCoded.HapticFeedback;
public class DustBinTrash : MonoBehaviour,InteractableObjects
{
    public int roomId;
    public void OnHoverItems()
    {

        if (GameController.instance.currentPicketItem == null)
        {
            var currentTool = GameController.instance.currentPickedTool;
            if (currentTool == null)
            {
                UIController.instance.DisplayHoverObjectName("Select Dustbin To Remove Trash!", true, HoverInstructionType.Tools);
            }
            else if (currentTool.name == "Dust Bin")
            {
                UIController.instance.DisplayHoverObjectName("Tap To Remove Trash!", true, HoverInstructionType.General);
                UIController.instance.OnChangeInteraction(0, true);

            }
            else
            {
                UIController.instance.DisplayHoverObjectName("Select Dust Bin to Remove Trash!", true, HoverInstructionType.Tools);
            }
            if (gameObject.GetComponent<Outline>())
            {
                gameObject.GetComponent<Outline>().enabled = true;
            }
        }
    }

    public void OnInteract()
    {
        var currentTool = GameController.instance.currentPickedTool;
        if (currentTool == null)
        {
            UIController.instance.DisplayInstructions("Use Dustbin To Remove Trash Items!");
        }
        else if (currentTool.name == "Dust Bin")
        {
            //RoomManager.instance.currentRoomNumber = roomId;
            PlayerInteraction.instance.MoveDustParticle(gameObject);
           // RoomManager.instance.rooms[roomId].inactiveCountofDustbin++;
            //RoomManager.instance.rooms[roomId].CheckForDustDinTrashPickedCount();
            gameObject.SetActive(false);
            HapticFeedback.LightFeedback();
           // RoomManager.instance.rooms[roomId].OnRemoveTrash(roomId);
        }
        else if(currentTool.name != "Dust Bin")
        {
            UIController.instance.DisplayInstructions("Use Dustbin To Remove Trash Items!");
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
