using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmployeesUIManager : ManagementTabUI
{
    [Header("Panels/Containers")]
    public Transform deptBtnsContainer;
    public Transform deptPanelsContainer;
    public GameObject firingConfirmPanel;

    [Header("Prefabs")]
    public GameObject deptBtnRef;
    public GameObject deptEmpBtnContainerRef;
    public GameObject empBtnRef;

    public Color[] deptBtnTextColors;
    public Sprite[] deptBtnsSprites;
    public AudioClip btnSound;
    public static EmployeesUIManager instance;
    public List<DeptReferences> deptRefs;
    [System.Serializable]
    public class DeptReferences
    {
        public GameObject deptBtn;
        public GameObject deptContainerPanel;
        public EmployeeType deptType;
    }
    int curDeptId = -1;
    GameObject curSelectedEmpBtn;
    public override void InitializeTab()
    {
        firingConfirmPanel.SetActive(false);
        deptRefs = new List<DeptReferences>();

        List<GameObject> _staff = EmployeeManager.Instance.spawnedEmployees;
        instance = this;
        float _btnCntnrLength = deptBtnsContainer.GetComponent<VerticalLayoutGroup>().padding.top;
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
            int k = i;
            _deptRef.deptBtn.GetComponent<Button>().onClick.AddListener(() => OnSelectDept(k));

            GameObject _panel = Instantiate(deptEmpBtnContainerRef, deptPanelsContainer);
            _panel.transform.localScale = Vector3.one;
            
            Transform _empBtnsCntnr = _panel.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0);

            for (int j = 0; j < EmployeeManager.Instance.spawnedEmployees.Count; j++)
            {
                if (j >= _staff.Count)
                    break;

                int _temp = j;
                EmployeeSavableData _data = _staff[j].GetComponent<Employee>().savableData;

                if (((int)_dept.deptEmployeeType) != _data.employeeType)
                    continue;

                EmployeeData _emp = _dept.avlEmployees[_data.employeeId];

                GameObject _btn = Instantiate(empBtnRef, _empBtnsCntnr);
                _btn.transform.localScale = Vector3.one;
                _btn.GetComponent<EmployeeUIBtn>().InitializeEmployeeBtn(_staff[_temp],_emp, k);
            }
            _panel.transform.GetChild(0).gameObject.SetActive(_empBtnsCntnr.childCount > 0);
            _panel.transform.GetChild(1).gameObject.SetActive(_empBtnsCntnr.childCount <= 0);

            _panel.SetActive(false);
            _deptRef.deptContainerPanel = _panel;
            _btnCntnrLength += _deptRef.deptBtn.GetComponent<RectTransform>().sizeDelta.y;
            _btnCntnrLength += deptBtnsContainer.GetComponent<VerticalLayoutGroup>().spacing;
            deptRefs.Add(_deptRef);
            SetContainerLength(k);
            
        }
        //deptBtnsContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(deptBtnsContainer.GetComponent<RectTransform>().sizeDelta.x, _btnCntnrLength);
    }
    public void SetContainerLength(int _deptIndex,int _addition=0)
    {

        Transform _empBtnsCntnr = deptRefs[_deptIndex].deptContainerPanel.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0);
        deptRefs[_deptIndex].deptContainerPanel.transform.GetChild(0).gameObject.SetActive((_empBtnsCntnr.childCount+_addition) > 0);
        deptRefs[_deptIndex].deptContainerPanel.transform.GetChild(1).gameObject.SetActive((_empBtnsCntnr.childCount + _addition) <= 0);

        //int _count = _empBtnsCntnr.childCount + _addition;

        //float _scrollLength = _empBtnsCntnr.GetComponent<RectTransform>().rect.width;
        //GridLayoutGroup _grid = _empBtnsCntnr.GetComponent<GridLayoutGroup>();
        //_scrollLength -= _grid.padding.left;
        //_scrollLength -= _grid.padding.right;
        //int _perRowItems = (int)(_scrollLength / (_grid.cellSize.x + _grid.spacing.x));

        //if (_scrollLength >= ((_perRowItems * (_grid.cellSize.x + _grid.spacing.x)) + _grid.cellSize.x))
        //{
        //    _perRowItems++;
        //}
        //int _totalRows = Mathf.CeilToInt(((float)_count / _perRowItems));
        //float _height = _totalRows * (_grid.cellSize.y + _grid.spacing.y);
        //_height += _grid.padding.top;
        //_empBtnsCntnr.GetComponent<RectTransform>().sizeDelta = new Vector2(_empBtnsCntnr.GetComponent<RectTransform>().sizeDelta.x, _height);
    }
    public void OnSpawnNewEmployee(EmployeeData _emp,GameObject _employee)
    {
        EmployeeSavableData _data = _employee.GetComponent<Employee>().savableData;
        int _deptIndex = -1;
        for(int i = 0; i < deptRefs.Count; i++)
        {
            if (((int)deptRefs[i].deptType) == _data.employeeType)
            {
                _deptIndex = i;
                break;
            }
        }
        if (_deptIndex < 0)
        {
            print("Invalid Employee-------------");
            return;
        }

        GameObject _btn = Instantiate(empBtnRef, deptRefs[_deptIndex].deptContainerPanel.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0));

        _btn.transform.localScale = Vector3.one;
        _btn.GetComponent<EmployeeUIBtn>().InitializeEmployeeBtn(_employee,_emp, _deptIndex);
        SetContainerLength(_deptIndex);
    }
    public void OnSelectDept(int _index)
    {
        SoundController.instance.OnPlayInteractionSound(btnSound);
        OnSelectBtn(_index);
       // GameManager.instance.CallFireBase("MngmtTab_" + _index.ToString());

    }
    public override void OnSelectBtn(int _index)
    {
        if (_index == curDeptId)
        {
            return;
        }
        if (curDeptId >= 0)
        {
            deptRefs[curDeptId].deptContainerPanel.SetActive(false);
            deptRefs[curDeptId].deptBtn.GetComponent<Image>().sprite = deptBtnsSprites[0];
            deptRefs[curDeptId].deptBtn.transform.GetChild(0).GetComponent<Text>().color = deptBtnTextColors[0];
        }
        print("This is Index in employee " + _index);
        curDeptId = _index;
        deptRefs[curDeptId].deptContainerPanel.SetActive(true);
        deptRefs[curDeptId].deptBtn.GetComponent<Image>().sprite = deptBtnsSprites[1];
        deptRefs[curDeptId].deptBtn.transform.GetChild(0).GetComponent<Text>().color = deptBtnTextColors[1];
        firingConfirmPanel.SetActive(false);
    }
    public override void ResetPopUpPanels()
    {
        firingConfirmPanel.SetActive(false);
    }
    public void EnableConfirmationPanel(GameObject _btnRef)
    {
        SoundController.instance.OnPlayInteractionSound(btnSound);
        curSelectedEmpBtn = _btnRef;
        firingConfirmPanel.SetActive(true);
    }
    public void OnConfirmFire()
    {
        SoundController.instance.OnPlayInteractionSound(btnSound);
        firingConfirmPanel.SetActive(false);
        if (curSelectedEmpBtn != null)
        {
            UIController.instance.DisplayInstructions("Employee Fired");
            curSelectedEmpBtn.GetComponent<EmployeeUIBtn>().OnFireEmployee();
        }
        
    }
    public void OnCloseFirePanel()
    {
        SoundController.instance.OnPlayInteractionSound(btnSound);
        firingConfirmPanel.SetActive(false);
    }
}
