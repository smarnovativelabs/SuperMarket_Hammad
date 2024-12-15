//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using static Room;

//public class RoomManager : MonoBehaviour
//{
//    public static RoomManager instance;
//    public AudioClip roomRentSound;
//    public int currentRoomNumber=-1;
//    public Transform[] stairsPoints;
//    public List<Room> rooms;
  

//    bool isDataInitialized = false;
//    private void Awake()
//    {
//        if (instance == null)
//        {
//            instance = this;
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//        //   LoadTankPartProperties();

//    }
//    private void Start()
//    {

//    }

//    void Update()
//    {

//    }
//    public void UpdateReceptionAreaProgress(bool _enable)
//    {
//        UIController.instance.UpdateGameProgressText(_enable, "Point and Tap on the Counter to Start Serving!");
//    }
//    /// <summary>
//    /// Setting palced item id,indicator id against roomid
//    /// </summary>
//    /// <param name="_roomId"></param>
//    /// <param name="_indicatorId"></param>
//    /// <param name="_itemId"></param>
//    public void SetPlacedItemId(int _roomId, int _indicatorId, int _itemId)
//    {
//    }
//    public void SetWallItemId(int _roomId, int wallId, int _itemId)
//    {
//        rooms[_roomId].SetWallItemId(wallId, _itemId);
//    }
//    public void SetCeilingItemId(int _roomId, int ceilingId, int _itemId)
//    {
//        rooms[_roomId].SetCeilingItemId(ceilingId, _itemId);
//    }
//    public void SetFloorItemId(int _roomId, int FloorId, int _itemId)
//    {
//        rooms[_roomId].SetFloorItemId(FloorId, _itemId);
//    }

//    /// <summary>
//    /// Initilize Rooms props here from the file
//    /// </summary>
//    /// <returns></returns>
//    public IEnumerator InitlizeLoadingRoomProperties()
//    {
//        for (int i = 0; i < rooms.Count; i++)
//        {
//            int _temp = i;
//            rooms[_temp].roomSaveable = (RoomSaveable)SerializationManager.LoadFile("_RoomData_" + _temp);
//            if (rooms[_temp].roomSaveable == null)
//            {
//                rooms[_temp].InitializeRoomData();
//            }
            
//            rooms[_temp].LoadSavedData(_temp);
//            yield return null;
//        }
//        isDataInitialized = true;
//        GameController.instance.AddSavingAction(SaveData);
//    }
//    public void EnablePlacableItemIndicater(CategoryName _mainCat, int _subCatId, bool _enable = true)
//    {
//        if (currentRoomNumber < 0 || currentRoomNumber >= rooms.Count)
//        {
//            return;
//        }
//    }
//    public void OnRemoveItemFromRoom(int _roomIndex, int _indicaterIndex)
//    {
//    }
//    public Vector3 GetClosestStairsPoint(Vector3 _position)
//    {
//        Vector3 _point = stairsPoints[0].position;
//        float _distance = Vector3.Distance(_point, _position);
//        for(int i = 1; i < stairsPoints.Length; i++)
//        {
//            if (Vector3.Distance(_position, stairsPoints[i].position) < _distance)
//            {
//                _distance = Vector3.Distance(_position, stairsPoints[i].position);
//                _point = stairsPoints[i].position;
//            }
//        }
//        return _point;
//    }
//    public Room GetEmptyRoom()
//    {
//        int _avialableRoomIndex = -1;
//        for(int i=0; i<rooms.Count; i++)
//        {
//            if (rooms[i].roomSaveable.roomStatus==((int)(Room.RoomStatus.Ready)))
//            {
//                _avialableRoomIndex = i;
//                break;
//            }
//        }
//        if(_avialableRoomIndex > -1)
//        {
//            return rooms[_avialableRoomIndex];
//        }
//        else
//        {
//            return null;
//        }
//    }
//    public bool OnPurchaseRoom(int _index)
//    {
//        if(_index<0 || _index > rooms.Count)
//        {
//            print("Invalid Room Id Passed");
//            return false;
//        }
//        return (rooms[_index].OnPurchaseRoom());
//    }
//    public void OnOpenDoor(int _index)
//    {
//        if (_index < 0 || _index > rooms.Count)
//        {
//            print("Invalid Room Id Passed");
//            return;
//        }
//        rooms[_index].OnOpenDoor();
//    }
//    public bool IsAnyRoomReady()
//    {
//        //return true;
//        bool _isRoomReady = false;

//        for (int i = 0; i < rooms.Count; i++)
//        {
//            if (rooms[i].roomSaveable.roomStatus > (int)(Room.RoomStatus.NotReady))
//            {
//                _isRoomReady = true;
//                break;
//            }
//        }
//        return _isRoomReady;
//    }
//    public void OnLevelUpdate()
//    {
//        for(int i = 0; i < rooms.Count; i++)
//        {
//            rooms[i].SetRoomLockStatus();
//        }
//    }
//    public void SaveData()
//    {
//        if (!isDataInitialized)
//            return;
//        for (int i = 0; i < rooms.Count; i++)
//        {
//            RoomSaveable _room = rooms[i].roomSaveable;
//            SerializationManager.Save(_room, "_RoomData_" + i.ToString());
//        }
//    }
//    /// <summary>
//    /// Cleaner will get dirty room if player is
//    /// not in room and current room is not being clened
//    /// and room status is dirty
//    /// </summary>
//    /// <returns></returns>
//    public Room GetDirtyRoom()
//    {
//        Room _room = null;
//        for (int i = 0; i < rooms.Count; i++)
//        {
//            if (rooms[i].roomProperties.beingCleaned) continue;
//            if ((rooms[i].roomSaveable.roomStatus == (int)RoomStatus.Dirty) && (rooms[i].roomProperties.roomNumber!= currentRoomNumber) )
//            {
//                _room = rooms[i];
//                break;
//            }
//        }
//        return _room;
//    }

//    public bool IsRoomBeingCleaned(int _roomId)
//    {
//        bool isCleaned = false; 
      
       
//            if (rooms[_roomId].roomProperties.beingCleaned)
//            {
//                isCleaned=true;
              
//            }
          
        
//        return isCleaned;
//    }

//    //private void OnApplicationPause(bool pause)
//    //{
//    //    if (pause)
//    //    {
//    //        SaveData();
//    //    }
//    //}

//    //private void OnApplicationQuit()
//    //{

//    //    SaveData();
//    //}
//    //private void OnDestroy()
//    //{
//    //    SaveData();
//    //}

//    public void UpdateCurrentRoomNumber(int _index)
//    {
//        currentRoomNumber = _index;
//    }
//}
