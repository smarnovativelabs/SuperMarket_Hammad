using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CashCounterManager : MonoBehaviour
{
  //  public static CashCounterManager instance;
    public Vector3 playerStartPosition;
    public Quaternion playerStartRotation;
    public Camera CashCounterCam;
    public CashCounter cashCounterState;
    //will be used for camera transitions
    [HideInInspector]
    public Countercamera counterCamScript;
    float initialFieldOfView;
  //  public Quaternion initialRotation;
   // public Vector3 initialPos;
    public GameObject[] CurrancyNotePrefabs;
    public ExchangeCurrency[] exchangedCurrencies;
    public Transform[] customerObjectsSpawnPoints;
    List<GameObject> CurrancyNotesListObjects;
    public Transform moneyTreyPos;
    public List<Changegiven> ChangeList;
    public Transform CustomerCartItemsCreationPoint;
    public Transform CustomerCartItemsDestructionPoint;
   // public GameObject customerItemPrefab;
    public GameObject totalBillAmountScreen;
    public GameObject cashBillingScreen;
    public TextMeshPro totalBillAmountText;
    public TextMeshPro amountGivenByCustomer;
    public TextMeshPro totalChangeRequiredByCustomer;
    public TextMeshPro totalChangeGiventoCustomer;
    public TextMeshPro cashBillingTotalBillText;

    public GameObject itemPricePrefab;
   /* public AudioClip itemBarcodeSound;
    public AudioClip cardCollectSound;
    public AudioClip cashMachineSound;
    public AudioSource counterSoundPlayer;*/
    decimal totalBill;
    decimal changeRequired;
    decimal changeGiven;
    decimal customerGivenAmount;
    string cardPaymentAmount = "";
   // [HideInInspector]
    public GameObject customer;
  //  public List<GameObject> currancyColliders;
    public int totalItemsToScan;
    public bool cashAnimationPlaying;
    public List<GameObject> customerItemsList;
    public int totalCustomersServed;
    public int totalCustomerNeedtoServeForCashierRV;

    [HideInInspector]
    public GameObject cashier;
    public GameObject rvCashier;
    public Transform rvCashierSpawnedPoint;
    public SuperStorePOS pos;
    [SerializeField]
    int counterId;
    public Transform cameraTransforms;
    private void Awake()
    {
      //  instance = this;
        initialFieldOfView = CashCounterCam.fieldOfView;
        ChangeList=new List<Changegiven>();
        CurrancyNotesListObjects=new List<GameObject>();
       // initialPos = cameraTransforms.position;
      //  initialRotation = cameraTransforms.rotation;

    }

   
    void Start()
    {
        counterCamScript= CashCounterCam.gameObject.GetComponent<Countercamera>();
    }
    /// <summary>
    /// Enable Cash coutner state 
    /// </summary>
    /// <param name="state"></param>
    public void ChangeCashCounterState(CashCounter state)
    {
       cashCounterState = state;
    }

    public void PlayerEntersCounter()
    {
       // counterCamScript.id
        if (cashier != null && cashier.GetComponent<Cashier>().employeestate != Employeestate.Leaving)
        {
            //  UIController.instance.DisplayInstructions("Superstore counter not available!");
            //if cashier is avaiable for this counter it will be set to rest
            cashier.GetComponent<Cashier>().UponEmployeeRest();
           // return;
        }
        playerStartRotation = Controlsmanager.instance.transform.GetChild(1).transform.rotation;
        playerStartPosition = Controlsmanager.instance.transform.GetChild(1).transform.position;

        CashCounterCam.transform.position = playerStartPosition;
        CashCounterCam.transform.rotation = playerStartRotation;
        Controlsmanager.instance.ActivateControls(false, 0);
        UIController.instance.EnableFPSPanel(false);
        EnableCounterCamera(true);

        counterCamScript.SetCameraState(CashCounterCameraState.Default, OnCompleteEnterToCounter);

    }
    void OnCompleteEnterToCounter()
    {
        TutorialManager.instance.OnCompleteTutorialTask(16);
        UIController.instance.EnableCounterPanel(true);
        UIController.instance.EnableCashContainers(true);
        EnableCashBillingScreen(false);//Sets To Display Total Bill Screen Only
        SuperStoreManager.instance.UpdateAtStoreCounterState(true);
        //Make A Condition to reset
        if (cashCounterState == CashCounter.CollectingItem)
        {
            UIController.instance.ResetCounterPanel();

        }else if (cashCounterState == CashCounter.CashBilling)
        {
            UIController.instance.EnableCashExchangePanel(true);
            EnableCashBillingScreen(true);
        }else if (cashCounterState == CashCounter.CardBilling)
        {
            UIController.instance.EnableCardPanel(true);
        }
    }
    void OnCompleteLeaveToCounter()
    {
        Controlsmanager.instance.ActivateControls(true, 4.5f);
        EnableCounterCamera(false);
        UIController.instance.EnableFPSPanel(true);

        UIController.instance.EnableCounterPanel(false);
        SuperStoreManager.instance.UpdateAtStoreCounterState(false);

        //if cashier is leaving do not set it back to workPlace
        if (cashier != null && cashier.GetComponent<Cashier>().employeestate!=Employeestate.Leaving)
        {
            cashier.GetComponent<Cashier>().ChangeEmployeeState(Employeestate.MovingToWorkPlace);
        }

        //player dont have any counter
        SuperStoreManager.instance.playerAtCounterId = -1;
    }

    public void LeaveCashCounter()
    {
        counterCamScript.SetCameraState(CashCounterCameraState.LeaveCounter,OnCompleteLeaveToCounter);
        UIController.instance.EnableCounterPanel(false);

        UIController.instance.EnableCashContainers(false);
    }

    public void EnableCounterCamera(bool state)
    {
        CashCounterCam.gameObject.SetActive(state);
    }

    public void CreateCash(int _index)
    {
        if(_index<0 || _index > exchangedCurrencies.Length)
        {
            print("Invalid Exchange Currency Selected");
            return;
        }
        ExchangeCurrency _currency = exchangedCurrencies[_index];
        cashAnimationPlaying = true;
        GameObject cash = Instantiate(_currency.spawnNote, _currency.spawnPosition.position, _currency.spawnPosition.rotation);
        Vector3 _pos = UnityEngine.Random.insideUnitSphere * 0.08f;
        _pos = new Vector3(_pos.x, 0, _pos.z);
        CurrancyNotesListObjects.Add(cash);
        cash.gameObject.GetComponent<Animatecurrancy>().setCurrenDestination(CurrancyMove.MovetoTry, moneyTreyPos.position + _pos);

        Changegiven instance = new Changegiven();

        instance.changevalud = _currency.currencyValue;

        instance.model = cash;


        if (_currency.isCent)
        {
            instance.isCent = true;
        }

        AddChangeToList(instance);

        decimal _cur = 0;
        if(decimal.TryParse(_currency.currencyValue, out _cur))
        {
            AddToChange(_cur);

        }
        SoundController.instance.OnPlayInteractionSound(_currency.currencySound);
    }

    //public void CreateCash(int index,float value,Vector3 pos,GameObject item,bool isCent)
    //{
    //  cashAnimationPlaying = true;
    //  GameObject cash=Instantiate(CurrancyNotePrefabs[index], pos, CurrancyNotePrefabs[index].transform.rotation);
    //  Vector3 _pos = UnityEngine.Random.insideUnitSphere*0.08f;
    //  _pos=new Vector3(_pos.x,0, _pos.z);
    //   CurrancyNotesListObjects.Add(cash);
    //  cash.gameObject.GetComponent<Animatecurrancy>().setCurrenDestination(CurrancyMove.MovetoTry, moneyTreyPos.position+ _pos);
  

    //  Changegiven instance=new Changegiven();

    //  instance.changevalud = value.ToString();

    //  instance.model = cash;


    //    if (isCent)
    //    {
    //        instance.isCent = true;
    //    }

    //  AddChangeToList(instance);
    //  AddToChange(value);
    //}
    /// <summary>
    /// Destroy change created by the cashier for the customer
    /// upon moving to next customer
    /// </summary>
    public void DestroyChangeCreated()
    {
        foreach (GameObject obj in CurrancyNotesListObjects)
        {
            Destroy(obj);
        }
        CurrancyNotesListObjects.Clear();
        
    }
    #region ListManegament

    public void AddChangeToList(Changegiven instance)
    {
        ChangeList.Add(instance);
    }

    public void OnConfirmChange()
    {
        if (changeGiven == changeRequired)
        {
            ///procedd to checkOut and move to next customer
            customer.GetComponent<SuperStoreCustomer>().OnLeavingArea();
            ClearAllChangeList();
            DestroyChangeCreated();
            EnableCashBillingScreen(false);
            StartCoroutine(MoveToNextCustomer());
            GameManager.instance.CallFireBase("CashCusSrvd");

        }
        else
        {
            GameManager.instance.CallFireBase("InvldCshEntr");
            customer.GetComponent<SuperStoreCustomer>().OnWrongCheckout();
            decimal _difference = Math.Abs(changeRequired - changeGiven);
            if (_difference < 1)
            {
                UIController.instance.DisplayInstructions(_difference.ToString("0.0#") + " in coins/cents still missing");
                UIController.instance.AnimateCentsContainer();
            }
            else
            {
                UIController.instance.DisplayInstructions("Invalid Change Amount!");
            }
        }
    }
    public void RollBackCurrancy()
    {
        if (ChangeList.Count > 0)
        {
            ChangeList[ChangeList.Count - 1].model.GetComponent<Animatecurrancy>().setCurrenDestination(CurrancyMove.MovetoBox, ChangeList[ChangeList.Count - 1].model.GetComponent<Animatecurrancy>().moneyBoxPos);
          //one frame delay is required for the proper removal
           StartCoroutine(RemoveChangeToList());
        }
    }
    public void OnResetExchangeCurrency()
    {
        ClearAllChangeList();
        DestroyChangeCreated();
        changeGiven = 0;
        totalChangeGiventoCustomer.text = "$" + changeGiven.ToString("####0.00");
        totalChangeGiventoCustomer.color = Color.red;
    }
    public IEnumerator RemoveChangeToList()
    {
        yield return new WaitForEndOfFrame();

        if (ChangeList.Count > 0)
        {
            decimal _cur = 0;
            if(decimal.TryParse(ChangeList[ChangeList.Count - 1].changevalud, out _cur))
            {
                RemoveToChange(_cur);
            }
            ChangeList.RemoveAt(ChangeList.Count-1);
        }
    }

    public void ClearAllChangeList()
    {
        ChangeList.Clear();
    }

    #endregion

    #region Billing

    public void AcceptWishList(GameObject customer,List<SuperStoreItems> _list)
    {
        this.customer = customer;
        CreateCustomerCartItemsOnCounter(_list);
    }
    /// <summary>
    /// This method creates customer items on the counter at random positions.
    /// It spawns a set number of items based on the provided count, positioning them near the creation point.
    /// </summary>
    /// <param name="count">The number of items to create on the counter.</param>
    public void CreateCustomerCartItemsOnCounter(List<SuperStoreItems> _list)
    {
        // Create up to 5 customer items at random positions near the counter
        totalItemsToScan = _list.Count;

        for (int i = 0; i < _list.Count; i++)
        {
            ItemData _item = GameManager.instance.GetItem((CategoryName)_list[i].catId, _list[i].subCatId, _list[i].itemId);

            if (_item != null)
            {
                GameObject customerItem = null;
                if (i >= customerObjectsSpawnPoints.Length)
                {
                    Vector3 _pos = UnityEngine.Random.insideUnitSphere * 0.25f;
                    _pos = new Vector3(_pos.x, 0, _pos.z); // Ensure items are placed on the same Y-axis level
                    customerItem = Instantiate(_item.singleItemPrefab, (CustomerCartItemsCreationPoint.position + _pos), Quaternion.identity);
                }
                else
                {
                    customerItem = Instantiate(_item.singleItemPrefab, customerObjectsSpawnPoints[i].position, customerObjectsSpawnPoints[i].rotation);
                }
                
                customerItem.AddComponent<Billingitemsanimation>();
                customerItem.GetComponent<Billingitemsanimation>().speed = 2f;
                customerItem.GetComponent<Billingitemsanimation>().counterId = counterId;
                customerItem.AddComponent<Superstoreitems>();
                customerItem.GetComponent<Superstoreitems>().Price= _item.itemSellingPrice;

                //adding item to list for the wprker to user
                customerItemsList.Add(customerItem);

            }


        }
        // Change the cash counter state to Billing after placing items
    }
    public void OnRemoveItem(GameObject _item)
    {
        if (customerItemsList.Contains(_item))
        {
            customerItemsList.Remove(_item);
        }
    }
    /// <summary>
    /// Handles the interaction when the user chooses a payment method (Cash or Card).
    /// Adjusts the camera state based on the selected payment method.
    /// </summary>
    /// <param name="method">The chosen payment method (Cash or Card).</param>
    public void OnInteractionWithCashOrCard(PaymentMethod method)
    {
        print("nice1");
        customer.GetComponent<SuperStoreCustomer>().UpdateAnimation(false);
        //customer.GetComponent<SuperStoreCustomer>().UpdateAnimation(false);

        if (method == PaymentMethod.Cash)
        {
            UIController.instance.EnableCashExchangePanel(true);
            cashCounterState = CashCounter.CashBilling;
            SetCashBillingValues();
            EnableCashBillingScreen(true);
            PlayCashCounterSound(SuperStoreManager.instance.cashMachineSound);
        }
        else
        {
            print("huuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuu");
            UIController.instance.EnableCardPanel(true);
            cashCounterState = CashCounter.CardBilling;
            print("Card Collect Sound is actoive" + SuperStoreManager.instance.cardCollectSound);
            PlayCashCounterSound(SuperStoreManager.instance.cardCollectSound);

        }
    }

    void SetCashBillingValues()
    {
        customerGivenAmount = ((int)totalBill) + (UnityEngine.Random.Range(1, 80));
        changeRequired = (customerGivenAmount) - totalBill;
        changeGiven = 0;
        cashBillingTotalBillText.text = "$" + totalBill.ToString("#####0.00");
        amountGivenByCustomer.text = "$" + customerGivenAmount.ToString("####0.00");
        totalChangeRequiredByCustomer.text = "$" + changeRequired.ToString("####0.00");
        totalChangeGiventoCustomer.text = "$0.00";
        UpdateGivenChangeColor(true);
    }
    void EnableCashBillingScreen(bool _enable)
    {
        cashBillingScreen.SetActive(_enable);
        totalBillAmountScreen.SetActive(!_enable);
    }

    /// <summary>
    /// This method is triggered when all items have been scanned.
    /// It adjusts the camera and updates the LCD display with the total bill and change information.
    /// </summary>
    public void UponAllItemsScanned()
    {
        // Change the camera focus to the customer
    //    counterCamScript.SetCameraState(CashCounterCameraState.FocusCustomer);

        //int _custoemrGivenAmount = ((int)totalBill) + (UnityEngine.Random.Range(1, 80));
        //float _changeAmount = ((float)_custoemrGivenAmount) - totalBill;

        //SetCustomerChangeRequiredOnScreen(_changeAmount);
        // Update the cash LCD display with total bill and change details

        // TODO: Enable the customer giving cash animation (not implemented)
        cashCounterState = CashCounter.PaymentMode;
       
        if (customer != null)
        {
            print("qwert");
            customer.GetComponent<SuperStoreCustomer>().UpdateAnimation(true);
        }
       // customer.GetComponent<SuperStoreCustomer>().UpdateAnimation(true);
    }
    /// <summary>
    /// Adds an amount to the total bill and updates the display.
    /// </summary>
    /// <param name="add">The amount to add to the total bill.</param>
    public void AddTotalBill(float add)
    {
        totalBill += (decimal)add;
        totalBillAmountText.text = "$ " + totalBill.ToString("####0.00");
        PlayCashCounterSound(SuperStoreManager.instance.itemBarcodeSound);
        // Update the swipe machine with the total bill
     //   swipeMachineTotalBillText.text = "$" + totalBill;
    }

    /// <summary>
    /// Sets the change required on the screen when the user taps on card or cash.
    /// </summary>
    /// <param name="add">The amount to display as the change required.</param>
    /// <param name="changeRequired">The actual change required amount.</param>
    //public void SetCustomerChangeRequiredOnScreen(float changeRequired)
    //{
    //    this.changeRequired = changeRequired; // Store the actual change required
    //    totalChangeRequiredByCustomer.text = "$" + this.changeRequired; // Display the added amount
    //}

    /// <summary>
    /// Appends the generated change to the total change given.
    /// </summary>
    /// <param name="add">The amount to add to the total change given.</param>
    public void AddToChange(decimal add)
    {
        changeGiven += add;
        totalChangeGiventoCustomer.text = "$" + changeGiven.ToString("#####0.00");
        UpdateGivenChangeColor();
    }
    void UpdateGivenChangeColor(bool _reset=false)
    {
        if (_reset)
        {
            totalChangeGiventoCustomer.color = Color.red;
            return;
        }
        if (changeGiven == changeRequired)
        {
            totalChangeGiventoCustomer.color = Color.green;
        }
        else
        {
            totalChangeGiventoCustomer.color = Color.red;//(changeGiven > changeRequired) ? Color.red : Color.yellow;
        }
    }
    /// <summary>
    /// Removes an amount from the total change given.
    /// </summary>
    /// <param name="minus">The amount to subtract from the total change given.</param>
    public void RemoveToChange(decimal minus)
    {
        changeGiven = changeGiven - minus;

        // Ensure that the change given does not go below zero
        if (changeGiven < 0) changeGiven = 0;

        totalChangeGiventoCustomer.text = "$" + changeGiven.ToString("#####0.00");
        UpdateGivenChangeColor();
    }

    public void ResetTotalBillAmount()
    {
        totalBill = 0;
        totalBillAmountText.text = "$ " + totalBill.ToString("####0.0");
    }
    /// <summary>
    /// Handles interactions with the card swipe machine, processing user inputs for card payment.
    /// This method processes different input types ("ok", "remove", or numerical inputs), 
    /// manages card payment entry, validates the entered amount, and provides feedback based on the payment's validity.
    /// </summary>
    /// <param name="input">The input string representing the user's interaction (e.g., "ok" for submit, "remove" for delete, or numeric value).</param>
    public void OnInteractionWithCardSwipeMachine(string input)
    {
        SoundController.instance.OnPlayInteractionSound(UIController.instance.uiButtonSound);

        // Exit the method if the cash counter is closed
      //  if (CashCounterState == CashCounter.Close) return;

        // Handle "ok" input, representing the Enter/Submit button press
        if (input == "OK")
        {
            decimal _addedAmount = 0;
            // Attempt to parse the entered card payment amount into a decimal
            if (decimal.TryParse(cardPaymentAmount, out _addedAmount))
            {
            }
            // Check if the entered amount matches the total bill
            if ( totalBill == _addedAmount)
            {
                customer.GetComponent<SuperStoreCustomer>().OnLeavingArea();
                StartCoroutine(MoveToNextCustomer()); // Move to the next customer after successful payment
                cardPaymentAmount = "0";
                UIController.instance.UpdateCardEnteredText(cardPaymentAmount);
                GameManager.instance.CallFireBase("CardCusSrvd");

            }
            else
            {
                GameManager.instance.CallFireBase("InvldCrdEntr");
                decimal _difference = Math.Abs(_addedAmount - totalBill);
                if (_difference < 1)
                {
                    UIController.instance.DisplayInstructions(_difference.ToString("0.0#") + " in point is still missing");
                    UIController.instance.AnimateCardDecimal();
                }
                else
                {
                    // Display a message for an invalid or incorrect amount
                    UIController.instance.DisplayInstructions("Invalid Amount Entered!");
                }

                
                customer.GetComponent<SuperStoreCustomer>().OnWrongCheckout();
            }
        }
        // Handle "remove" input, representing the backspace/delete action
        else if (input == "Remove")
        {
            // If there is an amount entered, remove the last character
            if (cardPaymentAmount.Length > 0)
            {
                cardPaymentAmount = cardPaymentAmount.Substring(0, cardPaymentAmount.Length - 1);
            }
            else
            {
                cardPaymentAmount = "0"; // Reset the input if no characters remain
            }
        }
        else if (input == "Clear")
        {
            cardPaymentAmount = "0";
        }
        else        // Handle numeric input for the card payment amount
        {
            if (string.IsNullOrEmpty(cardPaymentAmount) || cardPaymentAmount == "0")
            {
                cardPaymentAmount = input;
            }
            else
            {
                if (cardPaymentAmount.Length < 7)
                {
                    if (input == ".")
                    {
                        if (!cardPaymentAmount.Contains(input))
                        {
                            cardPaymentAmount += input; // Append the input to the current card payment string
                        }
                    }
                    else
                    {
                        cardPaymentAmount += input; // Append the input to the current card payment string

                    }
                }
            }
        }

        // Update the UI based on the entered amount
        if (string.IsNullOrEmpty(cardPaymentAmount) || cardPaymentAmount == "0")
        {
            cardPaymentAmount = "0";
        }
        //else
        //{
        //    // Display the current card payment amount
        //    swipeMachineEnterdText.text = cardPaymentAmount.ToString();
        //}
        UIController.instance.UpdateCardEnteredText(cardPaymentAmount);
        // Output the current ATM amount to the console (for debugging purposes)
        //print("Current atmAmount: " + atmAmount);
    }
    
    #endregion

    public IEnumerator MoveToNextCustomer()
    {
        totalCustomersServed++;
        //check For cashier RV spawning
        CheckForCashierSpawnRV();

     
        customerItemsList.Clear();
        cashCounterState = CashCounter.CollectingItem;
        ResetTotalBillAmount();
        UIController.instance.ResetCounterPanel();
        SuperStoreManager.instance.OnCustomerServed();
        SuperStoreManager.instance.DisplayInterstitialAd();
        customer = null;
        //counterCamScript.SetCameraState(CashCounterCameraState.Default);
        yield return null;

    }

    public void PlayCashCounterSound(AudioClip _clip)
    {
        print("20f");
        if (_clip == null)
            return;
        if (!SoundController.instance.isSoundOn)
        {
            return;
        }
        print("final");
        print("final1" + _clip);
        //PlayCounterSound(_clip);
   //     SuperStoreManager.instance.counterSoundPlayer.clip = _clip;
        SuperStoreManager.instance.counterSoundPlayer.PlayOneShot(_clip);
    }
    public void PlayCounterSound(AudioClip _clip)
    {
        if (_clip == null)
        {
            Debug.LogError("AudioClip is null!");
            return;
        }

        if (!SuperStoreManager.instance.counterSoundPlayer.enabled)
        {
            Debug.Log("AudioSource is disabled!");
            SuperStoreManager.instance.counterSoundPlayer.enabled = true;
        }

        if (SuperStoreManager.instance.counterSoundPlayer.volume <= 0)
        {
            Debug.Log("AudioSource volume is too low!");
            SuperStoreManager.instance.counterSoundPlayer.volume = 1f;
        }

        if (SuperStoreManager.instance.counterSoundPlayer.isPlaying)
        {
            Debug.Log("AudioSource is already playing a clip. Stopping it now.");
            SuperStoreManager.instance.counterSoundPlayer.Stop();
        }

        SuperStoreManager.instance.counterSoundPlayer.PlayOneShot(_clip);
        Debug.Log("Audio is now playing!");
    }
    #region Cashier

    /// <summary>
    /// Check for the list if list have items
    /// </summary>
    /// <returns></returns>
    public List<GameObject> CashierRequestForCartItems()
    {
        if (customer == null)
        {
            return null;
        }
        if (customerItemsList.Count > 0)
        {
            return customerItemsList;
        }
        else
        {
            return new List<GameObject>();
        }
    }
    public void OnCashierCheckOut()
    {
        customer.GetComponent<SuperStoreCustomer>().OnLeavingArea(true);
        ClearAllChangeList();
        DestroyChangeCreated();
        EnableCashBillingScreen(false);
        //  StartCoroutine(MoveToNextCustomer());
        cashCounterState = CashCounter.CollectingItem;
        ResetTotalBillAmount();
     //   UIController.instance.ResetCounterPanel();
        SuperStoreManager.instance.OnCustomerServed();
        customerItemsList.Clear();
        customer = null;
    }

    public void UpdateAnimationForCustomerForCashier()
    {
      //  print("called by cashier+++++++++++++++++++++++++++++++++++");
        customer.GetComponent<SuperStoreCustomer>().UpdateAnimation(false);
        
    }

    /* public void AddCashier(Register _cashier)
     {
         cashiers.Add(_cashier);
     }

     public void RemoveCashier()
     {
         if (cashiers.Count > 0)
         {
             cashiers.RemoveAt(0);
         }
     }

     public void RegisterEmployee(GameObject employee)
     {
         Register ins = new Register();
         ins.employeeId = employee.GetComponent<Cashier>().employeeId;
         if (ins.employeeId == -1) return;
         ins.employeeObject= employee;
         AddCashier(ins);
     }
    */

    /*  public void setCounterTakenState(bool _state)
      {
          isCounterTaken = _state;
      }*/

    #endregion

    public float GetInitialFielOfView()
    {
        return initialFieldOfView;
    }


    #region CashierRV

    public void CheckForCashierSpawnRV()
    {
        print("k1");
        if (cashier != null)
        {
            print("cashier already exist");
            return;
        }
        print("total customers served:" + totalCustomersServed);
        if (totalCustomersServed >= totalCustomerNeedtoServeForCashierRV)
        {
            print("spawning new customer");
            rvCashier.SetActive(true);
            print("RVcashier is " + rvCashierSpawnedPoint.name);
            print("RVcashier 1 is " + rvCashierSpawnedPoint.position.x);
            print("RVcashier 2 is " + rvCashierSpawnedPoint.position.y);
            print("RVcashier 3 is " + rvCashierSpawnedPoint.position.z);
            rvCashier.transform.position = rvCashierSpawnedPoint.position;

            // rvCashierSpawned.transform.rotation = rvCashierSpawnedPoint.transform.rotation;
            rvCashier.GetComponent<EmployeeRV>().SetEmployeeToIdle();
        }
        
    }

    public void DisableCashierRV()
    {
        if (rvCashier.gameObject.activeInHierarchy)
        {
            rvCashier.SetActive(false);

        }
    }

#endregion

    public void SetCounterID(int _id)
    {
        counterId = _id;
    }

    public int GettCounterID()
    {
       return counterId;
    }
}
public enum CashCounter
{
    CollectingItem,
    PaymentMode,
    CashBilling,
    CardBilling
}

public class Changegiven
{
    public string changevalud;
    public bool isCent;
    public GameObject model;
}
[System.Serializable]
public class ExchangeCurrency
{
    public string currencyValue;
    public Transform spawnPosition;
    public GameObject spawnNote;
    public bool isCent;
    public AudioClip currencySound;
}
[System.Serializable]
public class Register
{
    public int employeeId;
    public GameObject employeeObject;
}

