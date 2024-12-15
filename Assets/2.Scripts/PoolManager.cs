//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;
//using static Room;
//using static UnityEngine.InputManagerEntry;

//public class PoolManager: MonoBehaviour
//{
//    public static PoolManager instance;

//    [Header("Pool Properties")]
//    public PoolProperties poolProperties;
//    [Space(2)]

//    [Header("Pool Saveable Data")]
//    public PoolSavableData poolSaveableData;
//    [Space(2)]

//    [Header("Pool Customers")]
//    public PoolCustomerManager poolCustomerManager;
//    [Space(2)]

//    [Header("Non References Data")]
//    // trash
//    public int totalDustBinTrash;
//    public int totalMopTrash;

//    // dirty 
//  //  public int totalDirtyDustBinTrash;
//  //  public int totalDirtyMopTrash;
//  //  public int inactiveCountofDirtyDustbin;
//  //  public int inactiveCountofDirtyMopTrash;

//    // paint
//    public int totalInnerTiles;
//    public int totalOuterTiles;
//    public int inactiveCountofInnerTiles;
//    public int inactiveCountofOuterTiles;

//    bool gameStarted = false;
//    bool isDataInitialized;
//    bool isAroundPool;

//    /// <summary>
//    /// Locked = 0,
//    /// NotPurchased = 1,
//    /// NotReady = 2,
//    /// Ready = 3
//    /// </summary>
//    public enum PoolStatus
//    {
//        Locked = 0,
//        NotPurchased = 1,
//        NotReady = 2,
//        Ready = 3
//    }

//    #region Initial
//    private void Awake()
//    {
//        instance = this;
//        poolProperties.mustPlacedItemsCount = poolProperties.poolPlacedItems.Count;
//        totalDustBinTrash = poolProperties.poolTrashContainer.transform.childCount;
//        totalMopTrash = poolProperties.poolDustContainer.transform.childCount;
//        totalInnerTiles = poolProperties.innerTilesParent.transform.childCount;
//        totalOuterTiles = poolProperties.outerTilesParent.transform.childCount;
//    }
//    public void Start()
//    {
//        UpdateCounterPriceText();
//    }

//    public void OnChangeGameStatus(bool _enable)
//    {
//        gameStarted = _enable;
//    }

//    public bool IsGameStarted()
//    {
//        return gameStarted;
//    }

//    public void OpenPool(bool _open)
//    {
//        poolCustomerManager.startSpawning = _open;
//        poolSaveableData.openStatus = _open;
//        if (_open)
//        {
//        //    poolCustomerManager.StartSpawningCustomers(1f);
//      //      UpdateGameProgressBar(true);
//            GameManager.instance.CallFireBase("PoolOpened");
//        }
//        else
//        {
//        //    poolCustomerManager.HandlePoolClosure();
//            GameManager.instance.CallFireBase("PoolClosed");
//        }
//    }

//    void UpdateCounterPriceText()
//    {
//        int _len = poolProperties.counterPriceTexts.Length;
//        int _counterPrice = poolProperties.counterPrice;
//        for (int i = 0; i < _len; i++)
//        {
//            poolProperties.counterPriceTexts[i].text = "$" + _counterPrice;
//        }
//    }
//    #endregion

//    #region Saving, Loading
//    public IEnumerator InitializePoolData()
//    {
//        poolSaveableData = (PoolSavableData)SerializationManager.LoadFile("_PoolData");
//        if (poolSaveableData == null)
//        {
//            poolSaveableData = new PoolSavableData(poolProperties.mustPlacedItemsCount, poolProperties.outerTilesParent.transform.childCount,
//                poolProperties.innerTilesParent.transform.childCount, 0);
//        }
//        yield return null;

//        LoadSavedPoolData();
//        isDataInitialized = true;
//        GameController.instance.AddSavingAction(SavePoolData);
//    }

