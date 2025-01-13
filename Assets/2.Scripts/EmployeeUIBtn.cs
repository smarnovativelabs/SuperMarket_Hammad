using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmployeeUIBtn : MonoBehaviour
{
    public Text employeeHeading;
    public Text tagText;
   // public Text empDesc;
    public Image employeeTagImg;
    public Image employeeDisplayImg;
    public Text remTimeText;
    public GameObject fireBtn;
    GameObject employeeRef;
    int deptPanelIndex;
    bool isInitialized = false;

    public void InitializeEmployeeBtn(GameObject _employeeRef,EmployeeData _data,int _deptIndex)
    {
        employeeRef = _employeeRef;
        deptPanelIndex = _deptIndex;
        isInitialized = true;

        employeeHeading.text = _data.employeeType.ToString();
        tagText.text = _data.employeeTag;
        employeeTagImg.sprite = _data.tagSprite;
  //      empDesc.text = _data.empDescription;
        employeeDisplayImg.sprite = _data.displayImg;
        fireBtn.SetActive(!_employeeRef.GetComponent<Employee>().savableData.isPermanent);
    }
    private void Update()
    {
        if (!isInitialized)
            return;
        if (employeeRef == null)
        {
            RemoveBtn();
            return;
        }

        if (employeeRef.GetComponent<Employee>().savableData.isPermanent)
        {
            remTimeText.text = "Hired Permanently";
        }
        else
        {
            float _remTime = employeeRef.GetComponent<Employee>().savableData.remDutyTime;
            if (_remTime <= 0)
            {
                RemoveBtn();
                return;
            }

            int _remSec = Mathf.FloorToInt(_remTime);
            int _hours = (_remSec / 60);
            int _totalMinutes = _remSec % 60;

            int _days = _hours / GameController.instance.dayCycleMinutes;

            int remHours = _hours % GameController.instance.dayCycleMinutes;

            string _timeString = string.Format("{0:00} : {1:00} : {2:00}", _days.ToString() + "D", remHours.ToString() + "H", _totalMinutes.ToString() + "M");
            remTimeText.text = "Remaining Time:" + _timeString;
        }
    }
    public void OnFirePressed()
    {
        EmployeesUIManager.instance.EnableConfirmationPanel(gameObject);
    }
    public void OnFireEmployee()
    {
        if (employeeRef != null)
        {
            //permamant employees cannot be fired
            if (employeeRef.GetComponent<Employee>().isPermanent)
            {
                UIController.instance.DisplayInstructions("Cannot fire this employee!");
                return;
            }
            //Call Firing From Here
            employeeRef.GetComponent<Employee>().OnEndDuty();
            GameManager.instance.CallFireBase("EmpFrd_"+ ((int)employeeRef.GetComponent<Employee>().savableData.employeeType) + "_"+ employeeRef.GetComponent<Employee>().savableData.employeeId.ToString());

            RemoveBtn();
        }
    }
    void RemoveBtn()
    {
        EmployeesUIManager.instance.SetContainerLength(deptPanelIndex,-1);
        Destroy(gameObject);
    }
}
