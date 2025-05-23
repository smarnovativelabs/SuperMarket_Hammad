//using JetBrains.Annotations;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class EmployeeManager : MonoBehaviour
//{
//    // Start is called before the first frame update
//    public static EmployeeManager Instance;
//    public List<Departments> departments;
//    public List<GameObject> spawnedEmployees;

//    public EmplayeesSaveData saveData;
//    bool isDataInitialized;
//    public bool isPermotionalPanelCleanerDisplayed;
//    public bool isPermotionalPanelReceptionistDisplayed;
//    public bool isPermotionalPanelCashierDisplayed;
//    public bool isPermotionalPaneFuelAttendentDisplayed;

//    void Awake()
//    {
//        Instance = this;
//    }
//    void Start()
//    {
//    }
//    public void SetDeptWorkPlaceLockState(EmployeeType _deptType, int _workPlaceIndex, bool _isUnlocked = false)
//    {
//        for(int i = 0; i < departments.Count; i++)
//        {
//            if (departments[i].deptEmployeeType == _deptType)
//            {
//                if(_workPlaceIndex<0 || _workPlaceIndex>= departments[i].departmentsWorkPlaces.Count)
//                {
//                    break;
//                }
//                departments[i].departmentsWorkPlaces[_workPlaceIndex].isUnlocked = _isUnlocked;
//                break;
//            }
//        }
//    }
//    public IEnumerator InitializeEmployeeData()
//    {
//        LoadData();
//        yield return null;
//        for(int i = 0; i < saveData.savedEmployees.Count; i++)
//        {
//            EmployeeSavableData _data= saveData.savedEmployees[i];
//            SpawnSavedEmployee(_data);
//        }
//        isDataInitialized = true;
//        GameController.instance.AddSavingAction(SaveData);
//        //spawning new employee
//       // SpawnNewEmployee(0,0,0,1);
//    }
//    public void SpawnSavedEmployee(EmployeeSavableData _emp)
//    {
//        EmployeeType _type = (EmployeeType)_emp.employeeType;
//        Departments __curDept = null;
//        for (int i = 0; i < departments.Count; i++) 
//        {
//            if (departments[i].deptEmployeeType == _type)
//            {
//                __curDept= departments[i];
//                break;
//            }
//        }
//        if(__curDept == null)
//        {
//            print("Invalid Department");
//            return;
//        }
//        if (_emp.employeeId >= __curDept.avlEmployees.Count) 
//        {
//            print("Invalid Employe ID");
//            return;
//        }
//        if (_emp.workPointIndex >= __curDept.departmentsWorkPlaces.Count) 
//        {
//            print("Invalid Work Place");
//            return;
//        }
//        DepartmentsWorkPlace _curWorkPlace = __curDept.departmentsWorkPlaces[_emp.workPointIndex];
//        EmployeeData _employee = __curDept.avlEmployees[_emp.employeeId];
//        GameObject _spawnedEmployee = Instantiate(_employee.emplyeePrefab);
//        _spawnedEmployee.GetComponent<Employee>().OnSpawn(_emp, _curWorkPlace, _employee.servingTime, _employee.managerCommunicationDelay);
//        spawnedEmployees.Add(_spawnedEmployee);
//    }

//    public void SpawnNewEmployee(int _departmentId,int _emloyeeId,int _workPlaceIndex,float _dutyTimeInDays)
//    {
//        Departments __curDept = departments[_departmentId];
//        EmployeeData _employee = __curDept.avlEmployees[_emloyeeId];
//        DepartmentsWorkPlace _curWorkPlace = __curDept.departmentsWorkPlaces[_workPlaceIndex];
//        EmployeeSavableData _empData = new EmployeeSavableData();
//        _empData.employeeType = (int)__curDept.deptEmployeeType;
//        _empData.employeeId = _emloyeeId;
//        _empData.workPointIndex = _workPlaceIndex;
//        _empData.departmentID = _departmentId;
//        _empData.isPermanent = false;
//        _empData.remDutyTime = ((_dutyTimeInDays * GameController.instance.dayCycleMinutes) * 60f);
//        _empData.employeeState = (int)__curDept.defaultState;
//        GameObject _spawnedEmployee = Instantiate(_employee.emplyeePrefab);
//        _spawnedEmployee.GetComponent<Employee>().OnSpawn(_empData, _curWorkPlace, _employee.servingTime,_employee.managerCommunicationDelay);
//        spawnedEmployees.Add(_spawnedEmployee);
//        saveData.savedEmployees.Add(_empData);
//        EmployeesUIManager.instance.OnSpawnNewEmployee(_employee, _spawnedEmployee);
//    }

//    //public void SpawnPermanentEmployee(int _departmentId)
//    //{
//    //  /*  if (!CanPurchaseEmployee(_departmentId))
//    //    {
//    //        UIController.instance.DisplayInstructions("Cannot Purchase No WorkSpace available!");
//    //    }*/
//    //    if (_departmentId < 0 || _departmentId >= departments.Count)
//    //    {
//    //        print("Invalid dept");
//    //        return;
//    //    }
//    //    int _workPlaceIndex = GetWorkplaceIndexByDeptId(_departmentId);

//    //    /* if (_workPlaceIndex == -1)
//    //     {
//    //         print("No available workplace for a permanent employee.");
//    //         return;
//    //     }*/

