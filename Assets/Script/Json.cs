using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Security.Cryptography;
using LitJson;
using UnityEngine.UI;

public class SystemInfo
{
    public double currentBtc; //float�� json�� �����ϴ� ���� ������ �� ��
    public UInt64 currentMoney;
    public int pcCount;
    public int currentGpuLevel;
    public DateTime recentlyTerminatedAt;
    public double currentOverclockLevel;
    public int currentBgmVolume;
    public int currentSoundEffectVolume;
    public Boolean currentNotificationStatus;

    public SystemInfo(double _currentBtc, UInt64 _currentMoney, int _pcCount, int _currentGpuLevel, DateTime _recentlyTerminatedAt, double _currentOverclockLevel, int _currentBgmVolume, int _currentSoundEffectVolume, Boolean _currentNotificationStatus)
    {
        currentBtc = _currentBtc;
        currentMoney = _currentMoney;
        pcCount = _pcCount;
        currentGpuLevel = _currentGpuLevel;
        recentlyTerminatedAt = _recentlyTerminatedAt;
        currentOverclockLevel = _currentOverclockLevel;
        currentBgmVolume = _currentBgmVolume;
        currentSoundEffectVolume = _currentSoundEffectVolume;
        currentNotificationStatus = _currentNotificationStatus;
    }
}

public class Json : MonoBehaviour
{
    private static readonly string privateKey = "v8ntd6eqqcdxocqzovuq18m86v0nif03h2rtmjs";

    private GameSystem scriptGameSystem;
    private PcPanel scriptPcPanel;
    private Setting scriptSetting;

    public SystemInfo systemInfo;

    GameObject loadedBtcPanel;
    public Image loadedBtcBackgroundImageImage;
    public Text loadedBtcTextText;


    private string GetEncryptedString(string data)
    {
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(data);
        RijndaelManaged rm = CreateRijndaelManaged();
        ICryptoTransform ct = rm.CreateEncryptor();
        byte[] results = ct.TransformFinalBlock(bytes, 0, bytes.Length);
        return System.Convert.ToBase64String(results, 0, results.Length);
    }

    private string GetDecryptedString(string data)
    {
        byte[] bytes = System.Convert.FromBase64String(data);
        RijndaelManaged rm = CreateRijndaelManaged();
        ICryptoTransform ct = rm.CreateDecryptor();
        byte[] resultArray = ct.TransformFinalBlock(bytes, 0, bytes.Length);
        return System.Text.Encoding.UTF8.GetString(resultArray);
    }

    private RijndaelManaged CreateRijndaelManaged()
    {
        byte[] keyArray = System.Text.Encoding.UTF8.GetBytes(privateKey);
        RijndaelManaged result = new RijndaelManaged();

        byte[] newKeysArray = new byte[16];
        System.Array.Copy(keyArray, 0, newKeysArray, 0, 16);

        result.Key = newKeysArray;
        result.Mode = CipherMode.ECB;
        result.Padding = PaddingMode.PKCS7;
        return result;
    }

