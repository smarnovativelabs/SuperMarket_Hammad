using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager instance;
    public bool isInitialized = false;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
        
    }
    private void Start()
    {
        if (!instance.isInitialized)
        {
            InitializeFirebase();
        }
    }
    public void InitializeFirebase()
    {
        Firebase.FirebaseApp app;

        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {

            var dependencyStatus = task.Result;

            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                isInitialized = true;
                app = Firebase.FirebaseApp.DefaultInstance;
                Firebase.Analytics.FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                print("_______****Analytics are ready to use*****_________");

                //initilize remote config after main analytic sdk initlize
                Remoteconfig.instance.InitializeRemoteConfig();
            }
        });
    }
    public void CallFirebasEvent(string eventName)
    {
        if (!isInitialized)
            return;

        string paramName = eventName + "_prm";
        //  print("------" + paramName);
        int _paramValue = 1;

#if !UNITY_EDITOR
        Firebase.Analytics.FirebaseAnalytics.LogEvent(eventName, paramName, _paramValue);
#endif
    }
}