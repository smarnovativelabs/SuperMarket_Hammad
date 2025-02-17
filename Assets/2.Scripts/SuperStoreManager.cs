using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using System.Drawing;
using UnityEngine.Purchasing;
using Unity.VisualScripting.FullSerializer;
using System;
using System.Diagnostics;

public class SuperStoreManager : MonoBehaviour
{
    public int superStorePrice;
    public int superStoreReqLevel;
    public float appproxCustomerSpawnDelay;
    public Transform spawnedVehicleParent;
    public Transform spawnedCustomerParent;
    public Transform storeWoodsLockContainer;
    public Transform storeTrashContainer;
    public Transform storeDustContainer;
    public GameObject storeLockBtnImg;
    public GameObject storeLockPriceText;
    public DoorOpen superMarketDoor;
    public GameObject[] availableCustomers;
    public Transform[] customerSpawnPoints;

    public GameObject[] availableCars;
    public VehicleEnteringRoute[] vehicleRoutePoints;
    public VehicleParkingPoint[] parkingPoints;
    public CounterPoint[] counterQueue;
    public InitialRacksData[] initialRacks;
    public static SuperStoreManager instance;
    public List<GameObject> storeRacks;
    public SuperStoreSavableData storeData;
    public List<SuperStoreItems> storeItemsList;  // THis Will Be USed To Add/Remove Items From Store Based On Customer Wishlist will not be unless a customer actually pays for an item

    bool isDataInitialized;
    bool gameStarted = false;
    bool startSpawning = false;
    bool isAroundSuperStore;
    float spawningTimer;
    int spawnedCustomerIndex = 0;
    int spawnedVehicleIndex = 0;
    int customersInWaiting = 0;
    int servedCustomerCount = 0;
    int customerReachedAtCounter = 0;
    int removableTrashCount;
    int newSpawnedRackId = -1;
    float storeAdsTimer = 0f;
    bool canDisplayAds = false;
    bool isAtSuperStoreCounter = false;
    public List<PosCounters> posCounters;
    public int playerAtCounterId = -1;
    public AudioClip itemBarcodeSound;
    public AudioClip cardCollectSound;
    public AudioClip cashMachineSound;
    public AudioSource counterSoundPlayer;
  

    //Expensions realted data
    public Supermarketexpension[] superStoreExpension;

    [System.Serializable]
    public class InitialRacksData
    {
        public CategoryName mainCat;
        public int subCatId;
        public int itemId;
        public int rackId;
        public Transform rackPlacingPoint;
    }
 
    private void Awake()
    {
        instance = this;
    }
    public void Start()
    {
        GameController.instance.changeGameStatus += UpdateGameStatus;
    }
    public void UpdateGameStatus(bool _enable)
    {
        gameStarted = _enable;
    }

    public void OpenSuperStore(bool _open)
    {
        startSpawning = _open;
        storeData.openStatus = _open;
        if (startSpawning)
        {
            StartSpawningCustomers(1f);
            UpdateGameProgressBar(true);
            GameManager.instance.CallFireBase("StoreOpened");
        }
        else
        {
            GameManager.instance.CallFireBase("StoreClosed");
        }
    }
    public void UpdateAroundStoreTrigger(bool _isAround)
    {
        isAroundSuperStore = _isAround;
        UpdateGameProgressBar(_isAround);
    }
    public void UpdateGameProgressBar(bool _isEntering)
    {
        if (!_isEntering)
        {
            UIController.instance.UpdateGameProgressText(false);
            return;
        }
        if (!isAroundSuperStore)
            return;
        //if (storeData.doorWoodsRemovedCounter < storeWoodsContainer.childCount)
        //{
        //    UIController.instance.UpdateGameProgressText(true, "Use Hammer to Remove Wood (" + storeData.doorWoodsRemovedCounter.ToString() + "/" + storeWoodsContainer.childCount.ToString() +")");
        //} else
        if (storeData.trashPickedCounter < removableTrashCount)
        {
           
            UIController.instance.UpdateGameProgressText(true, "Dispose of Trash in Dustbin (" + storeData.trashPickedCounter.ToString() + "/" + removableTrashCount.ToString() + ")");
            
        }
        else if (storeData.dustRemovedCounter < storeDustContainer.childCount)
        {
            UIController.instance.UpdateGameProgressText(true, "Clean Dust with Mop (" + storeData.dustRemovedCounter.ToString() + "/" + storeDustContainer.childCount.ToString() + ")");
        }else if(storeRacks==null || storeRacks.Count < 1)
        {
            UIController.instance.UpdateGameProgressText(true, "Use the Store to order racks and place them in supermarket");

        }
        else if (storeData.placedItems.Count <= 0)
        {
            UIController.instance.UpdateGameProgressText(true, "Use the Store to order & Stock products in supermarket");
        } else if (storeData.openStatus == false)
        {
            UIController.instance.UpdateGameProgressText(true, "Open Superstore from PC");
        }/*else if (IsCustomerApproachingCounter())
        {
            UIController.instance.UpdateGameProgressText(true, "Tap Cash Counter to Serve Customer");
        }*/
        else
        {
            UIController.instance.UpdateGameProgressText(false);
        }
    }
    public bool IsAtSuperMarket()
    {
        return isAroundSuperStore;
    }

