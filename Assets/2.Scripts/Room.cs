//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System.Linq;
//using TMPro;
//using UnityEngine.UI;

//public class Room : MonoBehaviour
//{
//    public RoomProperties roomProperties;
//    public ReflectionProbe roomReflection;
//    public GameObject Door;
//    public GameObject playerLockCollider;
//    public GameObject currentCustomer;
//    public int totalDustBinTrash;
//    public int totalMopTrash;
//    public int totalWall;
//    public int totalFloor;
//    public int totalCeiling;
//    public int inactiveCountofDustbin;
//    public int inactiveCountofMopTrash;
//    public int inactiveCountofWall;
//    public int inactiveCountofCeiling;
//    public int inactiveCountofFloor;
//    public int dirtyTrashCount;
//    public int dirtyDustCount;
//    [Header("Bools")]
//    /*public bool allTaskComplete;
//    public bool isDoorWoodClear;
//    public bool isMopTrashClear;*/
//    //public bool isDustBinTrashClear;
//    bool gameStarted;
//    bool cleanXpsGiven;
//    public RoomSaveable roomSaveable;
//    int firebaseWoodQuarter = 0;
//    int firebaseTrashQuarter = 0;
//    int firebaseDustQuarter = 0;

//    int firebaseFloorQuarter = 0;
//    int firebaseCeilingQuarter = 0;
//    int firebaseWallQuarter = 0;
//    bool startOccupiedTimer;
  
//    public enum RoomStatus
//    {
//        Locked=0,
//        NotPurchased=1,
//        NotReady=2,
//        Ready=3,
//        Dirty=4,
//        Occopied=5
//    }
//    private void Start()
//    {
//        playerLockCollider.SetActive(false);
//        roomProperties.mustPlacedItemsCount = roomProperties.roomPlacedItems.Count;
//        totalDustBinTrash = roomProperties.dustbinTrashParent.transform.childCount;
//        totalMopTrash = roomProperties.mopTrashParent.transform.childCount;
//        totalWall = roomProperties.wall.transform.childCount;
//        totalFloor = roomProperties.floor.transform.childCount;
//        totalCeiling = roomProperties.ceiling.transform.childCount;
//        roomProperties.dirtyTrashParent.SetActive(false);
//        roomProperties.dustbinTrashParent.SetActive(false);
//        roomProperties.cleaningTag.SetActive(false);
//        //inactiveCountofFloor = PaintCount();
//        //CheckTrashBoolsInStart();
//    }
//    public bool AreAllTrashTasksComplete()
//    {
//        return roomSaveable.isMopTrashCleaned && roomSaveable.isDustBinTrashCleaned;
//    }

//    public void OnChangeGameStatus(bool _started)
//    {
//        gameStarted = _started;
//    }

//    #region Room Locking Implementation
//    public void SetRoomLockStatus()
//    {
//        roomProperties.doorWoodLockParent.SetActive(!(roomSaveable.roomStatus > 1));

//        bool _isLocked = roomProperties.reqLevel > PlayerDataManager.instance.playerData.playerLevel;
//        roomProperties.doorLockBtnImage.GetComponent<Image>().sprite = _isLocked ? UIController.instance.lock3dBtnImg : UIController.instance.unPurchase3dBtnImg;
//        roomProperties.doorLockPriceText.GetComponent<TextMeshProUGUI>().color = _isLocked ? UIController.instance.lock3dBtnTxtClr : UIController.instance.unPurchasedBtnTxtClr;
//        roomProperties.doorLockPriceText.GetComponent<TextMeshProUGUI>().text = _isLocked ? "Level " + roomProperties.reqLevel.ToString() : "$" + roomProperties.roomPurchasingPrice.ToString();

//        if ((!_isLocked) && (roomSaveable.roomStatus < 1))
//        {
//            roomSaveable.roomStatus = 1;
//            ReceptionUIManager.Instance.UpdateRoomImageAndState(1,roomProperties.roomNumber);
//        }
//    }

   
//    public bool OnPurchaseRoom()
//    {
//        if (roomSaveable.roomStatus>1)
//        {
//            return false;
//        }
//        if (roomProperties.reqLevel > PlayerDataManager.instance.playerData.playerLevel)
//        {
//            GameManager.instance.CallFireBase("RmPurUnlkBfLev");

//            UIController.instance.EnablePopupNotification("This Room Will Unlock At Level " + roomProperties.reqLevel.ToString());
//            return false;
//        }
//        if (roomProperties.roomPurchasingPrice> PlayerDataManager.instance.playerData.playerCash)
//        {
//            GameManager.instance.CallFireBase("NoCshRoom_" + roomProperties.roomNumber.ToString());

//            UIController.instance.EnableNoCashPanel();
//            return false;
//        }
//        //seting room state to not ready when it is purchased
//        roomSaveable.roomStatus = 2;
//        ReceptionUIManager.Instance.UpdateRoomImageAndState(roomSaveable.roomStatus, roomProperties.roomNumber);

//        SetRoomLockStatus();
//        Door.GetComponent<DoorOpen>().enabled = true;
//        Door.GetComponent<DoorOpen>().isLock = false;

