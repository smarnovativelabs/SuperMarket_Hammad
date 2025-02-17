using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Ump.Api;
using JetBrains.Annotations;

namespace AdsMediation
{
    [RequireComponent(typeof(GoogleMobileAdsConsentController))]
    [RequireComponent(typeof(AdMobController))]
    [RequireComponent(typeof(MaxAdsManager))]
    public class AdsMediationManager : MonoBehaviour
    {
        public bool isAdsRemoved;
        [Header("Delay Between 2 conssecutive Interstitial Ads ")]
        public bool displayAdmobBanner;
        public float interstitialAdDelay;
        public static AdsMediationManager instance;
        GoogleMobileAdsConsentController consentController;
        AdMobController admobAds;
        MaxAdsManager maxAds;

        public delegate void RewardedVieoResponse(string _responseMsg);
        int mediationToUse = -1;
        float interAdDelayTimer;
       // public float interAdDelayTimer1;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                consentController = GetComponent<GoogleMobileAdsConsentController>();
                admobAds = GetComponent<AdMobController>();
                maxAds = GetComponent<MaxAdsManager>();
                interAdDelayTimer = interstitialAdDelay;
                isAdsRemoved = PlayerPrefs.GetInt("RemoveAds", 0) == 1;
                mediationToUse = 1;
            }
            else
            {
                Destroy(gameObject);
            }
            if (mediationToUse >= 0)
            {
                instance.CheckForConsent();
            }
        }
        public void CheckForConsent()
        {
            if (consentController.CanRequestAds)
            {
                InitializeAds();
            }
            else
            {
                InitializeGoogleMobileAdsConsent();
            }
        }
        public void InitializeMediationAds(int _mediationVal)
        {
            mediationToUse = _mediationVal;
            CheckForConsent();
        }
        public bool IsAdsInitialized()
        {
            return (admobAds.isInitialized && maxAds.isInitialized);
        }
        private void InitializeGoogleMobileAdsConsent()
        {
            Debug.Log("Google Mobile Ads gathering consent.");

            consentController.GatherConsent((string error) =>
            {
                if (error != null)
                {
                    Debug.LogError("Failed to gather consent with error: " +
                        error);
                }
                else
                {
                    Debug.Log("Google Mobile Ads consent updated: "
                        + ConsentInformation.ConsentStatus);
                }

                if (consentController.CanRequestAds)
                {
                    print("Initializing Ads!!!!!");
                    InitializeAds();
                }
            });
        }
        public void InitializeAds()
        {
            if (mediationToUse == 0)
            {
                maxAds.InitializeAds();
            }
            else
            {
                //maxAds.InitializeAds();

                admobAds.InitializeAds(true);
            }
        }

        public void ShowInterstitial()
        {
            if (interAdDelayTimer > 0f)
                return;
            interAdDelayTimer = interstitialAdDelay;
            if (isAdsRemoved)
                return;
            if (mediationToUse == 0)
            {
                if (maxAds.IsInterstitialLoaded())
                {
                    maxAds.ShowInterstitial();
                }
            }
            else
            {
                if (admobAds.IsInterstitialLoaded())
                {
                    admobAds.ShowInterstitialAd();
                }
            }

        }

        public void ShowTestInterstitial()
        {
            if (mediationToUse == 0)
            {
                if (maxAds.IsInterstitialLoaded())
                {
                    maxAds.ShowInterstitial();
                }
            }
            else
            {
                if (admobAds.IsInterstitialLoaded())
                {
                    admobAds.ShowInterstitialAd();
                }
            }

        }

        public bool CanShowInterstitial()
        {
            if (isAdsRemoved)
                return false;
            bool _canShow = false;
            _canShow = (interAdDelayTimer <= 0);
            if (_canShow)
            {
                if (mediationToUse == 0)
                {
                    _canShow = maxAds.IsInterstitialLoaded();
                }
                else
                {
                    _canShow = admobAds.IsInterstitialLoaded();
                }
            }
            return _canShow;
        }
        public void RemoveBannerAd()
        {
            admobAds.RemoveBanner();
        }
        public void DisplayBanner()
        {
            if (displayAdmobBanner)
            {
                admobAds.DisplayBanner();
            }
        }
        public void ShowRewardedVideo(RewardedVieoResponse _success = null, RewardedVieoResponse _failure = null)
        {
            if (mediationToUse == 0)
            {
                if (maxAds.CanShowRewardedVideo())
                {
                    maxAds.ShowRewardedAd(_success, _failure);
                   
                }
                else
                {
                    _failure?.Invoke("Ad Not Available!");
                    
                }
            }
            else
            {
                if (admobAds.CanShowRewaredVideo())
                {
                    admobAds.ShowRewardedAd(_success, _failure);
                   
                }
                else
                {
                    _failure?.Invoke("Ad Not Available!");
                  

                }
            }
        }
        public void OnRemoveAds()
        {
            isAdsRemoved = true;
            PlayerPrefs.SetInt("RemoveAds", 1);
            if (admobAds.isInitialized)
                admobAds.DestroyBannerAd();
            if (maxAds.isInitialized)
                maxAds.HideBanner();
        }
        public void HideBannerAdd()
        {
            print("Hid Banner");
            admobAds.DestroyBannerAd();
        }
        public void OnInterstitialClosed()
        {
            interAdDelayTimer = interstitialAdDelay;
            
        }
        public void OnInterClosedForNonRewardedAd()
        {
            print("Happy");
            interAdDelayTimer = interstitialAdDelay;
            GameController.instance.ResetAdsTimer();
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

            interAdDelayTimer -= Time.deltaTime;
           // interAdDelayTimer1 = interAdDelayTimer;
        }
    }
}
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using GoogleMobileAds.Ump.Api;
//using static MaxSdkBase;

