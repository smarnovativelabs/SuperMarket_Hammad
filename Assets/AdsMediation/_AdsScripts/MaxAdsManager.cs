using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdsMediation
{
    public class MaxAdsManager : MonoBehaviour
    {
        private const string MaxSdkKey = "TRg6tE6Jqd63wDA1Egi746kgkf6hBDmP1x3ddKVkxqed_NUoJtPlTFEBu1FT03HiToJf035dbl7ZG8wrgvuCWc";
        [Header("IOS Max Ads Ids")]
        public string iosInterstitialAdUnitId = "ENTER_IOS_INTERSTITIAL_AD_UNIT_ID_HERE";
        public string iosRewardedAdUnitId = "ENTER_IOS_REWARD_AD_UNIT_ID_HERE";
        public string iosRewardedInterstitialAdUnitId = "ENTER_IOS_REWARD_INTER_AD_UNIT_ID_HERE";
        public string iosBannerAdUnitId = "ENTER_IOS_BANNER_AD_UNIT_ID_HERE";
        public string iosMRecAdUnitId = "ENTER_IOS_MREC_AD_UNIT_ID_HERE";

        [Header("Android Max Ads Ids")]
        public string androidInterstitialAdUnitId = "ENTER_ANDROID_INTERSTITIAL_AD_UNIT_ID_HERE";
        public string androidRewardedAdUnitId = "ENTER_ANDROID_REWARD_AD_UNIT_ID_HERE";
        public string androidRewardedInterstitialAdUnitId = "ENTER_ANDROID_REWARD_INTER_AD_UNIT_ID_HERE";
        public string androidBannerAdUnitId = "ENTER_ANDROID_BANNER_AD_UNIT_ID_HERE";
        public string androidMRecAdUnitId = "ENTER_ANDROID_MREC_AD_UNIT_ID_HERE";


        string InterstitialAdUnitId;
        string RewardedAdUnitId;
        string RewardedInterstitialAdUnitId;
        string BannerAdUnitId;
        string MRecAdUnitId;
        private bool isBannerShowing;
        private bool isMRecShowing;

        private int interstitialRetryAttempt;
        private int rewardedRetryAttempt;
        private int rewardedInterstitialRetryAttempt;

        bool rewardedInterReceived = false;
        bool rvRewardReceived = false;
        public bool isInitialized = false;
        AdsMediationManager.RewardedVieoResponse rvSuccessResponse;
        AdsMediationManager.RewardedVieoResponse rvFailureResponse;

        AdsMediationManager.RewardedVieoResponse rewardedInterSuccess;
        AdsMediationManager.RewardedVieoResponse rewardedInterFailure;


        public void InitializeAds(bool _loadAds = true)
        {
            if (isInitialized)
                return;
            MaxSdkCallbacks.OnSdkInitializedEvent += sdkConfiguration =>
            {
                TenjinManager.instance.SubscibeApplovinImpressions();
                // AppLovin SDK is initialized, configure and start loading ads.
                Debug.Log("MAX SDK Initialized");
                isInitialized = true;
               // InitializeBannerAds();

                //   MaxSdk.ShowMediationDebugger();
                InitializeInterstitialAds();
                InitializeRewardedAds();
                //  InitializeRewardedInterstitialAds();

                // Initialize Adjust SDK
                //AdjustConfig adjustConfig = new AdjustConfig("YourAppToken", AdjustEnvironment.Sandbox);
                //Adjust.start(adjustConfig);
            };

            MaxSdk.SetSdkKey(MaxSdkKey);
            //string[] _test = new string[] { "cb077989-c8c6-43d2-94bc-bac44c541fc5" };
            //MaxSdk.SetTestDeviceAdvertisingIdentifiers(_test);

            MaxSdk.InitializeSdk();
        }
        private void Awake()
        {
            InterstitialAdUnitId = androidInterstitialAdUnitId;
#if UNITY_IPHONE
            InterstitialAdUnitId = iosInterstitialAdUnitId;
#endif
            RewardedAdUnitId = androidRewardedAdUnitId;
#if UNITY_IPHONE
            RewardedAdUnitId = iosRewardedAdUnitId;
#endif
            BannerAdUnitId = androidBannerAdUnitId;
#if UNITY_IPHONE
            BannerAdUnitId = iosBannerAdUnitId;
#endif
            RewardedInterstitialAdUnitId = androidRewardedInterstitialAdUnitId;
#if UNITY_IPHONE
            RewardedInterstitialAdUnitId = iosRewardedInterstitialAdUnitId;
#endif
        }
        void Start()
        {


        }

        #region Interstitial Ad Methods

        private void InitializeInterstitialAds()
        {

            if (string.IsNullOrEmpty(InterstitialAdUnitId) || AdsMediationManager.instance.isAdsRemoved)
            {
                return;

            }
            // Attach callbacks
            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialFailedEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += InterstitialFailedToDisplayEvent;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialDismissedEvent;
            MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnInterstitialRevenuePaidEvent;

            // Load the first interstitial
            print("Requesting Interstitial");
            LoadInterstitial();
        }

        void LoadInterstitial()
        {
            MaxSdk.LoadInterstitial(InterstitialAdUnitId);
        }

        public void ShowInterstitial()
        {
            if (MaxSdk.IsInterstitialReady(InterstitialAdUnitId))
            {
                MaxSdk.ShowInterstitial(InterstitialAdUnitId);
            }
        }
        public bool IsInterstitialLoaded()
        {
            if (!isInitialized)
                return false;

            return MaxSdk.IsInterstitialReady(InterstitialAdUnitId);
        }
        private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Interstitial ad is ready to be shown. MaxSdk.IsInterstitialReady(interstitialAdUnitId) will now return 'true'
            Debug.Log("Interstitial loaded");
            // Reset retry attempt
            interstitialRetryAttempt = 0;
        }

        private void OnInterstitialFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            // Interstitial ad failed to load. We recommend retrying with exponentially higher delays up to a maximum delay (in this case 64 seconds).
            interstitialRetryAttempt++;
            double retryDelay = Mathf.Pow(2, Mathf.Min(7, interstitialRetryAttempt));

            Debug.Log("Interstitial failed to load with error code: " + errorInfo.Code);

            Invoke("LoadInterstitial", (float)retryDelay);
        }

        private void InterstitialFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
        {
            // Interstitial ad failed to display. We recommend loading the next ad
            Debug.Log("Interstitial failed to display with error code: " + errorInfo.Code);
            LoadInterstitial();
        }

        private void OnInterstitialDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Interstitial ad is hidden. Pre-load the next ad
            AdsMediationManager.instance.OnInterClosedForNonRewardedAd();
            Debug.Log("Interstitial dismissed");
            LoadInterstitial();
        }

        private void OnInterstitialRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Interstitial ad revenue paid. Use this callback to track user revenue.
            Debug.Log("Interstitial revenue paid");

            // Ad revenue
            double revenue = adInfo.Revenue;

            // Miscellaneous data
            string countryCode = MaxSdk.GetSdkConfiguration().CountryCode; // "US" for the United States, etc - Note: Do not confuse this with currency code which is "USD"!
            string networkName = adInfo.NetworkName; // Display name of the network that showed the ad (e.g. "AdColony")
            string adUnitIdentifier = adInfo.AdUnitIdentifier; // The MAX Ad Unit ID
            string placement = adInfo.Placement; // The placement this ad's postbacks are tied to

        }

        #endregion

        #region Rewarded Ad Methods

        private void InitializeRewardedAds()
        {
            if (string.IsNullOrEmpty(RewardedAdUnitId))
                return;
            // Attach callbacks
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
            MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdDismissedEvent;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
            MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;

            // Load the first RewardedAd
            LoadRewardedAd();
        }

        private void LoadRewardedAd()
        {
            MaxSdk.LoadRewardedAd(RewardedAdUnitId);
        }

        public void ShowRewardedAd(AdsMediationManager.RewardedVieoResponse _success = null, AdsMediationManager.RewardedVieoResponse _failure = null)
        {
            rvSuccessResponse = _success;
            rvFailureResponse = _failure;

            if (MaxSdk.IsRewardedAdReady(RewardedAdUnitId))
            {
                MaxSdk.ShowRewardedAd(RewardedAdUnitId);
            }
            else
            {
                rvFailureResponse?.Invoke("Rewarded Ad Not Loaded!");
            }

        }
        public bool CanShowRewardedVideo()
        {
            if (!isInitialized)
                return false;
            return MaxSdk.IsRewardedAdReady(RewardedAdUnitId);
        }

        private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Rewarded ad is ready to be shown. MaxSdk.IsRewardedAdReady(rewardedAdUnitId) will now return 'true'
            Debug.Log("Rewarded ad loaded");

            // Reset retry attempt
            rewardedRetryAttempt = 0;
        }

        private void OnRewardedAdFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            // Rewarded ad failed to load. We recommend retrying with exponentially higher delays up to a maximum delay (in this case 64 seconds).
            rewardedRetryAttempt++;
            double retryDelay = Mathf.Pow(2, Mathf.Min(7, rewardedRetryAttempt));

            Debug.Log("Rewarded ad failed to load with error code: " + errorInfo.Code);

            Invoke("LoadRewardedAd", (float)retryDelay);
        }

        private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
        {
            // Rewarded ad failed to display. We recommend loading the next ad
            Debug.Log("Rewarded ad failed to display with error code: " + errorInfo.Code);
            rvFailureResponse?.Invoke("Failed To display Rewarded Ad!");
            LoadRewardedAd();
        }

        private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log("Rewarded ad displayed");
        }

        private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log("Rewarded ad clicked");
        }

        private void OnRewardedAdDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Rewarded ad is hidden. Pre-load the next ad
            Debug.Log("Rewarded ad dismissed");
            LoadRewardedAd();
            StartCoroutine(CheckRVRewardReceive());
        }

        private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
        {
            // Rewarded ad was displayed and user should receive the reward
            Debug.Log("Rewarded ad received reward");
            rvRewardReceived = true;
        }

        IEnumerator CheckRVRewardReceive()
        {
            yield return new WaitForSeconds(0.1f);
            if (rvRewardReceived)
            {
                if (GameController.instance != null)
                {
                    GameController.instance.ResetAdsTimer();
                    AdsMediation.AdsMediationManager.instance.OnInterstitialClosed();
                }
                rvSuccessResponse?.Invoke("Reward Received!");
            }
            else
                rvFailureResponse?.Invoke("Rewarded Video Closed Early!");

            rvRewardReceived = false;
        }

        private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Rewarded ad revenue paid. Use this callback to track user revenue.
            Debug.Log("Rewarded ad revenue paid");

            // Ad revenue
            double revenue = adInfo.Revenue;

            // Miscellaneous data
            string countryCode = MaxSdk.GetSdkConfiguration().CountryCode; // "US" for the United States, etc - Note: Do not confuse this with currency code which is "USD"!
            string networkName = adInfo.NetworkName; // Display name of the network that showed the ad (e.g. "AdColony")
            string adUnitIdentifier = adInfo.AdUnitIdentifier; // The MAX Ad Unit ID
            string placement = adInfo.Placement; // The placement this ad's postbacks are tied to
        }



        #endregion

        #region Banner Ad Methods

        private void InitializeBannerAds()
        {
            if (string.IsNullOrEmpty(BannerAdUnitId) || AdsMediationManager.instance.isAdsRemoved)
                return;
            //// Attach Callbacks
            MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerAdLoadedEvent;
            MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnBannerAdFailedEvent;
            MaxSdkCallbacks.Banner.OnAdClickedEvent += OnBannerAdClickedEvent;
            MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnBannerAdRevenuePaidEvent;

            // Banners are automatically sized to 320x50 on phones and 728x90 on tablets.
            // You may use the utility method `MaxSdkUtils.isTablet()` to help with view sizing adjustments.
            MaxSdk.CreateBanner(BannerAdUnitId, MaxSdkBase.BannerPosition.TopCenter);

            // Set background or background color for banners to be fully functional.
            MaxSdk.SetBannerBackgroundColor(BannerAdUnitId, Color.black);
            DisplayBanner();
        }

        public void DisplayBanner()
        {
            if (!isBannerShowing)
            {
                MaxSdk.ShowBanner(BannerAdUnitId);
            }
            else
            {
                MaxSdk.HideBanner(BannerAdUnitId);
            }
            isBannerShowing = !isBannerShowing;
        }
        public void HideBanner()
        {
            if (isBannerShowing)
                MaxSdk.HideBanner(BannerAdUnitId);
        }
        private void OnBannerAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Banner ad is ready to be shown.
            // If you have already called MaxSdk.ShowBanner(BannerAdUnitId) it will automatically be shown on the next ad refresh.
            Debug.Log("Banner ad loaded");
        }

        private void OnBannerAdFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            // Banner ad failed to load. MAX will automatically try loading a new ad internally.
            Debug.Log("Banner ad failed to load with error code: " + errorInfo.Code);
        }

        private void OnBannerAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log("Banner ad clicked");
        }

        private void OnBannerAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Banner ad revenue paid. Use this callback to track user revenue.
            Debug.Log("Banner ad revenue paid");

            // Ad revenue
            double revenue = adInfo.Revenue;

            // Miscellaneous data
            string countryCode = MaxSdk.GetSdkConfiguration().CountryCode; // "US" for the United States, etc - Note: Do not confuse this with currency code which is "USD"!
            string networkName = adInfo.NetworkName; // Display name of the network that showed the ad (e.g. "AdColony")
            string adUnitIdentifier = adInfo.AdUnitIdentifier; // The MAX Ad Unit ID
            string placement = adInfo.Placement; // The placement this ad's postbacks are tied to

        }

        #endregion


    }
}