//        PlayerDataManager.instance.UpdateCash(-1 * roomProperties.roomPurchasingPrice);
//        UIController.instance.UpdateCurrency(-1 * roomProperties.roomPurchasingPrice);
//        //PlayerDataManager.instance.UpdateXP(roomProperties.roomPurchasingPrice / 2);
//        //UIController.instance.UpdateXP(roomProperties.roomPurchasingPrice / 2);
//        UIController.instance.DisplayInstructions("Room Unlocked!");
//        return false;
//    }
//    public void OnOpenDoor()
//    {
//        Door.GetComponent<DoorOpen>().InteractWithDoor(true);
//    }
//    #endregion
//    #region Loading/Initializing Data
//    public void InitializeRoomData()
//    {
//        roomSaveable = new RoomSaveable(roomProperties.mustPlacedItemsCount, roomProperties.ceiling.transform.childCount,
//            roomProperties.floor.transform.childCount, roomProperties.wall.transform.childCount, 0);
//        if (roomProperties.roomNumber == 0)
//        {
//            roomSaveable.roomStatus = 2;
//        }

//        if (roomProperties.roomNumber == 1 || roomProperties.roomNumber == 2)
//        {
//            roomSaveable.roomStatus = 1;
//        }
//    }
//    public void LoadSavedData(int _roomIndex)
//    {
//        /*isDoorWoodClear = roomSaveable.isDoorWoodCleaned;
//        isDustBinTrashClear = roomSaveable.isDustBinTrashCleaned;
//        isMopTrashClear = roomSaveable.isMopTrashCleaned;*/
//        GameController.instance.changeGameStatus += OnChangeGameStatus;
//        SetRoomLockStatus();
//        roomSaveable.UpdateCeilingTextureList(roomProperties.ceiling.transform.childCount);
//        roomSaveable.UpdateFloorTextureList(roomProperties.floor.transform.childCount);
//        roomSaveable.UpdateWallTextureList(roomProperties.wall.transform.childCount);

//        CheckTrashBoolsInStart();

//        LoadSaveLoopForFloor(_roomIndex);
//        LoadSaveLoopForWalls(_roomIndex);
//        LoadSaveLoopForCeiling(_roomIndex);
//        Door.GetComponent<DoorOpen>().enabled = roomSaveable.roomStatus>1;
       
//        if (roomSaveable.customerIndex>=0 && roomSaveable.vehicleIndex>=0 && roomSaveable.parkingIndex>=0 && roomSaveable.remOccupiedTime >= 0f)
//        {
//            currentCustomer = CustomerManager.instance.SpawnSavedCustomer(roomSaveable.customerIndex, roomSaveable.vehicleIndex, roomSaveable.parkingIndex, roomProperties.isOnTopFloor,
//                roomProperties.customerRoamPoints[0], roomProperties.customerRoamPoints);
//            Door.GetComponent<DoorOpen>().isLock = true;
//            startOccupiedTimer = true;
//        }
//        EnableRoomObjects(false);
//        UpdateDirtyRoomAccordingState();
//    }
//    void LoadSaveLoopForFloor(int _roomIndex)
//    {
//        for (int i = 0; i < roomProperties.floor.transform.childCount; i++)
//        {
//            int _temp = i;
//            roomProperties.floor.transform.GetChild(i).GetComponent<ItemPickandPlace>().SetIndexes(_roomIndex, _temp);
//            roomProperties.floor.transform.GetChild(i).GetComponent<ItemPickandPlace>().itemId = roomSaveable.floorTextureIds[i];

//            if (roomSaveable.floorTextureIds[i] >= 0)
//            {
//                ItemData _item = GameManager.instance.GetItem(roomProperties.floor.transform.GetChild(i).GetComponent<ItemPickandPlace>().mainCat,
//                    roomProperties.floor.transform.GetChild(i).GetComponent<ItemPickandPlace>().SubCatId, roomSaveable.floorTextureIds[i]);
//                if (_item != null)
//                {
//                    roomProperties.floor.transform.GetChild(i).GetComponent<ItemPickandPlace>().itemId = _item.itemID;
//                    roomProperties.floor.transform.GetChild(i).GetComponent<ItemPickandPlace>().OnSpawnItem(_item);
//                }
//            }
//        }
//    }
//    void LoadSaveLoopForWalls(int _roomIndex)
//    {
//        for (int i = 0; i < roomProperties.wall.transform.childCount; i++)
//        {
//            int _temp = i;
//            roomProperties.wall.transform.GetChild(i).GetComponent<ItemPickandPlace>().SetIndexes(_roomIndex, _temp);
//            roomProperties.wall.transform.GetChild(i).GetComponent<ItemPickandPlace>().itemId = roomSaveable.wallTextureIds[i];

//            if (roomSaveable.wallTextureIds[i] >= 0)
//            {
//                ItemData _item = GameManager.instance.GetItem(roomProperties.wall.transform.GetChild(i).GetComponent<ItemPickandPlace>().mainCat,
//                    roomProperties.wall.transform.GetChild(i).GetComponent<ItemPickandPlace>().SubCatId, roomSaveable.wallTextureIds[i]);
//                if (_item != null)
//                {
//                    roomProperties.wall.transform.GetChild(i).GetComponent<ItemPickandPlace>().OnSpawnItem(_item);
//                    roomProperties.wall.transform.GetChild(i).GetComponent<ItemPickandPlace>().itemId = _item.itemID;
//                }
//            }
//        }
//    }
//    void LoadSaveLoopForCeiling(int _roomIndex)
//    {
//        for (int i = 0; i < roomProperties.ceiling.transform.childCount; i++)
//        {
//            int _temp = i;
//            roomProperties.ceiling.transform.GetChild(i).GetComponent<ItemPickandPlace>().SetIndexes(_roomIndex, _temp);
//            roomProperties.ceiling.transform.GetChild(i).GetComponent<ItemPickandPlace>().itemId = roomSaveable.ceilingTextureIds[i];

