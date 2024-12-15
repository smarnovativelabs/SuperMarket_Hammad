using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Employee : MonoBehaviour
{
    // Animator for controlling cashier animations
    protected Animator anim;
    // NavMeshAgent for movement
    protected NavMeshAgent agent;
    public Employeestate employeestate; //employee state determin its animation and working
    public int employeeId;
    public int departmentId;
    public Transform workPoint; // where employee going to work
    public Transform restPoint; // ref to staff room or any point where it gona rest or disappear
    public Transform leavePoint;
    public float servingTime; // time differance in serving one order 
    public float managerCommunicationDelay; //performance based delay 
    public float servingTimer;
    protected float managerCommunicationDelayTimer;
    public EmployeeSavableData savableData; // For saving employee progress
    public DepartmentsWorkPlace deptWorkPlaceRefs; //ref to the working place 
    protected string timeString;
    public GameObject timerCanvas;
    public TextMeshProUGUI remaningutyTimeText;
    public bool isPermanent;
    float workPlaceUnlockTimer=3f;
    public virtual void OnSpawn(EmployeeSavableData _data, DepartmentsWorkPlace _workPlace, float _servingTime, float _managerCommunicationDelay)
    {
        _workPlace.isOccupied = true;
        workPoint = _workPlace.workPoint;
        restPoint = _workPlace.restPoint;
        leavePoint = _workPlace.leavingPoint;
        savableData = _data;
        deptWorkPlaceRefs = _workPlace;
        servingTime = _servingTime;
        managerCommunicationDelay = _managerCommunicationDelay;
        managerCommunicationDelayTimer = _managerCommunicationDelay;
        servingTimer = _servingTime;
        isPermanent = _data.isPermanent;
        departmentId=_data.departmentID;
        employeestate = (Employeestate)_data.employeeState;
        // Get required components
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        ChangeEmployeeState(employeestate);

        if (isPermanent)
        {
            timerCanvas.SetActive(false);
        }
        else
        {
            timerCanvas.SetActive(false);
        }
    }

    public virtual void SetPosition() { }

    public virtual void RegisterWithWorkManager() { }

    public virtual void UnRegisterWithWorkManager() { }

    public virtual void ResetServingCustomer() { }

    public virtual void UpdateServingCustomer(GameObject _customer) { }
    public virtual void OnEndDuty()
    {
        deptWorkPlaceRefs.isOccupied = false;
        EmployeeManager.Instance.OnRemoveEmployee(gameObject);
        EmployeeManager.Instance.OnRemoveEmployeeData(savableData);
        EnableAgent();
        ChangeEmployeeState(Employeestate.Leaving);
        agent.SetDestination(leavePoint.position);
        UnRegisterWithWorkManager();

        if (EmployeeManager.Instance.CanDisplayIAPPermotionPanel(departmentId))
        {
            IAPUiManager.Instance.DisplayEmployeePromoPanel(departmentId);
        }

      /*  if (!isPermanent)
        {
            //reassign this pint to permenent employee if permenet employee is in waiting
            //
          //  employee.GetComponent<FuelAttendant>().fuelNozelPoint = _fuelNozelPoint;
          //  employee.GetComponent<FuelAttendant>().fuelMachinePoint = _fuelMachinePoint;
          //  employee.GetComponent<FuelAttendant>().vehicleFuelPoint = _vehicleFuelPoint;
          //  employee.GetComponent<FuelAttendant>().fillingNozel = _nozle;
          //  employee.GetComponent<FuelAttendant>().fuelMachine = _fuelMachine;
            if (departmentId == 2)
            {
                EmployeeManager.Instance.AssignWorkPlaceToPermenantFreeEmployee(employeeId, departmentId, savableData.workPointIndex, workPoint,_fuelNozelPoint);
            }
            else
            {
                EmployeeManager.Instance.AssignWorkPlaceToPermenantFreeEmployee(employeeId, departmentId, savableData.workPointIndex, workPoint);
            }
        }*/
    
    }

    public virtual void Update()
    {
        //if the employee is permanent no timer will display on his head

        if (employeestate == Employeestate.Leaving)
        {
            timerCanvas.SetActive(false);
            // Check if the agent has reached the destination
            if (Vector3.Distance(transform.position, leavePoint.position) <= 0.2f)
            {
                DisableAgent();
                Destroy(gameObject);
            }
        }

        if(employeestate == Employeestate.WaitingForWorkPlaceToUnlock)
        {
            workPlaceUnlockTimer -= Time.deltaTime;

            if (Vector3.Distance(transform.position, restPoint.position) <= 0.2f)
            {
                SetWorkerToIdle();
                DisableAgent();

            }
            if (workPlaceUnlockTimer <= 0)
            {
                if (EmployeeManager.Instance.isWorkPlaceFreeAndUnlockYet(departmentId,gameObject.GetComponent<Employee>()))
                {
                    ChangeEmployeeState(Employeestate.MovingToWorkPlace);
                }
                workPlaceUnlockTimer = 3f;
            }
        }
        //no timer for permanent employee
        if (isPermanent)
        {
            return;
        }

        if(employeestate==Employeestate.Waiting || employeestate == Employeestate.Rest || employeestate == Employeestate.Serving)
        {
            savableData.remDutyTime -= Time.deltaTime;
            timerCanvas.SetActive(true);
            if (savableData.remDutyTime <= 0)
            {
                OnEndDuty();
               // GameManager.instance.CallFireBase("EmpLeav_" + savableData.employeeType + "_" + savableData.employeeId);
                return;
            }
            int _remSec = Mathf.FloorToInt(savableData.remDutyTime);
            int _hours = (_remSec / 60);
            int _totalMinutes = _remSec % 60;

            int _days =_hours / GameController.instance.dayCycleMinutes;

            int remHours = _hours % GameController.instance.dayCycleMinutes;

            string _timeString = string.Format("{0:00} : {1:00} : {2:00}", _days.ToString() + "D", remHours.ToString() + "H", _totalMinutes.ToString() + "M");
            remaningutyTimeText.text = _timeString;
        }
       
    }

    /// <summary>
    /// Changes the Employee's state.
    /// </summary>
    /// <param name="state">New state for the employee.</param>
    public void ChangeEmployeeState(Employeestate _state)
    {
        employeestate = _state;
        switch (employeestate)
        {
            case Employeestate.Leaving:
            case Employeestate.MovingToWorkPlace:
            case Employeestate.MovingToRestingPlace:
            case Employeestate.WaitingForWorkPlaceToUnlock:
                SetWorkerToWalking();
                break;

            case Employeestate.Waiting:
            case Employeestate.Rest:
                SetWorkerToIdle();
                break;

            case Employeestate.Serving:
                break;

        }
    }


    public void WorkPlaceIsLocked()
    {
        EnableAgent();
        Vector3 _target = restPoint.position;
        agent.SetDestination(_target);

        // Check if the agent has reached the destination
        if (Vector3.Distance(transform.position, _target) <= 0.2f)
        {
            DisableAgent();
        }
    }

    #region Animation Methods

    // Sets the cashier animation to idle
    public void SetWorkerToIdle()
    {
        anim.SetInteger("AnimationState", 0);
    }

    // Sets the cashier animation to Working (scanning items)
    public void SetWorkerToWorking()
    {
        anim.SetInteger("AnimationState", 2);
    }

    // Sets the cashier animation to Walking (moving)
    public void SetWorkerToWalking()
    {
        anim.SetInteger("AnimationState", 1);
    }

    #endregion

    /// <summary>
    /// Enables the NavMeshAgent.
    /// </summary>
    public void EnableAgent()
    {
        if (!agent.enabled)
        {
            agent.enabled = true;
        }
    }

    /// <summary>
    /// Disables the NavMeshAgent.
    /// </summary>
    public void DisableAgent()
    {
        agent.enabled = false;
    }

   public virtual int GetAgentId(int _indexId)
    {
        if (_indexId < 0 || _indexId >= NavMesh.GetSettingsCount())
        {
            Debug.LogError("Invalid Agent Index!");
            return -1;
        }
        return NavMesh.GetSettingsByIndex(_indexId).agentTypeID;
    }

}

public enum Employeestate
{
    MovingToWorkPlace=0,
    Waiting=1,
    Serving=2,
    MovingToRestingPlace=3,
    Rest=4,
    Leaving=5,
    SwitchingAgentSurface=6,
    WaitingForWorkPlaceToUnlock=7
}