//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class FuelMachineNozelPoint : MonoBehaviour,InteractableObjects
//{
//    public GameObject fuelNozelPoint;
//    public AudioClip nozelSound;
//    public void OnHoverItems()
//    {
//        GameObject _pickedItem = GameController.instance.currentPicketItem;
//        if (_pickedItem != null)
//        {
//            if (_pickedItem.gameObject.name != "Fuel Nozel")
//            {
//                UIController.instance.DisplayHoverObjectName("Fuel Nozel Point!", true,HoverInstructionType.General);
//                return;
//            }
//            UIController.instance.DisplayHoverObjectName("Tap To Place Fuel Nozel!", true, HoverInstructionType.Warning);
//            return;
//        }
//        UIController.instance.DisplayHoverObjectName("Fuel Nozel Point", true);
//    }

//    public void OnInteract()
//    {
//        GameObject _pickedItem = GameController.instance.currentPicketItem;
//        if (_pickedItem != null)
//        {
//            if (_pickedItem.gameObject.name != "Fuel Nozel")
//            {
//                UIController.instance.DisplayInstructions("Get Fuel Nozel To Place It Back");
//                return;
//            }
//            SoundController.instance.OnPlayInteractionSound(nozelSound);
//            _pickedItem.GetComponent<ItemPickandPlace>().OnItemRightPlaced(fuelNozelPoint.transform);
//            _pickedItem.GetComponent<ItemPickandPlace>().PlaceItem();
//            GasStationManager.instance.UpdateGameProgressText();

//        }
//        else
//        {
//            UIController.instance.DisplayInstructions("Get Fuel Nozel To Place It Back");
//        }
//    }

//    public void TurnOffOutline()
//    {
//    }

//    // Start is called before the first frame update
//    void Start()
//    {
        
//    }

//    // Update is called once per frame
//    void Update()
//    {
        
//    }
//}
