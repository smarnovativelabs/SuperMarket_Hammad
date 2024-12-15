//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;
//using UnityEngine.Events;
//using UnityEngine.Purchasing;

//public class GasStationManager : MonoBehaviour
//{
//    public float vehicleWaitTime = 120f;
//    public float vehicleApproxSpawnDelay = 30f;
//    public GameObject fuelTruck;
//    public FillingPoint[] fillingPoints;
//    public GameObject[] availableVehicles;
//    public Transform fuelDeliveryPoint;
//    public Transform pipeConnectionPoint;
//    public Transform fuelTruckRouteParent;
//    public Transform vehiclesRouteParent;
//    public Transform vehiclesEndRouteParent;
//    public GasStationData gasData;
//    public static GasStationManager instance;
//    bool isDataInitialized = false;
//    public UnityAction updateGasReserve;
//    float deliveryTruckTimer = 0f;
//    bool canSpawnVehicles = false;
//    float spawnTimer;
//    int vehicleSpawnCounter = 0;
//    int successFuelDeliverCount;
//    int failFuelDeliverCount;
//    int vehiclesInWait = 0;
//    int interactingFillingPoint = -1;
//    bool isInFuelStationArea = false;
//    private void Awake()
//    {
//        instance = this;
//    }
//    // Update is called once per frame
//    void Update()
//    {
//        if (deliveryTruckTimer > 0)
//        {
//            deliveryTruckTimer -= Time.deltaTime;
//            if (deliveryTruckTimer <= 0)
//            {
//                SpawnDeliveryTruck();
//            }
//        }
//        if (canSpawnVehicles)
//        {
//            spawnTimer -= Time.deltaTime;
//            if (spawnTimer <= 0f)
//            {
//                SpawnVehicle();
//            }
//        }
//    }
//    public void OpenFuelStation(bool _open)
//    {
//        canSpawnVehicles = _open;
//        if (_open)
//        {
//            SetSpawnTimer();
//        }
//    }
//    void SetSpawnTimer()
//    {
//        float _minSpawnDelay = vehicleApproxSpawnDelay - 5f;
//        _minSpawnDelay = _minSpawnDelay < 0 ? 2f : _minSpawnDelay;
//        float _maxSpawnDelay = vehicleApproxSpawnDelay + 5f;
//        spawnTimer = Random.Range(_minSpawnDelay, _maxSpawnDelay);
//    }


//    void SpawnVehicle()
//    {
//        KeyValuePair<int, int> _emptySpotIndex = GetEmptyStopPoint();
//        if (_emptySpotIndex.Key < 0 || _emptySpotIndex.Value < 0)
//        {
//            spawnTimer = 5f;
//            return;
//        }
//        GameObject _vehicle = Instantiate(availableVehicles[vehicleSpawnCounter]);
//        fillingPoints[_emptySpotIndex.Key].fuelStopPoints[_emptySpotIndex.Value].vehicle = _vehicle.transform;

//        _vehicle.GetComponent<GasStationVehicle>().OnSpawnVehicle(_emptySpotIndex, vehiclesRouteParent, fillingPoints[_emptySpotIndex.Key].routeParent,
//            vehiclesEndRouteParent,fillingPoints[_emptySpotIndex.Key].fuelStopPoints[_emptySpotIndex.Value].stopPoint);
//        vehicleSpawnCounter++;
//        if (vehicleSpawnCounter >= availableVehicles.Length)
//        {
//            vehicleSpawnCounter = 0;
//        }
//        SetSpawnTimer();
//    }
//    /// <summary>
//    /// Returns Index of Filling Point Which Have A Free Vehicle Space
//    /// And Value Is The Index Of Free Space
//    /// </summary>
//    /// <returns></returns>
//    KeyValuePair<int, int> GetEmptyStopPoint()
//    {
//        for (int i = 0; i < fillingPoints.Length; i++)
//        {
//            if (!fillingPoints[i].isActive)
//            {
//                continue;
//            }
//            if (fillingPoints[i].isUpdatingQueue)
//            {
//                continue;
//            }

