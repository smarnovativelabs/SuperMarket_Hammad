using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperStorePOS : MonoBehaviour,InteractableObjects
{
    int counterId;

    public void SetIdToPOS(int _id)
    {
        counterId = _id;
    }
    public void OnHoverItems()
    {
        UIController.instance.DisplayHoverObjectName("Tap To Serve Customers!", true, HoverInstructionType.General);
        UIController.instance.OnChangeInteraction(0, true);
        if (!SuperStoreManager.instance.IsAtCounter())
        {
            if (GetComponent<Outline>())
            {
                GetComponent<Outline>().enabled = true;
            }
        }
        else
        {
            if (GetComponent<Outline>())
            {
                GetComponent<Outline>().enabled = false;
            }
        }

    }

    public void OnInteract()
    {
        //  CashCounterManager.instance.PlayerEntersCounter();
        //set global counter id for accessing in diffrent scripts
        SuperStoreManager.instance.playerAtCounterId=counterId;
        SuperStoreManager.instance.OnPlayerInteractWithCounter(counterId);
        UIController.instance.DisplayHoverObjectName("Super Store Counter",false);
        if (GetComponent<Outline>())
        {
            GetComponent<Outline>().enabled = false;
        }
    }

    public void TurnOffOutline()
    {
        if (GetComponent<Outline>())
        {
            GetComponent<Outline>().enabled = false;
        }
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
