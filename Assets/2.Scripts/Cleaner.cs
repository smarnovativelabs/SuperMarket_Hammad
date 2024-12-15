//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using static Room;

//public class Cleaner : Employee, InteractableObjects
//{
//    public string employeeName = "Room Cleaner";

//    Room dirtyRoom = null;
//    public bool isOnTopFloor = false;
//    bool movingToStairs = false;
//    Vector3 stairsPoint;
//    int workPlaceIndex = -1;
//    public override void OnSpawn(EmployeeSavableData _data, DepartmentsWorkPlace _workPlace, float _servingTime, float managerCommunicationDelay)
//    {
//        base.OnSpawn(_data, _workPlace, _servingTime, managerCommunicationDelay);
//        workPlaceIndex=_data.workPointIndex;
//        agent.enabled = false;
//        //set position after spawning
//        SetPosition();
//        //add to manager
//        RegisterWithWorkManager();

//        dirtyRoom = RoomManager.instance.GetDirtyRoom();

//        if (dirtyRoom != null)
//        {
//            //lock the player so user will not enter in the room
//            DirtyRoomAssignment(dirtyRoom);
//            SetWorkerToWalking();
//        }
//        else
//        {
//            ChangeEmployeeState(Employeestate.MovingToRestingPlace);
//        }
//        SetWorkerToWalking();
//    }

//    /// <summary>
//    /// Add current created Empoyee to its required manager
//    /// </summary>
//    public override void RegisterWithWorkManager()
//    {
//        ///disable cleaner RV if avaiable impiment method here
//         CleanerManager.instance.cleaningStaff.Add(gameObject);
//         CleanerManager.instance.totalCleanerSpawned++;
//         CleanerManager.instance.DisableDisableCleanerAtWorkPlace(workPlaceIndex);
//    }
//    //When emplyee time is complete call this method
//    public override void UnRegisterWithWorkManager()
//    {
//        CleanerManager.instance.cleaningStaff.Remove(gameObject);
//        CleanerManager.instance.totalCleanerSpawned--;
//        CleanerManager.instance.EnableDisableCleanerAtWorkPlace(workPlaceIndex);


//        if (dirtyRoom != null)
//        {
//            if (dirtyRoom.roomProperties.beingCleaned)
//            {
//                dirtyRoom.roomProperties.beingCleaned = false;
//                dirtyRoom.playerLockCollider.SetActive(false);
//                dirtyRoom.CustomerLeaveRoom();
//                dirtyRoom.roomProperties.cleaningTag.gameObject.SetActive(false);
//                dirtyRoom.roomProperties.dirtyTagParent.SetActive(true);
//            }
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

//        // Manage Cleaner behavior based on the current state
//        switch (employeestate)
//        {
//            case Employeestate.MovingToWorkPlace:
//                MoveToWorkPlace();
//                break;
//            case Employeestate.Waiting:
//                WaitingForDirtyRoom();
//                break;
//            case Employeestate.Serving:
//                CleaningRoom();
//                break;
//            case Employeestate.MovingToRestingPlace:
//                MoveToRestArea();
//                WaitingForDirtyRoom();
//                break;
//            case Employeestate.WaitingForWorkPlaceToUnlock:
//                WorkPlaceIsLocked();
//                break;
//            case Employeestate.Leaving:
//                break;
//            case Employeestate.Rest:
//                WaitingForDirtyRoom();
//                break;
//        }
//    }
//    /// <summary>
//    /// Moves the Cleaner to the working point (using NavMeshAgent).
//    /// </summary>
//    private void MoveToWorkPlace()
//    {
//        EnableAgent();
//        Vector3 _target = movingToStairs ? stairsPoint : workPoint.position;


//        agent.SetDestination(_target);

//        // Check if the agent has reached the destination
//        if (Vector3.Distance(transform.position, _target) <= 0.2f)
//        {
//            if (movingToStairs)
//            {
//                movingToStairs = false;
//                if (!isOnTopFloor && dirtyRoom.roomProperties.isOnTopFloor)
//                {
//                    isOnTopFloor = true;
//                    agent.agentTypeID = GetAgentId(1);
//                }
//                else if (isOnTopFloor && !dirtyRoom.roomProperties.isOnTopFloor)
//                {
//                    isOnTopFloor = false;
//                    agent.agentTypeID = GetAgentId(0);
//                }
//                return;
//            }
//            DisableAgent();
//            //make clenaer to clean room
//            ChangeEmployeeState(Employeestate.Serving);
//            dirtyRoom.roomProperties.cleaningTag.gameObject.SetActive(true);
//            dirtyRoom.roomProperties.dirtyTagParent.SetActive(false);
//            dirtyRoom.CustomerEntersRoom();
//        }
//    }
//    /// <summary>
//    /// Moves the Cleaner to the rest area.
//    /// </summary>
//    /// 
//    float workPlaceUnlockTimer = 5f;
//    bool workSpaceUnlockCheck;
//    private void MoveToRestArea()
//    {
//        EnableAgent();
//        Vector3 _target = movingToStairs ? stairsPoint : restPoint.position;