    public void DisplayInterstitialAd()
    {
        if (canDisplayAds)
        {
            canDisplayAds = false;
            storeAdsTimer = 0f;
            AdsMediation.AdsMediationManager.instance.ShowInterstitial();
        }
    }
    #region Super Store Cleaning
    public void OnRemoveStoreWood()
    {
        storeData.doorWoodsRemovedCounter++;
        UpdateGameProgressBar(true);
        //if(storeData.doorWoodsRemovedCounter >= storeWoodsContainer.childCount)
        //{
        //    superMarketDoor.isLock = false;
        //    superMarketDoor.InteractWithDoor(true);

        //}
        GameManager.instance.CallFireBase("StoreWoodRmv" + storeData.doorWoodsRemovedCounter.ToString());
    }
    public void OnRemoveStoreTrash()
    {
        storeData.trashPickedCounter++;
        if (storeData.trashPickedCounter == removableTrashCount)
        {
            print("Hey Hello");
            TutorialManager.instance.OnCompleteTutorialTask(9);
        }
        UpdateGameProgressBar(true);
        GameManager.instance.CallFireBase("StoreTrashRmv" + storeData.trashPickedCounter.ToString());
    }
    public void OnRemoveStoreDust()
    {
        storeData.dustRemovedCounter++;
        if (storeData.dustRemovedCounter == storeDustContainer.childCount)
        {
            print("Hey Hello 2");
            TutorialManager.instance.OnCompleteTutorialTask(10);
        }
        UpdateGameProgressBar(true);
        GameManager.instance.CallFireBase("StoreDustRmv" + storeData.dustRemovedCounter.ToString());

    }
    #endregion
    #region Vehicle Spawning Mechanism
    private void Update()
    {
        if (!gameStarted)
            return;
        if (startSpawning)
        {
            spawningTimer -= Time.deltaTime;
            if (spawningTimer < 0f)
            {
                SpawnNewCustomer();
            }
            if (!canDisplayAds)
            {
                storeAdsTimer += Time.deltaTime;

                if (storeAdsTimer > 150f)
                {
                    canDisplayAds = true;
                }
            }
        }

    }
    public void StartSpawningCustomers(float _time = -1f)
    {
        if (_time < 0)
        {
            //float _mintime = appproxCustomerSpawnDelay - 1;
            //_mintime = _mintime < 0 ? 0 : _mintime;
            //float _maxTime = appproxCustomerSpawnDelay + 2;
            //_maxTime = _maxTime < 0 ? 5 : _maxTime;
            //spawningTimer = Random.Range(_mintime, _maxTime);
            spawningTimer = appproxCustomerSpawnDelay;
        }
        else
        {
            spawningTimer = _time;
        }
    }
    public void SpawnNewCustomer()
    {
       // int _counterIndex = GetEmptyCounterPoint();

        KeyValuePair<int, int> _emptySpotIndex = GetEmptyQueuePoint();
        if (_emptySpotIndex.Key < 0 || _emptySpotIndex.Value < 0)
        {
            spawningTimer += 1f;
            return;
        }
        List<SuperStoreItems> _wishList = CreateCustomersWishList();
        if (_wishList == null)
        {
            spawningTimer += 5f;
            return;
        }
        CounterPoint _counterPoint = posCounters[_emptySpotIndex.Key].CounterQueue[_emptySpotIndex.Value];

        GameObject _customer = Instantiate(availableCustomers[spawnedCustomerIndex], spawnedCustomerParent);
        Transform _customerSpawnPoint = customerSpawnPoints[UnityEngine.Random.Range(0, customerSpawnPoints.Length)];
        _customer.GetComponent<SuperStoreCustomer>().SpawnNewCustomer(_emptySpotIndex.Key,_emptySpotIndex.Value, _customerSpawnPoint, _wishList);
        _counterPoint.isOccupied = true;
        _counterPoint.queCustomerRef = _customer;
        spawnedCustomerIndex++;
        if (spawnedCustomerIndex >= availableCustomers.Length)
        {
            spawnedCustomerIndex = 0;
        }
        StartSpawningCustomers();
    }
    public void ReleaseParkingSpot(int _parkingIndex)
    {
        parkingPoints[_parkingIndex].isOccupied = false;
    }
    bool IsCounterQueueHaveSpace()
    {
        bool _haveSpace = false;
        for (int i = 0; i < counterQueue.Length; i++)
        {
            _haveSpace = !counterQueue[i].isOccupied;
            if (_haveSpace)
                break;
        }
        return _haveSpace;
    }
    public int GetEmptyCounterPoint()
    {
        int _pointIndex = -1;
        for (int i = 0; i < counterQueue.Length; i++)
        {
            if (!counterQueue[i].isOccupied)
            {
                _pointIndex = i;
                break;
            }
        }
        return _pointIndex;
    }
    public void OnRemoveCustomerFromQueue(int _counterIndex)
    {
        if(_counterIndex<0 || _counterIndex >= posCounters.Count)
        {
            return;
        }

        posCounters[_counterIndex].CounterQueue[0].isOccupied = false;
        posCounters[_counterIndex].CounterQueue[0].queCustomerRef = null;
        int _emptyIndex = 0;
        for (int i = 1; i < posCounters[_counterIndex].CounterQueue.Length; i++)
        {
            if (posCounters[_counterIndex].CounterQueue[i].isOccupied)
            {
                posCounters[_counterIndex].CounterQueue[_emptyIndex].isOccupied = true;
                posCounters[_counterIndex].CounterQueue[_emptyIndex].queCustomerRef = posCounters[_counterIndex].CounterQueue[i].queCustomerRef;
                posCounters[_counterIndex].CounterQueue[_emptyIndex].queCustomerRef.GetComponent<SuperStoreCustomer>().UpdateEnqueueCustomerTarget(_emptyIndex, posCounters[_counterIndex].CounterQueue[_emptyIndex].queuePoint);

                posCounters[_counterIndex].CounterQueue[i].isOccupied = false;
                posCounters[_counterIndex].CounterQueue[i].queCustomerRef = null;
                _emptyIndex = i;
            }
        }
        UpdateCustomersInWait(-1);
    }