//            if (roomSaveable.ceilingTextureIds[i] >= 0)
//            {
//                ItemData _item = GameManager.instance.GetItem(roomProperties.ceiling.transform.GetChild(i).GetComponent<ItemPickandPlace>().mainCat,
//                    roomProperties.ceiling.transform.GetChild(i).GetComponent<ItemPickandPlace>().SubCatId, roomSaveable.ceilingTextureIds[i]);
//                if (_item != null)
//                {
//                    roomProperties.ceiling.transform.GetChild(i).GetComponent<ItemPickandPlace>().OnSpawnItem(_item);
//                    roomProperties.ceiling.transform.GetChild(i).GetComponent<ItemPickandPlace>().itemId = _item.itemID;

//                }
//            }
//        }
//    }
//    public void CheckTrashBoolsInStart()
//    {
//        TurnOnOrOffTrash(roomSaveable.isDustBinTrashCleaned, roomProperties.dustbinTrashParent);
//        TurnOnOrOffTrash(roomSaveable.isMopTrashCleaned, roomProperties.mopTrashParent);
//        cleanXpsGiven = (roomSaveable.isDustBinTrashCleaned && roomSaveable.isMopTrashCleaned);
//        //isDoorWoodClear = roomSaveable.isDoorWoodCleaned;
//    }
//    void TurnOnOrOffTrash(bool _trash, GameObject trashParent)
//    {
//        if (_trash)
//        {
//            //trashParent.SetActive(false);
//            for (int i = 0; i < trashParent.transform.childCount; i++)
//            {
//                trashParent.transform.GetChild(i).gameObject.SetActive(false);
//            }
//        }
//    }

//    #endregion
//    #region Optimization
//    public void UpdateRoomReflection()
//    {
//        if (roomReflection != null)
//        {
//            print("Calling To Update Reflection...........");
//            //roomReflection.RenderProbe();
//        }
//    }
//    public void EnableRoomObjects(bool _enable)
//    {
//        roomProperties.wall.SetActive(_enable);
//        roomProperties.ceiling.SetActive(_enable);
//     //   roomProperties.floor.SetActive(_enable);
//        roomProperties.dustbinTrashParent.SetActive(_enable);
//        roomProperties.mopTrashParent.SetActive(_enable);
//        roomProperties.placedItemParent.SetActive(_enable);
//        roomProperties.dirtyTrashParent.SetActive(_enable);

//    }
//    #endregion
//    #region Room Paint Flags
//    public void DonePaintCountToShowOnUI()
//    {
//        inactiveCountofWall = PaintCount(roomSaveable.wallTextureIds);
//        inactiveCountofCeiling = PaintCount(roomSaveable.ceilingTextureIds);
//        inactiveCountofFloor = PaintCount(roomSaveable.floorTextureIds);
//    }
    
//    public int PaintCount(List<int> textureIds)
//    {
//        int paintedTiles = 0;
//        for (int i = 0; i < textureIds.Count; i++)
//        {
//            if (textureIds[i] >= 0)
//            {
//                paintedTiles++;
//            }
//        }
//        return paintedTiles; 
//    }

//    public bool AreAllFloorPainted()
//    {
//        for (int i = 0; i < roomProperties.floor.transform.childCount; i++)
//        {
//            if (i >= roomSaveable.floorTextureIds.Count)
//            {
//                return false;
//            }
//            if (roomSaveable.floorTextureIds[i] < 0)
//            {
//                return false;
//            }
//        }
//        return true; 
//    }

//    public bool AreAllCeilingPainted()
//    {
//        for (int i = 0; i < roomProperties.ceiling.transform.childCount; i++)
//        {
//            if (i >= roomSaveable.ceilingTextureIds.Count)
//            {
//                return false;
//            }
//            if (roomSaveable.ceilingTextureIds[i] < 0)
//            {
//                return false; 
//            }
//        }
//        return true; 
//    }
//    public bool AreAllWallPainted()
//    {
//        for (int i = 0; i < roomProperties.wall.transform.childCount; i++)
//        {
//            if (i >= roomSaveable.wallTextureIds.Count)
//            {
//                return false;
//            }
//            if (roomSaveable.wallTextureIds[i] < 0)
//            {
//                return false; 
//            }
//        }
//        return true; 
//    }
//    #endregion
//    #region Room Items Manipulation
//    public void OnPlaceItemInRoom(CategoryName _catName, int _subCatId, int _itemId)
//    {
//        if (roomProperties.roomPlacedItems != null)
//        {
//            for(int i = 0; i < roomProperties.roomPlacedItems.Count; i++)
//            {
//                if (roomProperties.roomPlacedItems[i].mainCat==_catName && roomProperties.roomPlacedItems[i].subCatId==_subCatId
//                    && roomProperties.roomPlacedItems[i].itemId < 0)
//                {
//                    roomProperties.roomPlacedItems[i].itemId = _itemId;
//                    return;
//                }
//            }
//        }
//        else
//        {
//            roomProperties.roomPlacedItems = new List<DynamicPlacedItems>();
//        }

