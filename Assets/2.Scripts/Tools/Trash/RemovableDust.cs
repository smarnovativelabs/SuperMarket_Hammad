using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CandyCoded.HapticFeedback;

public class RemovableDust : MonoBehaviour, InteractableObjects
{
    public int areaId;
    public string itemName;
    public ObjectRelavance relatedTo;
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
                UIController.instance.DisplayHoverObjectName("Pick Mop To Remove Dust!", true, HoverInstructionType.Tools);
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
            UIController.instance.DisplayInstructions("Pick Mop to remove Dust");
        }
        else if (currentTool.name == "Brush")
        {
            PlayerInteraction.instance.MoveDustParticle(gameObject);
            gameObject.SetActive(false);
            HapticFeedback.LightFeedback();
            if (relatedTo == ObjectRelavance.SuperStore)
            {
                SuperStoreManager.instance.OnRemoveStoreDust();
            }else if (relatedTo == ObjectRelavance.Room)
            {
               // RoomManager.instance.rooms[areaId].OnDirtyRoomDustRemove();
            }
        }
        else if (currentTool.name != "Brush")
        {
            UIController.instance.DisplayInstructions("Pick Mop to Remove Dust");
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