//            for (int j = 0; j < fillingPoints[i].fuelStopPoints.Length; j++)
//            {
//                if (fillingPoints[i].fuelStopPoints[j].vehicle == null)
//                {
//                    KeyValuePair<int, int> _stopPoint = new KeyValuePair<int, int>(i, j);
//                    return _stopPoint;
//                }
//            }
//        }
//        KeyValuePair<int, int> _val = new KeyValuePair<int, int>(-1, -1);
//        return _val;
//    }
//    public bool IsNozelAttachedToVehicle(int _fillingPointIndex)
//    {
//        if (_fillingPointIndex < 0)
//            return false;

//        if (fillingPoints[_fillingPointIndex].fuelStopPoints[0].vehicle == null)
//            return false;

//        return fillingPoints[_fillingPointIndex].fuelStopPoints[0].vehicle.gameObject.GetComponent<GasStationVehicle>().IsFuelNozelInjected();
//    }
//    public bool IsFuelingVehicle(int _fillingPointIndex)
//    {
//        if (_fillingPointIndex < 0)
//            return false;

//        return fillingPoints[_fillingPointIndex].fuelMachine.GetComponent<FuelMachine>().IsFueling();
//    }
//    public bool UpdateFuelingStatus(int _fillingPointIndex, bool _isFueling)
//    {
//        if (_fillingPointIndex < 0)
//            return false;

//        if (fillingPoints[_fillingPointIndex].fuelStopPoints[0].vehicle == null)
//            return false;

//        fillingPoints[_fillingPointIndex].fuelStopPoints[0].vehicle.gameObject.GetComponent<GasStationVehicle>().SetFuelingStatus(_isFueling);
//        return true;
//    }
//    public int GetFillingPointPrice(int _fillingPointIndex)
//    {
//        if (_fillingPointIndex < 0)
//            return -1;
//        return fillingPoints[_fillingPointIndex].fillingStationPrice;
//    }
//    public bool UnlockFillingPoint(int _fillingPointIndex)
//    {
//        if (_fillingPointIndex < 0)
//            return false ;

//        if (fillingPoints[_fillingPointIndex].isActive)
//        {
//            return false;
//        }

//        if (fillingPoints[_fillingPointIndex].reqLevel > PlayerDataManager.instance.playerData.playerLevel)
//        {
//            UIController.instance.EnablePopupNotification("Filling Station Will Unlock At Level " + fillingPoints[_fillingPointIndex].reqLevel.ToString());
//            GameManager.instance.CallFireBase("FlMchnUnlkBfLev_" + _fillingPointIndex.ToString());

//            return false;
//        }
//        if (fillingPoints[_fillingPointIndex].fillingStationPrice > PlayerDataManager.instance.playerData.playerCash)
//        {
//            UIController.instance.EnableNoCashPanel();
//            GameManager.instance.CallFireBase("NoCashFlMchn_" + _fillingPointIndex.ToString());

//            return false;
//        }
//        gasData.activeFillingPointsIndex.Add(_fillingPointIndex);
//        fillingPoints[_fillingPointIndex].isActive = true;
//        fillingPoints[_fillingPointIndex].fuelMachine.isUnlocked = true;
//        fillingPoints[_fillingPointIndex].fuelNozel.isUnlocked = true;
//        fillingPoints[_fillingPointIndex].lockImage.SetActive(false);
//        EmployeeManager.Instance.SetDeptWorkPlaceLockState(EmployeeType.FuelAttendants, _fillingPointIndex, true);

//        PlayerDataManager.instance.UpdateCash(-1 * fillingPoints[_fillingPointIndex].fillingStationPrice);
//        UIController.instance.UpdateCurrency(-1 * fillingPoints[_fillingPointIndex].fillingStationPrice);
//      //  PlayerDataManager.instance.UpdateXP(fillingPoints[_fillingPointIndex].fillingStationPrice / 2);
//      //  UIController.instance.UpdateXP(fillingPoints[_fillingPointIndex].fillingStationPrice / 2);
//        UIController.instance.DisplayInstructions("Fuel Machine Unlocked!");

