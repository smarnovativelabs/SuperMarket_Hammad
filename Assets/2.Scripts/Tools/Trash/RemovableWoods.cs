using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CandyCoded.HapticFeedback;

public class RemovableWoods :MonoBehaviour, InteractableObjects
{
    public string itemName;
    public ObjectRelavance relatedTo;
    public void OnHoverItems()
    {
        UIController.instance.DisplayHoverObjectName(itemName, true);
        UIController.instance.OnChangeInteraction(0, true);

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
        }
        else if (currentTool.name == "Hammer")
        {
            PlayerInteraction.instance.MoveDustParticle(gameObject);
            gameObject.SetActive(false);
            HapticFeedback.LightFeedback();
            if (relatedTo == ObjectRelavance.SuperStore)
            {
                SuperStoreManager.instance.OnRemoveStoreWood();
            }
            else if (relatedTo == ObjectRelavance.Pool)
            {
              //  PoolManager.instance.OnPoolWoodRemove();
            }
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
