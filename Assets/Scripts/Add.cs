using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using GameAnalyticsSDK;

public class Add : MonoBehaviour
{

    void Awake()
    {
        GameAnalytics.Initialize();

        if (FB.IsInitialized) // Fb sdk initilaze
            FB.ActivateApp();
        else
            FB.Init(() => { FB.ActivateApp(); });
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus)
        {
            if (FB.IsInitialized)
                FB.ActivateApp();
            else
                FB.Init(() => { FB.ActivateApp(); });
        }
    }
}