//        DynamicPlacedItems _item = new DynamicPlacedItems();
//        _item.mainCat = _catName;
//        _item.subCatId = _subCatId;
//        _item.itemId = _itemId;
//        roomProperties.roomPlacedItems.Add(_item);
//    }
//    public void OnRemoveItem(CategoryName _catName, int _subCatId, int _itemId)
//    {
//        if (roomProperties.roomPlacedItems == null)
//            return;
//        int _index = -1;
//        for (int i = roomProperties.roomPlacedItems.Count - 1; i > -1; i--)
//        {
//            if (roomProperties.roomPlacedItems[i].mainCat == _catName && roomProperties.roomPlacedItems[i].subCatId == _subCatId
//                    && roomProperties.roomPlacedItems[i].itemId ==_itemId)
//            {
//                _index = i;
//                break;
//            }
//        }
//        if (_index < roomProperties.mustPlacedItemsCount)
//        {
//            roomProperties.roomPlacedItems[_index].itemId = -1;
//        }
//        else
//        {
//            roomProperties.roomPlacedItems.RemoveAt(_index);
//        }
//        CheckRoomProgress();
//    }
//    public void SetWallItemId(int _indicaterId, int _itemId)
//    {
//        roomSaveable.wallTextureIds[_indicaterId] = _itemId;
//    }
//    public void SetCeilingItemId(int _indicaterId, int _itemId)
//    {
//        roomSaveable.ceilingTextureIds[_indicaterId] = _itemId;
//    }
//    public void SetFloorItemId(int _indicaterId, int _itemId)
//    {
//        roomSaveable.floorTextureIds[_indicaterId] = _itemId;
//    }
//    #endregion
//    #region Room Status Updations
//    public int AnyRemainingItemToPlace()
//    {
//        for(int i = 0; i < roomProperties.roomPlacedItems.Count; i++)
//        {
//            if (roomProperties.roomPlacedItems[i].itemId < 0)
//            {
//                return i;
//            }
//        }
//        return -1;
//    }
//    public void CustomerEntersRoom()
//    {
//        Door.GetComponent<DoorOpen>().isLock = true;

//        Door.GetComponent<DoorOpen>().InteractWithDoor(false);
//    }

//    public void CustomerLeaveRoom()
//    {
//        Door.GetComponent<DoorOpen>().isLock = false;

//        Door.GetComponent<DoorOpen>().InteractWithDoor(true);
//    }

//    public bool IsRoomReady()
//    {
//        for (int i=0; i< roomProperties.roomPlacedItems.Count; i++)
//        {
//            if (roomProperties.roomPlacedItems[i].itemId<0)
//            {
//                roomSaveable.isRoomReady = false;
//                return false;
//            }
//        }
//        UIController.instance.DisplayInstructions("Room is Ready");
//        TutorialManager.instance.OnCompleteTutorialTask(13);
//        roomSaveable.isRoomReady = true;
//        GameManager.instance.CallFireBase("Room_" + RoomManager.instance.currentRoomNumber.ToString() + "_Rdy", "ready", 1);
//        return true;
//    }

//    public void CheckRoomProgress(bool _isPreparingRoom=true)
//    {
//        int _roomStatus = (int)RoomStatus.NotReady;
//        if(!roomSaveable.isDustBinTrashCleaned)
//        {
//            UIController.instance.SetRoomProgress("Dustbin Trash ",true, inactiveCountofDustbin, totalDustBinTrash);
//        }
//        else if (!roomSaveable.isMopTrashCleaned)
//        {
//            UIController.instance.SetRoomProgress("Room Dust ", true, inactiveCountofMopTrash, totalMopTrash);
//        }
//        else if(!AreAllWallPainted())
//        {
//            UIController.instance.SetRoomProgress("Room Wall Paint ", true, inactiveCountofWall, totalWall);
//        }
//        else if (!AreAllFloorPainted())
//        {
//            UIController.instance.SetRoomProgress("Room Floor Paint ", true, inactiveCountofFloor, totalFloor);
//        }
//        else if(!AreAllCeilingPainted())
//        {
//            UIController.instance.SetRoomProgress("Room Ceiling Paint ", true, inactiveCountofCeiling, totalCeiling);
//        }
//        else if (AnyRemainingItemToPlace()>=0)
//        {
//            int _index = AnyRemainingItemToPlace();
//            DynamicPlacedItems _dynamic = roomProperties.roomPlacedItems[_index];
//            string _itemSubCatName = GameManager.instance.categoriesUIData[(int)_dynamic.mainCat].subCategoriesUIData[_dynamic.subCatId].subName;
//            UIController.instance.UpdateGameProgressText(true, "Now Place " + _itemSubCatName);
//            //UIController.instance.SetFurnitureProgress(true , _itemSubCatName);
//        }
//        else 
//        {
//            if(dirtyTrashCount>0 || dirtyDustCount > 0)
//            {
//                _roomStatus = 4;
//                UIController.instance.UpdateGameProgressText(true, "Clean Dust And Remove Trash");
//            }
//            else
//            {
//                _roomStatus = 3;
//                if (_isPreparingRoom)
//                {
//                    if (GameController.instance.gameData.motelOpenStatus)
//                    {
//                        UIController.instance.UpdateGameProgressText(true, "Serve This Room To Customers");
//                    }
//                    else
//                    {
//                        UIController.instance.UpdateGameProgressText(true,"Open Motel To Serve This Room To Customers");
//                    }
//                }
//                else
//                {
//                    UIController.instance.UpdateGameProgressText(false);
//                }
//            }            
//        }
//        if(roomSaveable.roomStatus>(int)RoomStatus.NotPurchased && roomSaveable.roomStatus < (int)RoomStatus.Occopied)
//        {
//            roomSaveable.roomStatus = _roomStatus;
//            ReceptionUIManager.Instance.UpdateRoomImageAndState(_roomStatus, roomProperties.roomNumber);
//        }
//        if(_roomStatus==3 && _isPreparingRoom)
//        {
//            UIController.instance.DisplayInstructions("Room is Ready");
//            GameManager.instance.CallFireBase("RoomRdy_" + roomProperties.roomDisplayNumber.ToString() );
//        }
//    }
//    #endregion

