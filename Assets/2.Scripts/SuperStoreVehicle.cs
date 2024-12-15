using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SuperStoreVehicle : MonoBehaviour
{
    public float moveSpeed;
    public float rotationSpeed;
    public Transform customerSpawnPoint;
    List <Transform> vehicleRoute;
    List<SuperStoreItems> customerWishList;
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

    public void OnSpawnNewVehicle(int _customerIndex,int _parkingIndex,int _vehicleIndex, Transform[] _route,List<SuperStoreItems> _wishList)
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
        for(int i=0;i< SuperStoreManager.instance.parkingPoints[_parkingIndex].parkingReachPoints.childCount; i++)
        {
            vehicleRoute.Add(SuperStoreManager.instance.parkingPoints[_parkingIndex].parkingReachPoints.GetChild(i));

        }
        vehicleRoute.Add(SuperStoreManager.instance.parkingPoints[_parkingIndex].parkingPoint);
        targetPoint = vehicleRoute[traverseCount];
        customerWishList = _wishList;
    }

    //public void OnVehicleLeavingArea()
    //{
    //    vehicleStatus = VehicleStatus.Reversed;
    //    targetPoint = SuperStoreManager.instance.parkingPoints[parkingIndex].parkingReversePoint;
    //    SuperStoreManager.instance.ReleaseParkingSpot(parkingIndex);
    //}
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
            MoveVehicle();
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
    void MoveVehicle()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);
        Vector3 directionToTarget = (targetPoint.position - transform.position).normalized;

        // Calculate rotation step
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        float _rotationSpeed = (rotationSpeed + (Vector3.Distance(transform.position, targetPoint.position) * Time.deltaTime * 10f));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
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
    IEnumerator SpawnCustomer(float _waitTime=1f)
    {
          yield return new WaitForSeconds(_waitTime);
          int _queueIndex = SuperStoreManager.instance.GetEmptyCounterPoint();
          while (_queueIndex<0)
          {
              yield return new WaitForSeconds(2f);
              _queueIndex = SuperStoreManager.instance.GetEmptyCounterPoint();
          }
          CounterPoint _counterPoint = SuperStoreManager.instance.counterQueue[_queueIndex];

          GameObject _customer = Instantiate(SuperStoreManager.instance.availableCustomers[customerIndex], SuperStoreManager.instance.spawnedCustomerParent);
          //_customer.GetComponent<SuperStoreCustomer>().SpawnNewCustomer(_queueIndex, gameObject, customerSpawnPoint, customerWishList);
          _counterPoint.isOccupied = true;
          _counterPoint.queCustomerRef = _customer;
       

    }

  
}
