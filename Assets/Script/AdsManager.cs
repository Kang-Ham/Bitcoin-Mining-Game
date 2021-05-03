using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour
{
    private const string ANDROID_GAME_ID = "4105315";
    private const string IOS_GAME_ID = "4105314";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
#if UNITY_ANDROID
        Advertisement.Initialize(ANDROID_GAME_ID, false);
#elif UNITY_IOS
        Advertisement.Initialize(IOS_GAME_ID, false);
#endif        
    }

    public void ShowAds()
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show("video");
        }
        else
        {
            Debug.Log("Advertisement.IsReady() returned False");
        }
    }
}
