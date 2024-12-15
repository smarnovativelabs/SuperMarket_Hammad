using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CustomerMovement;

public class CustomerManager : MonoBehaviour
{
    public float appproxCustomerSpawnDelay;
    public Transform spawnedVehicleParent;
    public Transform spawnedCustomerParent;
    public GameObject[] availableCustomers;
    public GameObject[] availableCars;
    public VehicleEnteringRoute[] vehicleRoutePoints;
    public VehicleEnteringRoute[] exitRoutePoints;
    public VehicleParkingPoint[] parkingPoints;
    public CounterPoint[] counterQueue;
    public static CustomerManager instance;
    bool gameStarted = false;
    bool startSpawning = false;
    float spawningTimer;
    int totalAllotedCustomers = 0;
    int totalDeclinedCustomers = 0;
    int spawnedCustomerIndex = 0;
    int spawnedVehicleIndex = 0;
    int customersInWaiting = 0;
    public List<GameObject> customersList = new List<GameObject>();
    public GameObject receptionist;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public void Start()
    {
        GameController.instance.changeGameStatus += UpdateGameStatus;
    }
    public void UpdateGameStatus(bool _enable)
    {
        gameStarted = _enable;
    }

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
        }
    }
    void SpawnNewCustomer()
    {
        int _parkingSpotIndex = GetEmptyParkingSpot();
        if (_parkingSpotIndex < 0)
        {
            spawningTimer += 5f;
            return;
        }
        if (!IsCounterQueueHaveSpace())
        {
            spawningTimer += 5f;
            return;
        }
        parkingPoints[_parkingSpotIndex].isOccupied = true;
        int _spawnIndex = Random.Range(0, vehicleRoutePoints.Length);

        GameObject _vehicle = Instantiate(availableCars[spawnedVehicleIndex], spawnedVehicleParent);
        _vehicle.GetComponent<Vehicle>().OnSpawnNewVehicle(spawnedCustomerIndex, _parkingSpotIndex, spawnedVehicleIndex, vehicleRoutePoints[_spawnIndex].routePoints);
        spawnedCustomerIndex++;
        if (spawnedCustomerIndex >= availableCustomers.Length)
        {
            spawnedCustomerIndex = 0;
        }
        spawnedVehicleIndex++;
        if (spawnedVehicleIndex >= availableCars.Length)
        {
            spawnedVehicleIndex = 0;
        }
        StartSpawningCustomers();

    }
    int GetEmptyParkingSpot()
    {
        int _emptySpotIndex = -1;
        for (int i = 0; i < parkingPoints.Length; i++)
        {
            if (!parkingPoints[i].isOccupied)
            {
                _emptySpotIndex = i;
                break;
            }
        }
        return _emptySpotIndex;
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
    public void OpenMotel(bool _open)
    {
        startSpawning = _open;
        if(startSpawning)
            StartSpawningCustomers(0);
    }

    public void StartSpawningCustomers(float _time = -1f)
    {
        if (_time < 0)
        {
            float _mintime = appproxCustomerSpawnDelay - 2;
            _mintime = _mintime < 0 ? 0 : _mintime;
            float _maxTime = appproxCustomerSpawnDelay + 2;
            _maxTime = _maxTime < 0 ? 5: _maxTime;
            spawningTimer = Random.Range(_mintime, _maxTime);
        }
        else
        {
            spawningTimer = _time;
        }
    }
    public GameObject SpawnSavedCustomer(int _customerIndex, int _vehicleIndex, int _parkingIndex,bool _isOnTopFloor, Transform _customerSpawnPoint, Transform[] _customerMovePoints)
    {
        if (_parkingIndex < 0 || _parkingIndex > parkingPoints.Length)
            return null;
        if (_customerIndex < 0 || _customerIndex > availableCustomers.Length)
            return null;
        if (_vehicleIndex < 0 || _vehicleIndex > availableCars.Length)
            return null;
        GameObject _vehicle = Instantiate(availableCars[_vehicleIndex], spawnedVehicleParent);
        parkingPoints[_parkingIndex].isOccupied = true;
        int _spawnIndex = Random.Range(0, vehicleRoutePoints.Length);
        _vehicle.GetComponent<Vehicle>().OnSpawnSavedVehicle(_customerIndex, _parkingIndex,_vehicleIndex,parkingPoints[_parkingIndex].parkingPoint, vehicleRoutePoints[_spawnIndex].routePoints);

        GameObject _customer = Instantiate(availableCustomers[_customerIndex], spawnedCustomerParent);
        //Assign MovePoints to spawned customer and vehicle index and vehicle reference to Custoemr for saving and fetching again
        _customer.GetComponent<CustomerMovement>().SpawnSavedCustomer(_vehicle, _customerSpawnPoint,_isOnTopFloor, _customerMovePoints);
        return _customer;
    }
    public int GetEmptyCounterPoint()
    {
        int _pointIndex = -1;
        for(int i = 0; i < counterQueue.Length; i++)
        {
            if (!counterQueue[i].isOccupied)
            {
                _pointIndex = i;
                break;
            }
        }
        return _pointIndex;
    }

   public void OnRemoveCustomerFromQueue()
    {
      
        counterQueue[0].isOccupied = false;
        counterQueue[0].queCustomerRef = null;

        int _emptyIndex = 0;

        for (int i = 1; i < counterQueue.Length; i++)
        {
            if (counterQueue[i].isOccupied)
            {
              
                counterQueue[_emptyIndex].isOccupied = true;
                counterQueue[_emptyIndex].queCustomerRef = counterQueue[i].queCustomerRef;

            
                var customerMovement = counterQueue[_emptyIndex].queCustomerRef.GetComponent<CustomerMovement>();
                customerMovement.queueIndex = _emptyIndex;
                customerMovement.UpdateEnqueueCustomerTarget(_emptyIndex, counterQueue[_emptyIndex].queuePoint);

                counterQueue[i].isOccupied = false;
                counterQueue[i].queCustomerRef = null;

                _emptyIndex = i;
            }
        }

        UpdateCustomersInWait(-1);
    }

  /*  public void OnRemoveCustomerFromQueue()
    {
        counterQueue[0].isOccupied = false;
        counterQueue[0].queCustomerRef = null;
        int _emptyIndex = 0;
        for(int i = 1; i < counterQueue.Length; i++)
        {
            if (counterQueue[i].isOccupied)
            {
                counterQueue[_emptyIndex].isOccupied = true;
                counterQueue[_emptyIndex].queCustomerRef = counterQueue[i].queCustomerRef;
                counterQueue[_emptyIndex].queCustomerRef.GetComponent<CustomerMovement>().UpdateEnqueueCustomerTarget(_emptyIndex, counterQueue[_emptyIndex].queuePoint);

                counterQueue[i].isOccupied = false;
                counterQueue[i].queCustomerRef = null;
                _emptyIndex = i;
              
            }
        }
        UpdateCustomersInWait(-1);
    }*/
    public void ReleaseParkingSpot(int _parkingIndex)
    {
        parkingPoints[_parkingIndex].isOccupied = false;
    }
    public void OnAllotRoom()
    {
        totalAllotedCustomers++;
       // GameManager.instance.CallFireBase("CusServe_" + totalAllotedCustomers.ToString(), "served", totalAllotedCustomers);
        if (PlayerPrefs.GetInt("CustomerServed", 0) == 0)
        {
            PlayerPrefs.SetInt("CustomerServed", 1);
            //UIController.instance.UpdateGameProgressText(true, "Open Gas Station From Your PC");
            //UIController.instance.DisplayInstructions("You Can Open Gas Station Now");
        }
    }
    public void OnDeclineRoom()
    {
        totalDeclinedCustomers++;
       // GameManager.instance.CallFireBase("CusDcline_" + totalDeclinedCustomers.ToString(), "declined", totalDeclinedCustomers);
    }
    public bool IsCustomerServed()
    {
        return (PlayerPrefs.GetInt("CustomerServed", 0) == 1);
    }
    public void UpdateCustomersInWait(int _val)
    {
        customersInWaiting += _val;
        if (customersInWaiting < 0)
        {
            customersInWaiting = 0;
        }
        //Update UI From Here
        UIController.instance.UpdateMotelWaitingCustomerText(customersInWaiting);
    }

    public GameObject GetCustomerAtCounter()
    {
        GameObject customer = null;

        if (customersList.Count > 0)
        {
            for(int i=0;i< customersList.Count; i++)
            {
                if (customersList[i].GetComponent<CustomerMovement>().customerStatus == CustomerStatus.AtCounter)
                {
                    customer= customersList[i];
                }
            }
        }
        return customer;
       
    }
}
[System.Serializable]
public class VehicleParkingPoint
{
    public bool isOccupied = false;
    public Transform parkingReachPoints; 
    public Transform parkingPoint;
    public Transform parkingReversePoint;
    public Transform enteringRoute;
    public Transform exitRoute;
}
[System.Serializable]
public class CounterPoint
{
    public bool isOccupied;
    public Transform queuePoint;
    public GameObject queCustomerRef;
}
[System.Serializable]
public class VehicleEnteringRoute
{
    public Transform[] routePoints;
}