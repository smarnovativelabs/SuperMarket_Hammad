using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpcomingCollider : MonoBehaviour,InteractableObjects
{
    public int requiredLevel = 22;
    void InteractableObjects.OnHoverItems()
    {
        UIController.instance.DisplayHoverObjectName("Area Locked!",true,HoverInstructionType.Warning);
    }

    void InteractableObjects.OnInteract()
    {
        if (PlayerDataManager.instance.playerData.playerLevel >= requiredLevel)
        {
            UIController.instance.DisplayInstructions("Coming Soon!");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void InteractableObjects.TurnOffOutline()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