//        updateGasReserve += fillingPoints[_fillingPointIndex].fuelMachine.UpdateGasReserve;
//        fillingPoints[_fillingPointIndex].fuelMachine.ResetValues();
//        fillingPoints[_fillingPointIndex].fuelMachine.UpdateFuelPriceText();
//        GameManager.instance.CallFireBase("Filling_" + _fillingPointIndex.ToString() + "_prchsd");
//        return true;
//    }

//    public bool CanOpenGasStation()
//    {
//        if (!CanOrderFuel())
//        {
//            return false;
//        }
//        if(gasData.inDeliveryFuel>0 || gasData.reserveGas > 0)
//        {
//            return true;
//        }
//        UIController.instance.DisplayInstructions("Order Some Fuel First!");
//        return false;
//    }
//    #region Vehicle Manipulation
//    public void DisplayVehicleOrderFuel(int _fillingPointIndex, float _val)
//    {
//        if (_fillingPointIndex < 0)
//            return;

//        fillingPoints[_fillingPointIndex].fuelMachine.GetComponent<FuelMachine>().SetOrderedFuelText(_val);
//    }

//    public void DisplayVehicleMaxFuel(int _fillingPointIndex, float _val)
//    {
//        if (_fillingPointIndex < 0)
//            return;

//        fillingPoints[_fillingPointIndex].fuelMachine.GetComponent<FuelMachine>().SetmaxFuel(_val);
//    }

//    public void AddFuelToVehicle(int _fillingPointIndex)
//    {
//        if (_fillingPointIndex < 0)
//            return;

//        if (gasData.reserveGas <= 0)
//        {
//            return;
//        }
//        if (fillingPoints[_fillingPointIndex].fuelStopPoints[0].vehicle == null)
//            return;
//        float _val = fillingPoints[_fillingPointIndex].fuelStopPoints[0].vehicle.gameObject.GetComponent<GasStationVehicle>().AddFuelToVehicle();
//        if (_val >= 0)
//        {
//            gasData.reserveGas -= (Time.deltaTime / 2f);
//            gasData.reserveGas = gasData.reserveGas <= 0 ? 0 : gasData.reserveGas;
//            fillingPoints[_fillingPointIndex].fuelMachine.GetComponent<FuelMachine>().UpdateCurrentFuelText(_val);
//            updateGasReserve?.Invoke();
//            if (gasData.reserveGas <= 0)
//            {
//                UIController.instance.DisplayInstructions("Fuel Finished, Order More Fuel From PC");
//            }
//        }
//    }
//    public void OnRemoveVehicle(int _fillingPointIndex)
//    {
//        if (_fillingPointIndex < 0)
//            return;

