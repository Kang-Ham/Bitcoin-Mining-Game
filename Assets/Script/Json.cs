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
    public List<int> levelOfPCs;

    public SystemInfo(double _curBitcoin, UInt64 _curMoney, List<int> _levelOfPCs)
    {
        curBitcoin = _curBitcoin;
        curMoney = _curMoney;
        levelOfPCs = _levelOfPCs;
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
        List<int> levelOfSystemPCs = new List<int>();
        for (int i = 0; i < scriptSystem.PCs.Count; i++) levelOfSystemPCs.Add(scriptSystem.PCs[i].level);

        systemInfo = new SystemInfo(Convert.ToDouble(scriptSystem.curBitcoin), scriptSystem.curMoney, levelOfSystemPCs);
        JsonData jsonSystemInfo = JsonMapper.ToJson(systemInfo);
        File.WriteAllText(Application.dataPath + "/Resources/SystemInfo.json", jsonSystemInfo.ToString());
    }

    public void load()
    {
        if (!scriptSystem) scriptSystem = GameObject.Find("system").GetComponent<system>();
        string jsonString = File.ReadAllText(Application.dataPath + "/Resources/SystemInfo.json");

        JsonData jsonSystemInfo = JsonMapper.ToObject(jsonString);
        scriptSystem.curBitcoin = Convert.ToSingle(jsonSystemInfo["curBitcoin"].ToString());
        scriptSystem.curMoney = Convert.ToUInt64(jsonSystemInfo["curMoney"].ToString());

        for (int i = 0; i < jsonSystemInfo["levelOfPCs"].Count; i++)
        {
            addPCAndSetLevel(i, (int)(i / 16) + 1, Convert.ToInt16(jsonSystemInfo["levelOfPCs"][i].ToString()));
        } 
    }
                 
    public void addPCAndSetLevel(int index, int pcType, int level)
    {
        if(!scriptAddPC) scriptAddPC = GameObject.Find("EventSystem").GetComponent<addPC>();
        scriptAddPC.addNewPC();
        scriptSystem.PCs[index].level = level;  
    }
}