    public void Save()
    {
        if (!scriptGameSystem) scriptGameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();

        systemInfo = new SystemInfo(Convert.ToDouble(scriptGameSystem.currentBtc), scriptGameSystem.currentMoney, scriptGameSystem.currentPcList.Count, scriptGameSystem.currentGpuLevel, DateTime.Now, Convert.ToDouble(scriptGameSystem.currentOverclockLevel), scriptGameSystem.currentBgmVolume, scriptGameSystem.currentSoundEffectVolume, scriptGameSystem.currentNotificationStatus);
        JsonData jsonSystemInfo = JsonMapper.ToJson(systemInfo);
        string encryptedData = GetEncryptedString(jsonSystemInfo.ToString());

        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WindowsEditor)
        {
            File.WriteAllText(Application.persistentDataPath + "/SystemInfo.json", encryptedData);
        }
        else
        {
            Debug.Log("Window, Android ���� �÷������� �������� �ʽ��ϴ�.");
        }
    }

    public void Load()
    {
        if (!scriptGameSystem) scriptGameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();
        if (!scriptPcPanel) scriptPcPanel = GameObject.Find("EventSystem").GetComponent<PcPanel>();
        if (!scriptSetting) scriptSetting = GameObject.Find("EventSystem").GetComponent<Setting>();

        try
        {
            string jsonString = null;
            string decryptedData = null;
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WindowsEditor)
            {
                jsonString = File.ReadAllText(Application.persistentDataPath + "/SystemInfo.json");
                decryptedData = GetDecryptedString(jsonString);
            }
            else
            {
                Debug.Log("Window, Android ���� �÷������� �������� �ʽ��ϴ�.");

            }
            JsonData jsonSystemInfo = JsonMapper.ToObject(decryptedData);

            scriptGameSystem.currentMoney = Convert.ToUInt64(jsonSystemInfo["currentMoney"].ToString());
            scriptGameSystem.currentBtc = Convert.ToSingle(jsonSystemInfo["currentBtc"].ToString());
            scriptGameSystem.currentGpuLevel = Convert.ToInt16(jsonSystemInfo["currentGpuLevel"].ToString());
            scriptGameSystem.currentOverclockLevel = Convert.ToSingle(jsonSystemInfo["currentOverclockLevel"].ToString());
            scriptGameSystem.currentBgmVolume = Convert.ToInt16(jsonSystemInfo["currentBgmVolume"].ToString());
            scriptGameSystem.currentSoundEffectVolume = Convert.ToInt16(jsonSystemInfo["currentSoundEffectVolume"].ToString());
            scriptGameSystem.currentNotificationStatus = Convert.ToBoolean(jsonSystemInfo["currentNotificationStatus"].ToString());

            //PC Load & �ð��� ���� Btc �߰�
            DateTime recentlyTerminatedAt = Convert.ToDateTime(jsonSystemInfo["recentlyTerminatedAt"].ToString());
            TimeSpan timeDifference = DateTime.Now - recentlyTerminatedAt;

            if (timeDifference > new TimeSpan(scriptGameSystem.MAX_BTC_STORING_HOUR, 0, 0)) //�ִ� 3�ð������� ����
            {
                timeDifference = new TimeSpan(scriptGameSystem.MAX_BTC_STORING_HOUR, 0, 0);
            }

            float btcToGet = 0f;
            for (int i = 0; i < Convert.ToInt16(jsonSystemInfo["pcCount"].ToString()); i++)
            {
                Pc scriptNewPcPanel = scriptPcPanel.AddNewPc();
                btcToGet = (timeDifference.Seconds + timeDifference.Minutes * 60 + timeDifference.Hours * 3600) * scriptNewPcPanel.btcPerSecond * scriptGameSystem.GPU_RATES[scriptGameSystem.currentGpuLevel];
                scriptGameSystem.currentBtc += btcToGet;
            }

            //�����ϴ� ���� �� �������� �˷��ִ� �޽��� ǥ��
            ShowLoadedBtcPanel(btcToGet);

            //�Ҹ� ���ÿ� ���� ����
            scriptSetting.SetSoundAfterJsonLoad();
        }
        catch //������ ���� ��� �Ǵ� �����Ͱ� �߸��� ���
        {
            Save();
        } 
    }

    private void ShowLoadedBtcPanel(float btcToGet)
    {
        loadedBtcPanel = GameObject.Find("PopupPanels").transform.Find("LoadedBtcPanel").gameObject;
        Text loadedBtcPanelText = loadedBtcPanel.transform.Find("LoadedBtcText").GetComponent<Text>();

        loadedBtcPanel.SetActive(true);
        loadedBtcPanelText.text = "�׵��� " + btcToGet.ToString("0." + new string('#', 8)) + "BTC�� ä���Ͽ����ϴ�!";

        loadedBtcBackgroundImageImage = loadedBtcPanel.transform.Find("LoadedBtcBackgroundImage").GetComponent<Image>();
        loadedBtcTextText = loadedBtcPanelText.GetComponent<Text>();

        StartCoroutine(SetLoadedBtcPanelFadeOut(3f, 1f));
    }

    IEnumerator SetLoadedBtcPanelFadeOut(float delay, float duration)
    {
        yield return new WaitForSeconds(delay);

        Color curColor = loadedBtcBackgroundImageImage.color;
        while (curColor.a > 0f) {
            curColor.a -= Time.deltaTime / duration;
            loadedBtcBackgroundImageImage.color = curColor;
            loadedBtcTextText.color = curColor;

            yield return null;
        }

        loadedBtcPanel.SetActive(false);
    }
}
