using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelUnlockCollider : MonoBehaviour,InteractableObjects
{
    public int unlockIndex;
    public void OnHoverItems()
    {
        UIController.instance.DisplayHoverObjectName("Unlock Fuel Machine!",true,HoverInstructionType.Warning);
        UIController.instance.OnChangeInteraction(0, true);
    }

    public void OnInteract()
    {
        //if (GasStationManager.instance.UnlockFillingPoint(unlockIndex))
        //{
        //    UpgradesUIManager.instance.UpdateLockStatus();
        //}
    }

    public void TurnOffOutline()
    {
    }

    
}
