using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CandyCoded.HapticFeedback;

public class MainMenu : MonoBehaviour
{
    public Text CashText;
    public Text rvRewardText;
    public Text qltyInsText;
    public GameObject qualityPanel;
    public Toggle[] qltyToggles;
    [Header("Sound")]
    public AudioClip uiButtonSound;
    public GameObject rvResponseContianer;
    public static MainMenu instance;
    private void Awake()
    {
        instance = this;

    }
    private void Start()
    {
        StartCoroutine(GameManager.instance.InitializeGameData());
    }

    public void InitializeMenu()
    {
        CashText.text = PlayerDataManager.instance.playerData.playerCash.ToString() + "$";
        rvResponseContianer.SetActive(false);
        rvRewardText.text = "+"+StoreManager.Instance.GetCashRVAmount().ToString();
        GameManager.instance.EnableLoadingScreen(false);
        qualityPanel.SetActive(false);
        if (!PlayerDataManager.instance.playerData.qualityPnlDisplayed)
        {
            DisplayInitialQualityPanel();
        }
    }
    public void DisplayInitialQualityPanel()
    {
        PlayerDataManager.instance.playerData.qualityPnlDisplayed = true;
        qualityPanel.SetActive(true);
        qltyInsText.text = "We recommend the " + ((PlayerDataManager.instance.playerData.selectedQualitySettings == 1) ? "Low" :
            ((PlayerDataManager.instance.playerData.selectedQualitySettings == 2) ? "Medium" : "High")) + " quality settings for your device";

        for (int i = 0; i < qltyToggles.Length; i++)
        {
            qltyToggles[i].isOn = (i == (PlayerDataManager.instance.playerData.selectedQualitySettings - 1));
        }
    }
    public void OnCloseQltyPnael()
    {
        qualityPanel.SetActive(false);
        int _qltyIndex = PlayerDataManager.instance.playerData.selectedQualitySettings;
        for (int i = 0; i < qltyToggles.Length; i++)
        {
           if(qltyToggles[i].isOn)
            {
                _qltyIndex = i;
                break;
            }
        }
        if ((_qltyIndex+1) == PlayerDataManager.instance.playerData.selectedQualitySettings)
            return;
        GameManager.instance.OnChangeQualityFromInitialPanel(_qltyIndex);
    }
    public void OnDiscordLinkPressed()
    {
        GameManager.instance.OnDiscordLinkPressed();
    }
    public void OnPressPlay()
    {
        SoundController.instance.OnPlayInteractionSound(uiButtonSound);
        GameManager.instance.EnableLoadingScreen(true,"Please Wait...");
        SceneManager.LoadScene(1);
        GameManager.instance.CallFireBase("OnPlay", "Play", 1);
        AdsMediation.AdsMediationManager.instance.ShowInterstitial();

    }
    public void OnPressSettingBtn()
    {
        GameManager.instance.OnPressSettings();
    }
    public void OnPressRV()
    {
        SoundController.instance.OnPlayInteractionSound(uiButtonSound);
        AdsMediation.AdsMediationManager.instance.ShowRewardedVideo(OnAdSuccess, OnAdFailed);
        GameManager.instance.CallFireBase("MainScrn_RV", "MainScrn_Rv", 1);
    }
    void OnAdSuccess(string _msg)
    {
        PlayerDataManager.instance.UpdateCash(StoreManager.Instance.GetCashRVAmount());
        CashText.text = PlayerDataManager.instance.playerData.playerCash.ToString();

        rvResponseContianer.SetActive(true);
        rvResponseContianer.transform.GetChild(0).GetComponent<LocalizeText>().UpdateText(_msg);
        rvResponseContianer.transform.GetChild(0).GetComponent<Text>().text = "Reward Granted";
        Invoke("DisableRvResponseText", 2f);
        GameManager.instance.CallFireBase("successRVAd_main", "successRVAd", 1);
    }
    void OnAdFailed(string _msg)
    {
        rvResponseContianer.SetActive(true);
        rvResponseContianer.transform.GetChild(0).GetComponent<LocalizeText>().UpdateText(_msg);
        rvResponseContianer.transform.GetChild(0).GetComponent<Text>().text = _msg;
        Invoke(nameof(DisableRvResponseText), 2f);
        GameManager.instance.CallFireBase("failRVAd_main", "failRVAd", 1);
    }
    public void DisplayResponseText(string _msg)
    {
        rvResponseContianer.SetActive(true);
        rvResponseContianer.transform.GetChild(0).GetComponent<LocalizeText>().UpdateText(_msg);
        rvResponseContianer.transform.GetChild(0).GetComponent<Text>().text = _msg;
        Invoke("DisableRvResponseText", 2f);

    }
    void DisableRvResponseText()
    {
        rvResponseContianer.SetActive(false);
    }

    public void LightVibration()
    {
        Debug.Log("Light Vibration");
        HapticFeedback.LightFeedback();
    }

    public void MediumVibration()
    {
        Debug.Log("Medium Vibration");
        HapticFeedback.MediumFeedback();
    }

    public void HardVibration()
    {
        Debug.Log("Hard Vibration");
        HapticFeedback.HeavyFeedback();
    }
    public void CrimeGameLink()
    {
        GameManager.instance.CallFireBase("CrimeAdClicked");
#if UNITY_EDITOR
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.peri.games.crime.scene.evidence.cleaner");
#elif UNITY_ANDROID
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.peri.games.crime.scene.evidence.cleaner");
#elif UNITY_IPHONE
        Application.OpenURL("https://apps.apple.com/us/app/crime-scene-cleaner-for-mafia/id6504433331");
#endif
    }
}
