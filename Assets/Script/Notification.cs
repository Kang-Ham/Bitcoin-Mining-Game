using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Notifications.Android;
using System;

public class Notification : MonoBehaviour
{
    GameSystem scriptGameSystem;
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_ANDROID
        AndroidNotificationCenter.CancelAllDisplayedNotifications();
#endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnApplicationPause(bool pause)
    {
        if (!scriptGameSystem) scriptGameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();

        if (pause)
        {
#if UNITY_ANDROID
            AndroidNotificationCenter.CancelAllNotifications();

            AndroidNotificationChannel notificationChannel = new AndroidNotificationChannel(
                "notification_channel_btc",
                "notification for full-stored",
                "This is a notification channel for the game",
                Importance.High
                );
            AndroidNotificationCenter.RegisterNotificationChannel(notificationChannel);

            DateTime toNotify = DateTime.Now.AddHours(scriptGameSystem.MAX_BTC_STORING_HOUR);
            AndroidNotification notification = new AndroidNotification(
                "컴퓨터가 비트코인을 최대치로 모았습니다.",
                "게임에 접속하여 비트코인을 받아가세요!",
                toNotify
                );

            AndroidNotificationCenter.SendNotification(notification, "notification_channel_btc");
#endif
        }
    }
}