    public void UpdateCustomersInWait(int _val)
    {
        customersInWaiting += _val;
        if (customersInWaiting < 0)
        {
            customersInWaiting = 0;
        }
        //Update UI From Here
        UIController.instance.UpdateSuperStoreWaitingCustomerText(customersInWaiting);
    }

    #endregion
    #region Store Items Update
    public List<SuperStoreItems> CreateCustomersWishList()
    {
        int _totalItems = UnityEngine.Random.Range(1, 6);
        if (_totalItems > storeItemsList.Count)
        {
            _totalItems = storeItemsList.Count;
        }
        if (_totalItems <= 0)
        {
            return null;
        }
        List<SuperStoreItems> _items = new List<SuperStoreItems>();
        for(int i = 0; i < _totalItems; i++)
        {
            SuperStoreItems _newItem = storeItemsList[UnityEngine.Random.Range(0, storeItemsList.Count)];
            storeItemsList.Remove(_newItem);
            _items.Add(_newItem);
        }
        return _items;
    }
    public void OnAddItem(SuperStoreItems _item)
    {
        storeData.placedItems.Add(_item);
        storeItemsList.Add(_item);
    }
    public void RemoveItemsFromStore(List<SuperStoreItems> _items)
    {
        for(int i = 0; i < _items.Count; i++)
        {
            SuperStoreItems _item = _items[i];
            if(_item.rackId<0)
            {
                print("Invalid Rack Id For Item");
                continue;
            }
            GameObject _rack = GetRack(_item.rackId);
            if (_rack != null)
            {
                _rack.GetComponent<StoreRack>().RemoveItemFromRack(_item.rackPlaceId);

            }
            if (storeData.placedItems.Contains(_item))
            {
                storeData.placedItems.Remove(_item);
            }
        }
        if (storeData.placedItems.Count <= 0)
        {
            UIController.instance.DisplayInstructions("Super Store Item Finished. Order More Items!");
        }
    }
    public int GetSuperItemsCount()
    {
        return storeData.placedItems.Count;
    }

