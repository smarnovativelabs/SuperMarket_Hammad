using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreUnlockCollider : MonoBehaviour,InteractableObjects
{
    public void OnHoverItems()
    {
        UIController.instance.DisplayHoverObjectName("Unlock Super Market!",true,HoverInstructionType.General);
        UIController.instance.OnChangeInteraction(0, true);
    }

    public void OnInteract()
    {
        if (SuperStoreManager.instance.OnPurchaseSuperStore())
        {
            TutorialManager.instance.OnCompleteTutorialTask(6);
            SuperStoreManager.instance.OpenMarketDoors();

        }
    }

    public void TurnOffOutline()
    {
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
