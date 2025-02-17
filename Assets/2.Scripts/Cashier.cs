//using System;
//using System.Collections;
//using System.Collections.Generic;
//using TMPro;
//using UnityEngine;
//using UnityEngine.AI;

//public class Cashier : Employee,InteractableObjects
//{
//    public string employeeName = "Cashier";
//    // Customer's cart items
//    [HideInInspector]
//    private List<GameObject> customerCartItems;

//    public override void OnSpawn(EmployeeSavableData _data, DepartmentsWorkPlace _workPlace, float _servingTime,float managerCommunicationDelay)
//    {
//        base.OnSpawn(_data, _workPlace, _servingTime, managerCommunicationDelay);
//        // Initialize the list of customer cart items
//        customerCartItems = new List<GameObject>();

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
//        // savableData.wo
//      //  print(savableData.workPointIndex);
//        SuperStoreManager.instance.AddCashierToCounter(savableData.workPointIndex,gameObject);
//        // CashCounterManager.instance.cashier=gameObject;
//        SuperStoreManager.instance.DisableCashierRV(savableData.workPointIndex);
//       // CashCounterManager.instance.DisableCashierRV();
//    }

//    //When emplyee time is complete call this method
//    public override void UnRegisterWithWorkManager()
//    {
//        SuperStoreManager.instance.RemoveCashierFromCounter(savableData.workPointIndex);
//       // CashCounterManager.instance.cashier = null;
//        //for next RV cashier / after time is finished first 2 customers required then 1 customer requird condition by boss
//       // CashCounterManager.instance.totalCustomersServed = 0;
//        SuperStoreManager.instance.SetTotalCustomerServed(savableData.workPointIndex);
//        SuperStoreManager.instance.SettotalCustomerNeedtoServeForCashierRV(savableData.workPointIndex);
//     //   CashCounterManager.instance.totalCustomerNeedtoServeForCashierRV = 1;


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

//        // Manage cashier behavior based on the current state
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
//    /// Moves the cashier to the working point (using NavMeshAgent).
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

//        /// <summary>
//        /// Rotate Object towards the working point
//        /// </summary>
//        /// <returns></returns>
//    IEnumerator RotateToWorkPoint()
//    {
//        yield return new WaitForSeconds(0.2f);
//        transform.rotation = workPoint.rotation;
//    }

//    /// <summary>
//    /// Moves the cashier to the rest area.
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
//    /// Handles the cashier's waiting state, requesting customer cart items when the timer reaches 0.
//    /// </summary>
//    private void WaitingForCustomer()
//    {
//        managerCommunicationDelayTimer -= Time.deltaTime;

//        if (managerCommunicationDelayTimer <= 0)
//        {
//            customerCartItems = SuperStoreManager.instance.RequestForCartItems(savableData.workPointIndex);

//            if (customerCartItems!=null)
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
//            if (customerCartItems == null)
//            {
//                ChangeEmployeeState(Employeestate.Waiting);
//            }
//            if (customerCartItems.Count > 0)
//            {
//                GameObject purchase = customerCartItems[0];

//                Superstoreitems superstoreItem = purchase.GetComponent<Superstoreitems>();

//                if (superstoreItem != null)
//                {
//                    superstoreItem.OnCashierInteract(savableData.workPointIndex);  // Simulate scanning
//                    customerCartItems.RemoveAt(0);  // Remove scanned item
//                }

//                servingTimer = servingTime;
//            }
//            else
//            {
//                servingTimer = servingTime;
//                SuperStoreManager.instance.UpdateAnimationForCustomerForCashier(savableData.workPointIndex);
//                SuperStoreManager.instance.OnCashierCheckOut(savableData.workPointIndex);
//               // CashCounterManager.instance.UpdateAnimationForCustomerForCashier();
//                // CashCounterManager.instance.OnCashierCheckOut();
//                ChangeEmployeeState(Employeestate.Waiting);
//            }
//        }
//    }

//    /// <summary>
//    /// Whenever player wants to take control set employee to rest state
//    /// </summary>
//    public void UponEmployeeRest()
//    {
//       // CashCounterManager.instance.setCounterTakenState(false);
//        ChangeEmployeeState(Employeestate.MovingToRestingPlace);
//    }
//    /// <summary>
//    /// Whenever employee timer is up set its state to leaving
//    /// </summary>
//    public void UponEmployeeLeave()
//    {
//       // CashCounterManager.instance.setCounterTakenState(false);

//        ChangeEmployeeState(Employeestate.Leaving);
//    }

