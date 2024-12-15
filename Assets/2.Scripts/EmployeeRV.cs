using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class EmployeeRV : MonoBehaviour,InteractableObjects
{
    public string employeeName;
    public int requiredRVToUnlock;
    public GameObject canvas;
    public EmployeeData employeeData;
    public int deptIndex;
    public int workPlaceIndex;
    public float dutyTime;
    public int totalRVWatched;
    public TextMeshProUGUI remaningAdsText;
    Animator animator;
    public EmployeeType deptEmployeeType;
    void Start()
    {
        remaningAdsText.text= requiredRVToUnlock.ToString();
    }
    public void SetEmployeeToIdle()
    {
        print("setting idle");
        animator = GetComponent<Animator>();
        animator.SetInteger("AnimationState", 0);
    }

    public void EmployeeRVSuceess()
    {
        totalRVWatched++;

        if (totalRVWatched >= requiredRVToUnlock)
        {
            workPlaceIndex = EmployeeManager.Instance.GetWorkplaceIndex(deptIndex);
            EmployeeManager.Instance.SpawnNewEmployee(deptIndex, employeeData.employeeId, workPlaceIndex, dutyTime);

            if (deptEmployeeType == EmployeeType.Cashier)
            {
                SuperStoreManager.instance.DisableCashierRV(workPlaceIndex);
                UIController.instance.DisplayInstructions("Cashier hired");
            }
            else if (deptEmployeeType == EmployeeType.Cleaner)
            {
                CleanerManager.instance.DisableCleanerRV(workPlaceIndex);
                UIController.instance.DisplayInstructions("Cleaner hired");
            }
            totalRVWatched = 0;
           
          //  GameManager.instance.CallFireBase("EmpHrRV_" + ((int)employeeData.employeeType).ToString());
        }
        updateUI();
    }

    void updateUI()
    {
        int required = (requiredRVToUnlock - totalRVWatched);
        remaningAdsText.text = required.ToString();
    }

    public void EmploeeRVFailure()
    {
      
    }
    public void OnHoverItems()
    {
        if (deptEmployeeType == EmployeeType.Cashier)
        {
            UIController.instance.EnableHiringPanel(EmployeeType.Cashier, dutyTime, requiredRVToUnlock, EmployeeRVSuceess, EmploeeRVFailure);
        }
       else if (deptEmployeeType == EmployeeType.Cleaner)
        {
            UIController.instance.EnableHiringPanel(EmployeeType.Cleaner, dutyTime, requiredRVToUnlock, EmployeeRVSuceess, EmploeeRVFailure);
        }
    }
    public void OnInteract()
    {
        if (deptEmployeeType == EmployeeType.Cashier)
        {
            UIController.instance.EnableHiringPanel(EmployeeType.Cashier, dutyTime, requiredRVToUnlock, EmployeeRVSuceess, EmploeeRVFailure);
        }

        if (deptEmployeeType == EmployeeType.Cleaner)
        {
            UIController.instance.EnableHiringPanel(EmployeeType.Cleaner, dutyTime, requiredRVToUnlock, EmployeeRVSuceess, EmploeeRVFailure);
        }
    }
    public void TurnOffOutline()
    {
        UIController.instance.OnCloseHiringRV();

    }

}
