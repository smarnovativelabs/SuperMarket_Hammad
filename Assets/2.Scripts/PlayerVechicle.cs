using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using KinematicCharacterController.Examples;
public class PlayerVechicle : MonoBehaviour, InteractableObjects
{
    public VechicleType vechicleType;
    public string vehicleName;
    public float vehicleSpeed;
    public float acquireTime;
    [HideInInspector]
    public int spawnedIndex;
    public int requiredRVToUnlocked;
    // [HideInInspector]
    public int totalRVWatched;
    public Transform itemsContainer;
    public Transform itemsThrowPoint;
    public TextMeshProUGUI totalAdsText;
    public Sprite icon;
    public enum VechicleType
    {
        Walk = 0,
        HoverBoard = 1,
        GolfCart = 2,
        Scooter = 3,
        Trolly = 4
    }

    void Start()
    {
        updateUI();
        Invoke("EnableOutline", 0.5f);
    }

    void EnableOutline()
    {
        if (gameObject.GetComponent<Outline>() != null)
            gameObject.GetComponent<Outline>().enabled = true;
    }
    void OnSuccessRV()
    {
        print("4th");
        totalRVWatched++;

        if (totalRVWatched >= requiredRVToUnlocked)
        {
            print("5th");
            //disable this vehicle and enable monetization reward
            if (vechicleType == VechicleType.HoverBoard)
            {
                MonetizationManager.instance.DisableAllVehicle();
                MonetizationManager.instance.totalHoverBoardsAdsWatched++;
                gameObject.SetActive(false);
            }
            print("This is Vehicle Type in PlayerVehicle" + vechicleType + acquireTime + gameObject);
            Controlsmanager.instance.playervehicleInteraction.EnterVechicle(vehicleSpeed, vechicleType, acquireTime, spawnedIndex, gameObject);



        }
        updateUI();
    }



    void updateUI()
    {
        int required = (requiredRVToUnlocked - totalRVWatched);
        // print("required rv :" + required);
        if (totalAdsText != null)
            totalAdsText.text = required.ToString();

    }

    void OnFailureRV()
    {

    }
    public void OnHoverItems()
    {
        //print("i am hover is called");
        if (Controlsmanager.instance.playervehicleInteraction.IsRiding()) return;
        if (totalRVWatched >= requiredRVToUnlocked)
        {
           // print("if of i am hover is called ");
            UIController.instance.DisplayHoverObjectName(vehicleName, true, HoverInstructionType.General);
            UIController.instance.OnChangeInteraction(0, true);
        }
        else
        {
          /*  print("else of i am hover is called ");
            print("tetra 1" + requiredRVToUnlocked);
            print("Tetra 2 " + totalRVWatched);*/
            int required = (requiredRVToUnlocked - totalRVWatched);
            print("Tetra 3 " + required);
            UIController.instance.EnableMonetizationRVPanel(vehicleName, icon, vechicleType, acquireTime, requiredRVToUnlocked, OnSuccessRV, OnFailureRV, required);
        }
    }

    public void OnInteract()
    {
        if (Controlsmanager.instance.playervehicleInteraction.IsRiding()) return;
        if (totalRVWatched >= requiredRVToUnlocked)
        {
            Controlsmanager.instance.playervehicleInteraction.EnterVechicleAgain(vehicleSpeed, vechicleType, spawnedIndex, gameObject);
            if (vechicleType == VechicleType.HoverBoard)
            {
                MonetizationManager.instance.DisableAllVehicle();
            }
        }
        else
        {
            int required = (requiredRVToUnlocked - totalRVWatched);
            UIController.instance.EnableMonetizationRVPanel(vehicleName, icon, vechicleType, acquireTime, requiredRVToUnlocked, OnSuccessRV, OnFailureRV, required);
        }
    }

    public void TurnOffOutline()
    {
        UIController.instance.ShowHoverbaordRVPanel(false);
    }
    public void EnableContainerItemColliders(bool _enable)
    {
        for (int i = 0; i < itemsContainer.childCount; i++)
        {
            if (_enable)
            {
                if (itemsContainer.GetChild(i).GetComponent<ItemPickandPlace>())
                {
                    itemsContainer.GetChild(i).GetComponent<ItemPickandPlace>().UpdateItemSavingPosition();
                }
            }

            if (itemsContainer.GetChild(i).GetComponent<Collider>() != null)
            {
                for (int j = 0; j < itemsContainer.GetChild(i).GetComponents<Collider>().Length; j++)
                {
                    itemsContainer.GetChild(i).GetComponents<Collider>()[j].enabled = _enable;
                }
            }
        }
    }
}