//    public void OpenGameStore()
//    {
//        int _tab = 0;
//        int _mainCat = -1;
//        int _subCat = -1;
//        if (roomSaveable.isDustBinTrashCleaned && roomSaveable.isMopTrashCleaned)
//        {
//            _tab = 1;
//            if (!AreAllWallPainted())
//            {
//                _mainCat = ((int)CategoryName.Paint);
//                _subCat = 0;
//            }
//            else if (!AreAllFloorPainted())
//            {
//                _mainCat = ((int)CategoryName.Paint);
//                _subCat = 1;
//            }
//            else if (!AreAllCeilingPainted())
//            {
//                _mainCat = ((int)CategoryName.Paint);
//                _subCat = 2;
//            }
//            else if (AnyRemainingItemToPlace() >=0)
//            {
//                int _index = AnyRemainingItemToPlace();
//                _mainCat = ((int)roomProperties.roomPlacedItems[_index].mainCat);
//                _subCat = roomProperties.roomPlacedItems[_index].subCatId;
//            }
//        }
//        UIController.instance.OpenGameStorePanel(_tab, _mainCat, _subCat);
//    }

//    #region Room Renting Implmentation
//    public Transform[] AssignCustomer(int _customerIndex, int _vehicleIndex, int _parkingIndex, GameObject _customer, float _stayTimeInDays = 1)
//    {
//        roomSaveable.customerIndex = _customerIndex;
//        roomSaveable.parkingIndex = _parkingIndex;
//        roomSaveable.vehicleIndex = _vehicleIndex;
//        roomSaveable.remOccupiedTime = ((GameController.instance.dayCycleMinutes * _stayTimeInDays) * 60f);
//        roomSaveable.roomStatus = (int)RoomStatus.Occopied;
//        ReceptionUIManager.Instance.UpdateRoomImageAndState((int)RoomStatus.Occopied, roomProperties.roomNumber);
//        currentCustomer = _customer;
//        playerLockCollider.SetActive(true);
//        int _roomRent = (int)(GetRoomTotalExpense() * 0.05f);
//        PlayerDataManager.instance.UpdateCash(_roomRent);
//        UIController.instance.DisplayInstructions(_roomRent + "$ Room Rent Collected");
//        SoundController.instance.OnPlayInteractionSound(RoomManager.instance.roomRentSound);

//        // UIController.instance.CashText.text = PlayerDataManager.instance.playerData.playerCash.ToString();
//        UIController.instance.UpdateCurrency(_roomRent);

//        return roomProperties.customerRoamPoints;
//    }
//    void Update()
//    {
//        //CheckIfTrashIsCleaned();
//        // Update allTaskComplete if all trashes are cleaned+
//        if (!gameStarted)
//            return;
//        if (roomSaveable.roomStatus==(int)RoomStatus.Occopied)
//            UpdateOccupiedTime();
//    }
//    void UpdateOccupiedTime()
//    {
//        if(!startOccupiedTimer)
//        {
//            return;
//        }
//        roomSaveable.remOccupiedTime -= Time.deltaTime;
//        if (roomSaveable.remOccupiedTime <= 0)
//        {
//            UpdateRoomStatus((int)RoomStatus.Dirty);
//            roomSaveable.customerIndex = -1;
//            roomSaveable.parkingIndex = -1;
//            roomSaveable.vehicleIndex = -1;
//            startOccupiedTimer = false;
//            if (currentCustomer != null)
//            {
//                currentCustomer.GetComponent<CustomerMovement>().OnLeavingArea(roomProperties.roomDisplayNumber);
//                playerLockCollider.SetActive(false);
//                Door.GetComponent<DoorOpen>().enabled = true;
//                Door.GetComponent<DoorOpen>().isLock = false;

//            }
//        }

//    }
//    public int GetRoomTotalExpense()
//    {
//        int _totalExpense = 0;