//        fillingPoints[_fillingPointIndex].fuelStopPoints[0].vehicle = null;
//        fillingPoints[_fillingPointIndex].isUpdatingQueue = true;
//        if (fillingPoints[_fillingPointIndex].fuelStationEmployee != null)
//        {
//            fillingPoints[_fillingPointIndex].fuelStationEmployee.GetComponent<Employee>().ResetServingCustomer();
//        }
//        UpdateVehicleInWait(-1);
//        StartCoroutine(RemoveVehicles(_fillingPointIndex));
//    }
//    IEnumerator RemoveVehicles(int _fillingPointIndex)
//    {
//        int _initialEmptySpot = 0;
//        for (int i = 1; i < fillingPoints[_fillingPointIndex].fuelStopPoints.Length; i++)
//        {
//            yield return new WaitForSeconds(1f);
//            if (fillingPoints[_fillingPointIndex].fuelStopPoints[i].vehicle != null)
//            {
//                FuelStopPoint _point = fillingPoints[_fillingPointIndex].fuelStopPoints[_initialEmptySpot];
//                _point.vehicle = fillingPoints[_fillingPointIndex].fuelStopPoints[i].vehicle;
//                _point.vehicle.GetComponent<GasStationVehicle>().OnUpdateVehicleStopPoint(_initialEmptySpot, _point.stopPoint);
//                fillingPoints[_fillingPointIndex].fuelStopPoints[i].vehicle = null;
//                _initialEmptySpot++;
//            }
//        }
//        fillingPoints[_fillingPointIndex].isUpdatingQueue = false;
//    }
//    public void UpdateVehicleInWait(int _val)
//    {
//        vehiclesInWait += _val;
//        if (vehiclesInWait < 0)
//        {
//            vehiclesInWait = 0;
//        }
//        UIController.instance.UpdateGasStationWaitingCustomers(vehiclesInWait);
//    }
//    public void OnSuccessFuelDelivered(int _fillingPointIndex)
//    {
//        successFuelDeliverCount++;
//        GameManager.instance.CallFireBase("FuelDelivered_" + successFuelDeliverCount.ToString());
//        fillingPoints[_fillingPointIndex].fuelMachine.ResetValues();
//    }
//    public void OnFailedFuelDelivery()
//    {
//        failFuelDeliverCount++;
//        GameManager.instance.CallFireBase("FuelFailDeliver_" + failFuelDeliverCount.ToString());
//    }
//    #endregion
//    #region Fuel Truck Deliver/Reserves
//    public void UpdateReservesText()
//    {
//        updateGasReserve?.Invoke();
//    }
//    public bool IsAlreadyFuelOrdered()
//    {
//        return (gasData.inDeliveryFuel > 0);
//    }
//    public bool CanOrderFuel()
//    {
//        if (fillingPoints[0].reqLevel > PlayerDataManager.instance.playerData.playerLevel)
//        {
//            UIController.instance.DisplayInstructions("Gas Station Will Unlock At Level " + fillingPoints[0].reqLevel.ToString());
//            return false;
//        }
//        if (gasData.activeFillingPointsIndex.Count < 1)
//        {
//            UIController.instance.DisplayInstructions("Purchase A Fuel Machine First!");
//            return false;
//        }
//        return true;
//    }
//    public void OrderFuel(float _val)
//    {
//        gasData.inDeliveryFuel = _val;
//        deliveryTruckTimer = Random.Range(4f, 6f);
//    }
//    void SpawnDeliveryTruck()
//    {
//        GameObject _truck = Instantiate(fuelTruck);
//        _truck.GetComponent<FuelTruck>().OnSpawnTruck(fuelTruckRouteParent, fuelDeliveryPoint, pipeConnectionPoint);
//    }
//    public void OnDeliverFuel()
//    {
//        gasData.reserveGas += gasData.inDeliveryFuel;
//        gasData.inDeliveryFuel = 0f;
//        UpdateReservesText();
//        if (!GameController.instance.gameData.stationOpenStatus)
//        {
//            UIController.instance.OnPressOpenGasStation();
//            GameManager.instance.CallFireBase("fuelOpenAuto");
//        }
//    }
//    #endregion
//    #region Gas Station Area Triggers/ Bottom Instruction Update
//    public void OnPlayerEnterGasStation(int _fillingPointId)
//    {
//        interactingFillingPoint = _fillingPointId;
//        isInFuelStationArea = true;
//        GameManager.instance.CallFireBase("PlayerEnterStation");
//        UpdateGameProgressText();
//    }
//    public void OnPlayerExitGasStation(int _fillingPointId)
//    {
//        isInFuelStationArea = false;

//        if (interactingFillingPoint != _fillingPointId)
//        {
//            return;
//        }
//        interactingFillingPoint = -1;
//        UIController.instance.UpdateGameProgressText(false);
//    }

//    public void UpdateGameProgressText()
//    {
//        if (!isInFuelStationArea)
//            return;
//        if (interactingFillingPoint < 0)
//        {
//            return;
//        }
//        if (!fillingPoints[interactingFillingPoint].isActive)
//        {
//            UIController.instance.UpdateGameProgressText(true, "Unlock This Fuel Machine!");