    public void OnCustomerReachedToCounter()
    {
        customerReachedAtCounter++;
        GameManager.instance.CallFireBase("StoreCusCntr_" + customerReachedAtCounter.ToString());
    }
    public void OnCustomerServed()
    {
        servedCustomerCount++;
        GameManager.instance.CallFireBase("StoreCusSrvd_" + servedCustomerCount.ToString());

    }
    public void UpdateAtStoreCounterState(bool _atCouner)
    {
        isAtSuperStoreCounter = _atCouner;
    }
    public bool IsAtCounter()
    {
        return isAtSuperStoreCounter;
    }
    #endregion
    #region Locking Implementation
    public void UpdateSuperStoreLocking()
    {
        if (storeData.isStorePurchased)
        {
            return;
        }
        bool _isLocked = superStoreReqLevel > PlayerDataManager.instance.playerData.playerLevel;
        storeLockBtnImg.GetComponent<Image>().sprite = _isLocked ? UIController.instance.lock3dBtnImg : UIController.instance.unPurchase3dBtnImg;
        storeLockPriceText.GetComponent<TextMeshProUGUI>().color = _isLocked ? UIController.instance.lock3dBtnTxtClr : UIController.instance.unPurchasedBtnTxtClr;
        storeLockPriceText.GetComponent<TextMeshProUGUI>().text = _isLocked ? "Level " + superStoreReqLevel.ToString() : "$" + superStorePrice.ToString();

    }
    public bool OnPurchaseSuperStore()
    {
        if (storeData.isStorePurchased)
        {
            return false;
        }
        if (superStoreReqLevel > PlayerDataManager.instance.playerData.playerLevel)
        {
            GameManager.instance.CallFireBase("SpStrUnlkBfLev");

            UIController.instance.EnablePopupNotification("Super Market Will Unlock At Level " + superStoreReqLevel.ToString());
            return false;
        }
        if(superStorePrice>PlayerDataManager.instance.playerData.playerCash)
        {
            GameManager.instance.CallFireBase("NoCshSpStr");

            UIController.instance.EnableNoCashPanel();
            return false;
        }

        storeData.isStorePurchased = true;
        storeWoodsLockContainer.gameObject.SetActive(!storeData.isStorePurchased);
        superMarketDoor.isLock = !storeData.isStorePurchased;
        PlayerDataManager.instance.UpdateCash(-1 * superStorePrice);
        UIController.instance.UpdateCurrency(-1 * superStorePrice);
        PlayerDataManager.instance.UpdateXP(superStorePrice / 2);
        UIController.instance.UpdateXP(superStorePrice / 2);
        UIController.instance.DisplayInstructions("Super Store Unlocked!");
        //unlock first counter
        UnlockCashCounter(0);
        return true;
    }
    public void OpenMarketDoors()
    {
       superMarketDoor.InteractWithDoor(true);
    }
    public bool CanOpenSuperStore()
    {
        if (superStoreReqLevel > PlayerDataManager.instance.playerData.playerLevel)
        {
            UIController.instance.DisplayInstructions("Super Market Will Unlock At Level " + superStoreReqLevel.ToString());
            return false;
        }
        if (!storeData.isStorePurchased)
        {
            UIController.instance.DisplayInstructions("Purchase Super Store First!");
            return false;
        }
        if (GetSuperItemsCount() <= 0)
        {
            UIController.instance.DisplayInstructions("Purchase Some Store Products First!");
        }
        return true;
    }
    #endregion
    #region Store Racks Implementation
    public void SetNewSpawningRackId()
    {
        for(int i = 0; i < storeRacks.Count; i++)
        {
            if (storeRacks[i].GetComponent<ItemPickandPlace>().itemsSavingProps != null)
            {
                if (storeRacks[i].GetComponent<ItemPickandPlace>().itemsSavingProps.itemUniqueId > newSpawnedRackId)
                {
                    newSpawnedRackId = storeRacks[i].GetComponent<ItemPickandPlace>().itemsSavingProps.itemUniqueId;
                }
            }
        }
        newSpawnedRackId++;
    }
    public int OnNewRackPlaced(GameObject _rack)
    {
        _rack.GetComponent<StoreRack>().InitializeRack();
        storeRacks.Add(_rack);
        newSpawnedRackId++;
        return (newSpawnedRackId-1);
    }
    public void OnPlacedRackSpawned(GameObject _rack)
    {
        _rack.GetComponent<StoreRack>().InitializeRack();
        if (storeRacks == null)
        {
            storeRacks = new List<GameObject>();
        }
        storeRacks.Add(_rack);
    }
    public GameObject GetRack(int _rackId)
    {
        if (storeRacks != null)
        {
            for (int i = 0; i < storeRacks.Count; i++)
            {
                if(storeRacks[i].GetComponent<ItemPickandPlace>().itemsSavingProps.itemUniqueId == _rackId)
                {
                    return storeRacks[i];
                }
            }

        }
        return null;
    }
    #endregion
    #region Saving/Loading DATA
    public IEnumerator InitializeStoreData()
    {
        storeData = (SuperStoreSavableData)SerializationManager.LoadFile("_SuperSoreData");
        if (storeData == null)
        {
            storeData = new SuperStoreSavableData();
        }
        if (storeData.activeCountersIndex == null)
        {
            storeData.activeCountersIndex = new List<int>();
            if(storeData.isStorePurchased)
                storeData.activeCountersIndex.Add(0);
        }
        if(storeData.activeExpensionIndex == null)
        {
            storeData.activeExpensionIndex = new List<int>();
        }
        storeItemsList = new List<SuperStoreItems>();

        if (storeRacks == null)
        {
            storeRacks = new List<GameObject>();
        }
        if (!storeData.initialRacksAssigned)
        {
            yield return null;
            for(int i = 0; i < initialRacks.Length; i++)
            {

                ItemData _item = GameManager.instance.GetItem(initialRacks[i].mainCat, initialRacks[i].subCatId, initialRacks[i].itemId);
                if (_item != null)
                {
                    GameObject _rack = Instantiate(_item.itemPrefab);
                    _rack.GetComponent<ItemPickandPlace>().itemId = _item.itemID;
                    _rack.GetComponent<ItemPickandPlace>().itemName = _item.itemName;

                    ItemSavingProps _props = new ItemSavingProps();
                    _props.mainCatId = (int)_item.mainCatID;
                    _props.subCatId = _item.subCatID;
                    _props.itemId = _item.itemID;
                    _props.itemCount = _item.itemquantity;
                    _props.itemUniqueId = initialRacks[i].rackId;
                    _rack.GetComponent<ItemPickandPlace>().UpdateItemSavingData(_props);
                    _rack.GetComponent<ItemPickandPlace>().AddItemToSavingList();

                    _rack.GetComponent<StoreRack>().OnSpawnInitialRack(initialRacks[i].rackPlacingPoint);
                    storeRacks.Add(_rack);
                }
            }
            storeData.initialRacksAssigned = true;
            yield return null;
        }
        SetNewSpawningRackId();

        for (int i = 0; i < storeData.placedItems.Count; i++)
        {
            SuperStoreItems _item = storeData.placedItems[i];
            if (_item.rackId >= 0 && _item.rackPlaceId >= 0)
            {
                storeItemsList.Add(_item);
                GameObject _rack = GetRack(_item.rackId);
                if (_rack != null)
                {
                    _rack.GetComponent<StoreRack>().SpawnItemToPlace(_item);

                }
                if ((i) % 10 == 9)
                {
                    yield return null;
                }
            }
        }

        for(int i=0; i < storeTrashContainer.childCount; i++)
        {
            storeTrashContainer.GetChild(i).gameObject.SetActive(i >= storeData.trashPickedCounter);
        }
        for (int i = 0; i < storeDustContainer.childCount; i++)
        {
            storeDustContainer.GetChild(i).gameObject.SetActive(i >= storeData.dustRemovedCounter);
        }

        UpdateSuperStoreLocking();

        storeWoodsLockContainer.gameObject.SetActive(!storeData.isStorePurchased);
        superMarketDoor.isLock = !storeData.isStorePurchased;
        //superMarketDoor.isLock = (storeData.doorWoodsRemovedCounter < storeWoodsContainer.childCount);

        removableTrashCount = storeTrashContainer.childCount;

        //setting saved state for counters set their active status
        for (int i = 0; i < storeData.activeCountersIndex.Count; i++)
        {
            int _temp = storeData.activeCountersIndex[i];
            if (_temp < 0 || _temp >= posCounters.Count)
            {
                continue;
            }
            posCounters[_temp].isActive = true;
            EmployeeManager.Instance.SetDeptWorkPlaceLockState(EmployeeType.Cashier, _temp, posCounters[_temp].isActive);
        }

        UpdateCashCountersLockState();

        //Supermarket expension data
        yield return null;

        for (int i = 0; i < storeData.activeExpensionIndex.Count; i++)
        {
            int _temp = storeData.activeExpensionIndex[i];
            if (_temp < 0 || _temp >= superStoreExpension.Length)
            {
                continue;
            }
            superStoreExpension[_temp].isActive = true;
        }

        UpdateSuperMarketExpensionLockState();
        yield return null;
        isDataInitialized = true;
        SetCountersIds();
        GameController.instance.AddSavingAction(SaveStoreData);
    }

