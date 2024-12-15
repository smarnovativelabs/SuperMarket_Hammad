using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public int dayCycleMinutes = 4;
    public float autoSaveTimeInterval = 180f;
    public Camera gameMainCamera;
    public Material invalidPlacerMaterial;
    public GameObject parentOFPickedObj;
    public GameObject FurnitureParent;
    public GameObject toolParent;

    public ItemData currentItemData;

    public GameObject currentPicketItem;
    public GameObject currentPickedTool;
    bool canPickItem = true;
    public GameData gameData;
    bool gameStarted = false;
    public delegate void ChangeGameStatus(bool _enable);
    public ChangeGameStatus changeGameStatus;
    List<UnityAction> savingActions;
    
    float adsTimer;
    bool isDataInitialized = false;
    float firebaseTimer = 0f;
    float autoSaveTimer = 0f;
    int firebaseQuarter = 0;
    bool isSavingData = false;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    
    void Start()
    {
        parentOFPickedObj.SetActive(true);
        Time.timeScale = 1;
        StartCoroutine(InitilizeGame());
        canPickItem = true;
    }

    IEnumerator InitilizeGame()
    {
        GamePlaySound.instance.OnChangeMusicGamePlay();
        changeGameStatus += UpdateGameStatus;
        savingActions = new List<UnityAction>();
        AddSavingAction(SaveGameData);
        AddSavingAction(PlayerDataManager.instance.SavePlayerData);
        GameManager.instance.EnableLoadingScreen(true, "Loading Game Data");
        yield return null;
        EnvironmentManager.instance.SetEnvironmentLights();
        yield return new WaitForSeconds(0.5f);
        bool _existingRewardGiven = false;
        if (PlayerDataManager.instance.playerData.playerRewardGivenState < 1)
        {
            GameManager.instance.EnableLoadingScreen(true, "Loading New Environment");
            yield return new WaitForSeconds(0.5f);
            yield return TotalSpendingCalculator.instance.CalculateTotalSpendings();
            int _playerCash = PlayerDataManager.instance.playerData.playerCash;
            PlayerDataManager.instance.playerData = new PlayerData();
            PlayerDataManager.instance.playerData.playerCash = _playerCash;
            PlayerDataManager.instance.UpdateCash(TotalSpendingCalculator.instance.totalSpent);
            if (PlayerDataManager.instance.playerData.playerCash < 1000)
            {
                PlayerDataManager.instance.playerData.playerCash = 1000;
            }
            if (TotalSpendingCalculator.instance.totalSpent > 0)
            {
                _existingRewardGiven = true;
            }
        }
        gameData = (GameData)SerializationManager.LoadFile("_gameData");
        if (gameData == null)
        {
            gameData = new GameData();
        }
        GameManager.instance.EnableLoadingScreen(true, "Loading Purchased Items Data");
        yield return StartCoroutine(ItemsSavingManager.instance.LoadAndSpawnItems());
        yield return null;
   //     GameManager.instance.EnableLoadingScreen(true, "Loading Rooms Data");
       // yield return RoomManager.instance.InitlizeLoadingRoomProperties();
  //      yield return null;
   //     GameManager.instance.EnableLoadingScreen(true, "Loading Gas Station Data");
        //GasStationManager.instance.InitializeGasStationData();
        GameManager.instance.EnableLoadingScreen(true, "Loading Super Store Data");
        yield return SuperStoreManager.instance.InitializeStoreData();
        yield return null;
   //     GameManager.instance.EnableLoadingScreen(true, "Loading Pool Data");
        //yield return PoolManager.instance.InitializePoolData();
   //     yield return null;
        CleanerManager.instance.EnableAllCleanersUponRequiredLevelReached();
        yield return null;
        GameManager.instance.EnableLoadingScreen(true, "Loading Employees Data");
        yield return EmployeeManager.Instance.InitializeEmployeeData();
        //yield return LevelManager.Instance.InitializeLevelUpData();
        yield return null;
        
        GameManager.instance.EnableLoadingScreen(true, "Setting Up Game");

        yield return UIController.instance.InitializeUIController();
        yield return null;
        GameManager.instance.EnableLoadingScreen(true, "Finalizing Data");

        IAPUiManager.Instance.InitializeIAPPanel();
        yield return new WaitForSeconds(0.1f);
        UpdateGameQualitySettings(PlayerDataManager.instance.playerData.selectedQualitySettings);
        isDataInitialized = true;
        yield return new WaitForSeconds(0.1f);
         CleanerManager.instance.EnableClanersRV();
        if (gameData.motelOpenStatus)
        {
            OnOpenMotel(true);
        }
        if (gameData.stationOpenStatus)
        {
            OnOpenFuelStation();
        }
        if (gameData.superStoreOpenStatus)
        {
            OnOpenSuperStore();
        }
        UpdatePlayerSensitivity(PlayerPrefs.GetFloat("Sensitivity", 8));
        GameManager.instance.EnableLoadingScreen(false);
        if (_existingRewardGiven)
        {
            //Display Existing User A Reward Panel
            UIController.instance.EnableExistingUserUpdatePanel(TotalSpendingCalculator.instance.totalSpent);
        }
        else
        {
            UIController.instance.SetPlayerCurrency();
        }
        changeGameStatus?.Invoke(true);

        MonetizationManager.instance.InitilizeMonetizationManager();
        yield return null;
        EnvironmentManager.instance.StartEnvironmentCycle();

    }
    public void OnOpenMotel(bool _open=true)
    {
        gameData.motelOpenStatus = _open;
        CustomerManager.instance.OpenMotel(_open);
    }
    public void OnOpenFuelStation(bool _open=true)
    {
        gameData.stationOpenStatus = _open;
        //GasStationManager.instance.OpenFuelStation(_open);
    }
    public void OnOpenSuperStore(bool _open = true)
    {
        gameData.superStoreOpenStatus = _open;
        SuperStoreManager.instance.OpenSuperStore(_open);
    }

    public void OnOpenPool(bool _open = true)
    {
        gameData.poolOpenStatus = _open;
        //PoolManager.instance.OpenPool(_open);
    }

    public void UpdatePlayerSensitivity(float _val)
    {
        Controlsmanager.instance.UpdateSensitivity(_val);
    }
    public void UpdateGameQualitySettings(int _val)
    {
        switch (_val)
        {
            case 1:
                gameMainCamera.fieldOfView = 45;
                gameMainCamera.farClipPlane = 60;
                break;
            case 2:
                gameMainCamera.fieldOfView = 55;
                gameMainCamera.farClipPlane = 75;
                break;
            case 3:
                gameMainCamera.fieldOfView = 60;
                gameMainCamera.farClipPlane = 300;
                break;
        }
    }
    void UpdateGameStatus(bool _started)
    {
        gameStarted = _started;
    }
    public void CanPickItem(bool _canPick)
    {
        canPickItem = _canPick;
    }

    public bool IsAlreadyPickedItem()
    {
        bool _alreadyPicked = currentPicketItem != null;
        if (!_alreadyPicked)
        {
            _alreadyPicked = !canPickItem;
        }
        return _alreadyPicked;
    }
    void Update()
    {
        if (gameStarted)
        {
         //   UpdateGameTime();
            UpdateAdsTimer();
            UpdateFIrebaseQuarter();
            UpdateAutoSavingTimer();
        }
    }
    void UpdateGameTime()
    {
        gameData.currentTime += Time.deltaTime;
        if (gameData.currentTime > (dayCycleMinutes * 60f))
        {
            gameData.currentTime = 0f;
            gameData.currentDay++;
        }
        UIController.instance.UpdateGameTime(gameData.currentTime, gameData.currentDay);
    }
    void UpdateAdsTimer()
    {
        int _difference = Mathf.CeilToInt(GameManager.instance.inGameAdsTimer - adsTimer);
        if (_difference > 5)
        {
            adsTimer += Time.deltaTime;
            UIController.instance.UpdateAdsTimer(-1);
        }
        //else if(AdsMediation.AdsMediationManager.instance.CanShowInterstitial())
        //{
        //    adsTimer += Time.deltaTime;
        //    UIController.instance.UpdateAdsTimer(_difference);
        //    if (_difference < 1)
        //    {
        //        adsTimer = 0f;
        //        AdsMediation.AdsMediationManager.instance.ShowInterstitial();
        //        EnvInteract.instance.OnInGameAd();
        //    }
        //}
        else
        {
            UIController.instance.UpdateAdsTimer(-1);
        }
    }
    public void ResetAdsTimer()
    {
        adsTimer = 0f;
        UIController.instance.UpdateAdsTimer(-1);

    }
    void UpdateFIrebaseQuarter()
    {
        firebaseTimer += Time.deltaTime;
        if (firebaseTimer >= 240f)
        {
            firebaseTimer = 0f;
            firebaseQuarter++;
           // GameManager.instance.CallFireBase("Q_" + firebaseQuarter.ToString(), "quarter", firebaseQuarter);
        }
    }
    public void UpdateCurrentPickedItem(GameObject _item)
    {
        currentPicketItem = _item;
    }

    public void UpdateCurrentPickedTool(GameObject _tool)
    {
        currentPickedTool = _tool;
    }

    #region Saving Implementation
    void UpdateAutoSavingTimer()
    {
        if (isSavingData)
        {
            return;
        }
        
        autoSaveTimer += Time.deltaTime;
        if (autoSaveTimer >= autoSaveTimeInterval)
        {
            autoSaveTimer = 0;
            SaveData(false);
        }
    }
    public void AddSavingAction(UnityAction _action)
    {
        savingActions.Add(_action);
    }
    public void SaveData(bool _SavingOnLeaving = false)
    {
        if (_SavingOnLeaving)
        {
            for (int i = 0; i < savingActions.Count; i++)
            {
                savingActions[i].Invoke();
            }
            autoSaveTimer = 0f;
            return;
        }
        if (isSavingData)
        {
            return;
        }
        isSavingData = true;
        StartCoroutine(SavePeriodicData());
    }
    enum SavingMsgType
    {
        InProgress=0,
        NotSaved=1,
        Completed=2
    }
    IEnumerator SavePeriodicData()
    {
        UIController.instance.EnableSavingProgressNotification(true);
        UIController.instance.UpdateSavingProgressText("Saving Progress!", (int)SavingMsgType.InProgress);
        SavingMsgType _type = SavingMsgType.Completed;
        string _msg = "Progress Saved!";
        for (int i = 0; i < savingActions.Count; i++)
        {
            savingActions[i].Invoke();
            if (i % 5 == 4)
            {
                yield return null;
            }
        }
        yield return new WaitForSecondsRealtime(0.5f);

        UIController.instance.UpdateSavingProgressText(_msg, (int)_type);

        yield return new WaitForSecondsRealtime(2f);
        UIController.instance.EnableSavingProgressNotification(false);

        autoSaveTimer = 0f;
        isSavingData = false;
    }
    public bool IsSavingInProgress()
    {
        return isSavingData;
    }
    void SaveGameData()
    {
        if (!isDataInitialized)
            return;
        if (gameData != null)
        {
            SerializationManager.Save(gameData, "_gameData");
        }
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SaveData(true);
        }
    }
    private void OnApplicationQuit()
    {
     //   SaveData(true);
    }
    private void OnDestroy()
    {
    //    SaveData(true);
    }
    #endregion
}

[System.Serializable]
public class GameData
{
    public int currentDay;
    public float currentTime;
    public bool motelOpenStatus;
    public bool stationOpenStatus;
    public bool superStoreOpenStatus;
    public bool poolOpenStatus;

    public GameData()
    {
        currentDay = 1;
        currentTime = 0f;
        motelOpenStatus = false;
        stationOpenStatus = false;
        superStoreOpenStatus = false;
        poolOpenStatus = false;
    }
}
[System.Serializable]
public enum ObjectRelavance
{
    SuperStore,
    Room,
    Reception,
    Pool
}