//    void LoadSavedPoolData()
//    {
//        GameController.instance.changeGameStatus += OnChangeGameStatus;
        
//        SetPoolLockStatus();
   
//        CheckTrashBoolInStart();
//        LoadSavePaintTiles();

//        LoadSavePoolWaterStauts();

//        CheckPoolCounterBuyed();
//    }

//    void LoadSavePaintTiles()
//    {
//        poolSaveableData.UpdateOuterTileTextureList(poolProperties.outerTilesParent.transform.childCount);
//        poolSaveableData.UpdateInnerTilesTextureList(poolProperties.innerTilesParent.transform.childCount);
//        LoadSaveLoopForPoolOuterTiles();
//        LoadSaveLoopForPoolInnerTiles();
//    }

//    public void CheckTrashBoolInStart()
//    {
//        TurnOnOrOffTrash(poolSaveableData.dustBinTrashCounter, poolProperties.poolTrashContainer);
//        TurnOnOrOffTrash(poolSaveableData.mopTrashCounter, poolProperties.poolDustContainer);
//        //   cleanXpsGiven = (roomSaveable.isDustBinTrashCleaned && roomSaveable.isMopTrashCleaned);
//        //isDoorWoodClear = roomSaveable.isDoorWoodCleaned;
//    }
//    void TurnOnOrOffTrash(int _cleanedCounter, GameObject _trashParent)
//    {
//        _cleanedCounter = Mathf.Clamp(_cleanedCounter, 0, _trashParent.transform.childCount);

//        for (int i = 0; i < _cleanedCounter; i++)
//        {
//            _trashParent.transform.GetChild(i).gameObject.SetActive(false);
//        }
//    }

//    void LoadSaveLoopForPoolOuterTiles()
//    {
//        for (int i = 0; i < poolProperties.outerTilesParent.transform.childCount; i++)
//        {
//            int _temp = i;
//            poolProperties.outerTilesParent.transform.GetChild(i).GetComponent<ItemPickandPlace>().SetOutSideRoomIndex(_temp);
//            poolProperties.outerTilesParent.transform.GetChild(i).GetComponent<ItemPickandPlace>().itemId = poolSaveableData.outerTilesTextureIds[i];

//            if (poolSaveableData.outerTilesTextureIds[i] >= 0)
//            {
//                ItemData _item = GameManager.instance.GetItem(poolProperties.outerTilesParent.transform.GetChild(i).GetComponent<ItemPickandPlace>().mainCat,
//                    poolProperties.outerTilesParent.transform.GetChild(i).GetComponent<ItemPickandPlace>().SubCatId, poolSaveableData.outerTilesTextureIds[i]);
//                if (_item != null)
//                {
//                    poolProperties.outerTilesParent.transform.GetChild(i).GetComponent<ItemPickandPlace>().itemId = _item.itemID;
//                    poolProperties.outerTilesParent.transform.GetChild(i).GetComponent<ItemPickandPlace>().OnSpawnItem(_item);
//                }
//            }
//        }
//    }

//    void LoadSaveLoopForPoolInnerTiles()
//    {
//        for (int i = 0; i < poolProperties.innerTilesParent.transform.childCount; i++)
//        {
//            int _temp = i;
//            poolProperties.innerTilesParent.transform.GetChild(i).GetComponent<ItemPickandPlace>().SetOutSideRoomIndex(_temp);
//            poolProperties.innerTilesParent.transform.GetChild(i).GetComponent<ItemPickandPlace>().itemId = poolSaveableData.innerTilesTextureIds[i];

