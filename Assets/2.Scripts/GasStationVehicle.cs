//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using TMPro;
//using UnityEngine.InputSystem.LowLevel;

//public class GasStationVehicle : MonoBehaviour
//{
//    public float vehicleSpeed;
//    public float rotationSpeed;
//    public float maxFuel;
//    public AudioClip customerHappySound;
//    public GameObject waitTimeContainer;
//    public TextMeshProUGUI remWaitTimeText;
//    public GameObject[] fuelPoints;
//    public GameObject activeFuelPoint;
//    public Transform[] wheels;
//    Transform targetPoint;
//    Transform vehicleStopPoint;
//    public List<Transform> vehicleCompleteRoute;
//    int stopPointIndex;
//    int routeTraverseCounter;
//    float targetFuel;
//    float currentDeliveredFuel;
//    float waitTime;
//    bool isNozelInjected;
//    bool isFueling;
//    GasVehicleStatus vehicleStatus;
//    bool isAddedToWaitQueue = false;
//    int fillingPointIndex;
//    int refuleXP = 5;
//    // Start is called before the first frame update
//    void Start()
//    {
        
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (vehicleStatus == GasVehicleStatus.MovingIn)
//        {
//            if (targetPoint == null)
//            {
//                if (routeTraverseCounter < vehicleCompleteRoute.Count)
//                {
//                    targetPoint = vehicleCompleteRoute[routeTraverseCounter];
//                }
//            }
//            MoveVehicle();
//            if (Vector3.Distance(transform.position, targetPoint.position) < 0.25f)
//            {
//                routeTraverseCounter++;
//                if (routeTraverseCounter < vehicleCompleteRoute.Count)
//                {
//                    targetPoint = vehicleCompleteRoute[routeTraverseCounter];
//                }
//                else
//                {
//                    Debug.LogError("Invalide Route For Vehicle Given");
//                }
//            }
//            if (Vector3.Distance(transform.position, vehicleStopPoint.position) < 0.25f)
//            {
//                if (stopPointIndex == 0)
//                {
//                    vehicleStatus = GasVehicleStatus.AtStation;
//                    activeFuelPoint.SetActive(true);
//                    waitTimeContainer.SetActive(true);
//                    GasStationManager.instance.UpdateGameProgressText();
//                    //Start Employee Working From Here
//                    GasStationManager.instance.UpdateFillingPointEmployeeVehicle(fillingPointIndex, gameObject);
//                }
//                else
//                {
//                    vehicleStatus = GasVehicleStatus.InQueue;
//                }
//                if (!isAddedToWaitQueue)
//                {
//                    isAddedToWaitQueue = true;
//                    GasStationManager.instance.UpdateVehicleInWait(1);
//                    UIController.instance.DisplayInstructions("Vehicle Arrived At Gas Station");
//                }
//                //Stop Tyres Here
                
//            }
//        }else if (vehicleStatus==GasVehicleStatus.AtStation)
//        {
//            if (isNozelInjected)
//            {
//                waitTimeContainer.SetActive(false);
//                UIController.instance.UpdateGasStationWaitingVehicleTime("");

//                return;
//            }
//            waitTimeContainer.SetActive(true);

//            waitTime += Time.deltaTime;
//            float _remTime = GasStationManager.instance.vehicleWaitTime - waitTime;
//            int _min = ((int)_remTime) / 60;
//            int _sec = ((int)_remTime) % 60;
//            remWaitTimeText.text = _min.ToString() + " : " + _sec.ToString("00");
//            UIController.instance.UpdateGasStationWaitingVehicleTime(_min.ToString() + " : " + _sec.ToString("00"));

//            if (_remTime <= 0)
//            {
//                waitTimeContainer.SetActive(false);
//                activeFuelPoint.SetActive(false);
//                UIController.instance.UpdateGasStationWaitingVehicleTime("");
//                vehicleStatus = GasVehicleStatus.MovingOut;

//                GasStationManager.instance.OnRemoveVehicle(fillingPointIndex);
//                GasStationManager.instance.OnFailedFuelDelivery();
//                if (GetComponent<Collider>())
//                {
//                    GetComponent<Collider>().isTrigger = true;
//                }
//            }
//        }
//        else if (vehicleStatus == GasVehicleStatus.MovingOut)
//        {
//            if (targetPoint == null)
//            {
//                if (routeTraverseCounter < vehicleCompleteRoute.Count)
//                {
//                    targetPoint = vehicleCompleteRoute[routeTraverseCounter];
//                }
//            }
//            MoveVehicle();
//            if (Vector3.Distance(transform.position, targetPoint.position) < 0.25f)
//            {
//                routeTraverseCounter++;
//                if (routeTraverseCounter < vehicleCompleteRoute.Count)
//                {
//                    targetPoint = vehicleCompleteRoute[routeTraverseCounter];
//                }
//                else
//                {
//                    Destroy(gameObject);
//                }
//            }
//        }
//    }
//    void MoveVehicle()
//    {
//        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, vehicleSpeed * Time.deltaTime);
//        Vector3 directionToTarget = (targetPoint.position - transform.position).normalized;

