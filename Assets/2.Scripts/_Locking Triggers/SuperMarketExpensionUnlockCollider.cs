using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperMarketExpensionUnlockCollider : MonoBehaviour, InteractableObjects
{
    public int unlockIndex;
    public void OnHoverItems()
    {
        UIController.instance.DisplayHoverObjectName("Unlock Super Market Expesnion!");
        UIController.instance.OnChangeInteraction(0, true);
    }

    public void OnInteract()
    {
        if (SuperStoreManager.instance.UnlockSuperMarketExpension(unlockIndex))
        {
            ///update on UI
            UpgradesUIManager.instance.UpdateLockStatus();
        }
    }

    public void TurnOffOutline()
    {
    }
}