//namespace AdsMediation
//{
//    [RequireComponent(typeof(GoogleMobileAdsConsentController))]
//    [RequireComponent(typeof(AdMobController))]
//    [RequireComponent(typeof(MaxAdsManager))]
//    public class AdsMediationManager : MonoBehaviour
//    {
//        public bool isAdsRemoved;
//        [Header("Delay Between 2 conssecutive Interstitial Ads ")]
//        public bool displayAdmobBanner;
//        public float interstitialAdDelay;
//        public static AdsMediationManager instance;
//        GoogleMobileAdsConsentController consentController;
//        AdMobController admobAds;
//        MaxAdsManager maxAds;
//        PluginInitializationStatus initializationStatus;

//        public delegate void RewardedVieoResponse(string _responseMsg);
//        int mediationToUse = -1;
//        float interAdDelayTimer;
//        int interstitialSuccessCounter;
//        bool isAdsInitializationRequesting = false;
//        ConsentFetchStatus consentStatus;
//        enum ConsentFetchStatus
//        {
//            Waiting,
//            Fetching,
//            Fetched,
//            Failed
//        }
//        private void Awake()
//        {
//            if (instance == null)
//            {
//                instance = this;
//                DontDestroyOnLoad(gameObject);
//                consentController = GetComponent<GoogleMobileAdsConsentController>();
//                admobAds = GetComponent<AdMobController>();
//                maxAds = GetComponent<MaxAdsManager>();
//                interAdDelayTimer = interstitialAdDelay;
//                interstitialSuccessCounter = 1;
//                isAdsRemoved = PlayerPrefs.GetInt("RemoveAds", 0) == 1;
//                mediationToUse = 0;
//                initializationStatus = PluginInitializationStatus.Loading;
//                consentStatus = ConsentFetchStatus.Waiting;
//            }
//            else
//            {
//                Destroy(gameObject);
//            }
//        }
//        public void CheckForConsent()
//        {
//            if (consentController.CanRequestAds)
//            {
//                consentStatus = ConsentFetchStatus.Fetched;

