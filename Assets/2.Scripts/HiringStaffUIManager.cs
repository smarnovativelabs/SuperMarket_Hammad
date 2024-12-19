using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HiringStaffUIManager : ManagementTabUI
{
    [Header("Panels/Containers")]
    public Transform deptBtnsContainer;
    public Transform deptPanelsContainer;
    [Header("Response")]
    public GameObject responsePanel;
    public Text responseText;
    [Header("Hrinig Confirmation")]
    public GameObject hiringConfirmPanel;
    public Text workerTypeHeading;
    public Text hiringPeriodText;
    public Text hiringPriceText;

    [Header("Prefabs")]
    public GameObject deptBtnRef;
    public GameObject deptEmpBtnContainerRef;
    public GameObject empBtnRef;

    public Color[] deptBtnTextColors;
    public Sprite[] deptBtnsSprites;
    public AudioClip btnSound;

    public List<DeptReferences> deptRefs;
    public class DeptReferences
    {
        public GameObject deptBtn;
        public GameObject deptContainerPanel;
        public EmployeeType deptType;
        public int reqLevel;
    }
    int curDeptId = -1;
    int purchasingEmployeeId = 0;
    float hiringPeriod = 0;//In Days
    float hiringPrice;
    public static HiringStaffUIManager instance;

    public override void InitializeTab()
    {
        hiringConfirmPanel.SetActive(false);
        responsePanel.SetActive(false);
        instance = this;
        float _btnCntnrLength = deptBtnsContainer.GetComponent<VerticalLayoutGroup>().padding.top;
        deptRefs = new List<DeptReferences>();
        for (int i = 0; i < EmployeeManager.Instance.departments.Count; i++)
        {
            Departments _dept = EmployeeManager.Instance.departments[i];
            DeptReferences _deptRef = new DeptReferences();
            _deptRef.deptType = _dept.deptEmployeeType;
            _deptRef.deptBtn = Instantiate(deptBtnRef, deptBtnsContainer);
            _deptRef.deptBtn.transform.localScale = Vector3.one;
            _deptRef.deptBtn.GetComponent<Image>().sprite = deptBtnsSprites[0];
            _deptRef.deptBtn.transform.GetChild(0).GetComponent<Text>().text = _dept.deptName;
            _deptRef.deptBtn.transform.GetChild(0).GetComponent<Text>().color = deptBtnTextColors[0];
            _deptRef.reqLevel = _dept.reqLevel;
            bool _isUnlocked = _dept.reqLevel <= PlayerDataManager.instance.playerData.playerLevel;

            _deptRef.deptBtn.transform.GetChild(1).gameObject.SetActive(!_isUnlocked);
            int _k = i;
            _deptRef.deptBtn.GetComponent<Button>().onClick.AddListener(() => OnSelectDept(_k));
            GameObject _panel = Instantiate(deptEmpBtnContainerRef, deptPanelsContainer);
            _panel.transform.localScale = Vector3.one;
            _panel.transform.GetChild(0).gameObject.SetActive(_dept.avlEmployees.Count > 0);
            _panel.transform.GetChild(1).gameObject.SetActive(_dept.avlEmployees.Count <= 0);

            Transform _empBtnsCntnr = _panel.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0);

            for (int j = 0; j < _dept.avlEmployees.Count; j++)
            {
                int _temp = j;
                EmployeeData _emp = _dept.avlEmployees[_temp];
                GameObject _btn = Instantiate(empBtnRef, _empBtnsCntnr);
                _btn.transform.localScale = Vector3.one;
                _btn.GetComponent<EmployeeDisplayButton>().SetItemUI(_emp, _temp, _dept.reqLevel);
            }
            _panel.SetActive(false);
            _deptRef.deptContainerPanel = _panel;

            _btnCntnrLength += _deptRef.deptBtn.GetComponent<RectTransform>().sizeDelta.y;
            _btnCntnrLength += deptBtnsContainer.GetComponent<VerticalLayoutGroup>().spacing;
            deptRefs.Add(_deptRef);
        }
    }
    public void OnSelectDept(int _index)
    {
        SoundController.instance.OnPlayInteractionSound(btnSound);
        OnSelectBtn(_index);
        //GameManager.instance.CallFireBase("MngmtTab_" + _index.ToString());
    }
    public void OnpurchaseEmployee(int _index)
    {
        print("This pressed");
        SoundController.instance.OnPlayInteractionSound(btnSound);
        purchasingEmployeeId = _index;
        hiringPeriod = 1;
        hiringConfirmPanel.SetActive(true);
        workerTypeHeading.text = deptRefs[curDeptId].deptType.ToString();
        hiringPeriodText.text = hiringPeriod.ToString();
        hiringPrice =EmployeeManager.Instance.departments[curDeptId].avlEmployees[purchasingEmployeeId].hiringCosts[0].price;
        hiringPriceText.text = (hiringPrice * hiringPeriod).ToString();
    }
    public override void OnSelectBtn(int _index)
    {
        if (_index == curDeptId)
        {
            Transform _empCntnr = deptRefs[curDeptId].deptContainerPanel.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0);
            for (int j = 0; j < _empCntnr.childCount; j++)
            {
                int _temp = j;
                _empCntnr.GetChild(j).GetComponent<EmployeeDisplayButton>().UpdateItemUI();
            }
            return;
        }
        if (curDeptId >= 0)
        {
            deptRefs[curDeptId].deptContainerPanel.SetActive(false);
            deptRefs[curDeptId].deptBtn.GetComponent<Image>().sprite = deptBtnsSprites[0];
            deptRefs[curDeptId].deptBtn.transform.GetChild(0).GetComponent<Text>().color = deptBtnTextColors[0];
        }
        curDeptId = _index;
        deptRefs[curDeptId].deptContainerPanel.SetActive(true);
        deptRefs[curDeptId].deptBtn.GetComponent<Image>().sprite = deptBtnsSprites[1];
        deptRefs[curDeptId].deptBtn.transform.GetChild(0).GetComponent<Text>().color = deptBtnTextColors[1];
        hiringConfirmPanel.SetActive(false);
        responsePanel.SetActive(false);
        Transform _empBtnsCntnr = deptRefs[curDeptId].deptContainerPanel.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0);
        for (int j = 0; j < _empBtnsCntnr.childCount; j++)
        {
            int _temp = j;
            _empBtnsCntnr.GetChild(j).GetComponent<EmployeeDisplayButton>().UpdateItemUI();
        }
    }
    public override void ResetPopUpPanels()
    {
        hiringConfirmPanel.SetActive(false);
        responsePanel.SetActive(false);
    }
    public override void SetMangementTabBtns()
    {
        print("Departments+++++++++" + deptRefs.Count);
        print("this is main dept ref" + deptRefs);
        for(int i = 0; i < deptRefs.Count; i++)
        {
            print("this is dept ref  0 " + deptRefs[i]);
            print("this is dept ref 1 " + deptRefs[1]);
            print("Inside the Loop *********");
            print("this is +++++ reqLevel " + deptRefs[i].reqLevel);
            print("this is +++++++++ " + PlayerDataManager.instance.playerData.playerLevel);
           
            bool _isUnlocked = deptRefs[i].reqLevel <= PlayerDataManager.instance.playerData.playerLevel;
            
            print("Inside the Loop *********  1");
            deptRefs[i].deptBtn.transform.GetChild(1).gameObject.SetActive(!_isUnlocked);
            print("Inside the Loop *********  2");
        }
    }
    public override void OnSetPanelBtns()
    {
        if(curDeptId<0 || curDeptId >= deptRefs.Count)
        {
            return;
        }
        Transform _empBtnsCntnr = deptRefs[curDeptId].deptContainerPanel.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0);
        for (int j = 0; j < _empBtnsCntnr.childCount; j++)
        {
            _empBtnsCntnr.GetChild(j).GetComponent<EmployeeDisplayButton>().UpdateItemUI();
        }
    }
    public void OnChangeHiringPeriod(float _val)
    {
        SoundController.instance.OnPlayInteractionSound(btnSound);

        hiringPeriod += _val;
        if (hiringPeriod < 1)
            hiringPeriod = 1;
        hiringPeriodText.text = hiringPeriod.ToString();

        hiringPriceText.text = (hiringPrice * hiringPeriod).ToString();
    }
    public void OnConfirmPurchase()
    {
        SoundController.instance.OnPlayInteractionSound(btnSound);
        EmployeeType _type = deptRefs[curDeptId].deptType;
        switch (_type)
        {
            case EmployeeType.Cashier:
                if (!GameController.instance.gameData.superStoreOpenStatus)
                {
                    UIController.instance.DisplayInstructions("Open Super Store To Hire Cashier");
                   // GameManager.instance.CallFireBase("HirTryBfStrOpn");
                    return;
                }
                break;
            //case EmployeeType.Receptionist:

            //    if (!GameController.instance.gameData.motelOpenStatus)
            //    {
            //        UIController.instance.DisplayInstructions("Open motel To Hire Receptionist");
            //       // GameManager.instance.CallFireBase("HirTryBfMotelOpn");
            //        return;
            //    }
            //    break;
            //case EmployeeType.FuelAttendants:
            //    if (!GameController.instance.gameData.stationOpenStatus)
            //    {
            //        UIController.instance.DisplayInstructions("Open Gas Station To Hire Fuel Attendant");
            //        //GameManager.instance.CallFireBase("HirTryBfGsStatnOpn");
            //        return;
            //    }
            //    break;
        }
        if (PlayerDataManager.instance.playerData.playerCash < (hiringPrice * hiringPeriod))
        {
           // GameManager.instance.CallFireBase("NoCshEmp_" + curDeptId.ToString() + "_" + purchasingEmployeeId.ToString());
            UIController.instance.EnableNoCashPanel();
            hiringConfirmPanel.SetActive(false);
            return;
        }

        int _workplaceIndex = EmployeeManager.Instance.GetWorkplaceIndex(curDeptId);
        responsePanel.SetActive(true);

        if (_workplaceIndex < 0)
        {
            responseText.text = "You do not have any counter for additional Employees!";
           // GameManager.instance.CallFireBase("NoCntrEmp_" + curDeptId.ToString()+"_" + purchasingEmployeeId.ToString());

            return;
        }
        int _empLev= EmployeeManager.Instance.departments[curDeptId].avlEmployees[purchasingEmployeeId].unlockLevel;
        if (_empLev > PlayerDataManager.instance.playerData.playerLevel)
        {
           // GameManager.instance.CallFireBase("EmpUnlkBfLev_" + curDeptId.ToString() + "_" + purchasingEmployeeId.ToString());
            responseText.text = "Need Level " + _empLev.ToString() + " to Unlock";
            return;
        }

        responseText.text = "Employee Hired Successfully!";

        //UIController.instance.DisplayInstructions("Employee Hired!");
        EmployeeManager.Instance.SpawnNewEmployee(curDeptId, purchasingEmployeeId, _workplaceIndex, hiringPeriod);
        PlayerDataManager.instance.UpdateCash(-1 * ((int)(hiringPrice * hiringPeriod)));
        UIController.instance.UpdateCurrency(-1 * ((int)(hiringPrice * hiringPeriod)));
        hiringConfirmPanel.SetActive(false);
       // GameManager.instance.CallFireBase("Cshr_" + curDeptId.ToString() + "_" + purchasingEmployeeId.ToString() + "_Hrd");
    }
    public void OnCloseConfirmPanel()
    {
        SoundController.instance.OnPlayInteractionSound(btnSound);
        hiringConfirmPanel.SetActive(false);
    }
    public void CloseResponsePanel()
    {
        SoundController.instance.OnPlayInteractionSound(btnSound);
        responsePanel.SetActive(false);
    }
}
