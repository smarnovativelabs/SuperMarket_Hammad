using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradesUIManager : MonoBehaviour
{
    public Transform upgradeCatBtnsCntnr;
    public Transform upgradePanelCntnr;
    public GameObject upgrdCatBtnRef;
    public GameObject upgrdCatPnlRef;

    public GameObject upgrdSubCatBtnRef;
    public GameObject upgrdSubCatPnlRef;

    public GameObject upgrdBtnRef;

    public Sprite[] upgrdCatBtnImgs;
    public Color[] upgrdCatBtnTxts;

    public Sprite[] upgrdSubCatBtnImgs;
    public Color[] upgrdSubCatBtnTxts;

    public Sprite[] upgrdBtnImgs;
    public Color[] upgrdBtnTxtClrs;

    public AudioClip uiButtonSound;

    public static UpgradesUIManager instance;
    public List<CategoryPanel> upgradeCategories;

    int currentUpgrdCat = -1;
    int currentSubCat = -1;
    private void Awake()
    {
        instance = this;
    }
    public IEnumerator InitializeUpgradePanels()
    {
        upgradeCategories = new List<CategoryPanel>();
        yield return InitializeMotelUpgrade();
        yield return null;
        yield return InitializGasStationUpgrade();
        yield return null;
        yield return InitializeSuperMarketUpgrade();
        //OnUpgradeCatBtnPressed(0);
        yield return null;
    }
    #region Motel Upgrade Region
    public IEnumerator InitializeMotelUpgrade()
    {
        yield return null;
        CategoryPanel _upgrdPanel = new CategoryPanel();
        
        _upgrdPanel.panelRefs.storeBtn = Instantiate(upgrdCatBtnRef, upgradeCatBtnsCntnr);
        _upgrdPanel.panelRefs.storeBtn.transform.localScale = Vector3.one;

        int _temp = upgradeCategories.Count;
        _upgrdPanel.panelRefs.storeBtn.GetComponent<Button>().onClick.AddListener(() => OnUpgradeCatBtnPressed(_temp));
        _upgrdPanel.panelRefs.storeBtn.transform.GetChild(0).GetComponent<LocalizeText>().UpdateText("Motel");

        _upgrdPanel.panelRefs.storePanel = Instantiate(upgrdCatPnlRef, upgradePanelCntnr);
        _upgrdPanel.panelRefs.storePanel.transform.localScale = Vector3.one;

        _upgrdPanel.panelRefs.storePanel.name = "Motel_Panel";
        _upgrdPanel.panelRefs.storePanel.gameObject.SetActive(false);

        //Sub Panel Implementation
        StorePanelReference _subCatRef = new StorePanelReference();

        // instantiaite sub category btn 
        _subCatRef.storeBtn = Instantiate(upgrdSubCatBtnRef, _upgrdPanel.panelRefs.storePanel.transform.GetChild(0));  // in the top gameobject
        _subCatRef.storeBtn.transform.localScale = Vector3.one;

        _subCatRef.storeBtn.GetComponent<Button>().onClick.AddListener(() => OnUpgradeSubCatBtnPressed(0));

        string _subCatName = "Rooms";
        _subCatRef.storeBtn.transform.GetChild(0).GetComponent<LocalizeText>().UpdateText(_subCatName);
       // _subCatRef.storeBtn.transform.GetChild(1).gameObject.SetActive(!_isSubCatUnlocked);

        _subCatRef.storeBtn.gameObject.name = _subCatName;   // just for understaning in the hierarchy

        // instantiate scroll rect 
        _subCatRef.storePanel = Instantiate(upgrdSubCatPnlRef, _upgrdPanel.panelRefs.storePanel.transform.GetChild(1)); // in the below gameobject
        _subCatRef.storePanel.transform.localScale = Vector3.one;
        _subCatRef.storePanel.gameObject.name = _subCatName + "_Panel";
        _subCatRef.storePanel.gameObject.SetActive(false);

        _upgrdPanel.subPanelRefs.Add(0, _subCatRef);

        // it points to the scroll rect content
        Transform _itemsScroller = _subCatRef.storePanel.transform.GetChild(0).GetChild(0);

        //for (int i = 0; i < RoomManager.instance.rooms.Count; i++)
        //{
        //    Room _room = RoomManager.instance.rooms[i];
        //    // item prefab
        //    GameObject _itemPrefab = Instantiate(upgrdBtnRef, _itemsScroller);
        //    _itemPrefab.transform.localScale = Vector3.one;
        //    _itemPrefab.transform.GetChild(0).GetComponent<Image>().sprite = _room.roomProperties.displayImg;
        //    _itemPrefab.transform.GetChild(0).GetChild(1).GetComponent<LocalizeText>().UpdateText("Room " + _room.roomProperties.roomDisplayNumber.ToString());

        //    _itemPrefab.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "$" + _room.roomProperties.roomPurchasingPrice.ToString();
        //    int _index = i;

        //    _itemPrefab.transform.GetChild(1).GetChild(1).GetComponent<Button>().onClick.AddListener(() => OnUpgradeBtnPressed(_index));
        //    int _lockStatus = GetRoomLockStatus(_index);
        //    string _status = _lockStatus == 0 ? "Level " + _room.roomProperties.reqLevel.ToString() : (_lockStatus == 1 ? "Purchase" : "Owned");
        //    _itemPrefab.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<LocalizeText>().UpdateText(_status);
        //    _itemPrefab.transform.GetChild(1).GetChild(0).gameObject.SetActive(!(_lockStatus == 2));
        //    if (i % 5 == 4)
        //    {
        //        yield return null;
        //    }
        //}
        upgradeCategories.Add(_upgrdPanel);
        yield return null;
        yield return null;
    }
    //int OnPurchaseRoomUpgrade(int _roomIndex)
    //{
    //    Room _room = RoomManager.instance.rooms[_roomIndex];
    //    if (RoomManager.instance.OnPurchaseRoom(_roomIndex))
    //    {
    //        return 2;
    //    }
    //    if (_room.roomSaveable.roomStatus>1)
    //    {
    //        return 2;
    //    }
    //    bool _isLocked = _room.roomProperties.reqLevel > PlayerDataManager.instance.playerData.playerLevel;
    //    int _tempVal = 0;

    //    if (!_isLocked)
    //    {
    //        _tempVal = 1;
    //    }
    //    return _tempVal;
    //}
    //int GetRoomLockStatus(int _roomIndex)
    //{
    //    int _tempVal = 0;
    //    bool _isLocked = RoomManager.instance.rooms[_roomIndex].roomProperties.reqLevel > PlayerDataManager.instance.playerData.playerLevel;
    //    if (!_isLocked)
    //    {
    //        _tempVal = 1;
    //        if (RoomManager.instance.rooms[_roomIndex].roomSaveable.roomStatus>1)
    //        {
    //            _tempVal = 2;
    //        }
    //    }
    //    return _tempVal;
    //}
    #endregion
    #region Gas Station Upgrades
    public IEnumerator InitializGasStationUpgrade()
    {
        yield return null;
        CategoryPanel _upgrdPanel = new CategoryPanel();

        _upgrdPanel.panelRefs.storeBtn = Instantiate(upgrdCatBtnRef, upgradeCatBtnsCntnr);
        _upgrdPanel.panelRefs.storeBtn.transform.localScale = Vector3.one;

        int _temp = upgradeCategories.Count;
        _upgrdPanel.panelRefs.storeBtn.GetComponent<Button>().onClick.AddListener(() => OnUpgradeCatBtnPressed(_temp));
        _upgrdPanel.panelRefs.storeBtn.transform.GetChild(0).GetComponent<LocalizeText>().UpdateText("Gas Station");

        _upgrdPanel.panelRefs.storePanel = Instantiate(upgrdCatPnlRef, upgradePanelCntnr);
        _upgrdPanel.panelRefs.storePanel.transform.localScale = Vector3.one;

        _upgrdPanel.panelRefs.storePanel.name = "GasStation_Panel";
        _upgrdPanel.panelRefs.storePanel.gameObject.SetActive(false);

        //Sub Panel Implementation
        StorePanelReference _subCatRef = new StorePanelReference();

        // instantiaite sub category btn 
        _subCatRef.storeBtn = Instantiate(upgrdSubCatBtnRef, _upgrdPanel.panelRefs.storePanel.transform.GetChild(0));  // in the top gameobject
        _subCatRef.storeBtn.transform.localScale = Vector3.one;

        _subCatRef.storeBtn.GetComponent<Button>().onClick.AddListener(() => OnUpgradeSubCatBtnPressed(0));

        string _subCatName = "Filling Machines";
        _subCatRef.storeBtn.transform.GetChild(0).GetComponent<LocalizeText>().UpdateText(_subCatName);
        // _subCatRef.storeBtn.transform.GetChild(1).gameObject.SetActive(!_isSubCatUnlocked);

        _subCatRef.storeBtn.gameObject.name = _subCatName;   // just for understaning in the hierarchy

        // instantiate scroll rect 
        _subCatRef.storePanel = Instantiate(upgrdSubCatPnlRef, _upgrdPanel.panelRefs.storePanel.transform.GetChild(1)); // in the below gameobject
        _subCatRef.storePanel.transform.localScale = Vector3.one;
        _subCatRef.storePanel.gameObject.name = _subCatName + "_Panel";
        _subCatRef.storePanel.gameObject.SetActive(false);

        _upgrdPanel.subPanelRefs.Add(0, _subCatRef);

        // it points to the scroll rect content
        Transform _itemsScroller = _subCatRef.storePanel.transform.GetChild(0).GetChild(0);

        //for (int i = 0; i < GasStationManager.instance.fillingPoints.Length; i++)
        //{
        //    FillingPoint _fillingPoint = GasStationManager.instance.fillingPoints[i];
        //    // item prefab
        //    GameObject _itemPrefab = Instantiate(upgrdBtnRef, _itemsScroller);
        //    _itemPrefab.transform.localScale = Vector3.one;

        //    _itemPrefab.transform.GetChild(0).GetComponent<Image>().sprite = _fillingPoint.displayImg;
        //    _itemPrefab.transform.GetChild(0).GetChild(1).GetComponent<LocalizeText>().UpdateText(_fillingPoint.displayName);

        //    _itemPrefab.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "$" + _fillingPoint.fillingStationPrice.ToString();
            
        //    int _index = i;
        //    _itemPrefab.transform.GetChild(1).GetChild(1).GetComponent<Button>().onClick.AddListener(() => OnUpgradeBtnPressed(_index));

        //    int _lockStatus = GetFillingPointLockStatus(_index);
        //    string _status = _lockStatus == 0 ? "Level " + _fillingPoint.reqLevel.ToString() : (_lockStatus == 1 ? "Purchase" : "Owned");
        //    _itemPrefab.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<LocalizeText>().UpdateText(_status);
        //    _itemPrefab.transform.GetChild(1).GetChild(0).gameObject.SetActive(!(_lockStatus == 2));

        //    if (i % 5 == 4)
        //    {
        //        yield return null;
        //    }
        //}
        upgradeCategories.Add(_upgrdPanel);
        yield return null;
        yield return null;
    }
    //int OnPurchaseGasStationMachine(int _index)
    //{
    //    FillingPoint _fillinPoint = GasStationManager.instance.fillingPoints[_index];
    //    if (GasStationManager.instance.UnlockFillingPoint(_index))
    //    {
    //        return 2;
    //    }
    //    if (_fillinPoint.isActive)
    //    {
    //        return 2;
    //    }
    //    if (_fillinPoint.reqLevel > PlayerDataManager.instance.playerData.playerLevel)
    //    {
    //        return 0;
    //    }
    //    return 1;
    //}
    //int GetFillingPointLockStatus(int _index)
    //{
    //    FillingPoint _fillingPoint = GasStationManager.instance.fillingPoints[_index];
    //    if (_fillingPoint.isActive)
    //    {
    //        return 2;
    //    }
    //    if (_fillingPoint.reqLevel > PlayerDataManager.instance.playerData.playerLevel)
    //    {
    //        return 0;
    //    }
    //    return 1;
    //}
    #endregion
    #region SuperMarket Upgrade
    IEnumerator InitializeSuperMarketUpgrade()
    {
        yield return null;
        CategoryPanel _upgrdPanel = new CategoryPanel();

        _upgrdPanel.panelRefs.storeBtn = Instantiate(upgrdCatBtnRef, upgradeCatBtnsCntnr);
        _upgrdPanel.panelRefs.storeBtn.transform.localScale = Vector3.one;

        int _temp = upgradeCategories.Count;
        _upgrdPanel.panelRefs.storeBtn.GetComponent<Button>().onClick.AddListener(() => OnUpgradeCatBtnPressed(_temp));
        _upgrdPanel.panelRefs.storeBtn.transform.GetChild(0).GetComponent<LocalizeText>().UpdateText("SuperMarket");

        _upgrdPanel.panelRefs.storePanel = Instantiate(upgrdCatPnlRef, upgradePanelCntnr);
        _upgrdPanel.panelRefs.storePanel.transform.localScale = Vector3.one;

        _upgrdPanel.panelRefs.storePanel.name = "SuperStore_Panel";
        _upgrdPanel.panelRefs.storePanel.gameObject.SetActive(false);
        float _subCatBtnPos = -10;

        for (int j = 0; j < 2; j++)
        {
            //Sub Panel Implementation
            StorePanelReference _subCatRef = new StorePanelReference();

            // instantiaite sub category btn 
            _subCatRef.storeBtn = Instantiate(upgrdSubCatBtnRef, _upgrdPanel.panelRefs.storePanel.transform.GetChild(0));  // in the top gameobject
            _subCatRef.storeBtn.transform.localScale = Vector3.one;
            _subCatRef.storeBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, _subCatBtnPos);
            int _k = j;
            _subCatRef.storeBtn.GetComponent<Button>().onClick.AddListener(() => OnUpgradeSubCatBtnPressed(_k));
            _subCatBtnPos -= (_subCatRef.storeBtn.GetComponent<RectTransform>().sizeDelta.y + 5f);

            string _subCatName = j == 0 ? "POS Counters" : "Expansions";

            _subCatRef.storeBtn.transform.GetChild(0).GetComponent<LocalizeText>().UpdateText(_subCatName);
            _subCatRef.storeBtn.gameObject.name = _subCatName;   // just for understaning in the hierarchy

            _subCatRef.storeBtn.GetComponent<Image>().sprite = upgrdSubCatBtnImgs[0];
            _subCatRef.storeBtn.transform.GetChild(0).GetComponent<Text>().color = upgrdSubCatBtnTxts[0];

            // instantiate scroll rect 
            _subCatRef.storePanel = Instantiate(upgrdSubCatPnlRef, _upgrdPanel.panelRefs.storePanel.transform.GetChild(1)); // in the below gameobject
            _subCatRef.storePanel.transform.localScale = Vector3.one;
            _subCatRef.storePanel.gameObject.name = _subCatName + "_Panel";
            _subCatRef.storePanel.gameObject.SetActive(false);

            _upgrdPanel.subPanelRefs.Add(_k, _subCatRef);
            // it points to the scroll rect content
            Transform _itemsScroller = _subCatRef.storePanel.transform.GetChild(0).GetChild(0);
            if (j == 0)
            {
                for (int i = 0; i < SuperStoreManager.instance.posCounters.Count; i++)
                {
                    PosCounters _counter = SuperStoreManager.instance.posCounters[i];
                    // item prefab
                    GameObject _itemPrefab = Instantiate(upgrdBtnRef, _itemsScroller);
                    _itemPrefab.transform.localScale = Vector3.one;
                    _itemPrefab.transform.GetChild(0).GetComponent<Image>().sprite = _counter.displayImg;

                    _itemPrefab.transform.GetChild(0).GetChild(1).GetComponent<LocalizeText>().UpdateText(_counter.counterName);

                    _itemPrefab.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "$" + _counter.Cost.ToString();
                    int _index = i;
                    int _itemType = j;
                    _itemPrefab.transform.GetChild(1).GetChild(1).GetComponent<Button>().onClick.AddListener(() => OnUpgradeBtnPressed(_index));
                    int _lockStatus = GetStoreItemLockStatus(_itemType, _index);

                    string _status = _lockStatus == 0 ? "Level " + _counter.requiredLevelToUnlock.ToString() : (_lockStatus == 1 ? "Purchase" : "Owned");
                    _itemPrefab.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<LocalizeText>().UpdateText(_status);
                    _itemPrefab.transform.GetChild(1).GetChild(0).gameObject.SetActive(!(_lockStatus == 2));
                    if (i % 5 == 4)
                    {
                        yield return null;
                    }
                }
            }
            else
            {
                for (int i = 0; i < SuperStoreManager.instance.superStoreExpension.Length; i++)
                {
                    Supermarketexpension _expansion = SuperStoreManager.instance.superStoreExpension[i];
                    // item prefab
                    GameObject _itemPrefab = Instantiate(upgrdBtnRef, _itemsScroller);
                    _itemPrefab.transform.localScale = Vector3.one;
                    _itemPrefab.transform.GetChild(0).GetComponent<Image>().sprite = _expansion.displayImg;
                    _itemPrefab.transform.GetChild(0).GetChild(1).GetComponent<LocalizeText>().UpdateText(_expansion.expansionName);
                    _itemPrefab.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "$" + _expansion.expensionCost.ToString();
                    int _index = i;
                    int _itemType = j;

                    _itemPrefab.transform.GetChild(1).GetChild(1).GetComponent<Button>().onClick.AddListener(() => OnUpgradeBtnPressed(_index));
                    int _lockStatus = GetStoreItemLockStatus(_itemType, _index);
                    string _status = _lockStatus == 0 ? "Level " + _expansion.levelRequired.ToString() : (_lockStatus == 1 ? "Purchase" : "Owned");
                    _itemPrefab.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<LocalizeText>().UpdateText(_status);
                    _itemPrefab.transform.GetChild(1).GetChild(0).gameObject.SetActive(!(_lockStatus == 2));
                    if (i % 5 == 4)
                    {
                        yield return null;
                    }
                }
            }
        }
        upgradeCategories.Add(_upgrdPanel);
        yield return null;
        yield return null;
    }

    int GetStoreItemLockStatus(int _itemType,int _index)
    {
        if(_itemType==0) // For Counters
        {
            PosCounters _counters = SuperStoreManager.instance.posCounters[_index];
            if (_counters.isActive)
            {
                return 2;
            }
            if (_counters.requiredLevelToUnlock > PlayerDataManager.instance.playerData.playerLevel)
            {
                return 0;
            }
        }
        else //For Expansions
        {
            Supermarketexpension _expansion = SuperStoreManager.instance.superStoreExpension[_index];
            if (_expansion.isActive)
            {
                return 2;
            }
            if (_expansion.levelRequired > PlayerDataManager.instance.playerData.playerLevel)
            {
                return 0;
            }
        }
        return 1;
    }
    int OnPurchaseStoreItem(int _itemType,int _index)
    {
        if (_itemType == 0) // For Counters
        {
            PosCounters _counters = SuperStoreManager.instance.posCounters[_index];
            if (SuperStoreManager.instance.UnlockCashCounter(_index))
            {
                return 2;
            }
            if (_counters.isActive)
            {
                return 2;
            }
            if (_counters.requiredLevelToUnlock > PlayerDataManager.instance.playerData.playerLevel)
            {
                return 0;
            }
        }
        else //For Expansions
        {
            Supermarketexpension _expansion = SuperStoreManager.instance.superStoreExpension[_index];
            if (SuperStoreManager.instance.UnlockSuperMarketExpension(_index))
            {
                return 2;
            }
            if (_expansion.isActive)
            {
                return 2;
            }
            if (_expansion.levelRequired > PlayerDataManager.instance.playerData.playerLevel)
            {
                return 0;
            }
        }
        return 1;
    }
    #endregion
    public void OnUpgradeCatBtnPressed(int _upgrdIndex)
    {
        if (_upgrdIndex == currentUpgrdCat)
        {
            UpdateLockStatus();
            return;
        }
        SoundController.instance.OnPlayInteractionSound(uiButtonSound);

        if (currentUpgrdCat > -1)
        {
            upgradeCategories[currentUpgrdCat].panelRefs.storeBtn.GetComponent<Image>().sprite = upgrdCatBtnImgs[0];
            upgradeCategories[currentUpgrdCat].panelRefs.storeBtn.transform.GetChild(0).GetComponent<Text>().color = upgrdCatBtnTxts[0];
            upgradeCategories[currentUpgrdCat].panelRefs.storePanel.SetActive(false);
            if (upgradeCategories[currentUpgrdCat].subPanelRefs.ContainsKey(currentSubCat))
            {
                upgradeCategories[currentUpgrdCat].subPanelRefs[currentSubCat].storePanel.SetActive(false);
                upgradeCategories[currentUpgrdCat].subPanelRefs[currentSubCat].storeBtn.GetComponent<Image>().sprite = upgrdSubCatBtnImgs[0];
                upgradeCategories[currentUpgrdCat].subPanelRefs[currentSubCat].storeBtn.transform.GetChild(0).GetComponent<Text>().color = upgrdSubCatBtnTxts[0];
            }
            if (upgradeCategories[currentUpgrdCat].subPanelRefs.ContainsKey(currentSubCat))
            {
                upgradeCategories[currentUpgrdCat].subPanelRefs[currentSubCat].storePanel.SetActive(false);
                upgradeCategories[currentUpgrdCat].subPanelRefs[currentSubCat].storeBtn.GetComponent<Image>().sprite = upgrdSubCatBtnImgs[0];
                upgradeCategories[currentUpgrdCat].subPanelRefs[currentSubCat].storeBtn.transform.GetChild(0).GetComponent<Text>().color = upgrdSubCatBtnTxts[0];
                upgradeCategories[currentUpgrdCat].subPanelRefs[currentSubCat].storeBtn.GetComponent<Button>().interactable = true;  // make currently selected subcategory button not interactable 
            }

        }
        currentUpgrdCat = _upgrdIndex;
        upgradeCategories[currentUpgrdCat].panelRefs.storeBtn.GetComponent<Image>().sprite = upgrdCatBtnImgs[1];
        upgradeCategories[currentUpgrdCat].panelRefs.storeBtn.transform.GetChild(0).GetComponent<Text>().color = upgrdCatBtnTxts[1];
        upgradeCategories[currentUpgrdCat].panelRefs.storePanel.SetActive(true);

        

        currentSubCat = -1;
        OnUpgradeSubCatBtnPressed(0);
    }
    public void OnUpgradeSubCatBtnPressed(int _subUpgrdIndex)
    {
        if (currentSubCat == _subUpgrdIndex)
        {
            return;
        }
        SoundController.instance.OnPlayInteractionSound(uiButtonSound);
        if (upgradeCategories[currentUpgrdCat].subPanelRefs.ContainsKey(currentSubCat))
        {
            upgradeCategories[currentUpgrdCat].subPanelRefs[currentSubCat].storePanel.SetActive(false);
            upgradeCategories[currentUpgrdCat].subPanelRefs[currentSubCat].storeBtn.GetComponent<Image>().sprite = upgrdSubCatBtnImgs[0];
            upgradeCategories[currentUpgrdCat].subPanelRefs[currentSubCat].storeBtn.transform.GetChild(0).GetComponent<Text>().color = upgrdSubCatBtnTxts[0];
            upgradeCategories[currentUpgrdCat].subPanelRefs[currentSubCat].storeBtn.GetComponent<Button>().interactable = true;  // make currently selected subcategory button not interactable 
        }

        if (upgradeCategories[currentUpgrdCat].subPanelRefs.ContainsKey(_subUpgrdIndex))
        {
            upgradeCategories[currentUpgrdCat].subPanelRefs[_subUpgrdIndex].storeBtn.GetComponent<Button>().interactable = false;  // make currently selected subcategory button not interactable 
            upgradeCategories[currentUpgrdCat].subPanelRefs[_subUpgrdIndex].storePanel.SetActive(true);  // show currently selected subcategory panel
            upgradeCategories[currentUpgrdCat].subPanelRefs[_subUpgrdIndex].storeBtn.GetComponent<Image>().sprite = upgrdSubCatBtnImgs[1];
            upgradeCategories[currentUpgrdCat].subPanelRefs[_subUpgrdIndex].storeBtn.transform.GetChild(0).GetComponent<Text>().color = upgrdSubCatBtnTxts[1];

            currentSubCat = _subUpgrdIndex;

            Transform _itemsScroller = upgradeCategories[currentUpgrdCat].subPanelRefs[currentSubCat].storePanel.transform.GetChild(0).GetChild(0);
            StartCoroutine(UpdateItemsScrollerPanel(_itemsScroller));
            UpdateLockStatus();
        }
    }
    IEnumerator UpdateItemsScrollerPanel(Transform _scrollPanel)
    {
        yield return null;
        int _count = _scrollPanel.childCount;
        float _scrollLength = _scrollPanel.GetComponent<RectTransform>().rect.width;
        GridLayoutGroup _grid = _scrollPanel.GetComponent<GridLayoutGroup>();
        _scrollLength -= _grid.padding.left;
        _scrollLength -= _grid.padding.right;
        int _perRowItems = (int)(_scrollLength / (_grid.cellSize.x + _grid.spacing.x));

        if (_scrollLength >= ((_perRowItems * (_grid.cellSize.x + _grid.spacing.x)) + _grid.cellSize.x))
        {
            _perRowItems++;
        }
        int _totalRows = Mathf.CeilToInt(((float)_count / _perRowItems));
        float _height = _totalRows * (_grid.cellSize.y + _grid.spacing.y);
        _height += _grid.padding.top;
        _scrollPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(_scrollPanel.GetComponent<RectTransform>().sizeDelta.x, _height);
    }

    public void OnUpgradeBtnPressed(int _index)
    {
        if(currentUpgrdCat<0 || currentSubCat < 0)
        {
            return;
        }
        if (currentUpgrdCat >= upgradeCategories.Count)
        {
            return;
        }
        if (!upgradeCategories[currentUpgrdCat].subPanelRefs.ContainsKey(currentSubCat))
        {
            return;
        }
        int _upgradeStatus = 0;
        string _status = "Purchase";
        switch (currentUpgrdCat)
        {
            case 0: //Room Upgraded
               // _upgradeStatus = OnPurchaseRoomUpgrade(_index);
                //_status = "Level " + RoomManager.instance.rooms[_index].roomProperties.reqLevel.ToString();
                break;
            case 1: //Gas Station Upgrade
                //_upgradeStatus = OnPurchaseGasStationMachine(_index);
               // _status = "Level " + GasStationManager.instance.fillingPoints[_index].reqLevel.ToString();
                break;
            case 2: //SuperMarketUpgrade
                _upgradeStatus = OnPurchaseStoreItem(currentSubCat, _index);
                if (currentSubCat == 0)    //For Store COunters
                {
                    _status = "Level " + SuperStoreManager.instance.posCounters[_index].requiredLevelToUnlock.ToString();
                }
                else    // For Store Expansions
                {
                    _status = "Level " + SuperStoreManager.instance.superStoreExpension[_index].levelRequired.ToString();
                }
                break;
        }
        Transform _btnRef = upgradeCategories[currentUpgrdCat].subPanelRefs[currentSubCat].storePanel.transform.GetChild(0).GetChild(0).GetChild(_index);
        _btnRef.GetChild(1).GetChild(1).GetComponent<Image>().sprite = upgrdBtnImgs[_upgradeStatus];
        _btnRef.GetChild(1).GetChild(1).GetChild(0).GetComponent<Text>().color = upgrdBtnTxtClrs[_upgradeStatus];

        if (_upgradeStatus == 1) _status = "Purchase";
        else if (_upgradeStatus == 2) _status = "Owned";

        _btnRef.GetChild(1).GetChild(1).GetChild(0).GetComponent<LocalizeText>().UpdateText(_status);
        _btnRef.transform.GetChild(1).GetChild(0).gameObject.SetActive(!(_upgradeStatus == 2));

    }

    public void UpdateLockStatus()
    {
        if (currentUpgrdCat < 0 || currentSubCat < 0)
        {
            return;
        }
        if (currentUpgrdCat >= upgradeCategories.Count)
        {
            return;
        }
        if (!upgradeCategories[currentUpgrdCat].subPanelRefs.ContainsKey(currentSubCat))
        {
            return;
        }
        Transform _scroller = upgradeCategories[currentUpgrdCat].subPanelRefs[currentSubCat].storePanel.transform.GetChild(0).GetChild(0);
        for(int i = 0; i < _scroller.childCount; i++)
        {
            int _index = i;
            int _tempVal = 0;
            string _status = "";
            switch (currentUpgrdCat)
            {
               // case 0: //Room Upgraded
                  //  _tempVal = GetRoomLockStatus(_index);
                   // _status = "Level " + RoomManager.instance.rooms[_index].roomProperties.reqLevel.ToString();

                    //break;
                case 1:
                 //   _tempVal = GetFillingPointLockStatus(_index);
                  //  _status = "Level " + GasStationManager.instance.fillingPoints[_index].reqLevel.ToString();
                    break;
                case 2:  //SuperStore Upgraded
                    _tempVal = GetStoreItemLockStatus(currentSubCat, _index);
                    if (currentSubCat == 0)    //For Store COunters
                    {
                        _status = "Level " + SuperStoreManager.instance.posCounters[_index].requiredLevelToUnlock.ToString();
                    }
                    else    // For Store Expansions
                    {
                        _status = "Level " + SuperStoreManager.instance.superStoreExpension[_index].levelRequired.ToString();
                    }
                    break;
            }
            _scroller.GetChild(i).GetChild(1).GetChild(1).GetComponent<Image>().sprite = upgrdBtnImgs[_tempVal];
            _scroller.GetChild(i).GetChild(1).GetChild(1).GetChild(0).GetComponent<Text>().color = upgrdBtnTxtClrs[_tempVal];
            if (_tempVal == 1) _status = "Purchase";
            else if (_tempVal == 2) _status = "Owned";

            _scroller.GetChild(i).GetChild(1).GetChild(1).GetChild(0).GetComponent<LocalizeText>().UpdateText(_status);
            _scroller.GetChild(i).transform.GetChild(1).GetChild(0).gameObject.SetActive(!(_tempVal == 2));

        }
    }
}