//                StartCoroutine(InitializeAds());
//            }
//            else
//            {
//                InitializeGoogleMobileAdsConsent();
//            }
//        }
//        public void InitializeMediationAds(int _mediationVal = 0)
//        {
//            try
//            {
//                mediationToUse = _mediationVal;
//                if (consentStatus == ConsentFetchStatus.Waiting || consentStatus == ConsentFetchStatus.Failed)
//                {
//                    consentStatus = ConsentFetchStatus.Fetching;
//                    CheckForConsent();
//                }
//            }
//            catch (System.Exception ex)
//            {
//                Debug.LogError("Exception occured:" + ex);
//            }
//        }

//        private void InitializeGoogleMobileAdsConsent()
//        {
//            Debug.Log("Google Mobile Ads gathering consent.");

//            consentController.GatherConsent((string error) =>
//            {
//                if (error != null)
//                {
//                    Debug.LogError("Failed to gather consent with error: " +
//                        error);
//                    consentStatus = ConsentFetchStatus.Failed;
//                    Invoke("RetryConsentRequest", 5f);
//                }
//                else
//                {
//                    Debug.Log("Google Mobile Ads consent updated: "
//                        + ConsentInformation.ConsentStatus);
//                }
//                consentStatus = ConsentFetchStatus.Fetched;
//                if (consentController.CanRequestAds)
//                {
//                    print("should Initialize Ads");
//                    StartCoroutine(InitializeAds());
//                }
//            });
//        }
//        void RetryConsentRequest()
//        {
//            if (InternetConnectivity.instance.isInterNetAvailable())
//            {
//                CheckForConsent();
//                return;
//            }

//            Invoke("RetryConsentRequest", 5f);
//        }
//        IEnumerator InitializeAds()
//        {
//            print("Initializing Ads:  " + mediationToUse);
//            if (mediationToUse == 0)
//            {
//                maxAds.InitializeAds(mediationToUse == 0);
//                while (!maxAds.isInitialized)
//                {
//                    yield return new WaitForSeconds(0.1f);
//                }
//                admobAds.InitializeAds(mediationToUse == 1, displayAdmobBanner);
//                while (!admobAds.isInitialized)
//                {
//                    yield return new WaitForSeconds(0.1f);
//                }
//                isAdsInitializationRequesting = false;
//            }
//            else
//            {
//                admobAds.InitializeAds(mediationToUse == 1, displayAdmobBanner);
//                while (!admobAds.isInitialized)
//                {
//                    yield return new WaitForSeconds(0.1f);
//                }

//                maxAds.InitializeAds(mediationToUse == 0);
//                while (!maxAds.isInitialized)
//                {
//                    yield return new WaitForSeconds(0.1f);
//                }

//                isAdsInitializationRequesting = false;
//            }

//        }

//        public void ShowInterstitial()
//        {
//            if (isAdsRemoved)
//                return;

//            GameManager.instance.CallFireBase("InterstitialCalled");
//            print("Ty0"+interAdDelayTimer); 
//            if (interAdDelayTimer > 0f)
//                print("Ty1");
//                return;
//            GameManager.instance.CallFireBase("AdTimeChckPsd");
//            interAdDelayTimer = interstitialAdDelay;
//            print("Ty2");
//            if (mediationToUse == 0)
//            {
//                if (maxAds.IsInterstitialLoaded())
//                {
//                    maxAds.ShowInterstitial();
//                    GameManager.instance.CallFireBase("MaxAdAvl");
//                }
//                else
//                {
//                    GameManager.instance.CallFireBase("MaxAdNotAvl");
//                    if (admobAds.IsInterstitialLoaded())
//                    {
//                        admobAds.ShowInterstitialAd();
//                        GameManager.instance.CallFireBase("AdmobAvl");
//                    }
//                    else
//                    {
//                        GameManager.instance.CallFireBase("AdmobNotAvl");
//                    }
//                }
//            }
//            else
//            {
//                if (admobAds.IsInterstitialLoaded())
//                {
//                    admobAds.ShowInterstitialAd();
//                }
//            }

