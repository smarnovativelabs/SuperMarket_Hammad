using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class TenjinManager : MonoBehaviour
{
    public static TenjinManager instance;

    public string tenjinAndroidKey;
    public string tenjinIosKey;
    string tenjinKey;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            tenjinKey = tenjinAndroidKey;
#if UNITY_IPHONE
            tenjinKey = tenjinIosKey;
#endif
        }
        else
        {
            Destroy(gameObject);
        }

        instance.TenjinConnect();

    }


    void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus)
        {
            TenjinConnect();
        }
    }


    public void TenjinConnect()
    {
        BaseTenjin instance = Tenjin.getInstance(tenjinKey);
        bool _canConnect = false;
        _canConnect = true;
        //    print("Connnt Called");
#if UNITY_IPHONE
        instance.RegisterAppForAdNetworkAttribution();

        string _version = UnityEngine.iOS.Device.systemVersion;
        int _len = _version.IndexOf(".");
        //   print(_version);
        string _initial = _len > 0 ? _version.Substring(0, _len) : "14";
        float iOSVersion = float.Parse(_initial);
        if (iOSVersion >= 14.0f)
        {
            print("Requesting Tenjin");
            // Tenjin wrapper for requestTrackingAuthorization
            instance.RequestTrackingAuthorizationWithCompletionHandler((status) =>
            {
                Debug.Log("===> App Tracking Transparency Authorization Status: " + status);
                switch (status)
                {
                    case 0:
                        Debug.Log("ATTrackingManagerAuthorizationStatusNotDetermined case");
                        Debug.Log("Not Determined");
                        Debug.Log("Unknown consent");
                        break;
                    case 1:
                        Debug.Log("ATTrackingManagerAuthorizationStatusRestricted case");
                        Debug.Log(@"Restricted");
                        Debug.Log(@"Device has an MDM solution applied");
                        break;
                    case 2:
                        Debug.Log("ATTrackingManagerAuthorizationStatusDenied case");
                        Debug.Log("Denied");
                        Debug.Log("Denied consent");
                        break;
                    case 3:
                        Debug.Log("ATTrackingManagerAuthorizationStatusAuthorized case");
                        Debug.Log("Authorized");
                        Debug.Log("Granted consent");

                        _canConnect = true;

                        break;
                    default:
                        Debug.Log("Unknown");
                        break;
                }
            });
        }
        else
        {
            print("Directly Called Connect");
            instance.Connect();
        }
#elif UNITY_ANDROID
        instance.SetAppStoreType(AppStoreType.googleplay);
        // Sends install/open event to Tenjin
        _canConnect = true;
#endif
        if (_canConnect)
        {
            instance.Connect();
        }
    }

    public void SubscibeApplovinImpressions()
    {
        BaseTenjin instance = Tenjin.getInstance(tenjinKey);
        TenjinConnect();
        instance.SubscribeAppLovinImpressions();
    }
    public void SubscribeAdmobInterstitialImppression(InterstitialAd _inter, string _adUnitId)
    {
        BaseTenjin instance = Tenjin.getInstance(tenjinKey);
        TenjinConnect();
        instance.SubscribeAdMobInterstitialAdImpressions(_inter, _adUnitId);
    }
    public void SubscribeAdmobRewardedVideoImpression(RewardedAd _rv, string _adUnitId)
    {
        BaseTenjin instance = Tenjin.getInstance(tenjinKey);
        TenjinConnect();
        instance.SubscribeAdMobRewardedAdImpressions(_rv, _adUnitId);
    }
    public void SubscribeAdmobBannerImpression(BannerView _banner, string _adUnitId)
    {
        BaseTenjin instance = Tenjin.getInstance(tenjinKey);
        TenjinConnect();
        instance.SubscribeAdMobBannerViewImpressions(_banner, _adUnitId);
    }

    public void CallTenjinEvent(string _eventName)
    {
        BaseTenjin instance = Tenjin.getInstance(tenjinKey);
        instance.SendEvent(_eventName);
    }
    public void OnAndroidPurcahaseTransaction(string _productId, string _currencyCode, int _quantity, double _unitPrice, string _receipt, string _signature)
    {
        BaseTenjin instance = Tenjin.getInstance(tenjinKey);
        instance.Transaction(_productId, _currencyCode, _quantity, _unitPrice, null, _receipt, _signature);
    }
    public void OnIOSPurcahseTransaction(string _productId, string _currencyCode, int _quantity, double _unitPrice, string _transactionId, string _receipt)
    {
        BaseTenjin instance = Tenjin.getInstance("SDK_KEY");
        instance.Transaction(_productId, _currencyCode, _quantity, _unitPrice, _transactionId, _receipt, null);
    }

}