//    /// <summary>
//    /// Resets the cashier state by clearing cart items and resetting the state to Waiting.
//    /// </summary>
//    public void ResetCashierState()
//    {
//        customerCartItems.Clear();
//        ChangeEmployeeState(Employeestate.Waiting);
//    }

//    public  void OnHoverItems()
//    {
//        UIController.instance.DisplayHoverObjectName(employeeName,true, HoverInstructionType.General);
//    }
//    public  void OnInteract()
//    {

//    }
//    public  void TurnOffOutline()
//    {

//    }

//}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Cashier : Employee, InteractableObjects
{
    public string employeeName = "Cashier";
    // Customer's cart items
    [HideInInspector]
    private List<GameObject> customerCartItems;

    public override void OnSpawn(EmployeeSavableData data, DepartmentsWorkPlace workPlace, float _servingTime, float managerCommunicationDelay)
    {
        print("1B");
        base.OnSpawn(data, workPlace, _servingTime, managerCommunicationDelay);
        // Initialize the list of customer cart items
        customerCartItems = new List<GameObject>();

        agent.enabled = false;
        //set position after spawning
        SetPosition();
        //add to manager
        RegisterWithWorkManager();

        SetWorkerToWalking();
    }

    /// <summary>
    /// Add current created Empoyee to its required manager
    /// </summary>
    public override void RegisterWithWorkManager()
    {
        // savableData.wo
        //  print(savableData.workPointIndex);
        SuperStoreManager.instance.AddCashierToCounter(savableData.workPointIndex, gameObject);
        // CashCounterManager.instance.cashier=gameObject;
        SuperStoreManager.instance.DisableCashierRV(savableData.workPointIndex);
        // CashCounterManager.instance.DisableCashierRV();
    }

    //When emplyee time is complete call this method
    public override void UnRegisterWithWorkManager()
    {
        //print("RVd Cashier ");
        //print("RVd Cashier "+savableData.workPointIndex);
        SuperStoreManager.instance.RemoveCashierFromCounter(savableData.workPointIndex);
        // CashCounterManager.instance.cashier = null;
        //for next RV cashier / after time is finished first 2 customers required then 1 customer requird condition by boss
        // CashCounterManager.instance.totalCustomersServed = 0;
        SuperStoreManager.instance.SetTotalCustomerServed(savableData.workPointIndex);
        SuperStoreManager.instance.SettotalCustomerNeedtoServeForCashierRV(savableData.workPointIndex);
        //   CashCounterManager.instance.totalCustomerNeedtoServeForCashierRV = 1;


    }
    public override void SetPosition()
    {
       // print("zzzzz");
        Transform _pos = null;

        switch (employeestate)
        {
            case Employeestate.MovingToWorkPlace:
                _pos = deptWorkPlaceRefs.spawnPoint;
               // print("aa");
                break;

            case Employeestate.Waiting:
            case Employeestate.Serving:
                _pos = deptWorkPlaceRefs.workPoint;
               // print("bb");
                break;

            case Employeestate.Rest:
                _pos = deptWorkPlaceRefs.restPoint;
               // print("cc");
                break;

            case Employeestate.Leaving:
                _pos = deptWorkPlaceRefs.leavingPoint;
               // print("dd");
                break;
        }

        // Set position if a valid point was found
        if (_pos != null)
        {
           // print("Position" + _pos.position.x);
          //  print("Position" + _pos.position.y);
          //  print("Position" + _pos.position.z);
            transform.position = _pos.position;
            transform.rotation = _pos.rotation;
        }
    }

    public override void Update()
    {
       // print("alpha");
        base.Update();
       // print("gamma");
     //   print("employee state : " + employeestate);
        
        // Manage cashier behavior based on the current state
        switch (employeestate)
        {
            case Employeestate.MovingToWorkPlace:

                MoveToWorkPlace();
                break;

            case Employeestate.WaitingForWorkPlaceToUnlock:

                WorkPlaceIsLocked();
                break;

            case Employeestate.Waiting:

                WaitingForCustomer();
                break;

            case Employeestate.Serving:
               // print("Serving is called");
                ServingCustomers();
                break;

            case Employeestate.MovingToRestingPlace:

                MoveToRestArea();
                break;

            case Employeestate.Leaving:

                break;
        }
    }

    /// <summary>
    /// Moves the cashier to the working point (using NavMeshAgent).
    /// </summary>
    private void MoveToWorkPlace()
    {
        EnableAgent();
        agent.SetDestination(workPoint.position);
        // Check if the agent has reached the destination
        if (Vector3.Distance(transform.position, workPoint.position) <= 0.2f)
        {
            DisableAgent();
            ChangeEmployeeState(Employeestate.Waiting);
            transform.position = workPoint.position;
            transform.rotation = workPoint.rotation;
            StartCoroutine(RotateToWorkPoint());
        }
    }

    /// <summary>
    /// Check on duty end employee reached its resting point then 
    /// Destroy it
    /// </summary>

    /// <summary>
    /// Rotate Object towards the working point
    /// </summary>
    /// <returns></returns>
    IEnumerator RotateToWorkPoint()
    {
        yield return new WaitForSeconds(0.2f);
        transform.rotation = workPoint.rotation;
    }

    /// <summary>
    /// Moves the cashier to the rest area.
    /// </summary>
    private void MoveToRestArea()
    {
        EnableAgent();
        agent.SetDestination(restPoint.position);

        // Check if the agent has reached the destination
        if (Vector3.Distance(transform.position, restPoint.position) <= 0.2f)
        {
            SetWorkerToIdle();
            DisableAgent();
            ChangeEmployeeState(Employeestate.Rest);
            transform.rotation = Quaternion.Euler(restPoint.transform.rotation.x, restPoint.transform.rotation.y, restPoint.transform.rotation.z);
            // You can handle the next state after reaching the rest area.
        }
    }

    /// <summary>
    /// Handles the cashier's waiting state, requesting customer cart items when the timer reaches 0.
    /// </summary>
    private void WaitingForCustomer()
    {
        managerCommunicationDelayTimer -= Time.deltaTime;
       // print("managerCommunicationDelayTimer :" + managerCommunicationDelayTimer);

        if (managerCommunicationDelayTimer <= 0)
        {
            customerCartItems = SuperStoreManager.instance.RequestForCartItems(savableData.workPointIndex);
           // print("SuperStore Customer cart Item  ::::: " + customerCartItems);

            if (customerCartItems != null)
            {

                ChangeEmployeeState(Employeestate.Serving);
                SetWorkerToWorking();
            }

            managerCommunicationDelayTimer = managerCommunicationDelay;
        }
    }

    /// <summary>
    /// Handles the serving of customers by scanning items and processing transactions.
    /// </summary>
    private void ServingCustomers()
    {
      //  print("Serving Function is successfully called");
      //  print("Serving Timer " + servingTimer);
        servingTimer -= Time.deltaTime;
       // print("Serving Timer " + servingTimer);
        SetWorkerToWorking();
        if (servingTimer <= 0)
        {
            if (customerCartItems == null)
            {
                ChangeEmployeeState(Employeestate.Waiting);
            }
            if (customerCartItems.Count > 0)
            {
                GameObject purchase = customerCartItems[0];

                Superstoreitems superstoreItem = purchase.GetComponent<Superstoreitems>();

                if (superstoreItem != null)
                {
                    print("Serving cashier called ");
                    print("savedata" + savableData.workPointIndex);
                    superstoreItem.OnCashierInteract(savableData.workPointIndex);  // Simulate scanning
                    customerCartItems.RemoveAt(0);  // Remove scanned item
                }

                servingTimer = servingTime;
            }
            else
            {
                servingTimer = servingTime;
                SuperStoreManager.instance.UpdateAnimationForCustomerForCashier(savableData.workPointIndex);
                SuperStoreManager.instance.OnCashierCheckOut(savableData.workPointIndex);
                // CashCounterManager.instance.UpdateAnimationForCustomerForCashier();
                // CashCounterManager.instance.OnCashierCheckOut();
                ChangeEmployeeState(Employeestate.Waiting);
            }
        }
    }

    /// <summary>
    /// Whenever player wants to take control set employee to rest state
    /// </summary>
    public void UponEmployeeRest()
    {
        // CashCounterManager.instance.setCounterTakenState(false);
        ChangeEmployeeState(Employeestate.MovingToRestingPlace);
    }
    /// <summary>
    /// Whenever employee timer is up set its state to leaving
    /// </summary>
    public void UponEmployeeLeave()
    {
        // CashCounterManager.instance.setCounterTakenState(false);

        ChangeEmployeeState(Employeestate.Leaving);
    }

    /// <summary>
    /// Resets the cashier state by clearing cart items and resetting the state to Waiting.
    /// </summary>
    public void ResetCashierState()
    {
        customerCartItems.Clear();
        ChangeEmployeeState(Employeestate.Waiting);
    }

    public void OnHoverItems()
    {
        UIController.instance.DisplayHoverObjectName(employeeName, true, HoverInstructionType.General);
    }
    public void OnInteract()
    {

    }
    public void TurnOffOutline()
    {

    }

}