    void SaveStoreData()
    {
        if (isDataInitialized)
        {
            SerializationManager.Save(storeData, "_SuperSoreData");
        }
    }
    //private void OnApplicationPause(bool pause)
    //{
    //    if (pause)
    //        SaveStoreData();
    //}
    //private void OnApplicationQuit()
    //{
    //    SaveStoreData();
    //}
    //private void OnDestroy()
    //{
    //    SaveStoreData();
    //}
    #endregion

    #region CounterSpecific

    public Transform GetCustomerCartItemsDestructionPoint(int _id)
    {
        Transform point = null;
        point=posCounters[_id].CashCounterManager.CustomerCartItemsDestructionPoint;
        return point;
    }

    public float GetinitialFieldOfView(int _id)
    {
        float _fieldOfView = -1;
        _fieldOfView = posCounters[_id].CashCounterManager.GetInitialFielOfView();
        return _fieldOfView;
    }

    public Vector3 GetInitialPos(int _id)
    {
        Vector3 _pos;
        _pos = posCounters[_id].CashCounterManager.cameraTransforms.position;
        return _pos;
    }

    public Quaternion GetInitialRotation(int _id)
    {
        Quaternion _rotation;
        _rotation = posCounters[_id].CashCounterManager.cameraTransforms.localRotation;
        return _rotation;
    }

    public void RemoveItemFromList(int _id,GameObject _item)
    {
        posCounters[_id].CashCounterManager.OnRemoveItem(_item);
    }

    public void OnInteractionWithCardSwipeMachine(int _id, string buttonValue)
    {
        posCounters[_id].CashCounterManager.OnInteractionWithCardSwipeMachine(buttonValue);
    }

    public void AddCashierToCounter(int _id,GameObject _cashier)
    {
        if (_cashier == null) print("cashier is null");
        posCounters[_id].CashCounterManager.cashier = _cashier;
    }

    public void RemoveCashierFromCounter(int _id)
    {
        posCounters[_id].CashCounterManager.cashier = null;
    }

    public void SetTotalCustomerServed(int _id)
    {
        posCounters[_id].CashCounterManager.totalCustomersServed = 0;
    }
    public void SettotalCustomerNeedtoServeForCashierRV(int _id)
    {
        posCounters[_id].CashCounterManager.totalCustomerNeedtoServeForCashierRV = 1;
    }

