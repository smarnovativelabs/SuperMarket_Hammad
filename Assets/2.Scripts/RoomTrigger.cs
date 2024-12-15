//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class RoomTrigger : MonoBehaviour
//{
//    public int roomId;
//    bool isTriggerExit;
//    private void Start()
//    {
//        gameObject.layer = 2;
//    }
//    public void OnTriggerEnter(Collider other)
//    {
//        if (other.gameObject.tag == "Player")
//        {
//            RoomManager.instance.UpdateCurrentRoomNumber(roomId);
//            if (RoomManager.instance.rooms[roomId].AreAllTrashTasksComplete())
//            {
//                RoomManager.instance.rooms[roomId].inactiveCountofDustbin = RoomManager.instance.rooms[roomId].totalDustBinTrash;
//            }
//            //UIController.instance.SetRoomProgress(true, RoomManager.instance.rooms[roomId].inactiveCountofDustbin, RoomManager.instance.rooms[roomId].totalDustBinTrash);
//            RoomManager.instance.rooms[roomId].DonePaintCountToShowOnUI();
//            RoomManager.instance.rooms[roomId].CheckRoomProgress(false);
//            RoomManager.instance.rooms[roomId].UpdateRoomReflection();
//            if (GameController.instance.currentPicketItem != null)
//            {
//                if (!RoomManager.instance.rooms[roomId].AreAllTrashTasksComplete())
//                {
//                    UIController.instance.DisplayInstructions("Clean all trash");
//                }
//            }

//            if (GameController.instance.currentPickedTool != null)
//            {
//                if (GameController.instance.currentPickedTool.GetComponent<PaintBrushTool>())
//                {
//                    UIController.instance.paintCountContainer.SetActive(true);
//                }
//            }
//        }
//    }

//    private void OnTriggerStay(Collider other)
//    {
//        if (other.gameObject.tag == "Player")
//        {
//            RoomManager.instance.UpdateCurrentRoomNumber(roomId);
//            if (isTriggerExit)
//            {
//                isTriggerExit = false;

//                RoomManager.instance.UpdateCurrentRoomNumber(roomId);
//                if (RoomManager.instance.rooms[roomId].AreAllTrashTasksComplete())
//                {
//                    RoomManager.instance.rooms[roomId].inactiveCountofDustbin = RoomManager.instance.rooms[roomId].totalDustBinTrash;
//                }
//                //UIController.instance.SetRoomProgress(true, RoomManager.instance.rooms[roomId].inactiveCountofDustbin, RoomManager.instance.rooms[roomId].totalDustBinTrash);
//                RoomManager.instance.rooms[roomId].DonePaintCountToShowOnUI();
//                RoomManager.instance.rooms[roomId].CheckRoomProgress(false);
//                RoomManager.instance.rooms[roomId].UpdateRoomReflection();
//                if (GameController.instance.currentPicketItem != null)
//                {
//                    if (!RoomManager.instance.rooms[roomId].AreAllTrashTasksComplete())
//                    {
//                        UIController.instance.DisplayInstructions("Clean all trash");
//                    }
//                }

//                if (GameController.instance.currentPickedTool != null)
//                {
//                    if (GameController.instance.currentPickedTool.GetComponent<PaintBrushTool>())
//                    {
//                        UIController.instance.paintCountContainer.SetActive(true);
//                    }
//                }

//            }
//        }
        

//    }
//    public void OnTriggerExit(Collider other)
//    {
       
//        if (other.gameObject.tag == "Player")
//        {
//            RoomManager.instance.UpdateCurrentRoomNumber(-1);

//            UIController.instance.UpdateGameProgressText(false);
//            //            UIController.instance.paintCountContainer.SetActive(false);
//            isTriggerExit = true;
//        }
//    }

//}
