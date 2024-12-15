using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CleanerManager : MonoBehaviour
{
    public static CleanerManager instance;
    public List<GameObject> cleaningStaff = new List<GameObject>();
    public GameObject[] cleanerRV;
    public int cleanerRequiredLevel;
    public int totalCleanerSpawned;
   
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void DisableCleanerRV(int _id)
    {
        cleanerRV[_id].gameObject.SetActive(false);
    }

    public void EnableCleanerRV(int _id)
    {
        cleanerRV[_id].gameObject.SetActive(true);
    }

    /// <summary>
    /// Call this method on level complete and on game start
    /// </summary>
    public void EnableAllCleanersUponRequiredLevelReached()
    {
        if (PlayerDataManager.instance.playerData.playerLevel >= cleanerRequiredLevel)
        {
            for (int i = 0; i < cleanerRV.Length; i++)
            {
                cleanerRV[i].gameObject.SetActive(true);
      
                EmployeeManager.Instance.SetDeptWorkPlaceLockState(EmployeeType.Cleaner, i, true);
            }
            List<int> occopiedSpots = GetOccopiedCleanerWorkPlaces(3);

            for (int i = 0; i < occopiedSpots.Count; i++)
            {
                cleanerRV[occopiedSpots[i]].SetActive(false);
            }
        }
    }

     List<int> GetOccopiedCleanerWorkPlaces(int _deptId){

        List<int> workPlaces = new List<int>();

        for(int i = 0; i < EmployeeManager.Instance.spawnedEmployees.Count; i++)
        {
            if (EmployeeManager.Instance.spawnedEmployees[i].GetComponent<Employee>().departmentId == _deptId)
            {
                workPlaces.Add(EmployeeManager.Instance.spawnedEmployees[i].GetComponent<Employee>().savableData.workPointIndex);
            }
        }
        return workPlaces;
    }

    public IEnumerator EnableRVCleanerOnGmaeStart()
    {
        yield return new WaitForSeconds(1f);

        if (PlayerDataManager.instance.playerData.playerLevel >= cleanerRV[0].GetComponent<EmployeeRV>().employeeData.unlockLevel)
        {
            EnableCleanersAtStart();
        }
    }

    public void EnableCleanersAtStart()
    {
        if (totalCleanerSpawned == 0)
        {
            for (int i = 0; i < cleanerRV.Length; i++)
            {
              cleanerRV[i].SetActive(true);
            }
        }
       
    }

    public void EnableDisableCleanerAtWorkPlace(int _cleanerWorkPlaceId)
    {
        cleanerRV[_cleanerWorkPlaceId].SetActive(true);
    }
    public void DisableDisableCleanerAtWorkPlace(int _cleanerWorkPlaceId)
    {
        cleanerRV[_cleanerWorkPlaceId].SetActive(false);
    }

    public void DisableRVCleaner(int _workPlace)
    {
        print("workplace index:" + _workPlace);
        if (_workPlace != -1)
        {
            cleanerRV[_workPlace].gameObject.SetActive(false);
        }
    }


  
    public void EnableClanersRV()
    {
        StartCoroutine(EnableRVCleanerOnGmaeStart());
    }


    public bool CanEnableCleaner(int id)
    {
        bool canEnable = true;

        for(int i=0;i< cleaningStaff.Count; i++)
        {
            //if (cleaningStaff[i].gameObject.GetComponent<Cleaner>().employeeId == id)
            //{
            //    canEnable = false;
            //    break;
            //}
        }

        return canEnable;
    }
}