//        for (int i = 0; i < roomProperties.roomPlacedItems.Count; i++)
//        {
//            int _temp = i;
//            if (roomProperties.roomPlacedItems[i].itemId >= 0)
//            {
//                ItemData _item = GameManager.instance.GetItem(roomProperties.roomPlacedItems[i].mainCat,
//                    roomProperties.roomPlacedItems[i].subCatId, roomProperties.roomPlacedItems[i].itemId);
//                if (_item != null)
//                {
//                    _totalExpense += _item.itemPrice;
//                }
//            }
//        }
//        int _prevColorId = -1;
//        for (int i = 0; i < roomProperties.floor.transform.childCount; i++)
//        {
//            if (roomSaveable.floorTextureIds[i] >= 0)
//            {
//                if(_prevColorId!= roomSaveable.floorTextureIds[i])
//                {
//                    ItemData _item = GameManager.instance.GetItem(roomProperties.floor.transform.GetChild(i).GetComponent<ItemPickandPlace>().mainCat,
//                    roomProperties.floor.transform.GetChild(i).GetComponent<ItemPickandPlace>().SubCatId, roomSaveable.floorTextureIds[i]);
//                    if (_item != null)
//                    {
//                        _totalExpense += _item.itemPrice;
//                        _prevColorId = roomSaveable.floorTextureIds[i];

//                    }
//                }
//            }
//        }
//        _prevColorId = -1;
//        for (int i = 0; i < roomProperties.wall.transform.childCount; i++)
//        {
//            if (roomSaveable.wallTextureIds[i] >= 0)
//            {
//                if (_prevColorId != roomSaveable.wallTextureIds[i])
//                {
//                    ItemData _item = GameManager.instance.GetItem(roomProperties.wall.transform.GetChild(i).GetComponent<ItemPickandPlace>().mainCat,
//                    roomProperties.wall.transform.GetChild(i).GetComponent<ItemPickandPlace>().SubCatId, roomSaveable.wallTextureIds[i]);
//                    if (_item != null)
//                    {
//                        _totalExpense += _item.itemPrice;
//                        _prevColorId = roomSaveable.wallTextureIds[i];
//                    }
//                }
                
//            }
//        }
//        _prevColorId = -1;
//        for (int i = 0; i < roomProperties.ceiling.transform.childCount; i++)
//        {
//            if (roomSaveable.ceilingTextureIds[i] >= 0)
//            {
//                if (_prevColorId != roomSaveable.ceilingTextureIds[i])
//                {
//                    ItemData _item = GameManager.instance.GetItem(roomProperties.ceiling.transform.GetChild(i).GetComponent<ItemPickandPlace>().mainCat,
//                   roomProperties.ceiling.transform.GetChild(i).GetComponent<ItemPickandPlace>().SubCatId, roomSaveable.ceilingTextureIds[i]);
//                    if (_item != null)
//                    {
//                        _totalExpense += _item.itemPrice;
//                        _prevColorId = roomSaveable.ceilingTextureIds[i];
//                    }
//                }
//            }
//        }
//        return _totalExpense;
//    }

//    /// <summary>
//    /// Cleaner Specific
//    /// </summary>
//    public void MakeRoomReady()
//    {
//        roomSaveable.roomStatus = (int)RoomStatus.Ready;
//        roomProperties.dirtyTagParent.gameObject.SetActive(false);
//        playerLockCollider.SetActive(false);
//        roomProperties.cleaningTag.gameObject.SetActive(false);
//        foreach (Transform child in roomProperties.dirtyDustParent.transform)
//        {
//            child.gameObject.SetActive(false);
//        }

//        foreach (Transform child in roomProperties.dirtyTrashParent.transform)
//        {
//            child.transform.GetChild(0).gameObject.SetActive(false);
//        }
//        dirtyTrashCount = 0;
//        dirtyDustCount = 0;
//        roomProperties.beingCleaned = false;
//        ReceptionUIManager.Instance.UpdateRoomImageAndState((int)RoomStatus.Ready, roomProperties.roomNumber);
//        CustomerLeaveRoom();

//    }
//    #endregion
//    #region Initial Trash Implementation
//    public void CheckIfTrashIsCleaned()
//    {
//        roomSaveable.isMopTrashCleaned = CheckForCleaning(roomProperties.mopTrashParent);
//        roomSaveable.isDustBinTrashCleaned =  CheckForCleaning(roomProperties.dustbinTrashParent);
//        if (roomSaveable.isMopTrashCleaned && roomSaveable.isDustBinTrashCleaned)
//        {
//            TutorialManager.instance.OnCompleteTutorialTask(6);
//            if (!cleanXpsGiven)
//            {
//                PlayerDataManager.instance.UpdateXP(roomProperties.cleaningCompleteXps);
//                UIController.instance.UpdateXP(roomProperties.cleaningCompleteXps);
//                cleanXpsGiven = true;
//            }
            
//        }
//    }
//    bool CheckForCleaning(GameObject trashParent)
//    {
//        foreach (Transform child in trashParent.transform)
//        {
//            if (child.gameObject.activeSelf)
//            {
//                return false;
//            }
//        }
//        return true;
//    }
//    #endregion
//    #region Dirty Room Immplementation
//    public void UpdateRoomStatus(int _currentState)
//    {
//        roomSaveable.roomStatus = _currentState;
//        ReceptionUIManager.Instance.UpdateRoomImageAndState(_currentState, roomProperties.roomNumber);
//        UpdateDirtyRoomAccordingState();
//    }

//    /// <summary>
//    /// Tags are of two states dirty and occupied
//    /// </summary>
//    public void StartOccupiedTimer()
//    {
//        roomProperties.occupiedTag.gameObject.SetActive(roomSaveable.roomStatus == ((int)RoomStatus.Occopied));
//        roomProperties.dirtyTagParent.SetActive(roomSaveable.roomStatus == (int)(RoomStatus.Dirty));
     