    public List<GameObject> RequestForCartItems(int _id)
    {
      //  print("abc:"+_id);
       return  posCounters[_id].CashCounterManager.CashierRequestForCartItems();
    }

    public void UpdateAnimationForCustomerForCashier(int _id)
    {
        posCounters[_id].CashCounterManager.UpdateAnimationForCustomerForCashier();
    }
    public void OnCashierCheckOut(int _id)
    {
        posCounters[_id].CashCounterManager.OnCashierCheckOut();
    }

    public void OnInteractionWithCashOrCard(int _id, PaymentMethod _paymentMethod)
    {
        print("20e");
        posCounters[_id].CashCounterManager.OnInteractionWithCashOrCard(_paymentMethod);
    }

    public GameObject GetItemPricePrefab(int _id)
    {
        return posCounters[_id].CashCounterManager.itemPricePrefab;
    }

    public void AddToBill(int _id,float _price)
    {
        posCounters[_id].CashCounterManager.AddTotalBill(_price);
    }

    public void PlayCashCounterSound(int _id)
    {
        posCounters[_id].CashCounterManager.PlayCashCounterSound(UIController.instance.uiButtonSound);
    }



    public void DisableCashierRV(int _id)
    {
        posCounters[_id].CashCounterManager.DisableCashierRV();
    }

    public void UpdateAndCheckTotalItemsScanned(int _id)
    {
        posCounters[_id].CashCounterManager.totalItemsToScan--;

        // If no more items are left to scan, trigger the final billing step
        if (posCounters[_id].CashCounterManager.totalItemsToScan <= 0)
        {
            posCounters[_id].CashCounterManager.UponAllItemsScanned();
        }
    }

    public void OnPlayerInteractWithCounter(int _id)
    {
        posCounters[_id].CashCounterManager.PlayerEntersCounter();
    }

    public void LeaveCashCounter(int _id)
    {
        posCounters[_id].CashCounterManager.LeaveCashCounter();
    }

  
    public void CreateCash(int _id,int _index)
    {
        posCounters[_id].CashCounterManager.CreateCash(_index);
    }

    public void OnResetExchangeCurrency(int _id)
    {
        posCounters[_id].CashCounterManager.OnResetExchangeCurrency();
    }

    public void RollBackCurrancy(int _id)
    {
        posCounters[_id].CashCounterManager.RollBackCurrancy();
    }
    public void OnConfirmChange(int _id)
    {
        posCounters[_id].CashCounterManager.OnConfirmChange();
    }

    public void AcceptCustomerWishList(int _id, GameObject customer, List<SuperStoreItems> _list)
    {
        posCounters[_id].CashCounterManager.AcceptWishList(customer, _list);
    }

    /// <summary>
    /// Set these Ids at the start so we dont need to set in each script
    /// </summary>
    public void SetCountersIds()
    {
        for(int i=0;i< posCounters.Count; i++)
        {
            posCounters[i].CashCounterManager.SetCounterID(i);
            posCounters[i].CashCounterManager.pos.SetIdToPOS(i);
            posCounters[i].CashCounterManager.counterCamScript.InitializeData(i);
        }
        print("All counters ids are set");
    }
    public KeyValuePair<int, int> GetEmptyQueuePoint()
    {
        for (int i = 0; i < posCounters.Count; i++)
        {
            if (!posCounters[i].isActive)
            {
                continue;
            }
            if (posCounters[i].isUpdatingQueue)
            {
                continue;
            }

            for (int j = 0; j < posCounters[i].CounterQueue.Length; j++)
            {
                if (posCounters[i].CounterQueue[j].queCustomerRef == null)
                {
                    KeyValuePair<int, int> _stopPoint = new KeyValuePair<int, int>(i, j);
                    return _stopPoint;
                }
            }
        }
        KeyValuePair<int, int> _val = new KeyValuePair<int, int>(-1, -1);
        return _val;
    }