//    //    FireNonPermanentEmployee(_departmentId, _workPlaceIndex);
//    //    int _employeeId = 2;
//    //    Departments __curDept = departments[_departmentId];
//    //    EmployeeData _employee = __curDept.avlEmployees[_employeeId];
//    //    DepartmentsWorkPlace _curWorkPlace = __curDept.departmentsWorkPlaces[_workPlaceIndex];
//    //    EmployeeSavableData _empData = new EmployeeSavableData();
//    //    _empData.employeeType = (int)__curDept.deptEmployeeType;
//    //    _empData.employeeId = _employeeId;
//    //    _empData.workPointIndex = _workPlaceIndex;
//    //    _empData.departmentID = _departmentId;
//    //    _empData.isPermanent = true;
//    //    _empData.remDutyTime = -1;
//    //    print("current employee workplace:"+IsWorkPlaceLocked(_departmentId, _workPlaceIndex));
//    //      _empData.employeeState = IsWorkPlaceLocked(_departmentId, _workPlaceIndex)
//    //                ? (int)Employeestate.WaitingForWorkPlaceToUnlock
//    //                : (int)__curDept.defaultState;
//    //   // _empData.employeeState= (int)__curDept.defaultState;
//    //    GameObject _spawnedEmployee = Instantiate(_employee.emplyeePrefab);
//    //    Employee _employeeComponent = _spawnedEmployee.GetComponent<Employee>();

//    //    if (_employeeComponent == null)
//    //    {
//    //        print("Failed to find Employee component on the instantiated object.");
//    //        Destroy(_spawnedEmployee);
//    //        return;
//    //    }
//    //    _employeeComponent.OnSpawn(_empData, _curWorkPlace, _employee.servingTime, _employee.managerCommunicationDelay);

//    //    spawnedEmployees.Add(_spawnedEmployee);
//    //    saveData.savedEmployees.Add(_empData);
//    //    EmployeesUIManager.instance.OnSpawnNewEmployee(_employee, _spawnedEmployee);
//    //    print("spawned permanent employee");

//    //    if (_departmentId == 3)
//    //    {
//    //        CleanerManager.instance.DisableRVCleaner(_workPlaceIndex);
//    //    }

//    //}
//    public bool IsWorkplaceUnlocked(int _deptId, int _workplaceIndex)
//    {
//        if (_deptId < 0 || _deptId >= departments.Count)
//        {
//            return false;
//        }
//        Departments __curDept = departments[_deptId];
//        if (_workplaceIndex < 0 || _workplaceIndex >= departments[_deptId].departmentsWorkPlaces.Count)
//        {
//            return false;
//        }
//        return (departments[_deptId].departmentsWorkPlaces[_workplaceIndex].isUnlocked);
//    }
//    //public void SpawnPermanentEmployee(int _departmentId)
//    //{
//    //    /*  if (!CanPurchaseEmployee(_departmentId))
//    //      {
//    //          UIController.instance.DisplayInstructions("Cannot Purchase No WorkSpace available!");
//    //      }*/
//    //    if (_departmentId < 0 || _departmentId >= departments.Count)
//    //    {
//    //        print("Invalid dept");
//    //        return;
//    //    }
//    //    int _workPlaceIndex = GetWorkplaceIndexByDeptId(_departmentId);

//    //    if (_workPlaceIndex < 0)
//    //    {
//    //        UIController.instance.DisplayInstructions("No More Workplace Available!");
//    //        print("No available workplace for a permanent employee.");
//    //        return;
//    //    }

//    //    FireNonPermanentEmployee(_departmentId, _workPlaceIndex);
//    //    int _employeeId = 2;
//    //    Departments __curDept = departments[_departmentId];
//    //    if (__curDept.avlEmployees.Count == 1)
//    //    {
//    //        _employeeId = 0;
//    //    }
//    //    EmployeeData _employee = __curDept.avlEmployees[_employeeId];
//    //    DepartmentsWorkPlace _curWorkPlace = __curDept.departmentsWorkPlaces[_workPlaceIndex];
//    //    EmployeeSavableData _empData = new EmployeeSavableData();
//    //    _empData.employeeType = (int)__curDept.deptEmployeeType;
//    //    _empData.employeeId = _employeeId;
//    //    _empData.workPointIndex = _workPlaceIndex;
//    //    _empData.departmentID = _departmentId;
//    //    _empData.isPermanent = true;
//    //    _empData.remDutyTime = -1;
//    //    _empData.employeeState = IsWorkplaceUnlocked(_departmentId, _workPlaceIndex)
//    //              ? (int)__curDept.defaultState : (int)Employeestate.WaitingForWorkPlaceToUnlock;
//    //    print("Employee state : " + _empData.employeeState);
//    //    // _empData.employeeState= (int)__curDept.defaultState;
//    //    GameObject _spawnedEmployee = Instantiate(_employee.emplyeePrefab);
//    //    Employee _employeeComponent = _spawnedEmployee.GetComponent<Employee>();
//    //    print("_employee Component : " + _employeeComponent);
//    //    if (_employeeComponent == null)
//    //    {
//    //        print("Failed to find Employee component on the instantiated object.");
//    //        Destroy(_spawnedEmployee);
//    //        return;
//    //    }
//    //    Debug.Log("This is _empData : " + _empData );
//    //    Debug.Log("This is _curWorkPlace : " + _curWorkPlace);
//    //    Debug.Log("This is _employee.servingTime : " + _employee.servingTime);
//    //    Debug.Log("This is_employee.managerCommunicationDelay : " + _employee.managerCommunicationDelay);
//    //    _employeeComponent.OnSpawn(_empData, _curWorkPlace, _employee.servingTime, _employee.managerCommunicationDelay);

//    //    spawnedEmployees.Add(_spawnedEmployee);
//    //    saveData.savedEmployees.Add(_empData);
//    //    EmployeesUIManager.instance.OnSpawnNewEmployee(_employee, _spawnedEmployee);
//    //    print("spawned permanent employee");

//    //    if (_departmentId == 3)
//    //    {
//    //        CleanerManager.instance.DisableRVCleaner(_workPlaceIndex);
//    //    }

