//using LitJson;
//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEngine;
//using UnityEngine.UI;
//using static CustomerMovement;
//public class ReceptionUIManager : MonoBehaviour
//{
//   public static ReceptionUIManager Instance;
//   public List<RoomssManagementInterface> roomsManagementInterfaceList = new List<RoomssManagementInterface>();
//   public Image ReceptionRoomManagerPanel;
//   public GameObject roomDataPrefab;
//   public GameObject dataParent;
//    public AudioClip btnSound;
//   Room assignedRoom;
//   CustomerMovement customerAtCounter;
//    public GameObject SpawndCustomersParent;
//    void Awake()
//    {
//        Instance = this;
//    }

//    public Sprite GetRoomButtonImage(int _roomState)
//    {
//        return roomsManagementInterfaceList[_roomState].roomStateImage;
//    }

//    public Color GetRoomButtonTextColor(int _roomState)
//    {
//        return roomsManagementInterfaceList[_roomState].textColor;
//    }

//    public void EnableReceptionRoomManagementPanel()
//    {
//        ReceptionRoomManagerPanel.gameObject.SetActive(true);

////        StartCoroutine(DestroyDataAndPapulateNew());
//    }
//    public void DisableReceptionRoomManagementPanel()
//    {
//        ReceptionRoomManagerPanel.gameObject.SetActive(false);
//    }
//    public void SetCustomerAtCounter(CustomerMovement customer)
//    {

//    }

//    IEnumerator DestroyDataAndPapulateNew()
//    {
//        yield return null;
//        foreach (Transform child in dataParent.transform)
//        {
//            Destroy(child.gameObject);
//        }
//        yield return null;
//    }
//    public IEnumerator InitializeRoomManagement()
//    {
//        for (int i = 0; i < RoomManager.instance.rooms.Count; i++)
//        {
//            GameObject _prefabData = Instantiate(roomDataPrefab, dataParent.transform);

//            int k = i;
//            _prefabData.GetComponent<Button>().onClick.AddListener(() => OnSelectRoom(k));
//            _prefabData.GetComponent<Image>().sprite = GetRoomButtonImage(RoomManager.instance.rooms[i].roomSaveable.roomStatus);
//            _prefabData.transform.GetChild(0).gameObject.GetComponent<Text>().text = RoomManager.instance.rooms[i].roomProperties.roomDisplayNumber.ToString();
//            if (RoomManager.instance.rooms[k].roomSaveable.roomStatus == 0)
//            {
//                _prefabData.transform.GetChild(1).gameObject.SetActive(false);
//                _prefabData.transform.GetChild(2).gameObject.SetActive(true);
//            }
//            else
//            {
//                _prefabData.transform.GetChild(1).gameObject.SetActive(true);
//                _prefabData.transform.GetChild(2).gameObject.SetActive(false);

//                int roomRent = RoomManager.instance.rooms[k].GetRoomTotalExpense();
//                roomRent = (int)(roomRent * 0.05f);
//                _prefabData.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = "$" + roomRent;
//            }
//            if (i % 5 == 4)
//            {
//                yield return null;
//            }
//        }
//    }
//    public void OnUpdateLevel()
//    {
//        for (int i = 0; i < RoomManager.instance.rooms.Count; i++)
//        {
//            dataParent.transform.GetChild(i).GetComponent<Image>().sprite = GetRoomButtonImage(RoomManager.instance.rooms[i].roomSaveable.roomStatus);

//            if (RoomManager.instance.rooms[i].roomSaveable.roomStatus > 0)
//            {
//                dataParent.transform.GetChild(i).gameObject.transform.GetChild(1).gameObject.SetActive(true);
//                dataParent.transform.GetChild(i).transform.GetChild(2).gameObject.SetActive(false);

//                int roomRent = RoomManager.instance.rooms[i].GetRoomTotalExpense();
//                roomRent = (int)(roomRent * 0.05f);
//                dataParent.transform.GetChild(i).transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = "$" + roomRent;
//            }
//        }
//    }

//    void OnSelectRoom(int k)
//    {
//        // Retrieve the selected room based on the index
//        SoundController.instance.OnPlayInteractionSound(btnSound);
//        assignedRoom = RoomManager.instance.rooms[k];

//        if (assignedRoom != null)
//        {
//            // Display appropriate message based on the room's status
//            switch (assignedRoom.roomSaveable.roomStatus)
//            {
//                case 0:
//                    UIController.instance.DisplayInstructions("Room is Locked");
//                    break;

//                case 1:
//                    UIController.instance.DisplayInstructions("Room Not Purchased");
//                    break;

//                case 2:
//                    UIController.instance.DisplayInstructions("Room is not Ready");
//                    break;

//                case 4:
//                    UIController.instance.DisplayInstructions("Room is Dirty");
//                    break;

//                case 5:
//                    UIController.instance.DisplayInstructions("Room is Occupied");
//                    break;

//                default:
//                    // Check if there is a customer at the counter to assign the room
                    
//                    customerAtCounter = null;
//                    GameObject _cus = CustomerManager.instance.GetCustomerAtCounter();
//                    if (_cus != null)
//                    {
//                        customerAtCounter = _cus.GetComponent<CustomerMovement>();
//                    }
//                    if (customerAtCounter != null)
//                    {
//                        customerAtCounter.AssignRoomFromReception(assignedRoom);
//                        //UpdateRoomImageAndState(5, assignedRoom.roomProperties.roomNumber);
//                    }
//                    else
//                    {
//                        UIController.instance.DisplayInstructions("No customer available!");
//                    }
//                    break;
//            }
//        }
//        else
//        {
//            UIController.instance.DisplayInstructions("Room Not Available");
//        }
//    }

//    /// <summary>
//    /// When room state changed from somewhere call this method
//    /// </summary>
//    /// <param name="_state"></param>
//    /// <param name="_roomId"></param>
//    public void UpdateRoomImageAndState(int _state,int _roomId)
//    {
//        dataParent.transform.GetChild(_roomId).GetComponent<Image>().sprite= GetRoomButtonImage(_state);

//        if (RoomManager.instance.rooms[_roomId].roomSaveable.roomStatus>0)
//        {
//            dataParent.transform.GetChild(_roomId).gameObject.transform.GetChild(1).gameObject.SetActive(true);
//            dataParent.transform.GetChild(_roomId).transform.GetChild(2).gameObject.SetActive(false);

//            int roomRent = RoomManager.instance.rooms[_roomId].GetRoomTotalExpense();
//            roomRent = (int)(roomRent * 0.05f);
//            dataParent.transform.GetChild(_roomId).transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = "$"+ roomRent;
//        }
//        else
//        {
//            dataParent.transform.GetChild(_roomId).gameObject.transform.GetChild(1).gameObject.SetActive(false);
//            dataParent.transform.GetChild(_roomId).transform.GetChild(2).gameObject.SetActive(true);
//        }
//    }

//}

//[System.Serializable]
//public class RoomssManagementInterface
//{
//    public Sprite roomStateImage;
//    public Color textColor;

//}
