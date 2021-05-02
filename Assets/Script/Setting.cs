using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class Setting : MonoBehaviour
{
    public int clickedButton;
    private GameObject settingPanel, settingButton1, settingButton2;

    public Sprite[] soundSprites;
    public Sprite[] switchSprites;

    private Image imageBgmVolume;
    private Image imageSoundEffectVolume;
    private Image imageNotification;
    private AudioSource bgmVolume;

    private GameSystem scriptGameSystem;

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {

    }

    async public void OnClickEvent()
    {
        ShowSetting();
        var task = Task.Run(() => GetClickedButton());
        int clickedButton = await task;
    }

    public void CloseSetting(int index)
    {
        HideYesOrNoMsgbox();
        clickedButton = index;
    }

    public void SetSoundAfterJsonLoad()
    {
        if(!scriptGameSystem) scriptGameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();
        bgmVolume = GameObject.Find("MainCamera").GetComponent<AudioSource>();
        bgmVolume.GetComponent<AudioSource>().volume = 0.03f * Convert.ToSingle(scriptGameSystem.currentBgmVolume);
    }

    public void ShowSetting()
    {
        if (!settingPanel) settingPanel = GameObject.Find("PopupPanels").transform.Find("SettingPanel").gameObject;
        if (!scriptGameSystem) scriptGameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();

        clickedButton = -1;
        settingPanel.SetActive(true);
        
        //bgmVolume
        imageBgmVolume = GameObject.Find("BgmVolumeImage").GetComponent<Image>();
        imageBgmVolume.sprite = soundSprites[scriptGameSystem.currentBgmVolume];

        //soundEffectVolume
        imageSoundEffectVolume = GameObject.Find("SoundEffectVolumeImage").GetComponent<Image>();
        imageSoundEffectVolume.sprite = soundSprites[scriptGameSystem.currentSoundEffectVolume];

        //notificationStatus
        imageNotification = GameObject.Find("NotificationImage").GetComponent<Image>();
        imageNotification.sprite = switchSprites[Convert.ToInt16(scriptGameSystem.currentNotificationStatus)];
    }

    private void HideYesOrNoMsgbox()
    {
        settingPanel.SetActive(false);
    }

    public int GetClickedButton()
    {
        while (clickedButton == -1)
        {

        }
        return clickedButton;
    }

    public void SetBgmVolume()
    {
        if (!scriptGameSystem) scriptGameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();
        if (!imageBgmVolume) imageBgmVolume = GameObject.Find("BgmVolumeImage").GetComponent<Image>();
        if (!bgmVolume) bgmVolume = GameObject.Find("MainCamera").GetComponent<AudioSource>();

        scriptGameSystem.currentBgmVolume += 1;
        if (soundSprites.Length == scriptGameSystem.currentBgmVolume)
        {
            scriptGameSystem.currentBgmVolume = 0;
        }
        bgmVolume.volume = 0.03f * Convert.ToSingle(scriptGameSystem.currentBgmVolume);
        imageBgmVolume.sprite = soundSprites[scriptGameSystem.currentBgmVolume];
    }

    public void SetSoundEffectVolume()
    {
        if (!scriptGameSystem) scriptGameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();
        if (!imageSoundEffectVolume) imageSoundEffectVolume = GameObject.Find("SoundEffectVolumeImage").GetComponent<Image>();

        scriptGameSystem.currentSoundEffectVolume += 1;
        if (soundSprites.Length == scriptGameSystem.currentSoundEffectVolume)
        {
            scriptGameSystem.currentSoundEffectVolume = 0;
        }
        imageSoundEffectVolume.sprite = soundSprites[scriptGameSystem.currentSoundEffectVolume];
    }

    public void SetNotificationStatus()
    {
        if (!scriptGameSystem) scriptGameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();

        if (scriptGameSystem.currentNotificationStatus)
        {
            scriptGameSystem.currentNotificationStatus = false;
            imageNotification.sprite = switchSprites[0];
        }
        else
        {
            scriptGameSystem.currentNotificationStatus = true;
            imageNotification.sprite = switchSprites[1];
        }
    }
}