//    //}
//    public void SpawnPermanentEmployee(int _departmentId)
//    {
//        /*  if (!CanPurchaseEmployee(_departmentId))
//          {
//              UIController.instance.DisplayInstructions("Cannot Purchase No WorkSpace available!");
//          }*/
//        if (_departmentId < 0 || _departmentId >= departments.Count)
//        {
//            print("Invalid dept");
//            return;
//        }
//        int _workPlaceIndex = GetWorkplaceIndexByDeptId(_departmentId);

//        if (_workPlaceIndex < 0)
//        {
//            UIController.instance.DisplayInstructions("No More Workplace Available!");
//            print("No available workplace for a permanent employee.");
//            return;
//        }

//        FireNonPermanentEmployee(_departmentId, _workPlaceIndex);
//        int _employeeId = 2;
//        Departments __curDept = departments[_departmentId];
//        if (__curDept.avlEmployees.Count == 1)
//        {
//            _employeeId = 0;
//        }
//        EmployeeData employee = __curDept.avlEmployees[_employeeId];
//        DepartmentsWorkPlace curWorkPlace = __curDept.departmentsWorkPlaces[_workPlaceIndex];
//        EmployeeSavableData _empData = new EmployeeSavableData();
//        _empData.employeeType = (int)__curDept.deptEmployeeType;
//        _empData.employeeId = _employeeId;
//        _empData.workPointIndex = _workPlaceIndex;
//        _empData.departmentID = _departmentId;
//        _empData.isPermanent = true;
//        _empData.remDutyTime = -1;
//        _empData.employeeState = IsWorkplaceUnlocked(_departmentId, _workPlaceIndex)
//                  ? (int)__curDept.defaultState : (int)Employeestate.WaitingForWorkPlaceToUnlock;
//        // _empData.employeeState= (int)__curDept.defaultState;
//        GameObject _spawnedEmployee = Instantiate(employee.emplyeePrefab);
//        Employee employeeComponent = _spawnedEmployee.GetComponent<Employee>();

//        if (employeeComponent == null)
//        {
//            print("Failed to find Employee component on the instantiated object.");
//            Destroy(_spawnedEmployee);
//            return;
//        }
//        employeeComponent.OnSpawn(_empData, curWorkPlace, employee.servingTime, employee.managerCommunicationDelay);

//        spawnedEmployees.Add(_spawnedEmployee);
//        saveData.savedEmployees.Add(_empData);
//        EmployeesUIManager.instance.OnSpawnNewEmployee(employee, _spawnedEmployee);
//        print("spawned permanent employee");

//        if (_departmentId == 3)
//        {
//            CleanerManager.instance.DisableRVCleaner(_workPlaceIndex);
//        }

//    }
//    public int GetWorkplaceIndexByDeptId(int _deptIndex)
//    {
//        if (_deptIndex < 0 || _deptIndex >= departments.Count)
//        {
//            return -1;
//        }
//        Departments __curDept = departments[_deptIndex];
//        List<DepartmentsWorkPlace> _workPlaces = __curDept.departmentsWorkPlaces;

//        for (int i = 0; i < _workPlaces.Count; i++)
//        {
//            if (!_workPlaces[i].isOccupied)
//            {
//                return i;
//            }
//        }
//        for (int i = 0; i < spawnedEmployees.Count; i++)
//        {
//            Employee employee = spawnedEmployees[i].GetComponent<Employee>();

//            if (employee != null &&
//                employee.departmentId == _deptIndex &&
//                !employee.isPermanent) 
//            {
//                int workPointIndex = employee.savableData.workPointIndex;


//                if (workPointIndex >= 0 && workPointIndex < _workPlaces.Count)
//                {
//                    return workPointIndex;
//                }
//            }
//        }
//        return -1;
//    }
//    /// <summary>
//    /// In case perment is hired when workplace is busy with non permenent employee 
//    /// we will assign non permenent workplace to permenent employee
//    /// </summary>
//    /// <param name="_workPlace"></param>
//    //public bool isWorkPlaceFreeAndUnlockYet(int _deptId,Employee employee)
//    //{
//    //    bool _workPlaceFree = false;

//    //    if (_deptId < 0 || _deptId >= departments.Count)
//    //    {
//    //        return false;
//    //    }
//    //    Departments __curDept = departments[_deptId];
//    //    List<DepartmentsWorkPlace> _workPlaces = __curDept.departmentsWorkPlaces;

//    //    Debug.Log("_workPlaces: " + _workPlaces.Count);
//    //    for (int i = 0; i < _workPlaces.Count; i++)
//    //    {

//    //        if (_workPlaces[i].isUnlocked && !_workPlaces[i].isOccupied)
//    //        {
//    //            ReassignData(_deptId, i, employee);
//    //            _workPlaceFree = true;
//    //             break;
//    //        }
//    //    }
//    //    return _workPlaceFree;
//    //}
//    public bool isWorkPlaceFreeAndUnlockYet(int _deptId, Employee employee)
//    {
//        bool _workPlaceFree = false;

//        if (_deptId < 0 || _deptId >= departments.Count)
//        {
//            return false;
//        }
//        Departments __curDept = departments[_deptId];
//        List<DepartmentsWorkPlace> workPlaces = __curDept.departmentsWorkPlaces;

//        for (int i = 0; i < workPlaces.Count; i++)
//        {
//            if (workPlaces[i].isUnlocked && !workPlaces[i].isOccupied)
//            {
//                employee.UnRegisterWithWorkManager();
//                employee.deptWorkPlaceRefs.isOccupied = false;
//                ReassignData(_deptId, i, employee);
//                return true;
//            }
//        }
//        return _workPlaceFree;
//    }

