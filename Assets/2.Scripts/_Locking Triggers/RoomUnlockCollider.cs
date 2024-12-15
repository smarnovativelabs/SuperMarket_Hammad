//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class RoomUnlockCollider : MonoBehaviour,InteractableObjects
//{
//    public int roomIndex;

//    public void OnHoverItems()
//    {
//        UIController.instance.DisplayHoverObjectName("Tap To Unlock Room!",true,HoverInstructionType.General);
//        UIController.instance.OnChangeInteraction(0, true);
//    }

//    public void OnInteract()
//    {
//        if (RoomManager.instance.OnPurchaseRoom(roomIndex))
//        {
//            RoomManager.instance.OnOpenDoor(roomIndex);
//            UpgradesUIManager.instance.UpdateLockStatus();

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