//        // Calculate rotation step
//        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
//        float _rotationSpeed = (rotationSpeed + (Vector3.Distance(transform.position, targetPoint.position) * Time.deltaTime * 10f));
//        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
//    }
//    public void OnSpawnVehicle(KeyValuePair<int, int> _stopPointIndex, Transform _routeParent, Transform _fillingStationRoute, Transform _exitRoute,Transform _stopPoint)
//    {
//        if (_routeParent.childCount < 1 || _fillingStationRoute.childCount<1 || _exitRoute.childCount<1)
//        {
//            Debug.LogError("Invalid Route For Vehicle " + gameObject.name);
//            return;
//        }
//        vehicleCompleteRoute = new List<Transform>();
//        for(int i = 0; i < _routeParent.childCount; i++)
//        {
//            vehicleCompleteRoute.Add(_routeParent.GetChild(i));
//        }
//        for (int i = 0; i < _fillingStationRoute.childCount; i++)
//        {
//            vehicleCompleteRoute.Add(_fillingStationRoute.GetChild(i));
//        }
//        for (int i = 0; i < _exitRoute.childCount; i++)
//        {
//            vehicleCompleteRoute.Add(_exitRoute.GetChild(i));
//        }

//        waitTimeContainer.SetActive(false);
//        stopPointIndex = _stopPointIndex.Value;
//        fillingPointIndex = _stopPointIndex.Key;
//        transform.position = _routeParent.GetChild(0).position;
//        transform.rotation = _routeParent.GetChild(0).rotation;
//        vehicleStopPoint = _stopPoint;
//        routeTraverseCounter = 1;
//        if (routeTraverseCounter >= vehicleCompleteRoute.Count)
//        {
//            Debug.LogError("Invalide Route To " + gameObject.name);
//            return;
//        }
//        vehicleStatus = GasVehicleStatus.MovingIn;
//        targetPoint = vehicleCompleteRoute[routeTraverseCounter];
//        targetFuel = Random.Range(8f, 18f);
//        for (int i = 0; i < fuelPoints.Length; i++)
//        {
//            fuelPoints[i].SetActive(false);
//        }
//        int _activeFuelSide = 1 - (fillingPointIndex % 2);
//        if (_activeFuelSide > fuelPoints.Length)
//        {
//            Debug.LogError("Invalid Fuel Point Side");
//            return;
//        }
//        activeFuelPoint = fuelPoints[_activeFuelSide];
//    }
//    public void OnUpdateVehicleStopPoint( int _stopPointIndex,Transform _stopPoint)
//    {
//        stopPointIndex = _stopPointIndex;
//        vehicleStopPoint = _stopPoint;
//        vehicleStatus = GasVehicleStatus.MovingIn;
//    }
//    public void SetFuelNozelInjected(bool _injected, bool _employeeDetached = false)
//    {
//        isNozelInjected = _injected;
//        if (_injected)
//        {
//            GasStationManager.instance.DisplayVehicleOrderFuel(fillingPointIndex,targetFuel);
//            GasStationManager.instance.DisplayVehicleMaxFuel(fillingPointIndex,maxFuel);
//        }
//        else
//        {
//            if (currentDeliveredFuel >= (targetFuel*0.9f))
//            {
//                //Complete Fuel Is Delivered

//                waitTimeContainer.SetActive(false);
//                activeFuelPoint.SetActive(false);
//                vehicleStatus = GasVehicleStatus.MovingOut;
//                if (GetComponent<Collider>())
//                {
//                    GetComponent<Collider>().isTrigger = true;
//                }
//                GasStationManager.instance.OnRemoveVehicle(fillingPointIndex);

//                int _cash = Mathf.CeilToInt(currentDeliveredFuel * GasStationManager.instance.gasData.gasPrice);
//                PlayerDataManager.instance.playerData.playerCash += _cash;
//                // UIController.instance.CashText.text = PlayerDataManager.instance.playerData.playerCash.ToString();
//                UIController.instance.UpdateCurrency(_cash);

//                UIController.instance.DisplayInstructions("$" + _cash.ToString() + " is Added!");
//                SoundController.instance.OnPlayInteractionSound(customerHappySound);

//                GasStationManager.instance.OnSuccessFuelDelivered(fillingPointIndex);

//                //grant xp reward on checkout
//                if (!_employeeDetached)
//                {
//                    PlayerDataManager.instance.UpdateXP(refuleXP);
//                    UIController.instance.UpdateXP(refuleXP);
//                }
//            }
//        }
//    }
//    public float AddFuelToVehicle()
//    {
//        if (currentDeliveredFuel >= maxFuel)
//        {
//            return -1;
//        }
//        currentDeliveredFuel += (Time.deltaTime / 2f);
//        return currentDeliveredFuel;
//    } 
//    public bool IsFuelNozelInjected()
//    {
//        return isNozelInjected;
//    }
//    public void SetFuelingStatus(bool _isFueling)
//    {
//        isFueling = _isFueling;
//        if (!isFueling)
//        {
//            if (currentDeliveredFuel >= (targetFuel * 0.9f))
//            {
//                GasStationManager.instance.UpdateGameProgressText();
//            }
//        }
//    }
//    public bool GetFuelingStatus()
//    {
//        return isFueling;
//    }
//    public bool IsRequiredFuelDelivered()
//    {
//        return (currentDeliveredFuel >= targetFuel);
//    }
//    public bool IsVehicleAtStation()
//    {
//        return (vehicleStatus == GasVehicleStatus.AtStation);
//    }
//}
//[System.Serializable]
//public enum GasVehicleStatus
//{
//    MovingIn,
//    InQueue,
//    AtStation,
//    MovingOut
//}