//    void ReassignData(int _departmentId,int _workPlaceIndex,Employee _employeeScript)
//    {
//        int _employeeId = 2;
//        Departments __curDept = departments[_departmentId];
//        EmployeeData _employee = __curDept.avlEmployees[_employeeId];
//        DepartmentsWorkPlace _curWorkPlace = __curDept.departmentsWorkPlaces[_workPlaceIndex];
//        EmployeeSavableData _empData = new EmployeeSavableData();
//        _empData.employeeType = (int)__curDept.deptEmployeeType;
//        _empData.employeeId = _employeeId;
//        _empData.workPointIndex = _workPlaceIndex;
//        _empData.departmentID = _departmentId;
//        _empData.isPermanent = true;
//        _empData.remDutyTime = -1;
//        _empData.employeeState = (int)__curDept.defaultState;
//        _employeeScript.OnSpawn(_empData, _curWorkPlace, _employee.servingTime, _employee.managerCommunicationDelay);
//    }

//    public bool isAlreadyEmployeedSpawnedWithId(int _deptId, int _empId)
//    {
//        bool isSpawned = false;

//        for (int i = 0; i < spawnedEmployees.Count; i++)
//        {
//            Employee employee = spawnedEmployees[i].GetComponent<Employee>();

//            if (employee != null &&
//                employee.departmentId == _deptId &&
//                employee.employeeId==_empId)
//            {
//                isSpawned=true;
//                break;
//            }
//        }
//        return isSpawned;
//    }
//    /// <summary>
//    /// For permotional panels
//    /// </summary>
//    public bool CanDisplayIAPPermotionPanel(int _dept)
//    {
//        int _EmployeeCount = 0;
//        bool canDisplay = false;
//        for (int i = 0; i < spawnedEmployees.Count; i++)
//        {
//            Employee employee = spawnedEmployees[i].GetComponent<Employee>();

//            if (employee != null &&
//                employee.departmentId == _dept &&
//                employee.isPermanent)
//            {
//                _EmployeeCount++;
//            }
//        }

//        switch (_dept)
//        {
//            case 0:

//                if (!isPermotionalPanelCashierDisplayed && _EmployeeCount == 0)
//                {
//                    isPermotionalPanelCashierDisplayed = true;
//                    canDisplay=true;
//                }
//                break;
//            //case 1:

//            //    if (!isPermotionalPanelReceptionistDisplayed && _EmployeeCount == 0)
//            //    {
//            //        isPermotionalPanelReceptionistDisplayed = true;
//            //        canDisplay= true;
//            //    }
//            //    break;
//            //case 2:

//            //    if (!isPermotionalPaneFuelAttendentDisplayed && _EmployeeCount == 0)
//            //    {
//            //        isPermotionalPaneFuelAttendentDisplayed = true;
//            //        canDisplay= true;
//            //    }
//            //    break;
//            case 1:

//                if (!isPermotionalPanelCleanerDisplayed && _EmployeeCount == 0)
//                {
//                    isPermotionalPanelCleanerDisplayed = true;
//                    canDisplay= true;
//                }
//                break;

//        }

//        return canDisplay;

//    }

//    /// <summary>
//    /// Use this before purchasing new Employee
//    /// </summary>
//    /// <param name="_departmentId"></param>
//    /// <returns></returns>
//    public bool CanPurchaseEmployee(int _departmentId)
//    {
//        if (_departmentId < 0 || _departmentId >= departments.Count)
//        {
//            return false;
//        }

//        Departments __curDept = departments[_departmentId];
//        int totalWorkPlaces = __curDept.departmentsWorkPlaces.Count;

//        if (totalWorkPlaces == 0)
//        {
//            print("No workplaces available in department");
//            return false;
//        }
//        int permanentEmployeeCount = 0;
//        foreach (var employeeObj in spawnedEmployees)
//        {
//            Employee employee = employeeObj.GetComponent<Employee>();
//            if (employee != null && employee.departmentId == _departmentId && employee.isPermanent)
//            {
//                permanentEmployeeCount++;
//            }
//        }
//        if (permanentEmployeeCount >= totalWorkPlaces)
//        {
//            print("Maximum permanent employees reached in department");
//            return false;
//        }

//        return true;
//    }
//    /// <summary>
//    /// Priority of inapp employee is high so we are going to fire tem employee of current department
//    /// </summary>
//    /// <param name="_departmentId"></param>
//    public int GetEmployeeIdByDeptId(int _departmentId,int _workPlaceId)
//    {
//        if (_departmentId < 0 || _departmentId >= departments.Count)
//        {
//            return -1;
//        }
//        Departments __curDept = departments[_departmentId];
//        if (_workPlaceId < 0 || _workPlaceId >= __curDept.departmentsWorkPlaces.Count)
//        {
//            print("No suitable workplace found the dept");
//            return -1;
//        }
//        for (int i = 0; i < spawnedEmployees.Count; i++)
//        {
//            Employee employee = spawnedEmployees[i].GetComponent<Employee>();

//            if (!employee.isPermanent)
//            {
//                if (employee.departmentId == _departmentId && employee.savableData.workPointIndex == _workPlaceId)
//                {
//                    print("Inside this check");
//                    employee.OnEndDuty();
//                    return employee.employeeId;
//                }
//            }
//        }

//        // If no suitable employee is found
//        print("No temp employee found in the dept");
//        return -1;
//    }

//    void FireNonPermanentEmployee(int _deptId,int _workPlaceId)
//    {
//        for (int i = 0; i < spawnedEmployees.Count; i++)
//        {
//            Employee employee = spawnedEmployees[i].GetComponent<Employee>();

//            if (!employee.isPermanent)
//            {
//                if (employee.departmentId == _deptId && employee.savableData.workPointIndex == _workPlaceId)
//                {
//                    employee.OnEndDuty();
//                    break;
//                }
//            }
//        }
//    }

