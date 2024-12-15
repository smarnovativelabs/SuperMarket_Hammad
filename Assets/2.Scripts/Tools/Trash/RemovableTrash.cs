using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CandyCoded.HapticFeedback;

public class RemovableTrash : MonoBehaviour, InteractableObjects
{
    public int areaId;
    public ObjectRelavance relatedTo;
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
            UIController.instance.DisplayInstructions("Use Dustbin To Remove Items!");
        }
        else if (currentTool.name == "Dust Bin")
        {
            PlayerInteraction.instance.MoveDustParticle(gameObject);
            gameObject.SetActive(false);
            HapticFeedback.LightFeedback();
            if (relatedTo == ObjectRelavance.SuperStore)
            {
                SuperStoreManager.instance.OnRemoveStoreTrash();
            }else if (relatedTo == ObjectRelavance.Room)
            {
                //RoomManager.instance.rooms[areaId].OnDirtyRoomTrashRemove();
            }
        }
        else if(currentTool.name != "Dust Bin")
        {
            UIController.instance.DisplayInstructions("Pick Dust Bin to remove trash");
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
