using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using CandyCoded.HapticFeedback;
using static PlayerVechicle;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    [SerializeField] private Text hoverItemNameText;
    [SerializeField] private Text instructionsText;
    public Text CashText;
    public Text CashTextAnimation;

    public Text BlitzText;
    public Text BlitzTextAnimation;

    public Image xpBar;
    public Text XPText;
    public Text XPTextAnimation;


    public Text motelLevelText;

    [Header("Timer in Main Screen")]
    public Text dayDisplay; // Reference to a UI Text for the day
    public Text timeDisplay; // Reference to a UI Text for the time
    public Text adsTimerDisplay;
    private string[] daysOfWeek = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

    [Header("Game Play UI")]
    public GameObject pickButton;
    public GameObject dynamicPlaceBtnsContainer;
    public Sprite[] interactButtons;
    public Sprite[] hoverInsImgs;
    public Image motelToggleImage;
    public Image superStoreToggle;
    public Image poolToggleImage;
    public Text existinUserUpdateRewardText;
    public Sprite lock3dBtnImg;
    public Sprite unPurchase3dBtnImg;
    public Color lock3dBtnTxtClr;
    public Color unPurchasedBtnTxtClr;
    public GameObject savingNotification;
    public Text savingNotificationText;
    public Image savingNotificationImg;
    public Sprite[] savingNotificationImgs;

    [Header("Notification UI")]
    public GameObject notificationPopupPanel;
    public Text notificationText;

    public Sprite[] toggleSprite;
    public Text tutorialPanelText;
    public Text motelCustomerWaitingText;
    public Text superStoreCustomerWaitingText;
    public Text gasStationCustomerWaitingText;
    public Text gasStationCustomerTimerText;

    [Header("Fuel Fields")]
    public Image fuelToggleImage;
    public Text homeFuelreserveTxt;
    public Image homeFuelBar;
    public Slider fuelOrderSlider;
    public Text fuelPrice;
    public Text orderFuelQuantity;
    public Text orderFuelPrice;
    public Text fuelreserveText;

    [Header("Super Store Fields")]
    public GameObject cashBillingPanel;
    public GameObject cardBillingPanel;
    public GameObject leaveCounterBtn;
    public Text cardEnterAmountText;
    public GameObject centAnimationRef;
    public GameObject cardAnimationRef;

    [Header("Panels")]
    public GameObject pausePanel;
    public GameObject toolOpenPanel;
    public GameObject pcClickPanel;
    public GameObject boxClickPanel;
    public GameObject paintCountContainer;
    public GameObject gameProgressContainer;
    public GameObject counterCheckOutContainer;
    public GameObject customerRequestPanel;
    public GameObject notEnoughCashPanel;
    public GameObject fpsModePanel;
    public GameObject counterModePanel;
    public GameObject existingUserUpdateRewardPanel;

    [Header("Hiring Staff RV Panel")]
    public GameObject hiringRvPanel;
    public Text hiringHeadingText;
    public Text hiringTimeText;
    public Text rvCountText;
    [Header("PC Panel References")]
    public PCTabObjects[] pcTabs;
    public Sprite[] tabBtnsSprites;
    public Color[] tabTextColors;


    [Header("Store Panel Parent References")]
    [SerializeField] GameObject pcHomePanel;
    [SerializeField] GameObject pcFuelPanel;
    //[SerializeField] Button restStoreBtn;   // in UI
    [SerializeField] GameObject storeCatBtnsContent;    // cateogries btn parent  where we instantiate
    [SerializeField] GameObject storeCatBtnPrefab;   // main categories btn prefab
    [Space(1)]
    [SerializeField] GameObject storeSubCatsPrefab;   // right side parent of the store panel
    [SerializeField] GameObject storeSubCatsParent;    // parent where we instantiate
    [SerializeField] GameObject storeCartPanel;
    [Space(1)]
    [SerializeField] GameObject storeSubCatBtnPrefab;  // sub categories btn prefab
    [Space(1)]
    [SerializeField] GameObject storeSubCatScrollPrefab;
    [SerializeField] GameObject storeItemPrefab;  // item prefab
    [Space(1)]                                       //  int previousStoreCatSelected = 0;
    public GameObject parentItemUnitPriceBar;// parent where we instantiate
    public GameObject itemPriceUnitBar;
    public Text totalBillPriceText;


    UnityAction customerAssignmentAction;
    UnityAction customerDeclineAction;

    Dictionary<CategoryName, CategoryPanel> categoryPanels = new Dictionary<CategoryName, CategoryPanel>();
    CategoryName currentCategory = CategoryName.Racks;
    int currentSubCatId = -1;
    int barheight = 45;
    public BoxManager boxmanager;
    [Header("Sound")]
    public AudioClip uiButtonSound;
    public Image vechicleControllerPanel;
    [Header("HoverBoard RV Panel")]
    public Image HoverBoardRVPanel;
    public Text HoverBoardRewardNameText;
    public Text accuireTextHoverboard;
    public Text totalRVNeededTextHoverboard;
    public Image playerVehicleTimerContainer;
    public Text playerVechicleTimerText;
    public GameObject timeContainerVehicle;
    public Text vehicleBenifit;
    public Image vehicleIcon;
    public Image vehicleInteractionPanel;
    [Header("MoneyBox RV Panel")]
    public Image moneyBoxPanel;
    public Text MoneyBoxRewardNameText;
    public Text  totalRVNeededTextMoneyBox;
    public Text MoneyBoxQuantityText;

    public Image turnInternetOnPanel;

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

    private void Start()
    {
        instructionsText.transform.parent.gameObject.SetActive(false);
        DisplayHoverObjectName("", false);
        pausePanel.SetActive(false);
        toolOpenPanel.SetActive(false);
        dynamicPlaceBtnsContainer.SetActive(false);
        pcClickPanel.SetActive(false);
        boxClickPanel.SetActive(false);
        paintCountContainer.SetActive(false);
        counterCheckOutContainer.SetActive(false);
        customerRequestPanel.SetActive(false);
        pickButton.SetActive(false);
        adsTimerDisplay.transform.parent.gameObject.SetActive(false);
        DisableAllToolsInStart();
        gameProgressContainer.SetActive(false);
        motelCustomerWaitingText.transform.parent.parent.gameObject.SetActive(false);
        gasStationCustomerWaitingText.transform.parent.parent.gameObject.SetActive(false);
        superStoreCustomerWaitingText.transform.parent.parent.gameObject.SetActive(false);
        totalBillPriceText.text = "$ 0";

        motelToggleImage.sprite = toggleSprite[0];
        fpsModePanel.SetActive(true);
        counterModePanel.SetActive(false);
        notEnoughCashPanel.SetActive(false);
        hiringRvPanel.SetActive(false);
        existingUserUpdateRewardPanel.SetActive(false);
        notificationPopupPanel.SetActive(false);
        //vechicleControllerPanel.gameObject.SetActive(false);
        HoverBoardRVPanel.gameObject.SetActive(false);
    }
    public void UpdateGameTime(float _time, int _day)
    {
        int _totalMinutes = Mathf.FloorToInt(_time);

        int _hours = (_totalMinutes / 60);
        int _remMinutes = _totalMinutes % 60;

        string timeString = string.Format("{0:00}:{1:00} {2}", _hours % 12 == 0 ? 12 : _hours % 12, _remMinutes, _hours >= 12 ? "PM" : "AM");
        if (timeDisplay != null)
        {
            timeDisplay.text = timeString;
        }
        // Display the current day
        if (dayDisplay != null)
        {
            dayDisplay.gameObject.GetComponent<LocalizeText>().UpdateText(daysOfWeek[_day % 7]);
            //dayDisplay.text = daysOfWeek[_day % 7];
        }
    }
    public void UpdateAdsTimer(int _time)
    {
        adsTimerDisplay.transform.parent.gameObject.SetActive(_time > 0);
        adsTimerDisplay.GetComponent<LocalizeText>().UpdateText("Displaying Ad In: " + _time.ToString() + " Seconds");
        adsTimerDisplay.text = "Displaying Ad In: " + _time.ToString() + " Seconds";
    }
    //public void UpdateGasReserves()
    //{
    //    float _maxReserve = GasStationManager.instance.gasData.maxReserve;
    //    float _currentReserve = GasStationManager.instance.gasData.reserveGas;
    //    homeFuelreserveTxt.text = _currentReserve.ToString("##0.0#") + "L / " + _maxReserve.ToString("###.00") + "L";
    //    homeFuelBar.fillAmount = _currentReserve / _maxReserve;
    //}
    public void UpdateMotelWaitingCustomerText(int _val)
    {
        motelCustomerWaitingText.transform.parent.parent.gameObject.SetActive(_val > 0);
        motelCustomerWaitingText.text = _val.ToString();
    }
    public void UpdateGasStationWaitingCustomers(int _val)
    {
        gasStationCustomerWaitingText.transform.parent.parent.gameObject.SetActive(_val > 0);
        gasStationCustomerWaitingText.text = _val.ToString();
    }
    public void UpdateSuperStoreWaitingCustomerText(int _val)
    {
        superStoreCustomerWaitingText.transform.parent.parent.gameObject.SetActive(_val > 0);
        superStoreCustomerWaitingText.text = _val.ToString();
    }
    public void UpdateGasStationWaitingVehicleTime(string _msg)
    {
        gasStationCustomerTimerText.gameObject.SetActive(!(_msg == ""));
        gasStationCustomerTimerText.text = _msg;
    }
    public void EnableFPSPanel(bool _enable)
    {
        fpsModePanel.SetActive(_enable);
        CashText.transform.parent.gameObject.SetActive(_enable);
        BlitzText.transform.parent.gameObject.SetActive(_enable);
    }
    public void EnableCashContainers(bool _enable)
    {
        CashText.transform.parent.gameObject.SetActive(_enable);
        BlitzText.transform.parent.gameObject.SetActive(_enable);
    }
    public IEnumerator InitializeUIController()
    {
        //   yield return null;
        UpdateStoreCartPanelVisibility();
        yield return StartCoroutine(SetGameStorePanel());
        yield return StartCoroutine(ManagementTabUIManager.instance.InitializeManagementTabs());
        yield return StartCoroutine(UpgradesUIManager.instance.InitializeUpgradePanels());
       // yield return StartCoroutine(ReceptionUIManager.Instance.InitializeRoomManagement());

        motelToggleImage.sprite = toggleSprite[GameController.instance.gameData.motelOpenStatus ? 1 : 0];
        fuelToggleImage.sprite = toggleSprite[GameController.instance.gameData.stationOpenStatus ? 1 : 0];
        superStoreToggle.sprite = toggleSprite[GameController.instance.gameData.superStoreOpenStatus ? 1 : 0];
        poolToggleImage.sprite = toggleSprite[GameController.instance.gameData.poolOpenStatus ? 1 : 0];

        BlitzText.text = PlayerDataManager.instance.playerData.playerBlitz.ToString();
        UpdateMotelLevelAndXPBar();
        //GasStationManager.instance.UpdateReservesText();
        string _variant = GameManager.instance.selectedDeliveryMode == 0 ? "DeliveryMode" : "InHandMode";
        GameManager.instance.CallFireBase(_variant);
        //PlayerInventoryObjectBtnClick(0); // call it for make the player State is empty when the game is start 
    }
    public void SetPlayerCurrency()
    {
        CashText.text = "$" + PlayerDataManager.instance.playerData.playerCash;
    }
    public void UpdateStoreCartPanelVisibility()
    {
        bool _isDeliveryMethodOn = GameManager.instance.selectedDeliveryMode == 0;
        storeCartPanel.SetActive(_isDeliveryMethodOn);
        if (_isDeliveryMethodOn)
        {
            storeSubCatsParent.GetComponent<RectTransform>().sizeDelta = new Vector2(-400, -160);
            storeSubCatsParent.GetComponent<RectTransform>().anchoredPosition = new Vector2(-200, 0);
        }
        else
        {
            storeSubCatsParent.GetComponent<RectTransform>().sizeDelta = new Vector2(-100, -160);
            storeSubCatsParent.GetComponent<RectTransform>().anchoredPosition = new Vector2(-50, 0);
        }
    }
    public void UpdateTutorialPanel(string _text)
    {
        //tutorialPanelText.text = _text;
        tutorialPanelText.GetComponent<LocalizeText>().UpdateText(_text);
        tutorialPanelText.transform.parent.gameObject.SetActive(!string.IsNullOrEmpty(_text));
    }
    //Btn refrence
    public void OnPressOpenMotelToggle()
    {
        //if (!GameController.instance.gameData.motelOpenStatus)
        //{
        //    openMotelPanel.SetActive(false);
        //    motelToggleImage.sprite = motelToggleSprite[1];
        //    GameController.instance.OnOpenMotel();
        //    DisplayInstructions("Motel is Opened");
        //    return;
        //}
        if (GameController.instance.gameData.motelOpenStatus)
        {
            motelToggleImage.sprite = toggleSprite[0];
            GameController.instance.OnOpenMotel(false);
           // GameManager.instance.CallFireBase("MotelClosed", "open", 1);
            DisplayInstructions("Motel is Closed");

            return;
        }
        //if (RoomManager.instance.IsAnyRoomReady())
        //{
        //    TutorialManager.instance.OnCompleteTutorialTask(14);
        //    motelToggleImage.sprite = toggleSprite[1];
        //    GameController.instance.OnOpenMotel(true);
        //    DisplayInstructions("Motel is Opened");
        //    GameManager.instance.CallFireBase("MotelOpened", "open", 1);
        //}
        //else
        //{
        //    DisplayInstructions("Prepare 1st Room");
        //    GameManager.instance.CallFireBase("MotelNotReady", "open", 1);
        //}
    }

    public void OnPressOpenGasStation()
    {
        //if (GameController.instance.gameData.stationOpenStatus)
        //{
        //    fuelToggleImage.sprite = toggleSprite[0];
        //    GameController.instance.OnOpenFuelStation(false);
        //    DisplayInstructions("Gas Station Closed!");
        //    GameManager.instance.CallFireBase("FuelStationClosed");
        //    return;
        //}
        //if (!CustomerManager.instance.IsCustomerServed())
        //{
        //    DisplayInstructions("Serve Motel Customer First!");
        //    GameManager.instance.CallFireBase("ReadyMotelFirst");
        //    return;
        //}
        //if (GasStationManager.instance.CanOpenGasStation())
        //{
        //    fuelToggleImage.sprite = toggleSprite[1];
        //    GameController.instance.OnOpenFuelStation();
        //    DisplayInstructions("Gas Station Opened!");
        //    GameManager.instance.CallFireBase("FuelStationOpened");
        //    UpdateGameProgressText(false);
        //}
        //else
        //{
        //    GameManager.instance.CallFireBase("StationNotReady");
        //}
    }
    public void OnPressOpenSuperStore()
    {
        TutorialManager.instance.OnCompleteTutorialTask(15);
        if (GameController.instance.gameData.superStoreOpenStatus)
        {
            superStoreToggle.sprite = toggleSprite[0];
            GameController.instance.OnOpenSuperStore(false);
            DisplayInstructions("Super Store Closed!");
            GameManager.instance.CallFireBase("SuperStoreClosed");
            return;
        }
        if (SuperStoreManager.instance.CanOpenSuperStore())
        {
            superStoreToggle.sprite = toggleSprite[1];
            GameController.instance.OnOpenSuperStore();
            DisplayInstructions("Super Store Opened!");
            GameManager.instance.CallFireBase("SuperStoreOpened");
        }
        else
        {
            GameManager.instance.CallFireBase("StoreNotReady");
        }
    }
    public void OnPressOpenPool()
    {
        //if (GameController.instance.gameData.poolOpenStatus)
        //{
        //    poolToggleImage.sprite = toggleSprite[0];
        //    GameController.instance.OnOpenPool(false);
        //    DisplayInstructions("Pool Closed!");
        //    GameManager.instance.CallFireBase("PoolClosed");
        //    return;
        //}
        //if (PoolManager.instance.CanOpenPool())
        //{
        //    poolToggleImage.sprite = toggleSprite[1];
        //    GameController.instance.OnOpenPool();
        //    DisplayInstructions("Pool Opened!");
        //    GameManager.instance.CallFireBase("PoolOpened");
        //}
        //else
        //{
        //    GameManager.instance.CallFireBase("PoolNotReady");
        //}
    }
    public void OnPressTutorialPanelBtn(int index)
    {
        SoundController.instance.OnPlayInteractionSound(uiButtonSound);
        TutorialManager.instance.OnCompleteTutorialTask(index);
    }
    public void DisplayHoverObjectName(string _name, bool _enableParent = true, HoverInstructionType _type = HoverInstructionType.Unknown)
    {
        hoverItemNameText.transform.parent.gameObject.SetActive(_enableParent);
        hoverItemNameText.gameObject.GetComponent<Text>().text = _name;
        hoverItemNameText.GetComponent<LocalizeText>().UpdateText(_name);
        if (_type == HoverInstructionType.Unknown)
        {
            hoverItemNameText.transform.parent.GetChild(0).gameObject.SetActive(false);

            return;
        }
        if ((int)_type < hoverInsImgs.Length)
        {
            hoverItemNameText.transform.parent.GetChild(0).gameObject.SetActive(true);

            hoverItemNameText.transform.parent.GetChild(0).GetComponent<Image>().sprite = hoverInsImgs[(int)_type];
        }
    }

    public void DisplayInstructions(string _instruction)
    {
        instructionsText.transform.parent.gameObject.SetActive(true);
        //instructionsText.text = _instruction;
        instructionsText.gameObject.GetComponent<LocalizeText>().UpdateText(_instruction);
        Invoke("TurnOffInstruction", 3f);
    }

    public void TurnOffInstruction()
    {
        instructionsText.transform.parent.gameObject.SetActive(false);
        instructionsText.text = "";
    }

    public void OnChangeInteraction(int _index, bool _enable)
    {
        pickButton.SetActive(_enable);
        pickButton.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = interactButtons[_index];
    }

    public void OnInteractBtnPressed()
    {
        PlayerInteraction.instance.OnInteract();
        ToolsManager.instance.OnPressActionButton();
    }
    public void EnableDynamicPlacingBtns(bool _enable)
    {
        dynamicPlaceBtnsContainer.SetActive(_enable);
    }
    public void RotatePlacingItem(float _val)
    {
        if (GameController.instance.currentPicketItem != null)
        {
            if (GameController.instance.currentPicketItem.GetComponent<ItemPickandPlace>())
            {
                GameController.instance.currentPicketItem.GetComponent<ItemPickandPlace>().UpdateItemRotation((int)_val);
            }
        }
    }
    public void PlaceDynamicItem()
    {
        TutorialManager.instance.OnCompleteTutorialTask(13);
        if (GameController.instance.currentPicketItem != null)
        {
            if (GameController.instance.currentPicketItem.GetComponent<ItemPickandPlace>())
            {
                GameController.instance.currentPicketItem.GetComponent<ItemPickandPlace>().OnPlaceItemDynamically();
            }
        }
    }
    public void SetPickBtnColor(Color _color)
    {
        pickButton.GetComponent<Image>().color = _color;
    }


    public void OnClickCollectTrashButton()
    {

    }

    public void OnPressSetting()
    {
        SoundController.instance.OnPlayInteractionSound(uiButtonSound);
        GameManager.instance.OnPressSettings();
    }

    public void OnPressPause()
    {
        SoundController.instance.OnPlayInteractionSound(uiButtonSound);
        pausePanel.SetActive(true);
        Time.timeScale = 0;
        GameManager.instance.CallFireBase("OnPausePrs", "Setting", 1);
    }
    public void OnLeaveCounter()
    {
        //  CashCounterManager.instance.LeaveCashCounter();
        SuperStoreManager.instance.LeaveCashCounter(SuperStoreManager.instance.playerAtCounterId);
    }
    public void OnPressToolButton()
    {
        bool isActive = toolOpenPanel.activeSelf;
        SoundController.instance.OnPlayInteractionSound(uiButtonSound);
        toolOpenPanel.SetActive(!isActive);
        GameManager.instance.CallFireBase("OnToolPanelPrs", "ToolPanel", 1);
    }

    public void DisableAllToolsInStart()
    {
        for (int i = 0; i < GameController.instance.toolParent.transform.childCount; i++)
        {
            GameController.instance.toolParent.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void OnClickToolsButton(string toolName)
    {
       // ToolsManager.instance.SetAutoUseButton(false);
        toolOpenPanel.SetActive(false);
        GameObject selectedTool = null;

        //if (toolName == "Paint Brush")
        //{
        //    paintCountButton.SetActive(true);
        //}
        //else
        //{
        //    paintCountButton.SetActive(false);
        //}

        for (int i = 0; i < GameController.instance.toolParent.transform.childCount; i++)
        {
            Transform tool = GameController.instance.toolParent.transform.GetChild(i);
            if (tool.name == toolName)
            {
                selectedTool = tool.gameObject;
                break;
            }
        }

        if (selectedTool != null && GameController.instance.currentPicketItem == null)
        {
            ToolsManager.instance.SelectTool(selectedTool);
        }
        else
        {
            DisplayInstructions(GameController.instance.currentPicketItem.gameObject.name + " alreay picked");
            DisableAllToolsInStart();
        }
    }

    public void OnPressResume()
    {
        SoundController.instance.OnPlayInteractionSound(uiButtonSound);
        pausePanel.SetActive(false);
        Time.timeScale = 1;
        if (AdsMediation.AdsMediationManager.instance.CanShowInterstitial())
        {
            if (GameController.instance != null)
            {
                GameController.instance.ResetAdsTimer();
            }
            AdsMediation.AdsMediationManager.instance.ShowInterstitial();
        }
       

        GameManager.instance.CallFireBase("OnResumePrs", "Setting", 1);
    }

    public void OnPressHome()
    {
        Time.timeScale = 1;
        GameManager.instance.EnableLoadingScreen(true, "Saving Progress...");
        GameController.instance.SaveData();
        SoundController.instance.OnPlayInteractionSound(uiButtonSound);
        GameManager.instance.CallFireBase("OnHomePrs", "EndPanel", 1);
        StartCoroutine(LoadHome());
        
    }
    IEnumerator LoadHome()
    {
        yield return null;
        while (GameController.instance.IsSavingInProgress())
        {
            yield return new WaitForSeconds(0.1f);
        }

        SceneManager.LoadScene(0);
    }
    public void OnPressDeactivateAllToolsButton()
    {
        //ToolsManager.instance.SetAutoUseButton(false);

        ToolsManager.instance.DeactivateAllTools();
        //    paintCountButton.SetActive(false);
        toolOpenPanel.SetActive(false);
    }

    public void SetPaintCountContainer(int _countOfPaint)
    {
        paintCountContainer.transform.GetChild(1).GetComponent<Text>().text = _countOfPaint.ToString();
    }
    public int updatePrice;
    public void EnableExistingUserUpdatePanel(int _reward)
    {
        updatePrice = _reward;
        existingUserUpdateRewardPanel.SetActive(true);
        existinUserUpdateRewardText.text = "$"+_reward.ToString() +" REFUNDED";
        GameManager.instance.CallFireBase("ExsRwdPnlDsply");

    }
    public void CloseExistingUpdatePanel()
    {
        existingUserUpdateRewardPanel.SetActive(false);
        UpdateCurrency(updatePrice);
        GameManager.instance.CallFireBase("ExsRwdPnlClsd");
    }
    public void OnPressResetPaint()
    {
        GameController.instance.currentPickedTool.GetComponent<PaintBrushTool>().paintCount = 0;
        SetPaintCountContainer(0);
    }
    public void SetRoomProgress(string _paintText, bool _enable, int _trashCount, int _totalTrash)
    {
        gameProgressContainer.SetActive(_enable);
        gameProgressContainer.transform.GetChild(0).GetComponent<LocalizeText>().UpdateText(_paintText + _trashCount.ToString() + "/" + _totalTrash.ToString());
        //gameProgressContainer.transform.GetChild(0).GetComponent<Text>().text = _paintText + _trashCount.ToString() + "/" + _totalTrash.ToString();
    }
    public void SetPoolProgress(string _paintText, bool _enable, int _trashCount, int _totalTrash)
    {
        gameProgressContainer.SetActive(_enable);
        gameProgressContainer.transform.GetChild(0).GetComponent<LocalizeText>().UpdateText(_paintText + _trashCount.ToString() + "/" + _totalTrash.ToString());
        //gameProgressContainer.transform.GetChild(0).GetComponent<Text>().text = _paintText + _trashCount.ToString() + "/" + _totalTrash.ToString();
    }
    public void UpdateGameProgressText(bool _enable, string _progressText = "")
    {
        Debug.Log("why3");
        gameProgressContainer.SetActive(_enable);
        gameProgressContainer.transform.GetChild(0).GetComponent<LocalizeText>().UpdateText(_progressText);
        //gameProgressContainer.transform.GetChild(0).GetComponent<Text>().text = _progressText;
    }
    UnityAction successRv;
    UnityAction failRV;
    public void EnableHiringPanel(EmployeeType _type,float _time,int _rvCount, UnityAction _successAction, UnityAction _failure) 
    {
        //hiringHeadingText.text = "Hire A " + _type.ToString();
        hiringHeadingText.GetComponent<LocalizeText>().UpdateText("Hire A " + _type.ToString());
        int _totalMinutes = (int)((_time * GameController.instance.dayCycleMinutes));
        hiringTimeText.text = (_totalMinutes.ToString("00")) + ":00" + "h";

        rvCountText.text = _rvCount.ToString();
        hiringRvPanel.SetActive(true);
        successRv = _successAction;
        failRV = _failure;
    }
    public void OnPressHiringRV()
    {
        AdsMediation.AdsMediationManager.instance.ShowRewardedVideo(OnSuccessHiringRV, OnFailHiringRV);
    }
    public void OnCloseHiringRV()
    {
        hiringRvPanel.SetActive(false);
    }
    public void OnSuccessHiringRV(string _msg)
    {
        hiringRvPanel.SetActive(false);
        successRv?.Invoke();
    }
    public void OnFailHiringRV(string _msg)
    {
        hiringRvPanel.SetActive(false);
        failRV?.Invoke();
        DisplayInstructions(_msg);
    }


    #region Monetizations

    UnityAction successRvMonetization;
    UnityAction failRVMonetization;

    public void EnableMonetizationRVPanel(string _vehicleName, Sprite icon, VechicleType vechicleType, float _time, int _rvCount, UnityAction _successAction, UnityAction _failure, int required)
    {
       // print("EnableMonetizationRVPanel" + required);
        if (vechicleType == VechicleType.HoverBoard)
        {
            HoverBoardRewardNameText.text = "ACQUIRE A " + _vehicleName;
            HoverBoardRewardNameText.GetComponent<LocalizeText>().UpdateText("ACQUIRE A " + _vehicleName);
            timeContainerVehicle.gameObject.SetActive(true);
            vehicleBenifit.text = "+50% Speed";

        }

        if (vechicleType == VechicleType.Trolly)
        {
            HoverBoardRewardNameText.text = "ACQUIRE A " + _vehicleName;
            HoverBoardRewardNameText.GetComponent<LocalizeText>().UpdateText("ACQUIRE A " + _vehicleName);
            timeContainerVehicle.gameObject.SetActive(false);
            vehicleBenifit.text = "Use to carry laugage";
        }
        vehicleIcon.sprite = icon;
        int _totalMinutes = (int)((_time * GameController.instance.dayCycleMinutes));
        accuireTextHoverboard.text = (_totalMinutes.ToString("00")) + ":00" + "h";
       // print("strstr ???????" + required);
        totalRVNeededTextHoverboard.text = required.ToString();
        ShowHoverbaordRVPanel(true);
        successRvMonetization = _successAction;
        failRVMonetization = _failure;
    }
    public void OnPressWatchRVMonetization()
    {
        if (!InternetConnectivity.instance.isInterNetAvailable())
        {
            GameManager.instance.CallFireBase("Clickhoverbox_no_net");
            OpenNoNetPanel();
            return;
        }
        GameManager.instance.CallFireBase("watchhoverboardad");
        AdsMediation.AdsMediationManager.instance.ShowRewardedVideo(OnSuccessMonetizationRV, OnFailMonetizationRV);
    }
    public void OnCloseMonetizationRV()
    {
        ShowHoverbaordRVPanel(false);
    }
    public void OnSuccessMonetizationRV(string _msg)
    {
        GameManager.instance.CallFireBase("hoverboardadacquired");
        ShowHoverbaordRVPanel(false);
        successRvMonetization?.Invoke();
    }
    public void OnFailMonetizationRV(string _msg)
    {
        GameManager.instance.CallFireBase("hoverboardnoad");
        ShowHoverbaordRVPanel(false);
        failRVMonetization?.Invoke();
        DisplayInstructions(_msg);
    }

    public void ShowHoverbaordRVPanel(bool _status)
    {
        HoverBoardRVPanel.gameObject.SetActive(_status);
    }

    //MoneyBox RV
    public void ShowMoneyBoxRVPanel(bool _status)
    {
        moneyBoxPanel.gameObject.SetActive(_status);
    }
 
    public void EnableMoneyBoxPanel(int _amount, int _rvCount, UnityAction _successAction, UnityAction _failure)
    {
        //MoneyBoxRewardNameText.text = "STASH OF CASH";
        MoneyBoxRewardNameText.GetComponent<LocalizeText>().UpdateText("STASH OF CASH");
        totalRVNeededTextMoneyBox.text = _rvCount.ToString();
        MoneyBoxQuantityText.text = "$"+ _amount;
        ShowMoneyBoxRVPanel(true);
        successRvMonetization = _successAction;
        failRVMonetization = _failure;
    }
    public void OnPressWatchRVMoneyBox()
    {
        if (!InternetConnectivity.instance.isInterNetAvailable())
        {
            GameManager.instance.CallFireBase("ClickMoneybox_no_net");
            OpenNoNetPanel();
            return;
        }
        GameManager.instance.CallFireBase("ClickMoneybox");
        AdsMediation.AdsMediationManager.instance.ShowRewardedVideo(OnSuccessRVMoneyBox, OnFailRVMoneyBox);
    }
    public void OnCloseRVMoneyBox()
    {
        ShowMoneyBoxRVPanel(false);
    }
    public void OnSuccessRVMoneyBox(string _msg)
    {
        GameManager.instance.CallFireBase("moneyboxcashcollected");
        ShowMoneyBoxRVPanel(false);
        successRvMonetization?.Invoke();
    }
    public void OnFailRVMoneyBox(string _msg)
    {
        GameManager.instance.CallFireBase("failmoneyboxnoAd");

        ShowMoneyBoxRVPanel(false);
        failRVMonetization?.Invoke();
        DisplayInstructions(_msg);
    }
    public void OpenNoNetPanel()
    {
        turnInternetOnPanel.gameObject.SetActive(true);
    }
    public void CloseNoNetPanel()
    {
        turnInternetOnPanel.gameObject.SetActive(false);
    }
    public void EnableVehicleControllerPanel(bool _state)
    {
        vehicleInteractionPanel.gameObject.SetActive(_state);
    }

    public void OpenOSInternetSettings()
    {
#if UNITY_ANDROID
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", "android.settings.WIRELESS_SETTINGS");
        currentActivity.Call("startActivity", intent);
#endif

#if UNITY_IOS
        // This opens the main settings app (Apple may reject deep links to specific settings).
        Application.OpenURL("App-Prefs:root=MOBILE_DATA_SETTINGS_ID");
#endif
    }
    #endregion

    #region Store UI Implementation

    public void EnableCashExchangePanel(bool _enable)
    {
        cashBillingPanel.SetActive(_enable);
        leaveCounterBtn.SetActive(!_enable);
        centAnimationRef.SetActive(false);
    }
    public void EnableCardPanel(bool _enable)
    {
        cardBillingPanel.SetActive(_enable);
        leaveCounterBtn.SetActive(!_enable);
        cardAnimationRef.SetActive(false);
    }
    public void ResetCounterPanel()
    {
        leaveCounterBtn.SetActive(true);
        cashBillingPanel.SetActive(false);
        cardBillingPanel.SetActive(false);
    }
    public void EnableCounterPanel(bool _enable)
    {
        fpsModePanel.SetActive(!_enable);
        counterModePanel.SetActive(_enable);
    }
    public void OnCardBtnPressed(string _val)
    {
       // CashCounterManager.instance.OnInteractionWithCardSwipeMachine(_val);
        SuperStoreManager.instance.OnInteractionWithCardSwipeMachine(SuperStoreManager.instance.playerAtCounterId, _val);
        HapticFeedback.LightFeedback();

    }
    public void OnExchangeCurrencyPressed(int _index)
    {
        SuperStoreManager.instance.CreateCash(SuperStoreManager.instance.playerAtCounterId,_index); 
       // CashCounterManager.instance.CreateCash(_index);
        HapticFeedback.LightFeedback();
        
    }
    public void OnResetExchangeCurrency()
    {
        SuperStoreManager.instance.OnResetExchangeCurrency(SuperStoreManager.instance.playerAtCounterId);
      //  CashCounterManager.instance.OnResetExchangeCurrency();
        SoundController.instance.OnPlayInteractionSound(uiButtonSound);

    }
    public void OnUndoExchangeCash()
    {
        SuperStoreManager.instance.RollBackCurrancy(SuperStoreManager.instance.playerAtCounterId);
     //   CashCounterManager.instance.RollBackCurrancy();
        SoundController.instance.OnPlayInteractionSound(uiButtonSound);

    }
    public void OnConfirmCash()
    {
        SuperStoreManager.instance.OnConfirmChange(SuperStoreManager.instance.playerAtCounterId);
        //CashCounterManager.instance.OnConfirmChange();
        SoundController.instance.OnPlayInteractionSound(uiButtonSound);
    }
    public void UpdateCardEnteredText(string _val)
    {
        cardEnterAmountText.text = _val;
    }
    public void AnimateCentsContainer()
    {
        centAnimationRef.SetActive(true);
        if (!isCentAnimating)
        {
            isCentAnimating = true;
            StartCoroutine(DisableCentAnimation());
        }
    }
    bool isCentAnimating = false;
    IEnumerator DisableCentAnimation()
    {
        yield return new WaitForSeconds(2f);
        centAnimationRef.SetActive(false);
        isCentAnimating = false;
    }
    public void AnimateCardDecimal()
    {
        cardAnimationRef.SetActive(true);
        if (!isCardDecimalAnimating)
        {
            isCardDecimalAnimating = true;
            StartCoroutine(DisableCardDecimalAnimation());
        }
    }
    bool isCardDecimalAnimating = false;
    IEnumerator DisableCardDecimalAnimation()
    {
        yield return new WaitForSeconds(2f);
        cardAnimationRef.SetActive(false);
        isCardDecimalAnimating = false;
    }
    #endregion

    #region Game Store Manipulation

    IEnumerator SetGameStorePanel()
    {
        int _catCounter = 0;
        float _catBnPos = 0;

        foreach (KeyValuePair<CategoryName, ItemCategories> _category in GameManager.instance.itemCategories)
        {
            CategoryPanel _catPanelRef = new CategoryPanel();
            // instantiate category button
            _catPanelRef.panelRefs.storeBtn = Instantiate(storeCatBtnPrefab, storeCatBtnsContent.transform);
            _catPanelRef.panelRefs.storeBtn.transform.localScale = Vector3.one;
            _catPanelRef.panelRefs.storeBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(_catBnPos, 0);

            _catBnPos += (_catPanelRef.panelRefs.storeBtn.GetComponent<RectTransform>().sizeDelta.x + 5f);//20 is gap between to two btns


            _catPanelRef.panelRefs.storeBtn.GetComponent<Button>().onClick.AddListener(() => StoreCategorySelected(_category.Key));  // send category name

            //_catPanelRef.panelRefs.storeBtn.transform.GetChild(0).GetComponent<Text>().text = GameManager.instance.categoriesUIData[_catCounter].catName;   // assign name 
            _catPanelRef.panelRefs.storeBtn.transform.GetChild(0).GetComponent<LocalizeText>().UpdateText(GameManager.instance.categoriesUIData[_catCounter].catName);
            _catPanelRef.panelRefs.storeBtn.gameObject.name = _category.Key.ToString();   // just for understaning in the hierarchy

            // instantiate a prefab on right side of categoy button in which we have two gameobjects, top (for sub categories heading), bottom (for item scroll)
            _catPanelRef.panelRefs.storePanel = Instantiate(storeSubCatsPrefab, storeSubCatsParent.transform);
            _catPanelRef.panelRefs.storePanel.transform.localScale = Vector3.one;

            _catPanelRef.panelRefs.storePanel.name = _category.Key.ToString() + "_Panel";
            _catPanelRef.panelRefs.storePanel.gameObject.SetActive(false);

            int _subCounter = 0;
            float _subCatBtnPos = -10;
            ItemSubCategories[] _subCategories = _category.Value.subCategories.Values.OrderBy(_obj => _obj.reqLevel).ToArray();
            yield return null;
            Transform _subPanelBtnsRef = _catPanelRef.panelRefs.storePanel.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0);
            foreach (ItemSubCategories _subCat in _subCategories)
            {
                StorePanelReference _subCatRef = new StorePanelReference();

                // instantiaite sub category btn 
                _subCatRef.storeBtn = Instantiate(storeSubCatBtnPrefab, _subPanelBtnsRef);  // in the top gameobject
                _subCatRef.storeBtn.transform.localScale = Vector3.one;
                _subCatRef.storeBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, _subCatBtnPos);
                _subCatBtnPos -= (_subCatRef.storeBtn.GetComponent<RectTransform>().sizeDelta.y + 5f);

                _subCatRef.storeBtn.GetComponent<Button>().onClick.AddListener(() => StoreSubCategorySelected(_subCat.subCatId));

                string _subCatName = GameManager.instance.categoriesUIData[(int)_category.Key].subCategoriesUIData[_subCat.subCatId].subName;
                bool _isSubCatUnlocked = _subCat.reqLevel <= PlayerDataManager.instance.playerData.playerLevel;
                _subCatRef.storeBtn.transform.GetChild(0).GetComponent<LocalizeText>().UpdateText(_subCatName);

                _subCatRef.storeBtn.transform.GetChild(1).gameObject.SetActive(!_isSubCatUnlocked);

                _subCatRef.storeBtn.gameObject.name = _subCatName;   // just for understaning in the hierarchy

                // instantiate scroll rect 
                _subCatRef.storePanel = Instantiate(storeSubCatScrollPrefab, _catPanelRef.panelRefs.storePanel.transform.GetChild(1)); // in the below gameobject
                _subCatRef.storePanel.transform.localScale = Vector3.one;
                _subCatRef.storePanel.gameObject.name = _subCatName + "_Panel";
                _subCatRef.storePanel.gameObject.SetActive(false);
                // it points to the scroll rect content
                List<ItemData> _items = _subCat.items.OrderBy(_obj => _obj.itemPrice).ToList();

                Transform _itemsScroller = _subCatRef.storePanel.transform.GetChild(0).GetChild(0);
                for (int i = 0; i < _items.Count; i++)
                {
                    // item prefab
                    GameObject _itemPrefab = Instantiate(storeItemPrefab, _itemsScroller);
                    _itemPrefab.transform.localScale = Vector3.one;
                    ItemData _itemData = _items[i];
                    _items[i].uiItemObject = _itemPrefab;
                    // assing image
                    _itemPrefab.GetComponent<StoreItemsValuse>().SetItemUI(_itemData);
                }
                if (!_catPanelRef.subPanelRefs.ContainsKey(_subCat.subCatId))
                {
                    _catPanelRef.subPanelRefs.Add(_subCat.subCatId, _subCatRef);
                }
                _subCounter++;
                yield return null;
            }
            _subPanelBtnsRef.GetComponent<RectTransform>().sizeDelta = new Vector2(_subPanelBtnsRef.GetComponent<RectTransform>().sizeDelta.x, Mathf.Abs(_subCatBtnPos));
            // Add the category panel reference to the dictionary
            categoryPanels.Add(_category.Key, _catPanelRef);
            _catCounter++;
        }
        storeCatBtnsContent.GetComponent<RectTransform>().sizeDelta = new Vector2(MathF.Abs(_catBnPos), storeCatBtnsContent.GetComponent<RectTransform>().sizeDelta.y);
        StoreCategorySelected((CategoryName.Products));
        StoreSubCategorySelected(0);
    }
    IEnumerator UpdateItemsScrollerPanel(Transform _scrollPanel)
    {
        yield return null;
        if (!_scrollPanel.gameObject.activeInHierarchy)
        {
            yield break;
        }
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
    // button ref
    public void OnPressRemoveAllItems()
    {
        SoundController.instance.OnPlayInteractionSound(uiButtonSound);
        foreach (Transform child in parentItemUnitPriceBar.transform)
        {
            // Disable the child GameObject
            //child.gameObject.SetActive(false);
            Destroy(child.gameObject);
        }
        StoreItemsValuse.totalBill = 0;
        totalBillPriceText.text = "$ 0";
        AdjustParentHeight();
        GameManager.instance.CallFireBase("ItemsCleared");
    }

    public void AdjustParentHeight(int _childToAdd=0)
    {
        // Calculate the new height based on the number of child elements
        int childCount = 0;
        if (_childToAdd != 0)
        {
            childCount = (parentItemUnitPriceBar.transform.childCount + _childToAdd);
        }
        
        float _barHeight = itemPriceUnitBar.GetComponent<RectTransform>().sizeDelta.y;
        float newHeight = (_barHeight + parentItemUnitPriceBar.GetComponent<VerticalLayoutGroup>().spacing) * childCount;

        // Set the new height of the parent container
        parentItemUnitPriceBar.GetComponent<RectTransform>().sizeDelta = new Vector2(parentItemUnitPriceBar.GetComponent<RectTransform>().sizeDelta.x, newHeight);
    }
    // button ref
    public void OnPressBuyItems()
    {
        SoundController.instance.OnPlayInteractionSound(uiButtonSound);
        if (StoreItemsValuse.totalBill <= 0)
        {
            return;
        }
        if (AdsMediation.AdsMediationManager.instance.CanShowInterstitial())
        {
            if (GameController.instance != null)
            {
                GameController.instance.ResetAdsTimer();
            }
           
        }
        AdsMediation.AdsMediationManager.instance.ShowInterstitial();
        TutorialManager.instance.OnCompleteTutorialTask(11);
        if (PlayerDataManager.instance.playerData.playerCash >= StoreItemsValuse.totalBill)
        {
            pcClickPanel.SetActive(false);
            DisplayInstructions("Items Will Be Delivered Shortly!");

            List<BoxDataToSpawn> _items = new List<BoxDataToSpawn>();
            for (int i = 0; i < parentItemUnitPriceBar.transform.childCount; i++)
            {
                if (parentItemUnitPriceBar.transform.GetChild(i).gameObject.activeSelf == true)
                {
                    BoxDataToSpawn _box = new BoxDataToSpawn();
                    _box.item = parentItemUnitPriceBar.transform.GetChild(i).GetComponent<CartItemValues>().itemData;
                    _box.count = parentItemUnitPriceBar.transform.GetChild(i).GetComponent<CartItemValues>().itemCount;
                    string _event = "ItemBuy_" + ((int)_box.item.mainCatID).ToString()+"_" + _box.item.subCatID.ToString()+"_" + _box.item.itemID.ToString();
                    GameManager.instance.CallFireBase(_event);
                    _items.Add(_box);
                }
            }
            boxmanager.StartSpawnBoxes(_items);
            //boxmanager.bo
          //  int _cash = PlayerDataManager.instance.playerData.playerCash - StoreItemsValuse.totalBill;
            PlayerDataManager.instance.UpdateCash(-1 * StoreItemsValuse.totalBill);

            UpdateCurrency(-1 * StoreItemsValuse.totalBill);
            DisplayInstructions("Order Placed!");
            GameManager.instance.CallFireBase("CartItemOrdered");
            OnPressRemoveAllItems();
        }
        else
        {
            GameManager.instance.CallFireBase("NoCashForCart");
            notEnoughCashPanel.SetActive(true);
        }
        
    }

    public void OnPurchaseItem(ItemData _item)
    {
        if (PlayerDataManager.instance.playerData.playerCash < _item.itemPrice)
        {
            GameManager.instance.CallFireBase("NoCashSinglePur");
            notEnoughCashPanel.SetActive(true);
            return;
        }
        GameObject _pickedItem = GameController.instance.currentPicketItem;
        GameObject _pickedTool = GameController.instance.currentPickedTool;
        if (_pickedItem != null)
        {
            _pickedItem.GetComponent<ItemPickandPlace>().ThrowPickedObjects();
        }
        if (_pickedTool != null)
        {
            OnPressDeactivateAllToolsButton();
        }
        if (GameController.instance.currentPicketItem != null)
        {
            return;
        }
        //if (_item.mainCatID == CategoryName.Paint)
        //{
        //    //Select Paint And Apply On Brush
        //    OnClickToolsButton("Paint Brush");
        //    var toolPicked = GameController.instance.currentPickedTool;
        //    if (toolPicked.gameObject.name == "Paint Brush")
        //    {
        //        toolPicked.GetComponent<PaintBrushTool>().SetPaintProperties(_item.PaintMaterial, _item.itemquantity,
        //                _item.mainCatID, _item.subCatID, _item.itemID);
        //        var _count = toolPicked.GetComponent<PaintBrushTool>().paintCount;
        //        SetPaintCountContainer(_count);
        //    }
        //}
        //else
        //{
        //    //Spwn Item And It In Hands
        //    GameObject _furniture = Instantiate(_item.itemPrefab);
        //    _furniture.GetComponent<ItemPickandPlace>().itemId = _item.itemID;
        //    ItemSavingProps _props = new ItemSavingProps();
        //    _props.mainCatId = (int)_item.mainCatID;
        //    _props.subCatId = _item.subCatID;
        //    _props.itemId = _item.itemID;
        //    _props.itemCount = _item.itemquantity;
        //    _furniture.GetComponent<ItemPickandPlace>().UpdateItemSavingData(_props);
        //    _furniture.GetComponent<ItemPickandPlace>().AddItemToSavingList();
        //    _furniture.GetComponent<IRuntimeSpawn>().OnNewSpawnItem();
        //}

        if (AdsMediation.AdsMediationManager.instance.CanShowInterstitial())
        {
            if (GameController.instance != null)
            {
                GameController.instance.ResetAdsTimer();
            }
            
        }
        AdsMediation.AdsMediationManager.instance.ShowInterstitial();
        //TutorialManager.instance.OnCompleteTutorialTask(9);
        TutorialManager.instance.OnCompleteTutorialTask(9);
        //int _cash = PlayerDataManager.instance.playerData.playerCash - _item.itemPrice;
        PlayerDataManager.instance.UpdateCash(-1 * _item.itemPrice);
        UpdateCurrency(-1 * _item.itemPrice);
        GameManager.instance.CallFireBase("ItemInHand_"+ ((int)_item.mainCatID).ToString() + "_" + _item.subCatID.ToString() + "_" + _item.itemID.ToString());
        //OnClosePCPanel();
    }

    /// <summary>
    /// Called On main Category Button Pressed
    /// </summary>
    /// <param name="_categoryName"></param>
    public void StoreCategorySelected(CategoryName _categoryName)
    {
        SoundController.instance.OnPlayInteractionSound(uiButtonSound);
        if (currentCategory != _categoryName)
        {
       //     SoundController.instance.OnPlayInteractionSound(buttonClickedSound);
            if (categoryPanels.ContainsKey(currentCategory))
            {
                categoryPanels[currentCategory].panelRefs.storeBtn.GetComponent<Button>().interactable = true;
                categoryPanels[currentCategory].panelRefs.storePanel.SetActive(false);
                if (categoryPanels[currentCategory].subPanelRefs.ContainsKey(currentSubCatId))
                {
                    categoryPanels[currentCategory].subPanelRefs[currentSubCatId].storePanel.SetActive(false);
                    categoryPanels[currentCategory].subPanelRefs[currentSubCatId].storeBtn.GetComponent<Button>().interactable = true;
                }
            }

            categoryPanels[_categoryName].panelRefs.storeBtn.GetComponent<Button>().interactable = false;
            categoryPanels[_categoryName].panelRefs.storePanel.SetActive(true);

            currentSubCatId = -1;
            currentCategory = _categoryName;

            foreach (KeyValuePair<int, StorePanelReference> _subCat in categoryPanels[currentCategory].subPanelRefs)
            {
                bool _isUnlocked = GameManager.instance.categoriesUIData[(int)currentCategory].subCategoriesUIData[_subCat.Key].reqLevel <= PlayerDataManager.instance.playerData.playerLevel;
                _subCat.Value.storeBtn.transform.GetChild(1).gameObject.SetActive(!_isUnlocked);
            }
            if (_categoryName == CategoryName.Products)
            {
                StoreSubCategorySelected(0);   // by default we are calling 0 sub categories

            }
            else
            {
                StoreSubCategorySelected(1);   // by default we are calling 0 sub categories

            }
        }
    }
    

    public void StoreSubCategorySelected(int _subCatId)
    {
        print("NSD" + _subCatId);
        if (currentSubCatId == _subCatId)
            return;
        SoundController.instance.OnPlayInteractionSound(uiButtonSound);

        if (categoryPanels[currentCategory].subPanelRefs.ContainsKey(currentSubCatId))
        {
            print("NSD 1");
            categoryPanels[currentCategory].subPanelRefs[currentSubCatId].storeBtn.GetComponent<Button>().interactable = true;  // make previously selected subcategory button interactable 
            categoryPanels[currentCategory].subPanelRefs[currentSubCatId].storePanel.gameObject.SetActive(false);  // hide previously selected subcategory panel
        }

        if (categoryPanels[currentCategory].subPanelRefs.ContainsKey(_subCatId))
        {
           
            categoryPanels[currentCategory].subPanelRefs[_subCatId].storeBtn.GetComponent<Button>().interactable = false;  // make currently selected subcategory button not interactable 
            categoryPanels[currentCategory].subPanelRefs[_subCatId].storePanel.SetActive(true);  // show currently selected subcategory panel
            print("NSD 2");
            Transform _itemsScroller = categoryPanels[currentCategory].subPanelRefs[_subCatId].storePanel.transform.GetChild(0).GetChild(0);
            for (int i = 0; i < _itemsScroller.transform.childCount; i++)
            {
                print("NSD 3"+ _itemsScroller);
               
                _itemsScroller.GetChild(i).GetComponent<StoreItemsValuse>().UpdateItemUI();
            }
            StartCoroutine(UpdateItemsScrollerPanel(_itemsScroller));
        }
        currentSubCatId = _subCatId;
    }
    // attach with listener
    void ItemSelectedFromStoreScroll(ItemData _itemData)
    {
        //SoundController.instance.OnPlayInteractionSound(buttonClickedSound);
        //if (currentCategory != CategoryName.Accessories)
        //{
        //    if (GameController.instance.IsAlreadyPickedItem())
        //    {
        //        DisplayInstructions("You Already Have An Item!");
        //        return;
        //    }
        //}
    }
    #endregion
    #region Level Upgrade Implementation
    public void OnUpdateLevel()
    {
        print("J 0" + pcTabs.Length);
        print("J 1");
        //if (!pcClickPanel.activeInHierarchy)
        //{
        //    print("J 2");
        //    return;
        //}
        print("J 3");
        //int _activeTab = -1;
        int _activeTab = 1;
        for (int i = 0; i < pcTabs.Length; i++)
        {
            print("J 4");
            if (pcTabs[i].tabPanel.activeInHierarchy)
            {
                print("J 5");
                _activeTab = i;
            }
        }
        print("J 6" + _activeTab);
        if (_activeTab < 0)
            return;
        switch (_activeTab)
        {
           
            case 1:     //Furniture Tab Is Opened
                foreach (KeyValuePair<int, StorePanelReference> _subCat in categoryPanels[currentCategory].subPanelRefs)
                {
                    bool _isUnlocked = GameManager.instance.categoriesUIData[(int)currentCategory].subCategoriesUIData[_subCat.Key].reqLevel <= PlayerDataManager.instance.playerData.playerLevel;
                    print("hexaMexa" + _isUnlocked);
                    print("Dexa" + _subCat.Value.storeBtn.transform.GetChild(1).gameObject.name);
                    _subCat.Value.storeBtn.transform.GetChild(1).gameObject.SetActive(!_isUnlocked);
                }
                if (categoryPanels[currentCategory].subPanelRefs.ContainsKey(currentSubCatId))
                {
                    Transform _itemsScroller = categoryPanels[currentCategory].subPanelRefs[currentSubCatId].storePanel.transform.GetChild(0).GetChild(0);
                    for (int i = 0; i < _itemsScroller.transform.childCount; i++)
                    {
                        print("gB" + _itemsScroller.name);
                        _itemsScroller.GetChild(i).GetComponent<StoreItemsValuse>().UpdateItemUI();
                    }
                }
                break;
            case 4:     //Staff Tab
                ManagementTabUIManager.instance.OnLeveUpgrade();
                break;
            case 5:     //Upgrades tab
                UpgradesUIManager.instance.UpdateLockStatus();
                break;
        }
    }
    #endregion
    #region PC Panel Manipulation
    public void OnPressPC()
    {

        print("111b");
        pcClickPanel.SetActive(true);
        
        AdsMediation.AdsMediationManager.instance.RemoveBannerAd();
        //SetFuelPanelValues();//Fuel Panel Values Is Set Everytime PC Panel is opened
        //AdsMediation.AdsMediationManager.instance.RemoveBannerAd();
        //if (RoomManager.instance.currentRoomNumber >= 0)
        //{
        //    RoomManager.instance.rooms[RoomManager.instance.currentRoomNumber].OpenGameStore();
        //    return;
        //}
        //if (GasStationManager.instance.IsInGasStationArea())
        //{
        //    OpenGameStorePanel(2);
        //    return;
        //}
        if (SuperStoreManager.instance.IsAtSuperMarket())
        {
            print("2b");
            OpenGameStorePanel(1, ((int)CategoryName.Products));
            return;
        }
        OnPcTabPressed(0);
    }
    public void OnClosePCPanel()
    {
        
        pcClickPanel.SetActive(false);
        SoundController.instance.OnPlayInteractionSound(uiButtonSound);
        AdsMediation.AdsMediationManager.instance.DisplayBanner();
    }
    public void OpenGameStorePanel(int _tabPressed = 0, int _mainCat = -1, int _subCatId = -1)
    {
        OnPcTabPressed(_tabPressed);
        if (_mainCat >= 0)
        {
            StoreCategorySelected((CategoryName)_mainCat);
        }
        if (_subCatId >= 0)
        {
            StoreSubCategorySelected(_subCatId);
        }
    }
   
    public void OnPressCashContainer()
    {
        pcClickPanel.SetActive(true);
        //SetFuelPanelValues();//Fuel Panel Values Is Set Everytime PC Panel is opened
        AdsMediation.AdsMediationManager.instance.RemoveBannerAd();
        
        OnPcTabPressed(2);
        IAPUiManager.Instance.OnSubPanelBtnPressed(2);

    }
    public void OnCloseNoCashPanel()
    {
        OnPressCashContainer();
        notEnoughCashPanel.SetActive(false);
    }
    public void EnableNoCashPanel()
    {
        notEnoughCashPanel.SetActive(true);
    }
    public bool notificationRoutineRunning = false;
    public void EnablePopupNotification(string _msg,float _waitTime=2f)
    {
        if (!notificationRoutineRunning)
        {
            notificationPopupPanel.SetActive(true);

            notificationText.text = _msg;

            notificationRoutineRunning = true;
            StartCoroutine(DisableNotification(_waitTime));
        }
    }
    IEnumerator DisableNotification(float _waitTime)
    {
        yield return new WaitForSeconds(_waitTime);
        notificationPopupPanel.SetActive(false);
        notificationRoutineRunning = false;
    }
    public void OnPcTabPressed(int _tabId)
    {
        print("2c");
        if (_tabId < 0 || _tabId >= pcTabs.Length)
        {
            Debug.LogError("Invalid TabId Pressed!");
            return;
        }
        for (int i = 0; i < pcTabs.Length; i++)
        {
            pcTabs[i].tabPanel.SetActive(false);
            pcTabs[i].tabBtn.sprite = tabBtnsSprites[0];
            pcTabs[i].tabIconImage.sprite = pcTabs[i].tabIcons[0];
            pcTabs[i].tabNameText.GetComponent<LocalizeText>().UpdateText(pcTabs[i].tabName);
            pcTabs[i].tabNameText.color = tabTextColors[0];
        }
        pcTabs[_tabId].tabPanel.SetActive(true);
        pcTabs[_tabId].tabBtn.sprite = tabBtnsSprites[1];
        pcTabs[_tabId].tabIconImage.sprite = pcTabs[_tabId].tabIcons[1];
        pcTabs[_tabId].tabNameText.color = tabTextColors[1];
        print("This is PC Tab ID ;;;;;;" + _tabId);
        if (_tabId == 1)
        {
            //StoreCategorySelected(CategoryName.Furniture);
            UpdateStoreCartPanelVisibility();
        }
        if (_tabId == 2)
        {
            IAPUiManager.Instance.OnSubPanelBtnPressed(0);
        }
        if (_tabId == 3)
        {
            ManagementTabUIManager.instance.OnPressManagementTab(0);
        }
        if (_tabId == 4)
        {
            UpgradesUIManager.instance.OnUpgradeCatBtnPressed(0);
        }
        GameManager.instance.CallFireBase("TabBtnPressed_" + _tabId.ToString());
        SoundController.instance.OnPlayInteractionSound(uiButtonSound);
    }
    //public void SetFuelPanelValues()
    //{
    //    float _reserveFuel = GasStationManager.instance.gasData.reserveGas;
    //    float _maxFuel = GasStationManager.instance.gasData.maxReserve;

    //    fuelreserveText.text = _reserveFuel.ToString("###.00") + "L / " + _maxFuel.ToString("###.0") + "L";
    //    fuelOrderSlider.maxValue = Mathf.FloorToInt(_maxFuel - _reserveFuel);
    //    fuelOrderSlider.value = 0f;
    //    orderFuelQuantity.text = "0L";
    //    orderFuelPrice.text = "$0";
    //    fuelPrice.text = "$ " +GasStationManager.instance.gasData.gasPurchasingPrice.ToString();

    //}
    //public void OnChangeFuelSlider(float _val)
    //{
    //    orderFuelQuantity.text = _val.ToString("###.00") + "L";
    //    orderFuelPrice.text = "$ "+(GasStationManager.instance.gasData.gasPurchasingPrice * _val).ToString("####.00");
    //}
    public void OrderFuel()
    {
        if (fuelOrderSlider.maxValue < 50f)
        {
            DisplayInstructions("Your Fuel Reserves Are Full!");
          //  GameManager.instance.CallFireBase("FullReserveOrdered");
            return;
        }
        if (fuelOrderSlider.value < 50f)
        {
            DisplayInstructions("Cannot Order Less than 50L fuel!");
           // GameManager.instance.CallFireBase("LessFuelOrder");
            return;
        }
        //if (GasStationManager.instance.IsAlreadyFuelOrdered())
        //{
        //    DisplayInstructions("Delivery On The Way!");
        //    GameManager.instance.CallFireBase("FuelInDelivery");
        //    return;
        //}
        //if (!GasStationManager.instance.CanOrderFuel())
        //{
        //    return;
        //}

        //float _totalPrice = GasStationManager.instance.gasData.gasPurchasingPrice * fuelOrderSlider.value;
        //if (Mathf.CeilToInt(_totalPrice) > PlayerDataManager.instance.playerData.playerCash)
        //{
        //    EnableNoCashPanel();
        //    GameManager.instance.CallFireBase("NoCashFuel");
        //    return;
        //}

        //int _cash = PlayerDataManager.instance.playerData.playerCash - Mathf.CeilToInt(_totalPrice);
        //PlayerDataManager.instance.UpdateCash(-1 * Mathf.CeilToInt(_totalPrice));
        //UpdateCurrency(-1*(int)_totalPrice);
        //PlayerDataManager.instance.UpdateXP(((int)_totalPrice) / 2);
       // UpdateXP(((int)_totalPrice) / 2);
      //  CashText.text = "$"+_cash.ToString();// PlayerDataManager.instance.playerData.playerCash.ToString() + "$";
        DisplayInstructions("Fuel Delivery On The Way!");
       // GasStationManager.instance.OrderFuel(fuelOrderSlider.value);
      //  GameManager.instance.CallFireBase("FuelOrdered");
      //  OnClosePCPanel();
    }
    #endregion
    
    #region Customer Manipulation
    public void EnableCustomerServingPanel(UnityAction _assignAction,UnityAction _declineAction)
    {
        customerAssignmentAction = _assignAction;
        customerDeclineAction = _declineAction;
        customerRequestPanel.SetActive(true);
    }
    public void DisableCustomerServingPanel()
    {
        customerRequestPanel.SetActive(false);
    }
    public void OnAssignRoomToCustomer()
    {
        customerAssignmentAction?.Invoke();
    }
    public void OnDeclineRoomToCustomer()
    {
        customerDeclineAction?.Invoke();
        customerRequestPanel.SetActive(false);
    }
    public void OnHoldCustomer()
    {
        customerRequestPanel.SetActive(false);
       // GameManager.instance.CallFireBase("CustomerHeld", "held", 0);
    }
    #endregion

    #region AddingCurracnyAnimations
    bool isPlayingCurrencyAnim = false;
    bool isPlayingBlitzAnim = false;
    bool isPlayingXPAnim = false;
    public void UpdateCurrency(int _amount)
    {
        // CashText.text = "$" + PlayerDataManager.instance.playerData.playerCash.ToString();
        //   return;
        string _sign = "+$";
        if (_amount < 0)
        {
            _sign = "-$";
        }
        CashTextAnimation.GetComponent<Text>().text = _sign + Math.Abs(_amount);
        if (isPlayingCurrencyAnim)
        {
            return;
        }
        isPlayingCurrencyAnim = true;
        StartCoroutine(AddCurrancy());
    }

    /// <summary>
    /// Add currancy $ cash to the player data and animate 
    /// </summary>
    /// <param name="_amount"></param>
    /// <returns></returns>
    public IEnumerator AddCurrancy()
    {
        CashTextAnimation.gameObject.SetActive(true);
        CashTextAnimation.GetComponent<Animator>().SetInteger("fade", 1);
        yield return new WaitForSeconds(1f);
        CashText.text ="$"+ PlayerDataManager.instance.playerData.playerCash.ToString();
        CashTextAnimation.GetComponent<Animator>().SetInteger("fade", 0);
        isPlayingCurrencyAnim = false;
        yield return null;
        CashTextAnimation.gameObject.SetActive(false);
    }

    public void UpdateBlitz(int _amount)
    {
        BlitzTextAnimation.GetComponent<Text>().text = "+" + _amount.ToString();
        if (isPlayingBlitzAnim)
        {
            return;
        }
        isPlayingBlitzAnim = true;
        StartCoroutine(AddBlitz());
    }

    /// <summary>
    /// Add blitz to the player data
    /// </summary>
    /// <param name="_amount"></param>
    /// <returns></returns>
    public IEnumerator AddBlitz()
    {
        BlitzTextAnimation.gameObject.SetActive(true);
  
        BlitzTextAnimation.GetComponent<Animator>().SetInteger("fade", 1);
        yield return new WaitForSeconds(1f);
        BlitzText.text = PlayerDataManager.instance.playerData.playerBlitz.ToString();
        BlitzTextAnimation.GetComponent<Animator>().SetInteger("fade", 0);
        isPlayingBlitzAnim = false;
        yield return null;
        BlitzTextAnimation.gameObject.SetActive(false);
    }

    public void UpdateXP(int _amount)
    {
        XPTextAnimation.GetComponent<Text>().text = "+" + _amount.ToString();
        if (isPlayingXPAnim)
        {
            return;
        }
        isPlayingXPAnim = true;
        StartCoroutine(AddXP());
    }

    /// <summary>
    /// Add XP to the player and also show its animation of 
    /// adding and display total xp current and total xp required to level up next 
    /// </summary>
    /// <param name="_amount"></param>
    /// <returns></returns>
    public IEnumerator AddXP()
    {
        XPTextAnimation.gameObject.SetActive(true);
     
        XPTextAnimation.GetComponent<Animator>().SetInteger("fade", 1);
        yield return new WaitForSeconds(1f);
        //also add a condition or formula for level> levels array
        XPTextAnimation.GetComponent<Animator>().SetInteger("fade", 0);
        isPlayingXPAnim = false;
        yield return null;
        XPTextAnimation.gameObject.SetActive(false);
        UpdateMotelLevelAndXPBar();
        //check for level up case if xp earned >required show level up panel
        LevelManager.Instance.CheckForLevelUp();
    }

    /// <summary>
    /// Motel level is player level
    /// fill amount is current xp / total xp required for next level
    /// </summary>
    public void UpdateMotelLevelAndXPBar()
    {
      //  print("bar value is:" + LevelManager.Instance.GetLevelBarFillValue());
        xpBar.fillAmount = LevelManager.Instance.GetLevelBarFillValue();
        motelLevelText.text = "SUPERMARKET LEVEL " + PlayerDataManager.instance.playerData.playerLevel;
        XPText.text = LevelManager.Instance.GetCurrentAndNextLevelRequiredXPS();
    }
    #endregion
    #region Saving Implementation
    public void SaveGameData()
    {
        GameController.instance.SaveData();
    }
    public void EnableSavingProgressNotification(bool _enable)
    {
        savingNotification.GetComponent<Animator>().SetBool("Display", _enable);
    }
    public void UpdateSavingProgressText(string _msg,int _typeIndex)
    {
        savingNotificationText.GetComponent<LocalizeText>().UpdateText(_msg);
        savingNotificationImg.sprite = savingNotificationImgs[_typeIndex];
        savingNotificationImg.transform.GetChild(0).gameObject.SetActive(_typeIndex == 0);
    }
    #endregion
}
[System.Serializable]
public class PCTabObjects
{
    public string tabName;
    public GameObject tabPanel;
    public Image tabBtn;
    public Image tabIconImage;
    public Text tabNameText;
    public Sprite[] tabIcons;
}
[System.Serializable]
public class StorePanelReference
{
    public GameObject storeBtn;
    public GameObject storePanel;
}
[System.Serializable]
public class CategoryPanel
{
    public StorePanelReference panelRefs;
    public Dictionary<int, StorePanelReference> subPanelRefs;
    public CategoryPanel()
    {
        panelRefs = new StorePanelReference();
        subPanelRefs = new Dictionary<int, StorePanelReference>();
    }
}
[System.Serializable]
public enum HoverInstructionType
{
    Unknown=-1,
    General=0,
    Warning=1,
    Tools=2,
    GameStore=3
}