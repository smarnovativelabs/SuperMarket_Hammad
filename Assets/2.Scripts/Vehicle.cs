using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Vehicle : MonoBehaviour
{
    public float moveSpeed;
    public float rotationSpeed;
    public Transform customerSpawnPoint;
     List <Transform> vehicleRoute;
    public int customerIndex;
    public int parkingIndex;
    public int vehicleIndex;
    Transform targetPoint;
    int traverseCount;
    enum VehicleStatus
    {
        MovingIn,
        Parked,
        Reversed,
        MovingOut
    }
    VehicleStatus vehicleStatus;

    public void OnSpawnNewVehicle(int _customerIndex,int _parkingIndex,int _vehicleIndex, Transform[] _route)
    {
        transform.position = _route[0].position;
        transform.rotation = _route[0].rotation;

        customerIndex = _customerIndex;
        parkingIndex = _parkingIndex;
        vehicleIndex = _vehicleIndex;

        vehicleStatus = VehicleStatus.MovingIn;
        traverseCount = 1;
        vehicleRoute = new List<Transform>();
        for(int i = 0; i < _route.Length; i++)
        {
            vehicleRoute.Add(_route[i]);
        }
        vehicleRoute.Add(CustomerManager.instance.parkingPoints[_parkingIndex].parkingPoint);
        targetPoint = vehicleRoute[traverseCount];
    }

    public void OnSpawnSavedVehicle(int _customerIndex,int _parkingIndex,int _vehicleIndex,Transform _spawnPoint, Transform[] _route)
    {
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;
        customerIndex = _customerIndex;
        parkingIndex = _parkingIndex;
        vehicleIndex = _vehicleIndex;

        vehicleStatus = VehicleStatus.Parked;
        vehicleRoute = new List<Transform>();
        for (int i = 0; i < _route.Length; i++)
        {
            vehicleRoute.Add(_route[i]);
        }
        vehicleRoute.Add(CustomerManager.instance.parkingPoints[parkingIndex].parkingReversePoint);
    }
    public void OnVehicleLeavingArea()
    {
        vehicleStatus = VehicleStatus.Reversed;
        targetPoint = CustomerManager.instance.parkingPoints[parkingIndex].parkingReversePoint;
        CustomerManager.instance.ReleaseParkingSpot(parkingIndex);

    }
    public Transform GetCustomerSpawnPoint()
    {
        return customerSpawnPoint;
    }
    private void Update()
    {
        if (vehicleStatus == VehicleStatus.MovingIn)
        {
            if (targetPoint == null)
            {
                print("Target Point Not Set");
                return;
            }
            transform.position=Vector3.MoveTowards (transform.position,targetPoint.position, moveSpeed * Time.deltaTime);
            Vector3 directionToTarget = (targetPoint.position - transform.position).normalized;

            // Calculate rotation step
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            float _rotationSpeed = (rotationSpeed + (Vector3.Distance(transform.position, targetPoint.position) * Time.deltaTime * 10f));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPoint.position) < 1f)
            {
                traverseCount++;
                if (traverseCount >= vehicleRoute.Count)
                {
                    transform.position = targetPoint.position;
                    transform.rotation = targetPoint.rotation;
                    OnVehicleReachedToParking();
                }
                else
                {
                    targetPoint = vehicleRoute[traverseCount];
                }
            }
        }
        else if (vehicleStatus == VehicleStatus.Reversed)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPoint.position) < 1f)
            {
                traverseCount = vehicleRoute.Count - 2;
                if (traverseCount < 0)
                {
                    vehicleStatus = VehicleStatus.Parked;
                    print("No Path Found!");
                    return;
                }
                vehicleStatus = VehicleStatus.MovingOut;
                targetPoint = vehicleRoute[traverseCount];
            }
        }
        else if (vehicleStatus == VehicleStatus.MovingOut)
        {
            if (targetPoint == null)
            {
                print("Target Point Not Set");
                return;
            }
            transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);
            Vector3 directionToTarget = (targetPoint.position - transform.position).normalized;

            // Calculate rotation step
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            float _rotationSpeed = (rotationSpeed + (Vector3.Distance(transform.position, targetPoint.position) * Time.deltaTime * 10f));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPoint.position) < 1f)
            {
                traverseCount--;
                if (traverseCount < 0)
                {
                    OnVehicleReachToExitPoint();
                }
                else
                {
                    targetPoint = vehicleRoute[traverseCount];
                }
            }
        }
    }

    void OnVehicleReachedToParking()
    {
        vehicleStatus = VehicleStatus.Parked;
        StartCoroutine(SpawnCustomer());
    }
    void OnVehicleReachToExitPoint()
    {
        vehicleStatus = VehicleStatus.Parked;
        Destroy(gameObject);
    }
   
    public bool isSpawningCustomer = false;

 
    IEnumerator SpawnCustomer(float _waitTime = 1f)
    {
       
        if (isSpawningCustomer)
        {
            print("spawning in progress return break **********");
            yield break;  // Exit if a spawn is in progress
        }

       isSpawningCustomer = true;  // Lock the spawn process

        yield return new WaitForSeconds(_waitTime);

        int _queueIndex = CustomerManager.instance.GetEmptyCounterPoint();

        while (_queueIndex < 0)
        {
            yield return new WaitForSeconds(0.5f);  
            _queueIndex = CustomerManager.instance.GetEmptyCounterPoint();
        }

        CounterPoint _counterPoint = CustomerManager.instance.counterQueue[_queueIndex];

       
        if (_counterPoint.isOccupied)
        {
            Debug.LogWarning("Counter point became occupied_____restart routine");
            isSpawningCustomer = false;  
            StartCoroutine(SpawnCustomer(0.5f)); 
            yield break;
        }

      
        GameObject _customer = Instantiate(CustomerManager.instance.availableCustomers[customerIndex], CustomerManager.instance.spawnedCustomerParent);
        _customer.GetComponent<CustomerMovement>().SpawnNewCustomer(_queueIndex, gameObject, customerSpawnPoint, _counterPoint.queuePoint);
        _counterPoint.isOccupied = true;
        _counterPoint.queCustomerRef = _customer;

       // print("__-customer is added to the list____");
        CustomerManager.instance.customersList.Add(_customer);

        
        isSpawningCustomer = false;
    }


}