//            return;
//        }
//        if (fillingPoints[interactingFillingPoint].fuelStopPoints[0].vehicle != null)
//        {
//            //In Case Of Fueling Vehicle Check For Removal Instructions
//            if (fillingPoints[interactingFillingPoint].fuelStopPoints[0].vehicle.GetComponent<GasStationVehicle>().GetFuelingStatus())
//            {
//                if (fillingPoints[interactingFillingPoint].fuelStopPoints[0].vehicle.GetComponent<GasStationVehicle>().IsRequiredFuelDelivered())
//                {
//                    if (fillingPoints[interactingFillingPoint].fuelStopPoints[0].vehicle.GetComponent<GasStationVehicle>().IsFuelNozelInjected())
//                    {
//                        UIController.instance.UpdateGameProgressText(true, "Tap On Fuel Machine To Stop Fueling");
//                        return;
//                    }
//                    UIController.instance.UpdateGameProgressText(true, "Tap On Fuel Machine To Place Fuel Nozel Back");
//                    return;
//                }
//                UIController.instance.UpdateGameProgressText(true, "Check Fuel Machine For Required Fuel Delivery");
//                return;
//            }
//            if (gasData.reserveGas > 0)
//            {
//                if (fillingPoints[interactingFillingPoint].fuelNozel.IsNozelSelected())
//                {
//                    if (fillingPoints[interactingFillingPoint].fuelStopPoints[0].vehicle.GetComponent<GasStationVehicle>().IsVehicleAtStation())
//                    {
//                        UIController.instance.UpdateGameProgressText(true, "Tap On Vehicle Fuel Point To Attach Nozel To Vehicle");
//                        return;
//                    }
//                    UIController.instance.UpdateGameProgressText(true, "Tap On Fuel Machine To Place Fuel Nozel Back");
//                    return;
//                }

//                //In Case Of Not Fueling Vehicle Instructions for start fueling
//                if (fillingPoints[interactingFillingPoint].fuelStopPoints[0].vehicle.GetComponent<GasStationVehicle>().IsRequiredFuelDelivered())
//                {
//                    if (fillingPoints[interactingFillingPoint].fuelStopPoints[0].vehicle.GetComponent<GasStationVehicle>().IsFuelNozelInjected())
//                    {
//                        UIController.instance.UpdateGameProgressText(true, "Tap On Fuel Nozel To Eject Nozel From Vehicle");
//                        return;
//                    }
//                    UIController.instance.UpdateGameProgressText(true, "Tap On Fuel Machine To Place Fuel Nozel Back");
//                    return;
//                }

//                if (fillingPoints[interactingFillingPoint].fuelStopPoints[0].vehicle.GetComponent<GasStationVehicle>().IsFuelNozelInjected())
//                {
//                    UIController.instance.UpdateGameProgressText(true, "Tap On Fuel Machine To Start Fueling");
//                    return;
//                }
//                UIController.instance.UpdateGameProgressText(true, "Tap On Fuel Machine To Pick Nozel");
//                return;
//            }
//            UIController.instance.UpdateGameProgressText(true, "No Fuel Left! Order Fuel From PC");
//            return;
//        }
//        if (canSpawnVehicles)
//        {
//            UIController.instance.UpdateGameProgressText(true, "Wait For Vehicle To Serve");
//            return;
//        }
//        UIController.instance.UpdateGameProgressText(true, "Open Gas Station From Your Cart Or PC");
//    }