//            if (poolSaveableData.innerTilesTextureIds[i] >= 0)
//            {
//                ItemData _item = GameManager.instance.GetItem(poolProperties.innerTilesParent.transform.GetChild(i).GetComponent<ItemPickandPlace>().mainCat,
//                    poolProperties.innerTilesParent.transform.GetChild(i).GetComponent<ItemPickandPlace>().SubCatId, poolSaveableData.innerTilesTextureIds[i]);
//                if (_item != null)
//                {
//                    poolProperties.innerTilesParent.transform.GetChild(i).GetComponent<ItemPickandPlace>().OnSpawnItem(_item);
//                    poolProperties.innerTilesParent.transform.GetChild(i).GetComponent<ItemPickandPlace>().itemId = _item.itemID;
//                }
//            }
//        }
//    }

//    void LoadSavePoolWaterStauts()
//    {
//        Vector3 _tempPos = poolProperties.waterObj.transform.position;
//        int _waterStaus = poolSaveableData.poolWaterStatus;

//        if (_waterStaus == (int)PoolWaterStatus.Empty)
//        {
//            _tempPos.y = -1.46f;
//            poolProperties.waterObj.GetComponent<MeshRenderer>().material = poolProperties.cleanWaterMaterial;
//        }
//        else if(_waterStaus == (int)PoolWaterStatus.Filled)
//        {
//            _tempPos.y = -0.6f;
//            poolProperties.waterObj.GetComponent<MeshRenderer>().material = poolProperties.cleanWaterMaterial;
//        }
//        else if(_waterStaus == (int)PoolWaterStatus.Dirty)
//        {
//            _tempPos.y = -0.6f;
//            poolProperties.waterObj.GetComponent<MeshRenderer>().material = poolProperties.dirtyWaterMaterial;
//        }
//        poolProperties.waterObj.transform.position = _tempPos;    
//    }

//    void CheckPoolCounterBuyed()
//    {
//        if (IsCounterBuyed())
//        {
//            HideCounterLockObj();
//        }
//    }

//    void SavePoolData()
//    {
//        if (isDataInitialized)
//        {
//            SerializationManager.Save(poolSaveableData, "_PoolData");
//        }
//    }
//    #endregion

//    #region Locking Implementation
//    public void SetPoolLockStatus()
//    {
//        poolProperties.poolWoodsLockParent.SetActive(!(poolSaveableData.poolStatus > 1));

//        bool _isLocked = poolProperties.poolReqLevel > PlayerDataManager.instance.playerData.playerLevel;
//        poolProperties.poolLockBtnImg.GetComponent<Image>().sprite = _isLocked ? UIController.instance.lock3dBtnImg : UIController.instance.unPurchase3dBtnImg;
//        poolProperties.poolLockPriceText.GetComponent<TextMeshProUGUI>().color = _isLocked ? UIController.instance.lock3dBtnTxtClr : UIController.instance.unPurchasedBtnTxtClr;
//        poolProperties.poolLockPriceText.GetComponent<TextMeshProUGUI>().text = _isLocked ? "Level " + poolProperties.poolReqLevel.ToString() : "$" + poolProperties.poolPrice.ToString();

//        if ((!_isLocked) && (poolSaveableData.poolStatus < 1))
//        {
//            poolSaveableData.poolStatus = 1;
//        }
//    }

//    public bool OnPurchasePool()
//    {
//        PoolManager.instance.OnPoolWoodRemove();
//        if (poolSaveableData.poolStatus > 1)
//        {
//            return false;
//        }
//        if (poolProperties.poolReqLevel > PlayerDataManager.instance.playerData.playerLevel)
//        {
//            GameManager.instance.CallFireBase("poolUnlkBfLev");
//            UIController.instance.EnablePopupNotification("Pool Will Unlock At Level " + poolProperties.poolReqLevel.ToString());
//            return false;
//        }
//        if (poolProperties.poolPrice > PlayerDataManager.instance.playerData.playerCash)
//        {
//            GameManager.instance.CallFireBase("NoCshPool");
//            UIController.instance.EnableNoCashPanel();
//            return false;
//        }