//        }
//        public bool IsAdsInitialized()
//        {
//            return (admobAds.isInitialized && maxAds.isInitialized);
//        }
//        public bool GetInitializationResponse()
//        {
//            return initializationStatus != PluginInitializationStatus.Loading;
//        }
//        public void ShowTestInterstitial()
//        {
//            if (mediationToUse == 0)
//            {
//                if (maxAds.IsInterstitialLoaded())
//                {
//                    maxAds.ShowInterstitial();
//                }
//            }
//            else
//            {
//                if (admobAds.IsInterstitialLoaded())
//                {
//                    admobAds.ShowInterstitialAd();
//                }
//            }

//        }

//        public bool CanShowInterstitial()
//        {
//            if (isAdsRemoved)
//                return false;
//            bool _canShow = false;
//            _canShow = (interAdDelayTimer <= 0);
//            print("can_Show" + _canShow);
//            if (_canShow)
//            {
//                print("za1");
//                if (mediationToUse == 0)
//                {
//                    print("za2");
//                    _canShow = maxAds.IsInterstitialLoaded();
//                }
//                else
//                {
//                    print("za3");
//                    _canShow = admobAds.IsInterstitialLoaded();
//                }
//            }
//            return _canShow;
//        }
//        public void ShowRewardedVideo(RewardedVieoResponse _success = null, RewardedVieoResponse _failure = null)
//        {
//            if (mediationToUse == 0)
//            {
//                if (maxAds.CanShowRewardedVideo())
//                {
//                    interAdDelayTimer = interstitialAdDelay;
//                    maxAds.ShowRewardedAd(_success, _failure);
//                }
//                else
//                {
//                    _failure?.Invoke("Ad Not Available!");
//                }
//            }
//            else
//            {
//                if (admobAds.CanShowRewaredVideo())
//                {
//                    interAdDelayTimer = interstitialAdDelay;
//                    admobAds.ShowRewardedAd(_success, _failure);
//                }
//                else
//                {
//                    _failure?.Invoke("Ad Not Available!");

//                }
//            }
//        }
//        public void OnRemoveAds()
//        {
//            isAdsRemoved = true;
//            PlayerPrefs.SetInt("RemoveAds", 1);
//            if (admobAds.isInitialized)
//                admobAds.DestroyBannerAd();
//            if (maxAds.isInitialized)
//                maxAds.HideBanner();
//        }
//        public void RemoveBannerAd()
//        {
//            admobAds.RemoveBanner();
//        }
//        public void DisplayBanner()
//        {
//            if (displayAdmobBanner)
//            {
//                admobAds.DisplayBanner();
//            }
//        }
//        bool adCounterUpdating = false;
//        public void OnInterstitialClosed()
//        {
//            if (MainMenu.instance != null)
//            {
//                return;
//            }
//            if (adCounterUpdating)
//                return;
//            adCounterUpdating = true;
//            interstitialSuccessCounter--;
//            Invoke("ResetAdUpdating", 1f);
//            if (interstitialSuccessCounter <= 0)
//            {
//                if (!isAdsRemoved)
//                {
//                    if (IAPUiManager.Instance != null)
//                    {
//                        IAPUiManager.Instance.EnablePromotionalPanel();

//                        interstitialSuccessCounter = 5;

//                    }
//                }
//            }
//        }
//        void ResetAdUpdating()
//        {
//            adCounterUpdating = false;
//        }
//        // Start is called before the first frame update
//        void Start()
//        {

//        }

//        // Update is called once per frame
//        void Update()
//        {

//            interAdDelayTimer -= Time.deltaTime;
//            //print("Interstital ad delay time is running"+interAdDelayTimer);
//        }
//    }
//}
