//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Receptionist : Employee, InteractableObjects
//{
//    public string employeeName = "Receptionist";
//    // Customer's cart items
//    [HideInInspector]
//    private GameObject currentCustomerAtCounter;

//    public override void OnSpawn(EmployeeSavableData _data, DepartmentsWorkPlace _workPlace, float _servingTime, float managerCommunicationDelay)
//    {
//        base.OnSpawn(_data, _workPlace, _servingTime, managerCommunicationDelay);
       

//        agent.enabled = false;
//        //set position after spawning
//        SetPosition();
//        //add to manager
//        RegisterWithWorkManager();

//        SetWorkerToWalking();
//    }

//    /// <summary>
//    /// Add current created Empoyee to its required manager
//    /// </summary>
//    public override void RegisterWithWorkManager()
//    {
//        CustomerManager.instance.receptionist = gameObject;
//    }

//    //When emplyee time is complete call this method
//    public override void UnRegisterWithWorkManager()
//    {
//        CustomerManager.instance.receptionist = null;
//    }
//    public override void SetPosition()
//    {
//        Transform _pos = null;

//        switch (employeestate)
//        {
//            case Employeestate.MovingToWorkPlace:
//                _pos = deptWorkPlaceRefs.spawnPoint;
//                break;

//            case Employeestate.Waiting:
//            case Employeestate.Serving:
//                _pos = deptWorkPlaceRefs.workPoint;
//                break;

//            case Employeestate.Rest:
//                _pos = deptWorkPlaceRefs.restPoint;
//                break;

//            case Employeestate.Leaving:
//                _pos = deptWorkPlaceRefs.leavingPoint;
//                break;
//        }

//        // Set position if a valid point was found
//        if (_pos != null)
//        {
//            transform.position = _pos.position;
//            transform.rotation = _pos.rotation;
//        }
//    }

//    public override void Update()
//    {
//        base.Update();

//        // Manage receptionist behavior based on the current state
//        switch (employeestate)
//        {
//            case Employeestate.MovingToWorkPlace:

//                MoveToWorkPlace();
//                break;

//            case Employeestate.WaitingForWorkPlaceToUnlock:

//                WorkPlaceIsLocked();
//                break;

//            case Employeestate.Waiting:

//                WaitingForCustomer();
//                break;

//            case Employeestate.Serving:

//                ServingCustomers();
//                break;

//            case Employeestate.MovingToRestingPlace:

//                MoveToRestArea();
//                break;

//            case Employeestate.Leaving:

//                break;
//        }
//    }

//    /// <summary>
//    /// Moves the receptionist to the working point (using NavMeshAgent).
//    /// </summary>
//    private void MoveToWorkPlace()
//    {
//        EnableAgent();
//        agent.SetDestination(workPoint.position);

//        // Check if the agent has reached the destination
//        if (Vector3.Distance(transform.position, workPoint.position) <= 0.2f)
//        {
//            DisableAgent();
//            ChangeEmployeeState(Employeestate.Waiting);
//            transform.position = workPoint.position;
//            transform.rotation = workPoint.rotation;
//            StartCoroutine(RotateToWorkPoint());
//        }
//    }

//    /// <summary>
//    /// Check on duty end employee reached its resting point then 
//    /// Destroy it
//    /// </summary>

//    /// <summary>
//    /// Rotate Object towards the working point
//    /// </summary>
//    /// <returns></returns>
//    IEnumerator RotateToWorkPoint()
//    {
//        yield return new WaitForSeconds(0.2f);

//        transform.rotation = workPoint.rotation;

//    }

//    /// <summary>
//    /// Moves the receptionist to the rest area.
//    /// </summary>
//    private void MoveToRestArea()
//    {
//        EnableAgent();
//        agent.SetDestination(restPoint.position);

//        // Check if the agent has reached the destination
//        if (Vector3.Distance(transform.position, restPoint.position) <= 0.2f)
//        {
//            SetWorkerToIdle();
//            DisableAgent();
//            ChangeEmployeeState(Employeestate.Rest);
//            transform.rotation = Quaternion.Euler(restPoint.transform.rotation.x, restPoint.transform.rotation.y, restPoint.transform.rotation.z);
//            // You can handle the next state after reaching the rest area.
//        }
//    }

//    /// <summary>
//    /// Handles the receptionist's waiting state
//    /// </summary>
//    private void WaitingForCustomer()
//    {
//        managerCommunicationDelayTimer -= Time.deltaTime;

//        if (managerCommunicationDelayTimer <= 0)
//        {
//            currentCustomerAtCounter = CustomerManager.instance.GetCustomerAtCounter();

//            if (currentCustomerAtCounter != null)
//            {
//                ChangeEmployeeState(Employeestate.Serving);
//                SetWorkerToWorking();
//            }

//            managerCommunicationDelayTimer = managerCommunicationDelay;
//        }
//    }

//    /// <summary>
//    /// Handles the serving of customers by scanning items and processing transactions.
//    /// </summary>
//    private void ServingCustomers()
//    {
//        servingTimer -= Time.deltaTime;

//        SetWorkerToWorking();

//        if (servingTimer <= 0)
//        {
           
//            if (currentCustomerAtCounter != null)
//            {
//                Room assignedRoom;

//                assignedRoom = RoomManager.instance.GetEmptyRoom();
//                bool _cusServed = true;
//                if (assignedRoom == null)
//                {
//                    print("No Room Avaiable for assignment");
//                    currentCustomerAtCounter.GetComponent<CustomerMovement>().OnRejectRoomRequestReceptionist();
//                }
//                else
//                {
//                    print("Room Avaiable for assignment");
//                    if (assignedRoom.roomProperties.roomNumber != RoomManager.instance.currentRoomNumber)
//                    {
//                        currentCustomerAtCounter.GetComponent<CustomerMovement>().AssignRoomReceptionist();
//                    }
//                    else
//                    {
//                        _cusServed = false;
//                    }
//                }
//                if (_cusServed)
//                {
//                    currentCustomerAtCounter = null;
//                    ChangeEmployeeState(Employeestate.Waiting);
//                }
//                servingTimer = servingTime;
//            }
//        }
//    }

//    /// <summary>
//    /// Whenever player wants to take control set employee to rest state
//    /// </summary>
//    public void UponEmployeeRest()
//    {
//        // CashCounterManager.instance.setCounterTakenState(false);
//        ChangeEmployeeState(Employeestate.MovingToRestingPlace);
//    }
//    /// <summary>
//    /// Whenever employee timer is up set its state to leaving
//    /// </summary>
//    public void UponEmployeeLeave()
//    {
//        // CashCounterManager.instance.setCounterTakenState(false);

//        ChangeEmployeeState(Employeestate.Leaving);
//    }

//    /// <summary>
//    /// Resets the receptionist state by clearing cart items and resetting the state to Waiting.
//    /// </summary>
  
//    public void OnHoverItems()
//    {
//        UIController.instance.DisplayHoverObjectName(employeeName);
//    }
//    public void OnInteract()
//    {

//    }
//    public void TurnOffOutline()
//    {

//    }
//}
