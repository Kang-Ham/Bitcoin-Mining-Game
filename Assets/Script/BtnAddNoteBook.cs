using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BtnAddNoteBook : MonoBehaviour
{
    system scriptSystem;
    public GameObject prefabPC;
    private string spriteName = "notebook1";
    // Start is called before the first frame update
    void Start()
    {
        scriptSystem = GameObject.Find("system").GetComponent<system>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            if(scriptSystem.cntNotebook>=0 && scriptSystem.cntNotebook <= 63)
            {
                List<int> nextPCPos = getPCPos(scriptSystem.cntNotebook + 1);
                Debug.Log(nextPCPos[0].ToString()+ nextPCPos[1].ToString()+ nextPCPos[2].ToString()+ nextPCPos[3].ToString());
                
                GameObject newPC = Instantiate(prefabPC, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                PC scriptNewPC = newPC.GetComponent<PC>();
                //Set Params
                scriptNewPC.spriteName = spriteName;
                scriptNewPC.pos = nextPCPos;
                scriptNewPC.bitcoinPerSecond = 0.0000000001f;

                scriptSystem.cntNotebook += 1;
            }
            else
            {
                Debug.Log("Error: PC CANNOT Exceed Over 64");
            }
        }
    }
    List<int> getPCPos(int cnt)
    {
        List<int> RET = new List<int>();
        List<int> tablePos = getTablePos(cnt);
        int eachTableIndex = cnt - (tablePos[0] * scriptSystem.NUMBER_OF_PC_AT_EACH_TABLE * scriptSystem.LENGTH_OF_TABLE + tablePos[1] * scriptSystem.NUMBER_OF_PC_AT_EACH_TABLE) - 1;
        RET.Add(tablePos[0]);
        RET.Add(tablePos[1]);
        RET.Add( (int)(eachTableIndex/Math.Sqrt(scriptSystem.NUMBER_OF_PC_AT_EACH_TABLE)) );
        RET.Add( (int)(eachTableIndex%Math.Sqrt(scriptSystem.NUMBER_OF_PC_AT_EACH_TABLE)) );
        return RET;
    }

    List<int> getTablePos(int cnt)
    {
        List<int> RET = new List<int>();
        RET.Add((cnt - 1) / (scriptSystem.LENGTH_OF_TABLE * scriptSystem.NUMBER_OF_PC_AT_EACH_TABLE));
        RET.Add(((cnt - 1) % (scriptSystem.LENGTH_OF_TABLE * scriptSystem.NUMBER_OF_PC_AT_EACH_TABLE))/scriptSystem.NUMBER_OF_PC_AT_EACH_TABLE);
        return RET;
    }
}
