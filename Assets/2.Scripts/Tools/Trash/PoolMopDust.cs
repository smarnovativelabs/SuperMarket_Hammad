using UnityEngine;
using CandyCoded.HapticFeedback;

public class PoolMopDust : MonoBehaviour, InteractableObjects
{
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
            PlayerInteraction.instance.MoveDustParticle(gameObject);
            //PoolManager.instance.OnRemoveMopDust();
            //RoomManager.instance.rooms[roomId].CheckForDustDinTrashPickedCount();
            gameObject.SetActive(false);
            HapticFeedback.LightFeedback();

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