    public bool UnlockCashCounter(int _counterIndex)
    {
        if (!storeData.isStorePurchased)
        {
            UIController.instance.DisplayInstructions("Unlock Super Store First!");
            return false;
        }

        if (_counterIndex < 0)
            return false;

        if (posCounters[_counterIndex].isActive)
        {
            return false;
        }
        if (posCounters[_counterIndex].requiredLevelToUnlock > PlayerDataManager.instance.playerData.playerLevel)
        {
            UIController.instance.EnablePopupNotification("Super Market Counter Will Unlock At Level " + posCounters[_counterIndex].requiredLevelToUnlock.ToString());
            GameManager.instance.CallFireBase("CcUnlkBfLev_" + _counterIndex.ToString());

            return false;
        }
        if (posCounters[_counterIndex].Cost > PlayerDataManager.instance.playerData.playerCash)
        {
            UIController.instance.EnableNoCashPanel();
            GameManager.instance.CallFireBase("NoCshCcMchn_" + _counterIndex.ToString());
            return false;
        }
        storeData.activeCountersIndex.Add(_counterIndex);
        posCounters[_counterIndex].isActive = true;
        posCounters[_counterIndex].lockImage.SetActive(false);
        EmployeeManager.Instance.SetDeptWorkPlaceLockState(EmployeeType.Cashier, _counterIndex, true);

        PlayerDataManager.instance.UpdateCash(-1 * posCounters[_counterIndex].Cost);
        UIController.instance.UpdateCurrency(-1 * posCounters[_counterIndex].Cost);
        //   PlayerDataManager.instance.UpdateXP(posCounters[_counterIndex].Cost / 2);
        // UIController.instance.UpdateXP(posCounters[_counterIndex].Cost / 2);
        //do not show this notification for level 1
        if (_counterIndex > 0)
        {
            UIController.instance.DisplayInstructions("Super Market Cash Counter Unlocked!");
        }
        GameManager.instance.CallFireBase("Cc_" + _counterIndex.ToString() + "_prchsd");
        return true;
    }

    public void UpdateCashCountersLockState()
    {
        for (int i = 0; i < posCounters.Count; i++)
        {
            posCounters[i].lockImage.SetActive(!posCounters[i].isActive);
            print("hmmmmmmmmmmmmm__PlayerLvl__"+PlayerDataManager.instance.playerData.playerLevel);
            print("heeeeeeeeeeeeeee__LevelToUnlock"+posCounters[i].requiredLevelToUnlock);
            bool _isLocked = posCounters[i].requiredLevelToUnlock > PlayerDataManager.instance.playerData.playerLevel;
            for (int j = 0; j < posCounters[i].lockBtnImgs.Length; j++)
            {
                posCounters[i].lockBtnImgs[j].GetComponent<Image>().sprite = _isLocked ? UIController.instance.lock3dBtnImg : UIController.instance.unPurchase3dBtnImg;
                if (j >= posCounters[i].lockPriceTexts.Length)
                {
                    continue;
                }
                posCounters[i].lockPriceTexts[j].GetComponent<TextMeshProUGUI>().color = _isLocked ? UIController.instance.lock3dBtnTxtClr : UIController.instance.unPurchasedBtnTxtClr;
                posCounters[i].lockPriceTexts[j].GetComponent<TextMeshProUGUI>().text = _isLocked ? "Level " + posCounters[i].requiredLevelToUnlock.ToString() : "$" + posCounters[i].Cost.ToString();
            }
        }
    }

    #endregion

    #region SuperMarketExpension


    public bool UnlockSuperMarketExpension(int _expensionIndex)
    {
        if (!storeData.isStorePurchased)
        {
            UIController.instance.DisplayInstructions("Unlock Super Store First!");
            return false;
        }
        if (_expensionIndex < 0)
            return false;

        if (superStoreExpension[_expensionIndex].isActive)
        {
            return false;
        }
        
        print("Super Store  Level Required $$$$$$$$$$$$$$$" + superStoreExpension[_expensionIndex].levelRequired);
        print("Super Store men player Data $$$$$$$$$$$" + PlayerDataManager.instance.playerData.playerLevel);
        print("Expamsion Index $$$$$$$$$$$$" + _expensionIndex);

        if (superStoreExpension[_expensionIndex].levelRequired > PlayerDataManager.instance.playerData.playerLevel)
        {
            UIController.instance.EnablePopupNotification("Super Market expension Will Unlock At Level " + superStoreExpension[_expensionIndex].levelRequired.ToString());
            GameManager.instance.CallFireBase("SSexpUnlkBfLev_" + _expensionIndex.ToString());

            return false;
        }
        if (superStoreExpension[_expensionIndex].expensionCost > PlayerDataManager.instance.playerData.playerCash)
        {
            UIController.instance.EnableNoCashPanel();
            GameManager.instance.CallFireBase("NoCshSSexpMchn_" + _expensionIndex.ToString());

            return false;
        }
        storeData.activeExpensionIndex.Add(_expensionIndex);
        superStoreExpension[_expensionIndex].isActive = true;
        superStoreExpension[_expensionIndex].lockImage.SetActive(false);
      //  EmployeeManager.Instance.SetDeptWorkPlaceLockState(EmployeeType.Cashier, _expensionIndex, true);

        PlayerDataManager.instance.UpdateCash(-1 * superStoreExpension[_expensionIndex].expensionCost);
        UIController.instance.UpdateCurrency(-1 * superStoreExpension[_expensionIndex].expensionCost);
        //   PlayerDataManager.instance.UpdateXP(posCounters[_counterIndex].Cost / 2);
        // UIController.instance.UpdateXP(posCounters[_counterIndex].Cost / 2);
       
       
        UIController.instance.DisplayInstructions("Super Market expension Unlocked!");
       
        GameManager.instance.CallFireBase("Sexp_" + _expensionIndex.ToString() + "_prchsd");
        return true;
    }
    public void UpdateSuperMarketExpensionLockState()
    {
        for (int i = 0; i < superStoreExpension.Length; i++)
        {
            superStoreExpension[i].lockImage.SetActive(!superStoreExpension[i].isActive);
            bool _isLocked = superStoreExpension[i].levelRequired > PlayerDataManager.instance.playerData.playerLevel;
            for (int j = 0; j < superStoreExpension[i].lockBtnImgs.Length; j++)
            {
                superStoreExpension[i].lockBtnImgs[j].GetComponent<Image>().sprite = _isLocked ? UIController.instance.lock3dBtnImg : UIController.instance.unPurchase3dBtnImg;
                if (j >= superStoreExpension[i].lockPriceTexts.Length)
                {
                    continue;
                }
                superStoreExpension[i].lockPriceTexts[j].GetComponent<TextMeshProUGUI>().color = _isLocked ? UIController.instance.lock3dBtnTxtClr : UIController.instance.unPurchasedBtnTxtClr;
                superStoreExpension[i].lockPriceTexts[j].GetComponent<TextMeshProUGUI>().text = _isLocked ? "Level " + superStoreExpension[i].levelRequired.ToString() : "$" + superStoreExpension[i].expensionCost.ToString();
            }
        }
    }

