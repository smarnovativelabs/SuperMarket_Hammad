using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagementTabUIManager : MonoBehaviour
{
    public ManagementTabs[] managementTabs;
    public AudioClip btnSound;
    public static ManagementTabUIManager instance;
    int currentSelectedTab = -1;
    private void Awake()
    {
        instance = this;
    }
    public IEnumerator InitializeManagementTabs()
    {
        for(int i = 0; i < managementTabs.Length; i++)
        {
            managementTabs[i].tabUI.InitializeTab();
            managementTabs[i].ResetTab();
            if (i % 3 == 2)
            {
                yield return null;
            }
        }
   //     SelectManagementTab(0);
    }
    void SelectManagementTab(int _id)
    {
        if (_id == currentSelectedTab)
        {
            OnLeveUpgrade();
            return;
        }
        print("This is currentSelectedTab" + currentSelectedTab);
        if (currentSelectedTab >= 0)
        {
            managementTabs[currentSelectedTab].ResetTab();
        }
        currentSelectedTab = _id;
        managementTabs[currentSelectedTab].SelectTab();
        managementTabs[currentSelectedTab].tabUI.ResetPopUpPanels();
        managementTabs[currentSelectedTab].tabUI.SetMangementTabBtns();
        managementTabs[currentSelectedTab].tabUI.OnSelectBtn(0);
    }
    public void OnPressManagementTab(int _id)
    {
        SoundController.instance.OnPlayInteractionSound(btnSound);
        SelectManagementTab(_id);
    }
    public void OnLeveUpgrade()
    {
        if (currentSelectedTab >= 0)
        {
            managementTabs[currentSelectedTab].tabUI.SetMangementTabBtns();
            managementTabs[currentSelectedTab].tabUI.OnSetPanelBtns();
        }
    }
}
[System.Serializable]
public class ManagementTabs
{
    public string tabName;
    public GameObject tabPanel;
    public Image tabBtn;
    public ManagementTabUI tabUI;
    public Text tabNameText;
    public Color[] tabNameColors;
    public Sprite[] tabSprites;

    public void ResetTab()
    {
        tabNameText.text = tabName;
        tabBtn.GetComponent<Image>().sprite = tabSprites[0];
        tabNameText.color = tabNameColors[0];
        tabPanel.SetActive(false);
        tabBtn.GetComponent<Button>().interactable = true;
    }
    public void SelectTab()
    {
        tabBtn.GetComponent<Image>().sprite = tabSprites[1];
        tabNameText.color = tabNameColors[1];
        tabPanel.SetActive(true);
        tabBtn.GetComponent<Button>().interactable = true;
    }
}