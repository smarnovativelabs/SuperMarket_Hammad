//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class FuelTruck : MonoBehaviour
//{
//    public float truckSpeed;
//    public float rotationSpeed;
//    public float wheelSpeed;
//    public float pipeConnectionTime = 5f;
//    public LineRenderer truckPipe;
//    public Transform pipeStartPosition;

//    public Transform[] truckWheels;

//    Transform pipeConnectionPoint;
//    Transform truckRouteParent;
//    Transform deliveryPoint;
//    Transform targetPoint;
//    public int routePointsCounter;

//    enum TruckState
//    {
//        Stationary,
//        MovingIn,
//        AtDelivery,
//        MovingOut
//    }
//    TruckState truckState;

//    public void OnSpawnTruck(Transform _routeParent, Transform _deliveryPoint,Transform _pipeConectionPoint)
//    {
//        if (_routeParent.childCount < 1)
//        {
//            Debug.LogError("Truck Path Not Set!");
//            return;
//        }
//        truckRouteParent = _routeParent;
//        deliveryPoint = _deliveryPoint;
//        pipeConnectionPoint = _pipeConectionPoint;
//        transform.position = _routeParent.GetChild(0).position;
//        transform.rotation = _routeParent.GetChild(0).rotation;
//        routePointsCounter = 1;
//        if (routePointsCounter >= _routeParent.childCount)
//        {
//            Debug.LogError("Invalid Path Given To Truck!");
//            return;
//        }
//        targetPoint = truckRouteParent.GetChild(routePointsCounter);
//        truckState = TruckState.MovingIn;
//        //Rotate Tyres Here

//    }

//    private void Update()
//    {
//        if (truckState == TruckState.MovingIn)
//        {
//            if (targetPoint == null)
//            {
//                if (routePointsCounter < truckRouteParent.childCount)
//                {
//                    targetPoint = truckRouteParent.GetChild(routePointsCounter);
//                }
//            }
//            MoveTruck();
//            if (Vector3.Distance(transform.position, targetPoint.position) < 0.25f)
//            {
//                routePointsCounter++;
//                if (routePointsCounter < truckRouteParent.childCount)
//                {
//                    targetPoint = truckRouteParent.GetChild(routePointsCounter);
//                }
//                else
//                {
//                    Debug.LogError("Invalide Route For Truck Given");
//                }
//            }
//            if (Vector3.Distance(transform.position, deliveryPoint.position) < 0.25f)
//            {
//                truckState = TruckState.AtDelivery;
//                truckPipe.gameObject.SetActive(true);
//                //Stop Tyres Here
//                truckPipe.SetPosition(0, pipeStartPosition.position);
//                truckPipe.SetPosition(1, pipeConnectionPoint.position);
//            }
//        }
//        else if (truckState == TruckState.AtDelivery)
//        {
//            pipeConnectionTime -= Time.deltaTime;
//            if (pipeConnectionTime <= 0)
//            {
//                truckState = TruckState.MovingOut;
//                truckPipe.SetPosition(0, pipeStartPosition.position);
//                truckPipe.SetPosition(1, pipeStartPosition.position);
//                truckPipe.gameObject.SetActive(false);
//                GasStationManager.instance.OnDeliverFuel();
//            }
//        }
//        else if (truckState == TruckState.MovingOut)
//        {
//            if (targetPoint == null)
//            {
//                if (routePointsCounter < truckRouteParent.childCount)
//                {
//                    targetPoint = truckRouteParent.GetChild(routePointsCounter);
//                }
//            }
//            MoveTruck();
//            if (Vector3.Distance(transform.position, targetPoint.position) < 0.25f)
//            {
//                routePointsCounter++;
//                if (routePointsCounter < truckRouteParent.childCount)
//                {
//                    targetPoint = truckRouteParent.GetChild(routePointsCounter);
//                }
//                else
//                {
//                    Destroy(gameObject);
//                }
//            }
//        }
//    }
//    void MoveTruck()
//    {
//        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, truckSpeed * Time.deltaTime);
//        Vector3 directionToTarget = (targetPoint.position - transform.position).normalized;

//        // Calculate rotation step
//        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
//        float _rotationSpeed = (rotationSpeed + (Vector3.Distance(transform.position, targetPoint.position) * Time.deltaTime * 10f));
//        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
//    }
//}