//        //seting room state to not ready when it is purchased
//        poolSaveableData.poolStatus = 2;
//        SetPoolLockStatus();
//        PlayerDataManager.instance.UpdateCash(-1 * poolProperties.poolPrice);
//        UIController.instance.UpdateCurrency(-1 * poolProperties.poolPrice);
//    //    PlayerDataManager.instance.UpdateXP(poolProperties.poolPrice / 2);
//    //    UIController.instance.UpdateXP(poolProperties.poolPrice / 2);
//        UIController.instance.DisplayInstructions("Pool Unlocked!");
//        CheckPoolProgress();
//        return true;
//    }
   
//    public bool CanOpenPool()
//    {
//        if (poolProperties.poolReqLevel > PlayerDataManager.instance.playerData.playerLevel)
//        {
//            UIController.instance.DisplayInstructions("Pool Will Unlock At Level " + poolProperties.poolReqLevel.ToString());
//            return false;
//        }
//        if (poolSaveableData.poolStatus <= 1)
//        {
//            UIController.instance.DisplayInstructions("Purchase Pool First!");
//            return false;
//        }
//        if(poolSaveableData.poolStatus != 3)
//        {
//            UIController.instance.DisplayInstructions("Prepare Pool First!");
//            return false;
//        }
//        return true;
//    }
//    #endregion

//    #region Trash Cleaning
//    public bool AreAllTrashTasksComplete()
//    {
//     //   return poolSaveableData.isMopTrashCleaned && poolSaveableData.isDustBinTrashCleaned;
//     return true;
//    }
//    public void OnPoolWoodRemove()
//    {
//   //     poolSaveableData.isDoorWoodRemoved = true;
//        CheckPoolProgress();
//    }

//    public void OnRemoveDustBinTrash()
//    {
//        poolSaveableData.dustBinTrashCounter++;
//    //    CheckIfTrashIsCleaned();
//        CheckPoolProgress();
//    }

//    public void OnRemoveMopDust()
//    {
//        poolSaveableData.mopTrashCounter++;
//   //     CheckIfTrashIsCleaned();
//        CheckPoolProgress();
//    }

//    bool IsTrashCleaned(int _counter, int _parentCount)
//    {
//        if(_counter < _parentCount)
//        {
//            return false;
//        }
//        return true;
//    }
//    #endregion

//    #region Paint Related
//    public bool AreAllPoolOuterTilesPainted()
//    {
//        for (int i = 0; i < poolProperties.outerTilesParent.transform.childCount; i++)
//        {
//            if (i >= poolSaveableData.outerTilesTextureIds.Count)
//            {
//                return false;
//            }
//            if (poolSaveableData.outerTilesTextureIds[i] < 0)
//            {
//                return false;
//            }
//        }
//        return true;
//    }

//    public bool AreAllPoolInnerTilesPainted()
//    {
//        for (int i = 0; i < poolProperties.innerTilesParent.transform.childCount; i++)
//        {
//            if (i >= poolSaveableData.innerTilesTextureIds.Count)
//            {
//                return false;
//            }
//            if (poolSaveableData.innerTilesTextureIds[i] < 0)
//            {
//                return false;
//            }
//        }
//        return true;
//    }

//    public void SetInnerTileItemId(int _indicaterId, int _itemId)
//    {
//        poolSaveableData.innerTilesTextureIds[_indicaterId] = _itemId;
//    }
//    public void SetOuterTileItemId(int _indicaterId, int _itemId)
//    {
//        poolSaveableData.outerTilesTextureIds[_indicaterId] = _itemId;
//    }

//    public void DonePaintCountToShowOnUI()
//    {
//        inactiveCountofInnerTiles = PaintCount(poolSaveableData.innerTilesTextureIds);
//        inactiveCountofOuterTiles = PaintCount(poolSaveableData.outerTilesTextureIds);
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
//    #endregion

