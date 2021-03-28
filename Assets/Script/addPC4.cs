using System.Collections.Generic;
using UnityEngine;
using System;

public class addPC4 : MonoBehaviour
{
    system scriptSystem;
    public GameObject prefabPC;
    private string spriteName = "pc4";
    // Start is called before the first frame update
    void Start()
    {
        scriptSystem = GameObject.Find("system").GetComponent<system>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnClickEvent()
    {
        if (scriptSystem.cntNotebook >= 48 && scriptSystem.cntNotebook <= 63)
        {
            List<int> nextPCPos = getPCPos(scriptSystem.cntNotebook + 1);

            GameObject newPC = Instantiate(prefabPC, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            PC scriptNewPC = newPC.GetComponent<PC>();
            //Set Params
            scriptNewPC.spriteName = spriteName;
            scriptNewPC.pos = nextPCPos;
            scriptSystem.gameBitcoinPerTimeSlice += 0.001f;

            scriptSystem.cntNotebook += 1;
        }
        else
        {
            Debug.Log("ERROR: PC CANNOT BE INSTALLED");
        }
    }

    List<int> getPCPos(int cnt)
    {
        List<int> RET = new List<int>();
        List<int> tablePos = getTablePos(cnt);
        int eachTableIndex = cnt - (tablePos[0] * scriptSystem.NUMBER_OF_PC_AT_EACH_TABLE * scriptSystem.LENGTH_OF_TABLE + tablePos[1] * scriptSystem.NUMBER_OF_PC_AT_EACH_TABLE) - 1;
        RET.Add(tablePos[0]);
        RET.Add(tablePos[1]);
        RET.Add((int)(eachTableIndex / Math.Sqrt(scriptSystem.NUMBER_OF_PC_AT_EACH_TABLE)));
        RET.Add((int)(eachTableIndex % Math.Sqrt(scriptSystem.NUMBER_OF_PC_AT_EACH_TABLE)));
        return RET;
    }

    List<int> getTablePos(int cnt)
    {
        List<int> RET = new List<int>();
        RET.Add((cnt - 1) / (scriptSystem.LENGTH_OF_TABLE * scriptSystem.NUMBER_OF_PC_AT_EACH_TABLE));
        RET.Add(((cnt - 1) % (scriptSystem.LENGTH_OF_TABLE * scriptSystem.NUMBER_OF_PC_AT_EACH_TABLE)) / scriptSystem.NUMBER_OF_PC_AT_EACH_TABLE);
        return RET;
    }
}