//    /// <summary>
//    /// use this method after purchasing a new employee
//    /// </summary>
//    /// <param name="_deptIndex"></param>
//    /// <param name="_workPlaceIndex"></param>
//    /// <returns></returns>
//    public bool IsWorkPlaceLocked(int _deptIndex,int _workPlaceIndex)
//    {
//        if (_deptIndex < 0 || _deptIndex >= departments.Count)
//        {
//            Debug.LogError($"Invalid department index: {_deptIndex}");
//            return false;
//        }

//        Departments __curDept = departments[_deptIndex];
//        List<DepartmentsWorkPlace> _workPlaces = __curDept.departmentsWorkPlaces;
//        if (_workPlaceIndex < 0 || _workPlaceIndex >= _workPlaces.Count)
//        {
//            Debug.LogError($"Invalid workplace index: {_workPlaceIndex} for department {_deptIndex}");
//            return false;
//        }
//        // Check if the workplace is locked
//        if (!_workPlaces[_workPlaceIndex].isUnlocked)
//        {
//            return true;
//        }

//        return false;
//    }



//    public int GetWorkplaceIndex(int _deptIndex)
//    {
//        print("current dept:" + _deptIndex);
//        if(_deptIndex<0 || _deptIndex >= departments.Count)
//        {
//            return - 1;
//        }
//        Departments __curDept = departments[_deptIndex];

//        int _workPlaceIndex = -1;

//        for (int i = 0; i < __curDept.departmentsWorkPlaces.Count; i++)
//        {
//            if (!__curDept.departmentsWorkPlaces[i].isOccupied && __curDept.departmentsWorkPlaces[i].isUnlocked)
//            {
//                _workPlaceIndex = i;
//                break;
//            }
//        }
//        return _workPlaceIndex;
//    }
//    public void OnRemoveEmployee(GameObject _employee)
//    {
//        if (spawnedEmployees.Contains(_employee))
//        {
//            spawnedEmployees.Remove(_employee);
//        }
//    }
//    public void OnRemoveEmployeeData(EmployeeSavableData _data)
//    {
//        if (saveData.savedEmployees.Contains(_data))
//        {
//            saveData.savedEmployees.Remove(_data);
//        }
//    } 
//    #region Saving/Loading Data

//    void LoadData()
//    {
//        saveData = (EmplayeesSaveData)SerializationManager.LoadFile("EmployeesData");
//        if(saveData == null)
//        {
//            saveData = new EmplayeesSaveData();
//        }
//    }
//    void SaveData()
//    {
//        if (isDataInitialized)
//        {
//            SerializationManager.Save(saveData, "EmployeesData");
//        }
//    }

//    //private void OnApplicationPause(bool pause)
//    //{
//    //    if (pause)
//    //        SaveData();
//    //}
//    //private void OnApplicationQuit()
//    //{
//    //    SaveData();
//    //}
//    //private void OnDestroy()
//    //{
//    //    SaveData();
//    //}
//    #endregion
//}
//[System.Serializable]
//public class Departments
//{
//    public string deptName;
//    public int reqLevel;
//    public EmployeeType deptEmployeeType;
//    public Employeestate defaultState;
//    public List<EmployeeData> avlEmployees;
//    public List<DepartmentsWorkPlace> departmentsWorkPlaces;
//}

//[System.Serializable]

//public class DepartmentsWorkPlace
//{
//    public Transform workPoint;
//    public Transform spawnPoint;
//    public Transform restPoint;
//    public Transform leavingPoint;
//    public bool isOccupied;
//    public bool isUnlocked;
//}

//[System.Serializable]
//public class HiringCost
//{
//    public CostType type;
//    public float price;
//}
//[System.Serializable]
//public enum CostType
//{
//    Cash,
//    Blitz
//}
//public enum EmployeeType
//{
//    Cashier = 0,
//    Cleaner= 1,
//   // Receptionist = 2,
//    //FuelAttendants=2,

//}
//[System.Serializable]
//public class EmployeeSavableData
//{
//    public int employeeType;
//    public int employeeId;
//    public int employeeState;
//    public int workPointIndex;
//    public float remDutyTime;
//    public int departmentID;
//    public bool isPermanent;
//}
//[System.Serializable]
//public class EmplayeesSaveData
//{
//    public List<EmployeeSavableData> savedEmployees;
//    public EmplayeesSaveData()
//    {
//        savedEmployees = new List<EmployeeSavableData>();
//    }
//}

