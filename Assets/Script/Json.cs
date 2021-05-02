using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

public class SystemInfo
{
    public double currentBtc; //float을 json에 저장하는 것이 지원이 안 됨
    public UInt64 currentMoney;
    public int pcCount;
    public int currentGpuLevel;
    public DateTime recentlyTerminatedAt;
    public double currentOverclockLevel;
    public int currentBgmVolume;
    public int currentSoundEffectVolume;

    public SystemInfo(double _currentBtc, UInt64 _currentMoney, int _pcCount, int _currentGpuLevel, DateTime _recentlyTerminatedAt, double _currentOverclockLevel, int _currentBgmVolume, int _currentSoundEffectVolume)
    {
        currentBtc = _currentBtc;
        currentMoney = _currentMoney;
        pcCount = _pcCount;
        currentGpuLevel = _currentGpuLevel;
        recentlyTerminatedAt = _recentlyTerminatedAt;
        currentOverclockLevel = _currentOverclockLevel;
        currentBgmVolume = _currentBgmVolume;
        currentSoundEffectVolume = _currentSoundEffectVolume;
    }
}

public class Json : MonoBehaviour
{
    private GameSystem scriptGameSystem;
    private PcPanel scriptPcPanel;

    public SystemInfo systemInfo;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Save()
    {
        if (!scriptGameSystem) scriptGameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();

        systemInfo = new SystemInfo(Convert.ToDouble(scriptGameSystem.currentBtc), scriptGameSystem.currentMoney, scriptGameSystem.currentPcList.Count, scriptGameSystem.currentGpuLevel, DateTime.Now, Convert.ToDouble(scriptGameSystem.currentOverclockLevel), scriptGameSystem.currentBgmVolume, scriptGameSystem.currentSoundEffectVolume);
        JsonData jsonSystemInfo = JsonMapper.ToJson(systemInfo);
        File.WriteAllText(Application.dataPath + "/Resources/SystemInfo.json", jsonSystemInfo.ToString());
    }

    public void Load()
    {
        if (!scriptGameSystem) scriptGameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();
        if (!scriptPcPanel) scriptPcPanel = GameObject.Find("EventSystem").GetComponent<PcPanel>();
        try
        {
            string jsonString = File.ReadAllText(Application.dataPath + "/Resources/SystemInfo.json");
            JsonData jsonSystemInfo = JsonMapper.ToObject(jsonString);
            scriptGameSystem.currentMoney = Convert.ToUInt64(jsonSystemInfo["currentMoney"].ToString());
            scriptGameSystem.currentBtc = Convert.ToSingle(jsonSystemInfo["currentBtc"].ToString());
            scriptGameSystem.currentGpuLevel = Convert.ToInt16(jsonSystemInfo["currentGpuLevel"].ToString());
            scriptGameSystem.currentOverclockLevel = Convert.ToSingle(jsonSystemInfo["currentOverclockLevel"].ToString());
            scriptGameSystem.currentBgmVolume = Convert.ToInt16(jsonSystemInfo["currentBgmVolume"].ToString());
            scriptGameSystem.currentSoundEffectVolume = Convert.ToInt16(jsonSystemInfo["currentSoundEffectVolume"].ToString());

            //PC Load & 시간에 따라 Btc 추가
            DateTime recentlyTerminatedAt = Convert.ToDateTime(jsonSystemInfo["recentlyTerminatedAt"].ToString());
            TimeSpan timeDifference = DateTime.Now - recentlyTerminatedAt;

            if(timeDifference > new TimeSpan(3, 0, 0)) //최대 3시간까지만 저장
            {
                timeDifference = new TimeSpan(3, 0, 0);
            }

            for (int i = 0; i < Convert.ToInt16(jsonSystemInfo["pcCount"].ToString()); i++)
            {
                Pc scriptNewPcPanel = scriptPcPanel.AddNewPc();
                scriptGameSystem.currentBtc += (timeDifference.Seconds + timeDifference.Minutes*60 + timeDifference.Hours*3600) * scriptNewPcPanel.btcPerSecond * scriptGameSystem.GPU_RATES[scriptGameSystem.currentGpuLevel];
            }
        }
        catch //파일이 없을 경우
        {
            Save();
        } 
    }
}