//    #region Pool Status Updations
//    public void CheckPoolProgress(bool _isPreparingPool = true)
//    {
//        int _poolStatus = (int)PoolStatus.NotReady;
//        if (!IsTrashCleaned(poolSaveableData.dustBinTrashCounter, totalDustBinTrash))
//        {
//            UIController.instance.SetPoolProgress("Dustbin Trash ", true, poolSaveableData.dustBinTrashCounter, totalDustBinTrash);
//        }
//        else if (!IsTrashCleaned(poolSaveableData.mopTrashCounter, totalMopTrash))
//        {
//            UIController.instance.SetPoolProgress("Pool Dust ", true, poolSaveableData.mopTrashCounter, totalMopTrash);
//        }
//        else if (poolSaveableData.poolWaterStatus == (int)PoolWaterStatus.Dirty)
//        {
//            UIController.instance.SetPoolProgress("Empty The Dirty Pool", true, 0, 0);
//        }
//        else if (!AreAllPoolInnerTilesPainted())
//        {
//            UIController.instance.SetPoolProgress("Pool Inner Tiles Paint ", true, inactiveCountofInnerTiles, totalInnerTiles);
//        }
//        else if (!AreAllPoolOuterTilesPainted())
//        {
//            UIController.instance.SetPoolProgress("Pool Outer Tiles Paint ", true, inactiveCountofOuterTiles, totalOuterTiles);
//        }
//        else if (AnyRemainingItemToPlace() >= 0)
//        {
//            int _index = AnyRemainingItemToPlace();
//            DynamicPlacedItems _dynamic = poolProperties.poolPlacedItems[_index];
//            string _itemSubCatName = GameManager.instance.categoriesUIData[(int)_dynamic.mainCat].subCategoriesUIData[_dynamic.subCatId].subName;
//            UIController.instance.UpdateGameProgressText(true, "Now Place " + _itemSubCatName);
//            //UIController.instance.SetFurnitureProgress(true , _itemSubCatName);
//        }
//        else if(!poolSaveableData.isCounterBuy)
//        {
//            UIController.instance.SetPoolProgress("Pruchase Pool Counter ", true, 0, 1);
//        }
//        else if(poolSaveableData.poolWaterStatus == (int)PoolWaterStatus.Empty)
//        {
//            UIController.instance.SetPoolProgress("Fill Water In The Pool", true, 0, 0);
//        }
//        else
//        {
//        //    if (totalDustBinTrash > 0 || totalMopTrash > 0)
//        //    {
//        //        _poolStatus = 4;
//        //        UIController.instance.UpdateGameProgressText(true, "Clean Dust And Remove Trash");
//        //    }
//       //     else
//       //     {
//                _poolStatus = (int)PoolStatus.Ready;
//                if (_isPreparingPool)
//                {
//                    if (GameController.instance.gameData.poolOpenStatus)
//                    {
//                        //       UIController.instance.UpdateGameProgressText(true, "Serve This Pool To Customers");
//                    }
//                    else
//                    {
//                        UIController.instance.UpdateGameProgressText(true, "Open Pool To Serve This Pool To Customers");
//                    }
//                }
//                else
//                {
//                    UIController.instance.UpdateGameProgressText(false);
//                }
//        //    }
//        }
//        if (poolSaveableData.poolStatus > (int)PoolStatus.NotPurchased && poolSaveableData.poolStatus <= (int)PoolStatus.Ready)
//        {
//            poolSaveableData.poolStatus = _poolStatus;
//            //   ReceptionUIManager.Instance.UpdateRoomImageAndState(_poolStatus, roomProperties.roomNumber);
//        }
//        if (_poolStatus == 3)
//        {
//            UIController.instance.DisplayInstructions("Pool is Ready");
//            GameManager.instance.CallFireBase("PoolRdy");
//        }
//    }

//    public void UpdateAroundStoreTrigger(bool _isAround)
//    {
//        isAroundPool = _isAround;
//        if(isAroundPool)
//        {
//            DonePaintCountToShowOnUI();
//            CheckPoolProgress();
//        }
//    }

