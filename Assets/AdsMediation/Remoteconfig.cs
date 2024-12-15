//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System;
////using Firebase.Extensions;

////using Firebase.Analytics;
////using Firebase.RemoteConfig;

//public class Remoteconfig : MonoBehaviour
//{
  
//    public static Remoteconfig instance;
//    string mediationFieldName = "mediation";
//    string adsTimerFieldName = "adsTimer";
//    string deliveryModeName = "deliveryMode";
//    string connectivity = "connectivity";
//    string requiredVersion = "requiredVersion";
//    string latestVersion = "latestVersion";
//    string updatepanelmessage = "updateMsg";
//    string rvCashAmount = "rvAmount";

//    //  public Image adsvariations;
//    bool fetchData;

//    void Awake()
//    {
//        if (instance == null)
//        {
//            instance = this;
//            DontDestroyOnLoad(gameObject);
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//    }

//    public void InitializeRemoteConfig()
//    {
//        print("Calling to initialize Remote Configurations!");
//        // [START set_defaults]
//        Dictionary<string, object> defaults = new Dictionary<string, object>();

//        // These are the values that are used if we haven't fetched data from the
//        // server
//        // yet, or if we ask for values that the server doesn't have:

//#if UNITY_IPHONE
//        mediationFieldName = "mediation_ios";
//        adsTimerFieldName = "adsTimer_ios";
//        deliveryModeName = "deliveryMode_ios";
//        requiredVersion = "requiredVersion_ios";
//        latestVersion = "latestVersion_ios";
//        updatepanelmessage = "updateMsg_ios";
//        connectivity = "connectivity_ios";
//        rvCashAmount = "rvvAmount_ios";
//#endif

//        defaults.Add(mediationFieldName, 0);
//        defaults.Add(adsTimerFieldName, 180);
//        defaults.Add(deliveryModeName, 0);
//        defaults.Add(requiredVersion, "nan");
//        defaults.Add(latestVersion, "nan");
//        defaults.Add(updatepanelmessage, "nan");
//        defaults.Add(connectivity, 1);
//        defaults.Add(rvCashAmount, 40);

//        //FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults)
//         // .ContinueWithOnMainThread(task =>
//          //{
//          //    // [END set_defaults]
//          //    Debug.Log("RemoteConfig configured and ready!------------------");
//          //    //  Firebase.RemoteConfig.ConfigSettings=1
//          //    FetchDataAsync();
//          //});
//    }

//    public void OnDataFetched()
//    {
//        Debug.Log("___Remote Config Data_____");
//        //AdsMediation.AdsMediationManager.instance.InitializeMediationAds((int)FirebaseRemoteConfig.DefaultInstance.GetValue(mediationFieldName).LongValue);
//        //GameManager.instance.UpdateInGameAdsTimer(FirebaseRemoteConfig.DefaultInstance.GetValue(adsTimerFieldName).LongValue);
//        //GameManager.instance.UpdateDeliveryModeVal(FirebaseRemoteConfig.DefaultInstance.GetValue(deliveryModeName).LongValue);
//        //Checkforupdate.instance.requiredVersion = FirebaseRemoteConfig.DefaultInstance.GetValue(requiredVersion).StringValue;
//        //Checkforupdate.instance.latestVersion = FirebaseRemoteConfig.DefaultInstance.GetValue(latestVersion).StringValue;
//        //Checkforupdate.instance.updatePanelTextString = FirebaseRemoteConfig.DefaultInstance.GetValue(updatepanelmessage).StringValue;
//        //InternetConnectivity.instance.UpateConnectivityCheckStatus((int)FirebaseRemoteConfig.DefaultInstance.GetValue(connectivity).LongValue);
//        //StoreManager.Instance.OnUpdateRVCashPrice((int)FirebaseRemoteConfig.DefaultInstance.GetValue(rvCashAmount).LongValue);

//    }

//    // [START fetch_async]
//    // Start a fetch request.
//    // FetchAsync only fetches new data if the current data is older than the provided
//    // timespan.  Otherwise it assumes the data is "recent enough", and does nothing.
//    // By default the timespan is 12 hours, and for production apps, this is a good
//    // number. For this example though, it's set to a timespan of zero, so that
//    // changes in the console will always show up immediately.
//    //public System.Threading.Tasks.Task FetchDataAsync()
//    //{
//    //    Debug.Log("Fetching data...");
//    //    System.Threading.Tasks.Task fetchTask =
//    //    Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.FetchAsync(
//    //        TimeSpan.Zero);
//    //    return fetchTask.ContinueWithOnMainThread(FetchComplete);
//    //}



//    private void FetchComplete(System.Threading.Tasks.Task fetchTask)
//    {
//        if (!fetchTask.IsCompleted)
//        {
//            Debug.LogError("Retrieval hasn't finished.");
//            return;
//        }

//       // var remoteConfig = FirebaseRemoteConfig.DefaultInstance;

//        //var info = remoteConfig.Info;

//        if (info.LastFetchStatus != LastFetchStatus.Success)
//        {
//            Debug.LogError($"{nameof(FetchComplete)} was unsuccessful\n{nameof(info.LastFetchStatus)}: {info.LastFetchStatus}");

//            if (!fetchData)
//            {
//                FetchDataAsync();
//                fetchData = true;
//            }
//            else
//            {
//                print("Loading Default Ads!");
//                AdsMediation.AdsMediationManager.instance.InitializeMediationAds(0);

//            }

//            return;
//        }

//        // Fetch successful. Parameter values must be activated to use.
//        remoteConfig.ActivateAsync()
//          .ContinueWithOnMainThread(
//            task =>
//            {
//                Debug.Log($"Remote data loaded and ready for use. Last fetch time {info.FetchTime}.");

//                OnDataFetched();
//            });
//    }

//}
