//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class FuelNozel : ItemPickandPlace, InteractableObjects
//{
//    bool fuelNozelSelected = false;
//    public GameObject fuelMachine;
//    public GameObject machineNozelPoint;
//    public float nozelPipeLength;
//    public AudioClip nozelPickSound;
//    public bool isUnlocked;
//    public int fillingPointIndex;
//    public bool employeeHaveNozel;

//    public override void Update()
//    {
//        base.Update();
//        if (Vector3.Distance(transform.position, fuelMachine.transform.position) >= nozelPipeLength)
//        {
//            //Should Detached From Player
//            UIController.instance.DisplayInstructions("Nozel Pipe Limit Reached! Nozel Placed Back!");
//            machineNozelPoint.GetComponent<FuelMachineNozelPoint>().OnInteract();
//        }

//    }
//    public void OnHoverItems()
//    {
//        UIController.instance.OnChangeInteraction(0, true);
//        if (gameObject.GetComponent<Outline>())
//        {
//            gameObject.GetComponent<Outline>().enabled = true;
//        }
//        if (!isUnlocked)
//        {
//            UIController.instance.DisplayHoverObjectName("Unlock Fuel Machine!", true, HoverInstructionType.Warning);
//            return;
//        }
//        if (GameController.instance.currentPicketItem == null)
//        {
//            UIController.instance.DisplayHoverObjectName("Tap To Pick Fuel Nozel", true, HoverInstructionType.General);
//        }
//    }

//    public void OnInteract()
//    {
//        var toolPicked = GameController.instance.currentPickedTool;
//        var itemPick = GameController.instance.currentPicketItem;
//        if (itemPick == null && toolPicked == null)
//        {
//            if (!isUnlocked)
//            {
//                //Implement Unlocking Here
//                return;
//            }
//            //Indicater is used to Keep The Refernce Of Injected Fuel Vehicle
//            if (indicator[0] != null)
//            {
//                if (indicator[0].GetComponent<VehicleFuelPoint>().GetVehicleFuelingStatus())
//                {
//                    UIController.instance.DisplayInstructions("Stop Fueling From Fuel Machine First");
//                    return;
//                }
//                indicator[0].GetComponent<VehicleFuelPoint>().OnFeulNozelDetached();
//                indicator[0] = null;
//                GasStationManager.instance.UpdateGameProgressText();

//                GameManager.instance.CallFireBase("nozelDetached");

//            }
//            if (employeeHaveNozel)
//                return;

//            GameManager.instance.CallFireBase("nozelPicked");
//            GasStationManager.instance.UpdateGameProgressText();
//            SetObjectToCam();
//            GameController.instance.UpdateCurrentPickedItem(gameObject);
//            fuelNozelSelected = true;
//            SoundController.instance.OnPlayInteractionSound(nozelPickSound);

//        }
//        else
//        {
//            if (itemPick == null)
//                UIController.instance.DisplayInstructions("Item is already picked");
//            else
//                UIController.instance.DisplayInstructions("Already picked a tool");
//        }
//    }
//    public void EmployeePickNozel(Transform _employeeNozelPoint)
//    {
//        if (!isUnlocked)
//        {
//            //Implement Unlocking Here
//            return;
//        }
//        if (employeeHaveNozel)
//        {
//            return;
//        }
//        //Indicater is used to Keep The Refernce Of Injected Fuel Vehicle
//        if (indicator[0] != null)
//        {
//            if (indicator[0].GetComponent<VehicleFuelPoint>().GetVehicleFuelingStatus())
//            {
//                //Should Close Fueling First
//                return;
//            }
//            indicator[0].GetComponent<VehicleFuelPoint>().OnFeulNozelDetached(true);
//            indicator[0] = null;
//        }
//        employeeHaveNozel = true;
//        transform.parent = _employeeNozelPoint;
//        transform.localPosition = Vector3.zero;
//        transform.localRotation = Quaternion.identity;
//        transform.localScale = Vector3.one;
//    }
//    public void EmployeePlaceNozel(GameObject _vehicleRef=null)
//    {
//        if (!employeeHaveNozel)
//            return;

//        if (_vehicleRef==null)
//        {
//            GetComponent<ItemPickandPlace>().OnItemRightPlaced(machineNozelPoint.GetComponent<FuelMachineNozelPoint>().fuelNozelPoint.transform);

//            GetComponent<ItemPickandPlace>().PlaceItem();
//            employeeHaveNozel = false;
//            return;
//        }
        
//        _vehicleRef.GetComponent<GasStationVehicle>().activeFuelPoint.GetComponent<VehicleFuelPoint>().PlaceNozelToFuelPoint(gameObject);
//        employeeHaveNozel = false;

//    }
//    public override void PlaceItem()
//    {
        
//        Collider[] _colliders = GetComponents<Collider>();

//        for (int i = 0; i < _colliders.Length; i++)
//        {
//            _colliders[i].enabled = true;
//        }

//        if (GetComponent<Rigidbody>())
//        {
//            GetComponent<Rigidbody>().isKinematic = true;
//        }
//        transform.parent = null;
//        if (!employeeHaveNozel)
//        {
//            GameController.instance.currentPicketItem = null;
//            fuelNozelSelected = false;

//        }
//        //SoundController.instance.OnPlayInteractionSound(placeObjectSound);

//        for (int i = 0; i < transform.childCount; i++)
//        {
//            if (transform.GetChild(i).GetComponent<BoxCollider>())
//                transform.GetChild(i).GetComponent<BoxCollider>().enabled = true;
//        }

//    }
//    public override void PlaceItem(GameObject _itemRef)
//    {
//        Collider[] _colliders = GetComponents<Collider>();

//        for (int i = 0; i < _colliders.Length; i++)
//        {
//            _colliders[i].enabled = true;
//        }

//        if (GetComponent<Rigidbody>())
//        {
//            GetComponent<Rigidbody>().isKinematic = true;
//        }
//        transform.parent = null;
//        if (!employeeHaveNozel)
//        {
//            GameController.instance.currentPicketItem = null;

//        }
//        //SoundController.instance.OnPlayInteractionSound(placeObjectSound);

//        for (int i = 0; i < transform.childCount; i++)
//        {
//            if (transform.GetChild(i).GetComponent<BoxCollider>())
//                transform.GetChild(i).GetComponent<BoxCollider>().enabled = true;
//        }
//        //Indicater list is user to keeep reference of the vehicle to whiich nozel is placed
//        GameManager.instance.CallFireBase("nozelAttached");
//        indicator[0] = _itemRef;
//        fuelNozelSelected = false;
//        print("Called NNozel Attached To Machjine");

//    }
//    public override void ThrowPickedObjects()
//    {
//        machineNozelPoint.GetComponent<FuelMachineNozelPoint>().OnInteract();
//        fuelNozelSelected = false;
//    }
//    public bool IsNozelSelected()
//    {
//        return fuelNozelSelected;
//    }
//    public bool IsNozelInjectedToVehicle()
//    {
//        return (indicator[0] != null);
//    }
//    public bool IsFuelingVehicle()
//    {
//        if (indicator[0] != null)
//        {
//            return indicator[0].GetComponent<VehicleFuelPoint>().GetVehicleFuelingStatus();
//        }
//        return false;
//    }
//    public void TurnOffOutline()
//    {
//        if (gameObject.GetComponent<Outline>())
//        {
//            gameObject.GetComponent<Outline>().enabled = true;
//        }
//    }
//}
