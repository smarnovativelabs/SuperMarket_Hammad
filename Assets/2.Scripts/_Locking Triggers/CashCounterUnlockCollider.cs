using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashCounterUnlockCollider : MonoBehaviour, InteractableObjects
{
    public int unlockIndex;
    public void OnHoverItems()
    {
        UIController.instance.DisplayHoverObjectName("Unlock Super Market Cash Counter!");
        UIController.instance.OnChangeInteraction(0, true);
    }

    public void OnInteract()
    {
        if (SuperStoreManager.instance.UnlockCashCounter(unlockIndex))
        {
            ///update on UI
            UpgradesUIManager.instance.UpdateLockStatus();
        }
    }

    public void TurnOffOutline()
    {
    }
}