//    public bool IsInGasStationArea()
//    {
//        return isInFuelStationArea;
//    }
//    #endregion
//    #region Employee Implementation
//    public void UpdateFillingPointEmployeeVehicle(int _fillingPointIndex,GameObject _vehicle)
//    {
//        if (_fillingPointIndex < 0 || _fillingPointIndex >= fillingPoints.Length)
//        {
//            print("Invalid Index For Customer Assignment");
//            return;
//        }
//        if (fillingPoints[_fillingPointIndex].fuelStationEmployee != null)
//        {
//            fillingPoints[_fillingPointIndex].fuelStationEmployee.GetComponent<Employee>().UpdateServingCustomer(_vehicle);
//        }
//    }
//    public void AddEmployeeToFillingPoint(int _fillingPointIndex,GameObject _employee)
//    {
//        if(_fillingPointIndex<0 || _fillingPointIndex >= fillingPoints.Length)
//        {
//            print("Invalid Index For Filling Point");
//            return;
//        }
//        if (fillingPoints[_fillingPointIndex].fuelStationEmployee != null)
//        {
//            print("Already Employee At Filling Point!");
//            return;
//        }
//        fillingPoints[_fillingPointIndex].fuelStationEmployee = _employee;
//    }
//    public void RemoveEmployeeFromFillingPoint(int _fillingPointIndex)
//    {
//        if (_fillingPointIndex < 0 || _fillingPointIndex >= fillingPoints.Length)
//        {
//            print("Invalid Index For Filling Point");
//            return;
//        }
//        if (fillingPoints[_fillingPointIndex].fuelStationEmployee == null)
//        {
//            print("No Employee At Filling Point!");
//            return;
//        }
//        fillingPoints[_fillingPointIndex].fuelStationEmployee = null;
//    }
//    public Transform GetFillingMachinePoint(int _fillingPointIndex)
//    {
//        if (_fillingPointIndex < 0 || _fillingPointIndex >= fillingPoints.Length)
//        {
//            print("Invalid Index For Filling Point");
//            return null;
//        }
//        return fillingPoints[_fillingPointIndex].fuelMachinePoint.transform;
//    }
//    public Transform GetFuelFillingPoint(int _fillingPointIndex)
//    {
//        if (_fillingPointIndex < 0 || _fillingPointIndex >= fillingPoints.Length)
//        {
//            print("Invalid Index For Filling Point");
//            return null;
//        }
//        return fillingPoints[_fillingPointIndex].fuelFillingPoint.transform;
//    }
//    public FuelMachine GetFuelFillingMachine(int _fillingPointIndex)
//    {
//        if (_fillingPointIndex < 0 || _fillingPointIndex >= fillingPoints.Length)
//        {
//            print("Invalid Index For Filling Point");
//            return null;
//        }
//        return fillingPoints[_fillingPointIndex].fuelMachine;
//    }
//    public FuelNozel GetFuelFillingNozel(int _fillingPointIndex)
//    {
//        if (_fillingPointIndex < 0 || _fillingPointIndex >= fillingPoints.Length)
//        {
//            print("Invalid Index For Filling Point");
//            return null;
//        }
//        return fillingPoints[_fillingPointIndex].fuelNozel;
//    }

//    public GameObject GetFillingFuelVehicle(int _fillingPointIndex)
//    {
//        if (_fillingPointIndex < 0 || _fillingPointIndex >= fillingPoints.Length)
//        {
//            print("Invalid Index For Filling Point");
//            return null;
//        }
//        if (fillingPoints[_fillingPointIndex].fuelStopPoints[0].vehicle == null)
//        {
//            return null;
//        }
//        if (fillingPoints[_fillingPointIndex].fuelStopPoints[0].vehicle.GetComponent<GasStationVehicle>().IsVehicleAtStation())
//        {
//            return fillingPoints[_fillingPointIndex].fuelStopPoints[0].vehicle.gameObject;
//        }
//        return null;
//    }
//    #endregion
//    #region Saving/Loading Mechanism
//    public void InitializeGasStationData()
//    {
//        gasData = (GasStationData)SerializationManager.LoadFile("_GasData");
//        if (gasData == null)
//        {
//            gasData = new GasStationData();
//        }
//        if (gasData.activeFillingPointsIndex == null || gasData.activeFillingPointsIndex.Count==0)
//        {
//            gasData.activeFillingPointsIndex = new List<int>();
//        //    gasData.activeFillingPointsIndex.Add(0);
//        }
        
//        updateGasReserve += UIController.instance.UpdateGasReserves;
//        for(int i = 0; i < gasData.activeFillingPointsIndex.Count; i++)
//        {
//            int _temp = gasData.activeFillingPointsIndex[i];
//            if (_temp < 0 || _temp >= fillingPoints.Length)
//            {
//                continue;
//            }
//            fillingPoints[_temp].isActive = true;
//        }
//        for (int i = 0; i < fillingPoints.Length; i++)
//        {
//            int _temp = i;
//            fillingPoints[i].fuelMachine.isUnlocked = fillingPoints[i].isActive;
//            fillingPoints[i].fuelNozel.isUnlocked = fillingPoints[i].isActive;
            
