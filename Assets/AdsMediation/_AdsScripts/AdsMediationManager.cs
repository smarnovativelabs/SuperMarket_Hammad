using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Ump.Api;

namespace AdsMediation
{
    [RequireComponent(typeof(GoogleMobileAdsConsentController))]
    [RequireComponent(typeof(AdMobController))]
    [RequireComponent(typeof(MaxAdsManager))]
    public class AdsMediationManager : MonoBehaviour
    {
        public bool isAdsRemoved;
        [Header("Delay Between 2 conssecutive Interstitial Ads ")]
        public float interstitialAdDelay;
        public static AdsMediationManager instance;
        GoogleMobileAdsConsentController consentController;
        AdMobController admobAds;
        MaxAdsManager maxAds;

        public delegate void RewardedVieoResponse(string _responseMsg);
        int mediationToUse = -1;
        float interAdDelayTimer;
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

                admobAds.InitializeAds();
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
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            interAdDelayTimer -= Time.deltaTime;
        }
    }
}