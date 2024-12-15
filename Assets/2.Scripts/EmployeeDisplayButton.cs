using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EmployeeDisplayButton : MonoBehaviour
{
    public Text empTypeTxt;
    public Text empDescTxt;
    public Text responseTimeTxt;
    public Text tagTxt;
    public Text lockLevTxt;
    public Text priceTxt;
    public Image tagImg;
    public Image displayImg;
    public Image lockImg;
    //public Button actionBtn;
    int empIndex;
    int reqLevel;
    public void SetItemUI(EmployeeData _emp,int _empIndex,int _reqLevel)
    {
        empIndex = _empIndex;
        reqLevel = _reqLevel;
        empTypeTxt.text = _emp.employeeType.ToString();
        empDescTxt.text = _emp.empDescription;
        responseTimeTxt.text = "Response Time: " + _emp.servingTime.ToString() + "s";
        tagTxt.text = _emp.employeeTag;
        lockLevTxt.text = "Unlocks At Level " + _reqLevel.ToString();
        priceTxt.text = ((int)_emp.hiringCosts[0].price).ToString();

        tagImg.sprite = _emp.tagSprite;
        displayImg.sprite = _emp.displayImg;
        lockImg.gameObject.SetActive(_reqLevel > PlayerDataManager.instance.playerData.playerLevel);
    }
    public void UpdateItemUI()
    {
        lockImg.gameObject.SetActive(reqLevel > PlayerDataManager.instance.playerData.playerLevel);
    }
    public void OnActionPressed()
    {
        HiringStaffUIManager.instance.OnpurchaseEmployee(empIndex);
    }
}
