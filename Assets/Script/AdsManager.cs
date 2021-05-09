using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour
{
    private const string ANDROID_GAME_ID = "4105315";
    private const string IOS_GAME_ID = "4105314";
    private string placementId = "BannerBMG";
    private bool testMode = true;


    private void Start()
    {
#if UNITY_ANDROID
         Advertisement.Initialize(ANDROID_GAME_ID, testMode);
         StartCoroutine(ShowBannerWhenReady());
#elif UNITY_IOS
        Advertisement.Initialize(IOS_GAME_ID, testMode);
        StartCoroutine(ShowBannerWhenReady());
#endif
    }

    IEnumerator ShowBannerWhenReady()
    {
        while (!Advertisement.IsReady(placementId))
        {
            yield return new WaitForSeconds(0.5f);
        }
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Advertisement.Banner.Show(placementId);
    }
}