//    public bool IsPlayerAtPoolArea()
//    {
//        return isAroundPool;
//    }

//    public void UpdateGameProgressBar(bool _isEntering)
//    {
//        if (!_isEntering)
//        {
//            UIController.instance.UpdateGameProgressText(false);
//            return;
//        }
//        if (!isAroundPool)
//            return;

//     //   if (!poolSaveableData.isDustBinTrashCleaned)
//     //   {
//     //       UIController.instance.UpdateGameProgressText(true, "Dispose of Trash in Dustbin (" + inactiveCountofDustbin.ToString() + "/" + totalDustBinTrash.ToString() + ")");
//     //   }
//     //   else if (!poolSaveableData.isMopTrashCleaned)
//      //  {
//            //       UIController.instance.UpdateGameProgressText(true, "Clean Dust with Mop (" + poolSaveableData.dustRemovedCounter.ToString() + "/" + poolDustContainer.childCount.ToString() + ")");
//     //   }
//        else if (poolSaveableData.placedItemsIds.Count <= 0)
//        {
//            UIController.instance.UpdateGameProgressText(true, "Use the Store to order & Stock products in pool");
//        }
//        else if (poolSaveableData.openStatus == false)
//        {
//            UIController.instance.UpdateGameProgressText(true, "Open Pool from PC");
//        }
//        else
//        {
//            UIController.instance.UpdateGameProgressText(false);
//        }
//    }
//    #endregion

//    #region Placed Items Related
//    public void OnPlaceItemInPool(CategoryName _catName, int _subCatId, int _itemId)
//    {
//        if (poolProperties.poolPlacedItems != null)
//        {
//            for (int i = 0; i < poolProperties.poolPlacedItems.Count; i++)
//            {
//                if (poolProperties.poolPlacedItems[i].mainCat == _catName && poolProperties.poolPlacedItems[i].subCatId == _subCatId
//                    && poolProperties.poolPlacedItems[i].itemId < 0)
//                {
//                    poolProperties.poolPlacedItems[i].itemId = _itemId;
//                    return;
//                }
//            }
//        }
//        else
//        {
//            poolProperties.poolPlacedItems = new List<DynamicPlacedItems>();
//        }

//        DynamicPlacedItems _item = new DynamicPlacedItems();
//        _item.mainCat = _catName;
//        _item.subCatId = _subCatId;
//        _item.itemId = _itemId;
//        poolProperties.poolPlacedItems.Add(_item);
//    }

//    public void OnRemoveItem(CategoryName _catName, int _subCatId, int _itemId)
//    {
//        if (poolProperties.poolPlacedItems == null)
//            return;
//        int _index = -1;
//        for (int i = poolProperties.poolPlacedItems.Count - 1; i > -1; i--)
//        {
//            if (poolProperties.poolPlacedItems[i].mainCat == _catName && poolProperties.poolPlacedItems[i].subCatId == _subCatId
//                    && poolProperties.poolPlacedItems[i].itemId == _itemId)
//            {
//                _index = i;
//                break;
//            }
//        }
//        if (_index < poolProperties.mustPlacedItemsCount)
//        {
//            poolProperties.poolPlacedItems[_index].itemId = -1;
//        }
//        else
//        {
//            poolProperties.poolPlacedItems.RemoveAt(_index);
//        }
//        CheckPoolProgress();
//    }

//    public int AnyRemainingItemToPlace()
//    {
//        for (int i = 0; i < poolProperties.poolPlacedItems.Count; i++)
//        {
//            if (poolProperties.poolPlacedItems[i].itemId < 0)
//            {
//                return i;
//            }
//        }
//        return -1;
//    }
//    #endregion

//    #region Counter & Employee
//    public bool IsCounterBuyed()
//    {
//        return poolSaveableData.isCounterBuy;
//    }

