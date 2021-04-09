using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

public class SystemInfo
{
    public double curBitcoin; //float을 json에 저장하는 것이 지원이 안 됨
    public UInt64 curMoney;
    public int cntPC;
    public int curGPULevel; //TODO
    public DateTime recentlyTerminatedAt;

    public SystemInfo(double _curBitcoin, UInt64 _curMoney, int _cntPC, int _curGPULevel, DateTime _recentlyTerminatedAt)
    {
        curBitcoin = _curBitcoin;
        curMoney = _curMoney;
        cntPC = _cntPC;
        curGPULevel = _curGPULevel;
        recentlyTerminatedAt = _recentlyTerminatedAt;
    }
}

public class Json : MonoBehaviour
{
    private system scriptSystem;
    private addPC scriptAddPC;

    public SystemInfo systemInfo;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void save()
    {
        if (!scriptSystem) scriptSystem = GameObject.Find("system").GetComponent<system>();

        systemInfo = new SystemInfo(Convert.ToDouble(scriptSystem.curBitcoin), scriptSystem.curMoney, scriptSystem.PCs.Count, scriptSystem.curGPULevel, DateTime.Now);
        JsonData jsonSystemInfo = JsonMapper.ToJson(systemInfo);
        File.WriteAllText(Application.dataPath + "/Resources/SystemInfo.json", jsonSystemInfo.ToString());
    }

    public void load()
    {
        if (!scriptSystem) scriptSystem = GameObject.Find("system").GetComponent<system>();
        if (!scriptAddPC) scriptAddPC = GameObject.Find("EventSystem").GetComponent<addPC>();
        string jsonString = File.ReadAllText(Application.dataPath + "/Resources/SystemInfo.json");

        JsonData jsonSystemInfo = JsonMapper.ToObject(jsonString);
        scriptSystem.curMoney = Convert.ToUInt64(jsonSystemInfo["curMoney"].ToString());
        scriptSystem.curBitcoin = Convert.ToSingle(jsonSystemInfo["curBitcoin"].ToString());
        scriptSystem.curGPULevel = Convert.ToInt16(jsonSystemInfo["curGPULevel"].ToString());

        //PC Load & 시간에 따라 Bitcoin 추가
        DateTime recentlyTerminatedAt = Convert.ToDateTime(jsonSystemInfo["recentlyTerminatedAt"].ToString());
        TimeSpan timeDifference = DateTime.Now - recentlyTerminatedAt;

        for (int i = 0; i < Convert.ToInt16(jsonSystemInfo["cntPC"].ToString()); i++)
        {
            PC scriptNewPC = scriptAddPC.addNewPC();
            scriptSystem.curBitcoin += timeDifference.Seconds * scriptNewPC.bitcoinPerSecond * scriptSystem.GPU_RATES[scriptSystem.curGPULevel];
        }       
    }
}
