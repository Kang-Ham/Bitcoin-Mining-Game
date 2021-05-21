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

        if (pause && scriptGameSystem.currentNotificationStatus)
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
                "��ǻ�Ͱ� ��Ʈ������ �ִ�ġ�� ��ҽ��ϴ�.",
                "���ӿ� �����Ͽ� ��Ʈ������ �޾ư�����!",
                toNotify
                );

            AndroidNotificationCenter.SendNotification(notification, "notification_channel_btc");
#endif
        }
    }
}