using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class EmployeeManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static EmployeeManager Instance;
    public List<Departments> departments;
    public List<GameObject> spawnedEmployees;
    //public Transform permanentEmployeeSpawnPoint;
    public Transform permeanentSpawnPointForEmployee;

    public EmplayeesSaveData saveData;
    bool isDataInitialized;
    public bool isPermotionalPanelCleanerDisplayed;
    public bool isPermotionalPanelReceptionistDisplayed;
    public bool isPermotionalPanelCashierDisplayed;
    public bool isPermotionalPaneFuelAttendentDisplayed;

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
    }
    public void SetDeptWorkPlaceLockState(EmployeeType deptType, int workPlaceIndex, bool _isUnlocked = false)
    {
        for (int i = 0; i < departments.Count; i++)
        {
            if (departments[i].deptEmployeeType == deptType)
            {
                if (workPlaceIndex < 0 || workPlaceIndex >= departments[i].departmentsWorkPlaces.Count)
                {
                    break;
                }
                departments[i].departmentsWorkPlaces[workPlaceIndex].isUnlocked = _isUnlocked;
                break;
            }
        }
    }
    public IEnumerator InitializeEmployeeData()
    {
        LoadData();
        yield return null;
        for (int i = 0; i < saveData.savedEmployees.Count; i++)
        {
            EmployeeSavableData _data = saveData.savedEmployees[i];
            SpawnSavedEmployee(_data);
        }
        isDataInitialized = true;
        GameController.instance.AddSavingAction(SaveData);
    }
    public void SpawnSavedEmployee(EmployeeSavableData _emp)
    {
        print("1C");
        EmployeeType _type = (EmployeeType)_emp.employeeType;
        Departments __curDept = null;
        for (int i = 0; i < departments.Count; i++)
        {
            if (departments[i].deptEmployeeType == _type)
            {
                __curDept = departments[i];
                break;
            }
        }
        if (__curDept == null)
        {
            print("Invalid Department");
            return;
        }
        if (_emp.employeeId >= __curDept.avlEmployees.Count)
        {
            print("Invalid Employe ID");
            return;
        }
        if (_emp.workPointIndex >= __curDept.departmentsWorkPlaces.Count)
        {
            print("Invalid Work Place");
            return;
        }
        DepartmentsWorkPlace curWorkPlace = __curDept.departmentsWorkPlaces[_emp.workPointIndex];
        EmployeeData employee = __curDept.avlEmployees[_emp.employeeId];
        GameObject _spawnedEmployee = Instantiate(employee.emplyeePrefab, permeanentSpawnPointForEmployee);
        _spawnedEmployee.GetComponent<Employee>().OnSpawn(_emp, curWorkPlace, employee.servingTime, employee.managerCommunicationDelay);
        spawnedEmployees.Add(_spawnedEmployee);
    }

    public void SpawnNewEmployee(int departmentId, int emloyeeId, int workPlaceIndex, float dutyTimeInDays)
    {
        Departments __curDept = departments[departmentId];
        EmployeeData employee = __curDept.avlEmployees[emloyeeId];
        print("department workplace index:" + workPlaceIndex);
        DepartmentsWorkPlace curWorkPlace = __curDept.departmentsWorkPlaces[workPlaceIndex];
        EmployeeSavableData _empData = new EmployeeSavableData();
        _empData.employeeType = (int)__curDept.deptEmployeeType;
        _empData.employeeId = emloyeeId;
        _empData.workPointIndex = workPlaceIndex;
        _empData.departmentID = departmentId;
        _empData.isPermanent = false;
        //_empData.remDutyTime = ((dutyTimeInDays  GameController.instance.dayCycleMinutes)  60f);
        _empData.remDutyTime = ((dutyTimeInDays * GameController.instance.dayCycleMinutes) * 60f);
        _empData.employeeState = (int)__curDept.defaultState;
        GameObject _spawnedEmployee = Instantiate(employee.emplyeePrefab);
        _spawnedEmployee.GetComponent<Employee>().OnSpawn(_empData, curWorkPlace, employee.servingTime, employee.managerCommunicationDelay);
        spawnedEmployees.Add(_spawnedEmployee);
        saveData.savedEmployees.Add(_empData);
        EmployeesUIManager.instance.OnSpawnNewEmployee(employee, _spawnedEmployee);
    }

    public void SpawnPermanentEmployee(int _departmentId)
    {
        /*  if (!CanPurchaseEmployee(_departmentId))
          {
              UIController.instance.DisplayInstructions("Cannot Purchase No WorkSpace available!");
          }*/
        if (_departmentId < 0 || _departmentId >= departments.Count)
        {
            print("Invalid dept");
            return;
        }
        int _workPlaceIndex = GetWorkplaceIndexByDeptId(_departmentId);

        if (_workPlaceIndex < 0)
        {
            UIController.instance.DisplayInstructions("No More Workplace Available!");
            print("No available workplace for a permanent employee.");
            return;
        }

        FireNonPermanentEmployee(_departmentId, _workPlaceIndex);
        int _employeeId = 2;
        Departments __curDept = departments[_departmentId];
        if (__curDept.avlEmployees.Count == 1)
        {
            _employeeId = 0;
        }
        EmployeeData employee = __curDept.avlEmployees[_employeeId];
        DepartmentsWorkPlace curWorkPlace = __curDept.departmentsWorkPlaces[_workPlaceIndex];
        EmployeeSavableData _empData = new EmployeeSavableData();
        _empData.employeeType = (int)__curDept.deptEmployeeType;
        _empData.employeeId = _employeeId;
        _empData.workPointIndex = _workPlaceIndex;
        _empData.departmentID = _departmentId;
        _empData.isPermanent = true;
        _empData.remDutyTime = -1;
        _empData.employeeState = IsWorkplaceUnlocked(_departmentId, _workPlaceIndex)
                  ? (int)__curDept.defaultState : (int)Employeestate.WaitingForWorkPlaceToUnlock;
        // _empData.employeeState= (int)__curDept.defaultState;
        GameObject _spawnedEmployee = Instantiate(employee.emplyeePrefab, permeanentSpawnPointForEmployee);
        Employee employeeComponent = _spawnedEmployee.GetComponent<Employee>();

        if (employeeComponent == null)
        {
            print("Failed to find Employee component on the instantiated object.");
            Destroy(_spawnedEmployee);
            return;
        }
        print("oho this is employee serving timer >>>>>>>>>" + employee.servingTime);
        employeeComponent.OnSpawn(_empData, curWorkPlace, employee.servingTime, employee.managerCommunicationDelay);

        spawnedEmployees.Add(_spawnedEmployee);
        saveData.savedEmployees.Add(_empData);
        EmployeesUIManager.instance.OnSpawnNewEmployee(employee, _spawnedEmployee);
        print("spawned permanent employee");

        if (_departmentId == 3)
        {
            CleanerManager.instance.DisableRVCleaner(_workPlaceIndex);
        }

    }
    public int GetWorkplaceIndexByDeptId(int _deptIndex)
    {
        if (_deptIndex < 0 || _deptIndex >= departments.Count)
        {
            return -1;
        }
        Departments __curDept = departments[_deptIndex];
        List<DepartmentsWorkPlace> workPlaces = __curDept.departmentsWorkPlaces;

        for (int i = 0; i < workPlaces.Count; i++)
        {
            if (!workPlaces[i].isOccupied)
            {
                return i;
            }
        }
        for (int i = 0; i < spawnedEmployees.Count; i++)
        {
            Employee employee = spawnedEmployees[i].GetComponent<Employee>();

            if (employee != null &&
                employee.departmentId == _deptIndex &&
                !employee.isPermanent)
            {
                int workPointIndex = employee.savableData.workPointIndex;
                if (workPointIndex >= 0 && workPointIndex < workPlaces.Count)
                {
                    return workPointIndex;
                }
            }
        }
        return -1;
    }
    /// <summary>
    /// In case perment is hired when workplace is busy with non permenent employee 
    /// we will assign non permenent workplace to permenent employee
    /// </summary>
    /// <param name="_workPlace"></param>
    public bool isWorkPlaceFreeAndUnlockYet(int deptId, Employee employee)
    {
        bool _workPlaceFree = false;

        if (deptId < 0 || deptId >= departments.Count)
        {
            return false;
        }
        Departments __curDept = departments[deptId];
        List<DepartmentsWorkPlace> workPlaces = __curDept.departmentsWorkPlaces;

        for (int i = 0; i < workPlaces.Count; i++)
        {
            if (workPlaces[i].isUnlocked && !workPlaces[i].isOccupied)
            {
                employee.UnRegisterWithWorkManager();
                employee.deptWorkPlaceRefs.isOccupied = false;
                ReassignData(deptId, i, employee);
                return true;
            }
        }
        return _workPlaceFree;
    }
    public bool IsWorkplaceUnlocked(int deptId, int workplaceIndex)
    {
        if (deptId < 0 || deptId >= departments.Count)
        {
            return false;
        }
        Departments __curDept = departments[deptId];
        if (workplaceIndex < 0 || workplaceIndex >= departments[deptId].departmentsWorkPlaces.Count)
        {
            return false;
        }
        return (departments[deptId].departmentsWorkPlaces[workplaceIndex].isUnlocked);
    }
    void ReassignData(int departmentId, int workPlaceIndex, Employee _employeeScript)
    {
        int _employeeId = 2;
        Departments __curDept = departments[departmentId];
        EmployeeData employee = __curDept.avlEmployees[_employeeId];
        DepartmentsWorkPlace curWorkPlace = __curDept.departmentsWorkPlaces[workPlaceIndex];
        EmployeeSavableData _empData = new EmployeeSavableData();
        _empData.employeeType = (int)__curDept.deptEmployeeType;
        _empData.employeeId = _employeeId;
        _empData.workPointIndex = workPlaceIndex;
        _empData.departmentID = departmentId;
        _empData.isPermanent = true;
        _empData.remDutyTime = -1;
        _empData.employeeState = (int)__curDept.defaultState;
        _employeeScript.OnSpawn(_empData, curWorkPlace, employee.servingTime, employee.managerCommunicationDelay);
    }

    public bool isAlreadyEmployeedSpawnedWithId(int deptId, int empId)
    {
        bool isSpawned = false;

        for (int i = 0; i < spawnedEmployees.Count; i++)
        {
            Employee employee = spawnedEmployees[i].GetComponent<Employee>();

            if (employee != null &&
                employee.departmentId == deptId &&
                employee.employeeId == empId)
            {
                isSpawned = true;
                break;
            }
        }
        return isSpawned;
    }
    /// <summary>
    /// For permotional panels
    /// </summary>
    public bool CanDisplayIAPPermotionPanel(int _dept)
    {
        int _EmployeeCount = 0;
        bool canDisplay = false;
        for (int i = 0; i < spawnedEmployees.Count; i++)
        {
            Employee employee = spawnedEmployees[i].GetComponent<Employee>();

            if (employee != null &&
                employee.departmentId == _dept &&
                employee.isPermanent)
            {
                _EmployeeCount++;
            }
        }

        switch (_dept)
        {
            case 0:

                if (!isPermotionalPanelCashierDisplayed && _EmployeeCount == 0)
                {
                    isPermotionalPanelCashierDisplayed = true;
                    canDisplay = true;
                }
                break;
            case 1:

                if (!isPermotionalPanelReceptionistDisplayed && _EmployeeCount == 0)
                {
                    isPermotionalPanelReceptionistDisplayed = true;
                    canDisplay = true;
                }
                break;
            case 2:

                if (!isPermotionalPaneFuelAttendentDisplayed && _EmployeeCount == 0)
                {
                    isPermotionalPaneFuelAttendentDisplayed = true;
                    canDisplay = true;
                }
                break;
            case 3:

                if (!isPermotionalPanelCleanerDisplayed && _EmployeeCount == 0)
                {
                    isPermotionalPanelCleanerDisplayed = true;
                    canDisplay = true;
                }
                break;

        }

        return canDisplay;

    }

    /// <summary>
    /// Use this before purchasing new Employee
    /// </summary>
    /// <param name="_departmentId"></param>
    /// <returns></returns>
    public bool CanPurchaseEmployee(int departmentId)
    {
        if (departmentId < 0 || departmentId >= departments.Count)
        {
            return false;
        }

        Departments __curDept = departments[departmentId];
        int totalWorkPlaces = __curDept.departmentsWorkPlaces.Count;

        if (totalWorkPlaces == 0)
        {
            print("No workplaces available in department");
            return false;
        }
        int permanentEmployeeCount = 0;
        foreach (var employeeObj in spawnedEmployees)
        {
            Employee employee = employeeObj.GetComponent<Employee>();
            if (employee != null && employee.departmentId == departmentId && employee.isPermanent)
            {
                permanentEmployeeCount++;
            }
        }
        if (permanentEmployeeCount >= totalWorkPlaces)
        {
            print("Maximum permanent employees reached in department");
            return false;
        }

        return true;
    }
    /// <summary>
    /// Priority of inapp employee is high so we are going to fire tem employee of current department
    /// </summary>
    /// <param name="_departmentId"></param>
    public int GetEmployeeIdByDeptId(int departmentId, int workPlaceId)
    {
        if (departmentId < 0 || departmentId >= departments.Count)
        {
            return -1;
        }
        Departments __curDept = departments[departmentId];
        if (workPlaceId < 0 || workPlaceId >= __curDept.departmentsWorkPlaces.Count)
        {
            print("No suitable workplace found the dept");
            return -1;
        }
        for (int i = 0; i < spawnedEmployees.Count; i++)
        {
            Employee employee = spawnedEmployees[i].GetComponent<Employee>();

            if (!employee.isPermanent)
            {
                if (employee.departmentId == departmentId && employee.savableData.workPointIndex == workPlaceId)
                {
                    print("Inside this check");
                    employee.OnEndDuty();
                    return employee.employeeId;
                }
            }
        }

        // If no suitable employee is found
        print("No temp employee found in the dept");
        return -1;
    }

    public Employee GetEmployeeByDeptIdAndWorkPlace(int departmentId, int workPlaceId)
    {
        Employee employeeHired = null;

        if (departmentId < 0 || departmentId >= departments.Count)
        {
            return null;
        }
        Departments __curDept = departments[departmentId];
        if (workPlaceId < 0 || workPlaceId >= __curDept.departmentsWorkPlaces.Count)
        {
            print("No suitable workplace found the dept");
            return null;
        }
        for (int i = 0; i < spawnedEmployees.Count; i++)
        {
            Employee employee = spawnedEmployees[i].GetComponent<Employee>();

            if (!employee.isPermanent)
            {
                if (employee.departmentId == departmentId && employee.savableData.workPointIndex == workPlaceId)
                {
                    employeeHired = employee;
                    break;
                }
            }
        }
        return employeeHired;
    }

    void FireNonPermanentEmployee(int deptId, int workPlaceId)
    {
        for (int i = 0; i < spawnedEmployees.Count; i++)
        {
            Employee employee = spawnedEmployees[i].GetComponent<Employee>();

            if (!employee.isPermanent)
            {
                if (employee.departmentId == deptId && employee.savableData.workPointIndex == workPlaceId)
                {
                    employee.OnEndDuty();
                    break;
                }
            }
        }
    }

    public int GetWorkplaceIndex(int _deptIndex)
    {
        print("current dept:" + _deptIndex);
        if (_deptIndex < 0 || _deptIndex >= departments.Count)
        {
            return -1;
        }
        Departments __curDept = departments[_deptIndex];

        int _workPlaceIndex = -1;

        for (int i = 0; i < __curDept.departmentsWorkPlaces.Count; i++)
        {
            if (!__curDept.departmentsWorkPlaces[i].isOccupied && __curDept.departmentsWorkPlaces[i].isUnlocked)
            {
                _workPlaceIndex = i;
                break;
            }
        }
        return _workPlaceIndex;
    }
    public void OnRemoveEmployee(GameObject _employee)
    {
        if (spawnedEmployees.Contains(_employee))
        {
            spawnedEmployees.Remove(_employee);
        }
    }
    public void OnRemoveEmployeeData(EmployeeSavableData _data)
    {
        if (saveData.savedEmployees.Contains(_data))
        {
            saveData.savedEmployees.Remove(_data);
        }
    }


    #region Saving/Loading Data

    void LoadData()
    {
        saveData = (EmplayeesSaveData)SerializationManager.LoadFile("EmployeesData");
        if (saveData == null)
        {
            saveData = new EmplayeesSaveData();
        }
    }
    void SaveData()
    {
        if (isDataInitialized)
        {
            SerializationManager.Save(saveData, "EmployeesData");
        }
    }

    //private void OnApplicationPause(bool pause)
    //{
    //    if (pause)
    //        SaveData();
    //}
    //private void OnApplicationQuit()
    //{
    //    SaveData();
    //}
    //private void OnDestroy()
    //{
    //    SaveData();
    //}
    #endregion
}
[System.Serializable]
public class Departments
{
    public string deptName;
    public int reqLevel;
    public EmployeeType deptEmployeeType;
    public Employeestate defaultState;
    public List<EmployeeData> avlEmployees;
    public List<DepartmentsWorkPlace> departmentsWorkPlaces;
}

[System.Serializable]

public class DepartmentsWorkPlace
{
    public Transform workPoint;
    public Transform spawnPoint;
    public Transform restPoint;
    public Transform leavingPoint;
    public bool isOccupied;
    public bool isUnlocked;
}

[System.Serializable]
public class HiringCost
{
    public CostType type;
    public float price;
}
[System.Serializable]
public enum CostType
{
    Cash,
    Blitz,
    RV
}
[System.Serializable]
public enum EmployeeType
{
    Cashier = 0,
    Receptionist = 1,
    FuelAttendants = 2,
    Cleaner = 3,
    StoreRestocker = 4

}
[System.Serializable]
public class EmployeeSavableData
{
    public int employeeType;
    public int employeeId;
    public int employeeState;
    public int workPointIndex;
    public float remDutyTime;
    public int departmentID;
    public bool isPermanent;
}
[System.Serializable]
public class EmplayeesSaveData
{
    public List<EmployeeSavableData> savedEmployees;
    public EmplayeesSaveData()
    {
        savedEmployees = new List<EmployeeSavableData>();
    }
}