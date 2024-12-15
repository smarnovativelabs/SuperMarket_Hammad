//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class FuelAttendant : Employee,InteractableObjects
//{
//    public string employeeName;
//    public Transform fuelNozelPoint;

//    public Transform fuelMachinePoint;
//    public Transform vehicleFuelPoint;

//    public GameObject servingVehicle;
//    public FuelNozel fillingNozel;
//    public FuelMachine fuelMachine;

//    Transform target;
//    bool isFuelingVehicle = false;
//    enum ServingTaskStatus
//    {
//        Waiting,
//        MovingToServe,
//        Serving
//    }
//    enum GasAttendantTask
//    {
//        Wait,
//        GetNozel,
//        InjectNozel,
//        OnFuelMachine,
//        OffFuelMachine,
//        RemoveNozel,
//        PlaceNozelBack,
//    }
//    GasAttendantTask attendantTask;
//    ServingTaskStatus serveState;

//    public override void OnSpawn(EmployeeSavableData _data, DepartmentsWorkPlace _workPlace, float _servingTime, float _managerCommunicationDelay)
//    {
//        base.OnSpawn(_data, _workPlace, _servingTime, _managerCommunicationDelay);
//        agent.enabled = false;
//        attendantTask = GasAttendantTask.Wait;
//        SetPosition();
//        RegisterWithWorkManager();
//        fuelMachinePoint = GasStationManager.instance.GetFillingMachinePoint(savableData.workPointIndex);
//        vehicleFuelPoint = GasStationManager.instance.GetFuelFillingPoint(savableData.workPointIndex);

//        fillingNozel = GasStationManager.instance.GetFuelFillingNozel(savableData.workPointIndex);
//        fuelMachine = GasStationManager.instance.GetFuelFillingMachine(savableData.workPointIndex);

//    }
//    public override void RegisterWithWorkManager()
//    {
//        GasStationManager.instance.AddEmployeeToFillingPoint(savableData.workPointIndex, gameObject);
//    }
//    public override void UnRegisterWithWorkManager()
//    {
//        GasStationManager.instance.RemoveEmployeeFromFillingPoint(savableData.workPointIndex);
//    }
//    public override void ResetServingCustomer()
//    {
//        servingVehicle = null;
//    }
//    public override void UpdateServingCustomer(GameObject _customer)
//    {
//        if (servingVehicle == null)
//        {
//            servingVehicle = _customer;
//        }
//    }
//    public override void OnEndDuty()
//    {
//        base.OnEndDuty();
//        serveState = ServingTaskStatus.Waiting;
//        StopAllCoroutines();
//        if (isFuelingVehicle)
//        {
//            isFuelingVehicle = false;
//            fuelMachine.OnEmployeeStopFueling();
//        }
//        if (fillingNozel.employeeHaveNozel)
//        {
//            fillingNozel.EmployeePlaceNozel();
//        }
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
//                AskForCustomer();
//                break;
//            case Employeestate.Serving:
//                if (isFuelingVehicle)
//                {
//                    if (!fuelMachine.IsFueling() || servingVehicle == null)
//                    {
//                        isFuelingVehicle = false;
//                        SetWorkerToIdle();
//                        serveState = ServingTaskStatus.Waiting;
//                    }
//                    if (servingVehicle.GetComponent<GasStationVehicle>().IsRequiredFuelDelivered())
//                    {
//                        isFuelingVehicle = false;
//                        serveState = ServingTaskStatus.Serving;
//                        target = fuelMachinePoint;
//                        StartCoroutine(PerformTask());
//                    }
//                    return;
//                }
//                ServeCustomer();
//                MoveEmployee();
//                break;

//            case Employeestate.MovingToRestingPlace:

//                break;

//            case Employeestate.Leaving:

//                break;
//        }
//    }

 
//    void ServeCustomer()
//    {
//        if (serveState!=ServingTaskStatus.Waiting)
//        {
//            return;
//        }
//        servingTimer -= Time.deltaTime;
//        if (servingTimer > 0)
//        {
//            return;
//        }
//        servingTimer = servingTime;
//        GetNextServingTask();
//    }
//    void GetNextServingTask()
//    {
//        if (servingVehicle == null)
//        {
//            if (fillingNozel.employeeHaveNozel)
//            {
//                target = fuelMachinePoint;
//                SetWorkerToWalking();
//                serveState = ServingTaskStatus.MovingToServe;
//                return;
//            }
            