    #endregion
}
[System.Serializable]
public class SuperStoreSavableData
{
    public bool openStatus;
    public bool isStorePurchased;
    public int trashPickedCounter;
    public int dustRemovedCounter;
    public int doorWoodsRemovedCounter;
    public bool initialRacksAssigned;
    //public long spawnedRackCount;// This will be used in future build as user can delete a rack also
    public List<SuperStoreItems> placedItems;
    public List<StoreRacksData> placedRacks;
    public List<int> activeCountersIndex;
    public List<int> activeExpensionIndex;
    public SuperStoreSavableData()
    {
        placedItems = new List<SuperStoreItems>();
        placedRacks = new List<StoreRacksData>();
        activeCountersIndex = new List<int>();
        activeExpensionIndex=new List<int>();
        openStatus = false;
        initialRacksAssigned = true;
    }
}


[System.Serializable]
public class SuperStoreItems
{
    public int catId;
    public int subCatId;
    public int itemId;
    public int rackId;
    public int rackPlaceId;
    public float sellingPrice;
}
[System.Serializable]
public class StoreRacksData
{
    public int catId;
    public int subCatId;
    public int itemId;
}

[System.Serializable]
public class PosCounters
{
    public string counterName;
    public int counterId;
    public Sprite displayImg;
    public CashCounterManager CashCounterManager;
    public CounterPoint[] CounterQueue;
    public int requiredLevelToUnlock;
    public int Cost;
    public bool isActive;
    public bool isUpdatingQueue;

    public GameObject lockImage;
    public GameObject[] lockBtnImgs;
    public GameObject[] lockPriceTexts;
}

[System.Serializable]
public class Supermarketexpension
{
    public string expansionName;
    public int expensionCost;
    public int levelRequired;
    public bool isActive;
    public Sprite displayImg;
    public GameObject lockImage;
    public GameObject[] lockBtnImgs;
    public GameObject[] lockPriceTexts;
}


//public void SpawnNewVehicle()
//{
//    //int _parkingSpotIndex = GetEmptyParkingSpot();
//    //if (_parkingSpotIndex < 0)
//    //{
//    //    spawningTimer += 1f;
//    //    return;
//    //}
//    int _counterIndex = GetEmptyCounterPoint();
//    if (_counterIndex < 0)
//    {
//        spawningTimer += 1f;
//        return;
//    }
//    List<SuperStoreItems> _wishList = CreateCustomersWishList();
//    if (_wishList == null)
//    {
//        spawningTimer += 5f;
//        return;
//    }
//    // parkingPoints[_parkingSpotIndex].isOccupied = true;
//    //int _spawnIndex = Random.Range(0, vehicleRoutePoints.Length);

//    //GameObject _vehicle = Instantiate(availableCars[spawnedVehicleIndex], spawnedVehicleParent);
//    //_vehicle.GetComponent<SuperStoreVehicle>().OnSpawnNewVehicle(spawnedCustomerIndex, _parkingSpotIndex, spawnedVehicleIndex, vehicleRoutePoints[_spawnIndex].routePoints, _wishList);

//    spawnedCustomerIndex++;
//    if (spawnedCustomerIndex >= availableCustomers.Length)
//    {
//        spawnedCustomerIndex = 0;
//    }
//    //spawnedVehicleIndex++;
//    //if (spawnedVehicleIndex >= availableCars.Length)
//    //{
//    //    spawnedVehicleIndex = 0;
//    //}
//    StartSpawningVehicle();
//}

//int GetEmptyParkingSpot()
//{
//    int _emptySpotIndex = -1;
//    for (int i = 0; i < parkingPoints.Length; i++)
//    {
//        if (!parkingPoints[i].isOccupied)
//        {
//            _emptySpotIndex = i;
//            break;
//        }
//    }
//    return _emptySpotIndex;
//}