//    public void OnCounterPurchase()
//    {
//        if (poolProperties.counterPrice > PlayerDataManager.instance.playerData.playerCash)
//        {
//            GameManager.instance.CallFireBase("NoCshPoolCounter");
//            UIController.instance.EnableNoCashPanel();
//        }
//        else
//        {
//            poolSaveableData.isCounterBuy = true;
//            PlayerDataManager.instance.UpdateCash(-1 * poolProperties.counterPrice);
//            UIController.instance.UpdateCurrency(-1 * poolProperties.counterPrice);
//            HideCounterLockObj();
//            CheckPoolProgress();
//        }
//    }

//    void HideCounterLockObj()
//    {
//        poolProperties.poolCounterLockObj.SetActive(false);
//    }
//    #endregion

//    #region Water Pump Related
//    public bool IsWaterFillIednPool()
//    {
//        return poolSaveableData.poolWaterStatus == (int)PoolWaterStatus.Filled || poolSaveableData.poolWaterStatus == (int)PoolWaterStatus.Dirty;
//    }

//    public void DoFillThePool()
//    {
//        poolSaveableData.poolWaterStatus = (int)PoolWaterStatus.Filled;
//        poolProperties.waterFillPump.GetComponent<PoolWaterPump>().WaterFillPumpRotation();
//        poolProperties.waterObj.GetComponent<MeshRenderer>().material = poolProperties.cleanWaterMaterial;
//        StopCoroutine(PlayPoolWaterAnimation(0,0));
//        StartCoroutine(PlayPoolWaterAnimation(-1.46f, -0.6f));
//        UIController.instance.DisplayInstructions("The Pool Is Filled With Clean Water.");
//        CheckPoolProgress();
//    }

//    public void DoEmptyThePool()
//    {
//        poolSaveableData.poolWaterStatus = (int)PoolWaterStatus.Empty;
//        poolProperties.waterOutPump.GetComponent<PoolWaterPump>().WaterOutPumpRotation();
//        StopCoroutine(PlayPoolWaterAnimation(0, 0));
//        StartCoroutine(PlayPoolWaterAnimation(-0.6f, -1.46f));
//        UIController.instance.DisplayInstructions("The Pool Is Now Empty.");
//        CheckPoolProgress();
//    }

//    public int GetPoolWaterStatus()
//    {
//        return poolSaveableData.poolWaterStatus;
//    }

//    IEnumerator PlayPoolWaterAnimation(float _startYPoint, float _endYPoint)
//    {
//        float _elapsed = 0f;
//        float _duration = 1.5f; 
//        Vector3 _waterObjPos = poolProperties.waterObj.transform.position;
//        _waterObjPos.y = _startYPoint;
//        Vector3 _startPosition = _waterObjPos;
//        Vector3 _endPosition = new Vector3(_waterObjPos.x, _endYPoint, _waterObjPos.z);

//        while (_elapsed < _duration)
//        {
//            _elapsed += Time.deltaTime;
//            float progress = Mathf.Clamp01(_elapsed / _duration);

//            poolProperties.waterObj.transform.position = Vector3.Lerp(_startPosition, _endPosition, progress);
//            yield return null;
//        }

//        poolProperties.waterObj.transform.position = _endPosition;
//    }
//    #endregion
//}

//[System.Serializable]
//public class PoolSavableData
//{
//    public bool isPoolReady;
//    public bool openStatus;
//    public bool isCounterBuy;
//    public bool isFilterChange;
//    public int poolWaterStatus;
// //   public bool isDoorWoodRemoved;
//    public int mopTrashCounter;
//    public int dustBinTrashCounter;
//    public int poolStatus;
//    public List<int> placedItemsIds;
//    public List<int> outerTilesTextureIds;
//    public List<int> innerTilesTextureIds;