//            SetWorkerToWalking();
//            employeestate = Employeestate.MovingToWorkPlace;
//            serveState = ServingTaskStatus.Waiting;
//            return;
//        }

//        if (fillingNozel.employeeHaveNozel)
//        {
//            target = vehicleFuelPoint;
//            SetWorkerToWalking();
//            serveState = ServingTaskStatus.MovingToServe;
//            return;
//        }
//        if (servingVehicle.GetComponent<GasStationVehicle>().IsFuelNozelInjected())
//        {
//            if (servingVehicle.GetComponent<GasStationVehicle>().IsRequiredFuelDelivered() && !fuelMachine.IsFueling())
//            {
//                target = vehicleFuelPoint;
//                SetWorkerToWalking();
//                serveState = ServingTaskStatus.MovingToServe;
//                return;
//            }
//            target = fuelMachinePoint;
//            SetWorkerToWalking();
//            serveState = ServingTaskStatus.MovingToServe;
//            return;
//        }
//        target = fuelMachinePoint;
//        SetWorkerToWalking();
//        serveState = ServingTaskStatus.MovingToServe;
//    }
//    void MoveEmployee()
//    {
//        if (serveState!=ServingTaskStatus.MovingToServe)
//            return;
//        EnableAgent();
//        agent.SetDestination(target.position);
//        if (Vector3.Distance(transform.position, target.position) < 0.4f)
//        {
//            serveState = ServingTaskStatus.Serving;
//            StartCoroutine(PerformTask());
//        }
//    }
//    IEnumerator PerformTask()
//    {
//        DisableAgent();
//        SetWorkerToIdle();
//        yield return null;
//        if (target == null)
//        {
//            serveState = ServingTaskStatus.Waiting;
//            yield break;
//        }
//        transform.position = target.position;
//        transform.rotation = target.rotation;
//        if (target == fuelMachinePoint)
//        {
//            //Employee Moved To machine to perform machine Specific Task
//            yield return StartCoroutine(PerformMachineSpecificTask());
//        }
//        else
//        {
//            yield return StartCoroutine(PerformFuelPointSpecificTask());

//        }
//    }
//    IEnumerator PerformMachineSpecificTask()
//    {
//        if (fillingNozel.IsNozelSelected())
//        {
//            serveState = ServingTaskStatus.Waiting;
//            yield break;
//        }
//        if (fuelMachine.IsFueling())
//        {
//            if (servingVehicle != null)
//            {
//                if (servingVehicle.GetComponent<GasStationVehicle>().IsRequiredFuelDelivered())
//                {
//                    SetWorkerToWorking();
//                    fuelMachine.OnEmployeeStopFueling();
//                    yield return new WaitForSeconds(0.75f);                    
//                    serveState = ServingTaskStatus.Waiting;
//                    yield break;
//                }
//                //Check For Fuel Filling
//                isFuelingVehicle = true;
//                yield break;
//            }
//            SetWorkerToWalking();
//            employeestate = Employeestate.MovingToWorkPlace;
//            serveState = ServingTaskStatus.Waiting;
//            yield break;
//        }
//        if (servingVehicle == null)
//        {
//            if (fillingNozel.employeeHaveNozel)
//            {
//                print("Placing Nozel Back After Fuel Delivery");
//                SetWorkerToWorking();
//                yield return new WaitForSeconds(0.75f);
//                fillingNozel.EmployeePlaceNozel();
//                yield return new WaitForSeconds(0.25f);
//            }
//            if (servingVehicle != null)
//            {
//                SetWorkerToIdle();
//                serveState = ServingTaskStatus.Waiting;
//                yield break;
//            }
//            SetWorkerToWalking();
//            employeestate = Employeestate.MovingToWorkPlace;
//            serveState = ServingTaskStatus.Waiting;
//            yield break;
//        }