//        startOccupiedTimer = true;
//    }

//    public void UpdateDirtyRoomAccordingState()
//    {
//        roomProperties.occupiedTag.gameObject.SetActive(roomSaveable.roomStatus == ((int)RoomStatus.Occopied));

//        bool _isRoomDirty = roomSaveable.roomStatus == (int)RoomStatus.Dirty;
//        roomProperties.dirtyTagParent.SetActive(_isRoomDirty);

//        //roomProperties.dirtyTrashParent.SetActive(_isRoomDirty);
//        // roomProperties.dirtyDustParent.SetActive(_isRoomDirty);
//        for (int i = 0; i < roomProperties.dirtyDustParent.transform.childCount; i++)
//        {
//            roomProperties.dirtyDustParent.transform.GetChild(i).gameObject.SetActive(_isRoomDirty);
//        }
//        for (int i = 0; i < roomProperties.dirtyTrashParent.transform.childCount; i++)
//        {
//            roomProperties.dirtyTrashParent.transform.GetChild(i).GetChild(0).gameObject.SetActive(_isRoomDirty);
//            roomProperties.dirtyTrashParent.transform.GetChild(i).GetChild(0).transform.localPosition = Vector3.zero;
//        }

//        if (_isRoomDirty)
//        {
//            dirtyTrashCount = roomProperties.dirtyTrashParent.transform.childCount;
//            dirtyDustCount = roomProperties.dirtyDustParent.transform.childCount;
//        }
//    }
//    public void OnDirtyRoomTrashRemove()
//    {
//        dirtyTrashCount--;
//        if (dirtyTrashCount <= 0 && dirtyDustCount<=0)
//        {
//            if (roomSaveable.roomStatus == ((int)RoomStatus.Dirty))
//            {
//                UpdateRoomStatus((int)RoomStatus.Ready);
//                CheckRoomProgress();
//            }
//        }
//    }
//    public void OnDirtyRoomDustRemove()
//    {
//        dirtyDustCount--;
//        if (dirtyTrashCount <= 0 && dirtyDustCount <= 0)
//        {
//            if (roomSaveable.roomStatus == ((int)RoomStatus.Dirty))
//            {
//                UpdateRoomStatus((int)RoomStatus.Ready);
//                CheckRoomProgress();
//            }
//        }
//    }
//    #endregion
//    #region Firbase Calls
//    public void OnRemoveWood(int _roomIndex)
//    {
//        GetQuarterEvents("Room_" + _roomIndex.ToString() + "_WoodRmv_", roomProperties.doorWoodLockParent.transform, ref firebaseWoodQuarter);
//    }
//    public void OnRemoveTrash(int _roomIndex)
//    {
//        GetQuarterEvents("Room_" + _roomIndex.ToString() + "_TrashRmv_", roomProperties.dustbinTrashParent.transform, ref firebaseTrashQuarter);
//    }
//    public void OnRemoveDust(int _roomIndex)
//    {
//        GetQuarterEvents("Room_" + _roomIndex.ToString() + "_DustRmv_", roomProperties.mopTrashParent.transform, ref firebaseDustQuarter);
//    }
//    void GetQuarterEvents(string _eventName,Transform _parent,ref int _prevQuarter)
//    {
//        int _removedCount = 0;
//        foreach (Transform child in _parent)
//        {
//            if (!child.gameObject.activeSelf)
//            {
//                _removedCount++;
//            }
//        }
//        int _total = _parent.childCount;

//        float _percent = ((float)_removedCount) / ((float)_total);
//        _percent *= 100f;
//        int _currenQuarter = (int)(_percent / 25);
//        if (_currenQuarter > _prevQuarter)
//        {
//            GameManager.instance.CallFireBase(_eventName + (_currenQuarter * 25).ToString(), "percent", _currenQuarter);
//            _prevQuarter = _currenQuarter;
//        }
//    }
//    public void OnFloorPaint(int _roomIndex)
//    {
//        GetQuarterPaintEvents("Room_" + _roomIndex.ToString() + "_FlrPnt_", roomSaveable.floorTextureIds, ref firebaseFloorQuarter);
//    }
//    public void OnCeilPaint(int _roomIndex)
//    {
//        GetQuarterPaintEvents("Room_" + _roomIndex.ToString() + "_ClnPnt_", roomSaveable.ceilingTextureIds, ref firebaseCeilingQuarter);
//    }
//    public void OnWallPaint(int _roomIndex)
//    {
//        GetQuarterPaintEvents("Room_" + _roomIndex.ToString() + "_WallPnt_", roomSaveable.wallTextureIds, ref firebaseWallQuarter);
//    }

//    void GetQuarterPaintEvents(string _eventName, List<int> _list, ref int _prevQuarter)
//    {
//        int _removedCount = PaintCount(_list);

//        int _total = _list.Count;