//        agent.SetDestination(_target);
//        // Check if the agent has reached the destination
//        if (Vector3.Distance(transform.position, _target) <= 0.2f)
//        {
//            if (movingToStairs)
//            {
//                movingToStairs = false;
//                isOnTopFloor = false;
//                agent.agentTypeID = GetAgentId(0);

//            }
//            else
//            {
//                SetWorkerToIdle();
//                DisableAgent();
//                ChangeEmployeeState(Employeestate.Rest);
//                isOnTopFloor = false;
//                transform.rotation = Quaternion.Euler(restPoint.transform.rotation.x, restPoint.transform.rotation.y, restPoint.transform.rotation.z);
//            }
//                // You can handle the next state after reaching the rest area.
//        }
//    }

//    /// <summary>
//    /// Handles the Cleaner's waiting state
//    /// </summary>
//    private void WaitingForDirtyRoom()
//    {
//        managerCommunicationDelayTimer -= Time.deltaTime;

//        if (managerCommunicationDelayTimer <= 0)
//        {
//            dirtyRoom = RoomManager.instance.GetDirtyRoom();
//            if (dirtyRoom != null)
//            {
//                //lock the player so user will not enter in the room
//                DirtyRoomAssignment(dirtyRoom);
//                SetWorkerToWalking();
//            }
//            managerCommunicationDelayTimer = managerCommunicationDelay;
//        }
//    }

//    /// <summary>
//    /// Handles the serving of customers by scanning items and processing transactions.
//    /// </summary>
//    private void CleaningRoom()
//    {
//        servingTimer -= Time.deltaTime;

//        SetWorkerToWorking();

//        if (servingTimer <= 0)
//        {
//            if(dirtyRoom != null)
//            {
//                dirtyRoom.MakeRoomReady();
//                dirtyRoom = null;
//            }
//            //get dirtyroom again if exist
//            dirtyRoom = RoomManager.instance.GetDirtyRoom();

//            if (dirtyRoom != null)
//            {
//                DirtyRoomAssignment(dirtyRoom);
//            }
//            else
//            {
//                if (isOnTopFloor)
//                {
//                    GetStairsPoint();
//                }
//                ChangeEmployeeState(Employeestate.MovingToRestingPlace);
//                SetWorkerToWalking();
//            }
//            servingTimer = servingTime; 
//        }
//    }
//    void GetStairsPoint()
//    {
//        movingToStairs = true;
//        stairsPoint = RoomManager.instance.GetClosestStairsPoint(transform.position);
//    }
//    void DirtyRoomAssignment(Room _dityRoom)
//    {
//        dirtyRoom.roomProperties.beingCleaned = true;
//        dirtyRoom.playerLockCollider.SetActive(true);
//        workPoint = dirtyRoom.roomProperties.customerRoamPoints[0];
//        ChangeEmployeeState(Employeestate.MovingToWorkPlace);
//        SetWorkerToWalking();

//        //if (isOnTopFloor && _dityRoom.roomProperties.isOnTopFloor)
//        //{
//        //    agent.agentTypeID = GetAgentId(1);
//        //}
      
//        if (!isOnTopFloor && _dityRoom.roomProperties.isOnTopFloor)
//        {
//            GetStairsPoint();
//            //agent.agentTypeID = GetAgentId(1);
//        }
//       else if (!isOnTopFloor && !_dityRoom.roomProperties.isOnTopFloor)
//        {
//            agent.agentTypeID = GetAgentId(0);
//        }
//        else if (isOnTopFloor && !_dityRoom.roomProperties.isOnTopFloor)
//        {
//            GetStairsPoint();
//            //agent.agentTypeID = GetAgentId(0);

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
//    /// Resets the Cleaner state by clearing cart items and resetting the state to Waiting.
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
