//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class VehicleFuelPoint : MonoBehaviour,InteractableObjects
//{
//    public GameObject vehicle;
//    public GameObject nozelPoint;
//    public AudioClip nozelSound;
//    public void OnHoverItems()
//    {
//        GameObject _pickedItem = GameController.instance.currentPicketItem;
//        if (_pickedItem != null)
//        {
//            if (_pickedItem.gameObject.name != "Fuel Nozel")
//            {
//                UIController.instance.DisplayHoverObjectName("Vehicle Fuel Point!", true, HoverInstructionType.General);
//                return;
//            }
//            UIController.instance.DisplayHoverObjectName("Tap To Place Fuel Nozel!", true, HoverInstructionType.Warning);
//            return;
//        }

//        UIController.instance.DisplayHoverObjectName("Vehicle Fuel Point", true);
//    }

//    public void OnInteract()
//    {
//        if (GameController.instance.currentPicketItem == null)
//        {
//            UIController.instance.DisplayInstructions("Get Fuel Nozel From Fuel Machine");
//            return;
//        }
//        if(GameController.instance.currentPicketItem.gameObject.name!="Fuel Nozel")
//        {
//            UIController.instance.DisplayInstructions("Get Fuel Nozel From Fuel Machine");
//            return;
//        }
//        PlaceNozelToFuelPoint(GameController.instance.currentPicketItem);
//        GasStationManager.instance.UpdateGameProgressText();
//        SoundController.instance.OnPlayInteractionSound(nozelSound);
//    }
//    public void PlaceNozelToFuelPoint(GameObject _fuelNozel)
//    {
//        _fuelNozel.GetComponent<ItemPickandPlace>().OnItemRightPlaced(nozelPoint.transform);
//        _fuelNozel.GetComponent<ItemPickandPlace>().PlaceItem(gameObject);
//        vehicle.GetComponent<GasStationVehicle>().SetFuelNozelInjected(true);
        
//    }
//    public void OnFeulNozelDetached(bool _employeeDetached = false)
//    {
//        vehicle.GetComponent<GasStationVehicle>().SetFuelNozelInjected(false,_employeeDetached);
//    }
//    public bool GetVehicleFuelingStatus()
//    {
//        return vehicle.GetComponent<GasStationVehicle>().GetFuelingStatus();
//    }
//    public void TurnOffOutline()
//    {
//    }
//}