//        float _percent = ((float)_removedCount) / ((float)_total);
//        _percent *= 100f;
//        int _currenQuarter = (int)(_percent / 25);
//        if (_currenQuarter > _prevQuarter)
//        {
//            GameManager.instance.CallFireBase(_eventName + (_currenQuarter * 25).ToString(), "percent", _currenQuarter);
//            _prevQuarter = _currenQuarter;
//        }
//    }
//    #endregion
//    [System.Serializable]
//    public class RoomProperties
//    {
//        public int roomDisplayNumber;
//        public int roomNumber;
//        public int roomPrice;
//        public int roomPurchasingPrice;
//        public int reqLevel;
//        public int mustPlacedItemsCount = 10;
//        public bool isOnTopFloor;
//        public int cleaningCompleteXps;
//        public Sprite displayImg;
//        public GameObject floor;
//        public GameObject wall;
//        public GameObject ceiling;
//        public GameObject doorWoodLockParent;
//        public GameObject mopTrashParent;
//        public GameObject placedItemParent;
//        public GameObject dustbinTrashParent;
//        public GameObject doorLockBtnImage;
//        public GameObject doorLockPriceText;
//        public List<DynamicPlacedItems> roomPlacedItems;
//        public Transform[] customerRoamPoints;

//        public GameObject occupiedTag;
//        public GameObject dirtyTagParent;
//        public GameObject cleaningTag;
//        public GameObject dirtyTrashParent;
//        public GameObject dirtyDustParent;
//        public bool beingCleaned;

//    }
//}
//[System.Serializable]
//public class RoomSaveable
//{
//    public bool isDoorWoodCleaned;
//    public bool isMopTrashCleaned;
//    public bool isDustBinTrashCleaned;
//    public bool isRoomReady;
//    public int roomTime;
//    public bool isRoomOccupied;
//    public bool isRoomPurchased;
//    public int customerIndex;
//    public int vehicleIndex;
//    public int parkingIndex;
//    public int roomStatus;

//    public float remOccupiedTime;

//    public List<int> placedItemsIds;
//    public List<int> ceilingTextureIds;
//    public List<int> floorTextureIds;
//    public List<int> wallTextureIds;
//    public List<IndicatorPlacedItemsIdexes> indicatorPlacedItems;

//    public RoomSaveable(int _itemsCount, int _ceilingsCount, int _floorCount, int _wallCount, int _roomStatus)
//    {
//        placedItemsIds = new List<int>(_itemsCount);
//        ceilingTextureIds = new List<int>(_ceilingsCount);
//        floorTextureIds = new List<int>(_floorCount);
//        wallTextureIds = new List<int>(_wallCount);
//        customerIndex = -1;
//        vehicleIndex = -1;
//        parkingIndex = -1;
//        remOccupiedTime = -1;
//        roomStatus = _roomStatus;

//        for (int i = 0; i < _itemsCount; i++) placedItemsIds.Add(-1);
//        for (int i = 0; i < _ceilingsCount; i++) ceilingTextureIds.Add(-1);
//        for (int i = 0; i < _floorCount; i++) floorTextureIds.Add(-1);
//        for (int i = 0; i < _wallCount; i++) wallTextureIds.Add(-1);
//    }
//    public void UpdateWallTextureList(int _wallCount)
//    {
//        if (wallTextureIds.Count == _wallCount)
//            return;
//        if (wallTextureIds.Count < _wallCount)
//        {
//            int _loopCount = _wallCount - wallTextureIds.Count;
//            for(int i = 0; i < _loopCount; i++)
//            {
//                wallTextureIds.Add(-1);
//            }
//        }
//        else
//        {
//            while (wallTextureIds.Count > _wallCount)
//            {
//                int _index = wallTextureIds.Count - 1;
//                if (_index < 0)
//                    break;
//                wallTextureIds.RemoveAt(_index);
//            }
//        }
//    }
//    public void UpdateCeilingTextureList(int _ceilingCount)
//    {
//        if (ceilingTextureIds.Count == _ceilingCount)
//            return;
//        if (ceilingTextureIds.Count < _ceilingCount)
//        {
//            int _loopCount = _ceilingCount - ceilingTextureIds.Count;
//            for (int i = 0; i < _loopCount; i++)
//            {
//                ceilingTextureIds.Add(-1);
//            }
//        }
//        else
//        {
//            while (ceilingTextureIds.Count > _ceilingCount)
//            {
//                int _index = ceilingTextureIds.Count - 1;
//                if (_index < 0)
//                    break;
//                ceilingTextureIds.RemoveAt(_index);
//            }
//        }
//    }
//    public void UpdateFloorTextureList(int _floorCount)
//    {
//        if (floorTextureIds.Count == _floorCount)
//            return;
//        if (floorTextureIds.Count < _floorCount)
//        {
//            int _loopCount = _floorCount - floorTextureIds.Count;
//            for (int i = 0; i < _loopCount; i++)
//            {
//                floorTextureIds.Add(-1);
//            }
//        }
//        else
//        {
//            while (floorTextureIds.Count > _floorCount)
//            {
//                int _index = floorTextureIds.Count - 1;
//                if (_index < 0)
//                    break;
//                floorTextureIds.RemoveAt(_index);
//            }
//        }
//    }
//}
//[System.Serializable]
//public class IndicatorPlacedItemsIdexes
//{
//    public List<int> placedItemIds;
//    public IndicatorPlacedItemsIdexes()
//    {
//        placedItemIds = new List<int>();
//    }
//}
//[System.Serializable]
//public class DynamicPlacedItems
//{
//    public CategoryName mainCat;
//    public int subCatId;
//    public int itemId = -1;
//}