//    public PoolSavableData(int _itemsCount, int _outerTilesCount, int _innerTilesCount, int _poolStatus)
//    {
//        poolWaterStatus = 1;
//        placedItemsIds = new List<int>(_itemsCount);
//        outerTilesTextureIds = new List<int>(_outerTilesCount);
//        innerTilesTextureIds = new List<int>(_innerTilesCount);
//        poolStatus = _poolStatus;

//        for (int i = 0; i < _itemsCount; i++) placedItemsIds.Add(-1);
//        for (int i = 0; i < _outerTilesCount; i++) outerTilesTextureIds.Add(-1);
//        for (int i = 0; i < _innerTilesCount; i++) innerTilesTextureIds.Add(-1);
//    }
//    public void UpdateInnerTilesTextureList(int _innerTilesCount)
//    {
//        if (innerTilesTextureIds.Count == _innerTilesCount)
//            return;
//        if (innerTilesTextureIds.Count < _innerTilesCount)
//        {
//            int _loopCount = _innerTilesCount - innerTilesTextureIds.Count;
//            for (int i = 0; i < _loopCount; i++)
//            {
//                innerTilesTextureIds.Add(-1);
//            }
//        }
//        else
//        {
//            while (innerTilesTextureIds.Count > _innerTilesCount)
//            {
//                int _index = innerTilesTextureIds.Count - 1;
//                if (_index < 0)
//                    break;
//                innerTilesTextureIds.RemoveAt(_index);
//            }
//        }
//    }

//    public void UpdateOuterTileTextureList(int _outerTilesCount)
//    {
//        if (outerTilesTextureIds.Count == _outerTilesCount)
//            return;
//        if (outerTilesTextureIds.Count < _outerTilesCount)
//        {
//            int _loopCount = _outerTilesCount - outerTilesTextureIds.Count;
//            for (int i = 0; i < _loopCount; i++)
//            {
//                outerTilesTextureIds.Add(-1);
//            }
//        }
//        else
//        {
//            while (outerTilesTextureIds.Count > _outerTilesCount)
//            {
//                int _index = outerTilesTextureIds.Count - 1;
//                if (_index < 0)
//                    break;
//                outerTilesTextureIds.RemoveAt(_index);
//            }
//        }
//    }
//}

//[System.Serializable]
//public class PoolProperties
//{
//    [Header("Unlock Related")]
//    public int poolPrice;
//    public int poolReqLevel;
//    public GameObject poolWoodsLockParent;
//    public GameObject poolLockBtnImg;
//    public GameObject poolLockPriceText;
//    [Space(1)]

//    [Header("Cleaning Related")]
//    public GameObject poolTrashContainer;
//    public GameObject poolDustContainer;
//    [Space(1)]

//    [Header("Paint Related")]
//    public GameObject innerTilesParent;
//    public GameObject outerTilesParent;
//    [Space(1)]

//    [Header("Water Pool Related")]
//    public GameObject waterObj;
//    public Material dirtyWaterMaterial;
//    public Material cleanWaterMaterial;
//    public GameObject waterFillPump;
//    public GameObject waterOutPump;
//    [Space(2)]

//    //  [Header("Dirty Related")]
//    //  public GameObject poolDirtyTrashContainer;
//    //  public GameObject poolDirtyDustContainer;
//    //  [Space(1)]

//    [Header("Placed Items Related")]
//    public int mustPlacedItemsCount = 10;   
//    public GameObject placedItemParent;
//    public List<DynamicPlacedItems> poolPlacedItems;
//    [Space(1)]

//    [Header("Counter and Employee Related")]
//    public GameObject poolCounterLockObj;
//    public int counterPrice;
//    public TextMeshProUGUI[] counterPriceTexts;
//    [Space(1)]

//    [Header("XP Related")]
//    public int cleaningCompleteXps;
// //   [Space(1)]

//   // [Header("Customer Related")]
//}

//[System.Serializable]
//public enum PoolWaterStatus
//{
//    Empty,
//    Dirty,
//    Filled
//}
