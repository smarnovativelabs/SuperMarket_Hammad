using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelUpUiManager : MonoBehaviour
{
    public static LevelUpUiManager Instance;
    public Image levelUpPanel;
    public GameObject noThankxButton;
    public GameObject levelUpVFX;
    public GameObject canvas;
    public Text currentLevelupText;
    public Text BlitzRewardText;
    public Text CashRewardText;
   // Canvas canvasComponent;
    public int currancyEarned;
    public int blitzEarned;
    public Button doubleRewardButton;
    public void Awake()
    {
        Instance = this;
      //  canvasComponent = canvas.GetComponent<Canvas>();
    }
    /// <summary>
    /// Show level up panel and display no thankx button after desitred time
    /// </summary>
    public void ShowLevelUpPanel()
    {
        LevelManager.Instance.PlayLevelUpSound();
        levelUpPanel.gameObject.SetActive(true);

       // canvasComponent.renderMode = RenderMode.ScreenSpaceCamera;

        SetDataOnLevelUp();

        if (levelUpVFX != null)
        {
            levelUpVFX.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();
            levelUpVFX.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().Play();
            levelUpVFX.transform.GetChild(0).GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();
            levelUpVFX.transform.GetChild(1).GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();
        }


        Invoke("ShowNoThankx",2.0f);
        
    }

  
    public void HideLevelUpPanel()
    {
        levelUpPanel.gameObject.SetActive(false);
      //  canvasComponent.renderMode = RenderMode.ScreenSpaceOverlay;
        //show reward animation in UI
       // UIController.instance.UpdateBlitz(blitzEarned);
      //  UIController.instance.UpdateCurrency(currancyEarned);
        UIController.instance.UpdateMotelLevelAndXPBar();
        noThankxButton.gameObject.SetActive(false);
    }

    /// <summary>
    /// In this method we are setting level up reward earned ny the user and also max level reward
    /// </summary>
    void SetDataOnLevelUp()
    {
        doubleRewardButton.interactable = true;
        currentLevelupText.text ="LEVEL "+ PlayerDataManager.instance.playerData.playerLevel.ToString();
        //set reward here
        if (PlayerDataManager.instance.playerData.playerLevel < LevelManager.Instance.levels.Length) {

            int _currentLevel = PlayerDataManager.instance.playerData.playerLevel - 1;
            BlitzRewardText.text = LevelManager.Instance.levels[_currentLevel].blitzReward.ToString();
            CashRewardText.text = "$" + LevelManager.Instance.levels[_currentLevel].cashReward;
            PlayerDataManager.instance.UpdateCash(LevelManager.Instance.levels[_currentLevel].cashReward);
            PlayerDataManager.instance.UpdateBlitz(LevelManager.Instance.levels[_currentLevel].blitzReward);
            currancyEarned = LevelManager.Instance.levels[_currentLevel].cashReward;
            blitzEarned= LevelManager.Instance.levels[_currentLevel].blitzReward;
        }
        else
        {
            //Reward user a fix value

            int _maxLevelCash = 10 * (PlayerDataManager.instance.playerData.playerLevel-1);
            int _maxLevelBlitz = PlayerDataManager.instance.playerData.playerLevel;
            

           // BlitzRewardText.text = _maxLevelBlitz.ToString();
            CashRewardText.text = "$" + _maxLevelCash;
            BlitzRewardText.text = _maxLevelBlitz.ToString();
            PlayerDataManager.instance.UpdateCash(_maxLevelCash);
            PlayerDataManager.instance.UpdateBlitz(_maxLevelBlitz);
            currancyEarned = _maxLevelCash;
            blitzEarned = _maxLevelBlitz;
        }

        UIController.instance.UpdateBlitz(blitzEarned);
        UIController.instance.UpdateCurrency(currancyEarned);
    }


    #region DoubleReward

    public void OnDoubleLevelUpReward()
    {
        doubleRewardButton.interactable = false;
        AdsMediation.AdsMediationManager.instance.ShowRewardedVideo(OnAdSuccess, OnAdFailed);
    }

    void OnAdSuccess(string msg)
    {
      UIController.instance.DisplayInstructions("2x Reward Has been Aded");
      PlayerDataManager.instance.UpdateCash(currancyEarned);
      PlayerDataManager.instance.UpdateBlitz(blitzEarned);
      //for only display purpose
      //currancyEarned = currancyEarned*2;
      //blitzEarned = blitzEarned * 2;
      BlitzRewardText.text = blitzEarned.ToString();
      CashRewardText.text = "$" + currancyEarned;

        //show reward animation in UI
        UIController.instance.UpdateBlitz(blitzEarned);
        UIController.instance.UpdateCurrency(currancyEarned);

        HideLevelUpPanel();

    }

    void OnAdFailed(string msg)
    {
        doubleRewardButton.interactable = true;
        UIController.instance.DisplayInstructions(msg);
    }

    #endregion

    void ShowNoThankx()
    {
        noThankxButton.gameObject.SetActive(true);
    }


}