//            fillingPoints[i].fuelMachine.connectedFillingPointIndex = _temp;
//            fillingPoints[i].fuelNozel.fillingPointIndex = _temp;
//            EmployeeManager.Instance.SetDeptWorkPlaceLockState(EmployeeType.FuelAttendants, _temp, fillingPoints[_temp].isActive);
//            if (fillingPoints[i].isActive)
//            {
//                updateGasReserve += fillingPoints[i].fuelMachine.UpdateGasReserve;
//                fillingPoints[i].fuelMachine.ResetValues();
//                fillingPoints[i].fuelMachine.UpdateFuelPriceText();
//            }
//        }
//        UpdateFillingPointsLockState();
//        if (gasData.inDeliveryFuel > 0)
//        {
//            OrderFuel(gasData.inDeliveryFuel);
//        }
//        isDataInitialized = true;
//        GameController.instance.AddSavingAction(SaveStationData);
//    }
//    public void UpdateFillingPointsLockState()
//    {
//        for(int i = 0; i < fillingPoints.Length; i++)
//        {
//            fillingPoints[i].lockImage.SetActive(!fillingPoints[i].isActive);
//            bool _isLocked = fillingPoints[i].reqLevel > PlayerDataManager.instance.playerData.playerLevel;
//            for(int j = 0; j < fillingPoints[i].lockBtnImgs.Length; j++)
//            {
//                fillingPoints[i].lockBtnImgs[j].GetComponent<Image>().sprite = _isLocked ? UIController.instance.lock3dBtnImg : UIController.instance.unPurchase3dBtnImg;
//                if (j >= fillingPoints[i].lockPriceTexts.Length)
//                {
//                    continue;
//                }
//                fillingPoints[i].lockPriceTexts[j].GetComponent<TextMeshProUGUI>().color = _isLocked ? UIController.instance.lock3dBtnTxtClr : UIController.instance.unPurchasedBtnTxtClr;
//                fillingPoints[i].lockPriceTexts[j].GetComponent<TextMeshProUGUI>().text = _isLocked ? "Level " + fillingPoints[i].reqLevel.ToString() : "$" + fillingPoints[i].fillingStationPrice.ToString();
//            }
            
//        }
//    }
//    void SaveStationData()
//    {
//        if (isDataInitialized)
//        {
//            SerializationManager.Save(gasData, "_GasData");
//        }
//    }
//    //private void OnApplicationPause(bool pause)
//    //{
//    //    if (pause)
//    //        SaveStationData();
//    //}
//    //private void OnApplicationQuit()
//    //{
//    //    SaveStationData();
//    //}
//    //private void OnDestroy()
//    //{
//    //    SaveStationData();
//    //}
//    #endregion
//}

//[System.Serializable]
//public class GasStationData
//{
//    public float maxReserve;
//    public float reserveGas;
//    public float inDeliveryFuel;
//    public float gasPrice;
//    public float gasPurchasingPrice;
//    public List<int> activeFillingPointsIndex;
//    public GasStationData()
//    {
//        maxReserve = 500f;
//        reserveGas = 0f;
//        inDeliveryFuel = 0f;
//        gasPrice = 2f;
//        gasPurchasingPrice = 1f;
//        activeFillingPointsIndex = new List<int>();
//        //activeFillingPointsIndex.Add(0);
//    }
//}
//[System.Serializable]
//public class FuelStopPoint
//{
//    public Transform stopPoint;
//    public Transform vehicle;
//}

//[System.Serializable]
//public class FillingPoint
//{
//    public string displayName;
//    public bool isActive;
//    public bool isUpdatingQueue;
//    public int fillingStationPrice;
//    public int reqLevel;
//    public Sprite displayImg;
//    public FuelMachine fuelMachine;
//    public FuelNozel fuelNozel;
//    public GameObject fuelPipe;
//    public GameObject lockImage;
//    public GameObject[] lockBtnImgs;
//    public GameObject[] lockPriceTexts;
//    public GameObject fuelStationEmployee;
//    public Transform routeParent;
//    public Transform fuelMachinePoint;  //To Be Used By Employee
//    public Transform fuelFillingPoint;  //To Be Used By Employee
//    public FuelStopPoint[] fuelStopPoints;

//}