//        if (servingVehicle.GetComponent<GasStationVehicle>().IsFuelNozelInjected())
//        {
//            SetWorkerToWorking();
//            if (servingVehicle != null && servingVehicle.GetComponent<GasStationVehicle>().IsFuelNozelInjected())
//            {
//                fuelMachine.OnEmployeeStartFueling();
//                isFuelingVehicle = true;
//            }
//            //yield return new WaitForSeconds(0.75f);            
//            SetWorkerToIdle();
//            serveState = ServingTaskStatus.Waiting;
//            yield break;
//        }

//        if (fillingNozel.employeeHaveNozel)
//        {
//            SetWorkerToIdle();
//            serveState = ServingTaskStatus.Waiting;
//            yield break;
//        }

//        SetWorkerToWorking();
//        yield return new WaitForSeconds(0.75f);
//        if (fillingNozel.IsNozelSelected())
//        {
//            SetWorkerToIdle();
//            serveState = ServingTaskStatus.Waiting;
//            yield break;
//        }
//        fillingNozel.EmployeePickNozel(fuelNozelPoint);
//        yield return new WaitForSeconds(0.1f);
//        SetWorkerToIdle();
//        serveState = ServingTaskStatus.Waiting;
//    }

//    IEnumerator PerformFuelPointSpecificTask()
//    {
//        if(servingVehicle==null)
//        {
//            SetWorkerToWalking();
//            employeestate = Employeestate.MovingToWorkPlace;
//            serveState = ServingTaskStatus.Waiting;
//            yield break;
//        }
//        if (fillingNozel.employeeHaveNozel)
//        {
//            SetWorkerToWorking();
//            yield return new WaitForSeconds(0.75f);

//            fillingNozel.EmployeePlaceNozel(servingVehicle);
//            SetWorkerToIdle();
//            serveState = ServingTaskStatus.Waiting;
//        }
//        else if (servingVehicle.GetComponent<GasStationVehicle>().IsFuelNozelInjected())
//        {
//            if (fuelMachine.IsFueling())
//            {
//                SetWorkerToWalking();
//                target = fuelMachinePoint;
//                serveState = ServingTaskStatus.MovingToServe;
//            }
//            SetWorkerToWorking();
//            yield return new WaitForSeconds(0.75f);
//            if (servingVehicle != null)
//            {
//                if (servingVehicle.GetComponent<GasStationVehicle>().IsFuelNozelInjected())
//                {
//                    fillingNozel.EmployeePickNozel(fuelNozelPoint);
//                }
//            }
           
//            SetWorkerToIdle();
//            serveState = ServingTaskStatus.Waiting;
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
//        if (Vector3.Distance(transform.position, workPoint.position) <= 0.4f)
//        {
//            DisableAgent();
//            ChangeEmployeeState(Employeestate.Waiting);
//            transform.position = workPoint.position;
//            transform.rotation = workPoint.rotation;
//            StartCoroutine(RotateTowardsTarget(workPoint));
//        }
//    }
//    /// <summary>
//    /// Ask Gas Station Manager For Any Vehicle ehich is at current employee Filling Point
//    /// </summary>
//    void AskForCustomer()
//    {
//        if (servingVehicle != null)
//        {
//            //Already ASsigned A Vehicle For Fuelling
//            employeestate = Employeestate.Serving;
//            return;
//        }
//        managerCommunicationDelayTimer -= Time.deltaTime;
//        if (managerCommunicationDelayTimer <= 0f)
//        {
//            managerCommunicationDelayTimer = managerCommunicationDelay;
//            servingVehicle = GasStationManager.instance.GetFillingFuelVehicle(savableData.workPointIndex);
//            if (servingVehicle != null)
//            {
//                employeestate = Employeestate.Serving;
//                serveState = ServingTaskStatus.MovingToServe;
//                attendantTask = GasAttendantTask.GetNozel;
//                SetWorkerToWalking();
//                target = fuelMachinePoint;

//            }
//        }
//    }
    
//    IEnumerator RotateTowardsTarget(Transform _target)
//    {
//        yield return null;
//        transform.rotation = _target.rotation;
//    }
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
