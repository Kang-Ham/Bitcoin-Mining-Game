using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class addPC2 : MonoBehaviour
{
    system scriptSystem;
    public GameObject prefabPC;
    private msgboxYesOrNo scriptMsgboxYesOrNo;
    private msgbox scriptMsgbox;
    private string spriteName = "pc2";
    // Start is called before the first frame update
    void Start()
    {
        scriptSystem = GameObject.Find("system").GetComponent<system>();
        scriptMsgboxYesOrNo = GameObject.Find("EventSystem").GetComponent<msgboxYesOrNo>();
        scriptMsgbox = GameObject.Find("EventSystem").GetComponent<msgbox>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    async public void OnClickEvent()
    {
        if (scriptSystem.cntNotebook >= 16 && scriptSystem.cntNotebook <= 31)
        {
            addPC();
        }
        else
        {
            scriptMsgbox.showMsgbox("You Can't Do That.", "예");
            var task = Task.Run(() => getMsgboxYesOrNoClickedBtn());
            int clickedBtn = await task;
        }
    }

    async void addPC()
    {
        scriptMsgboxYesOrNo.showMsgboxYesOrNo("Add PC2?", "예", "아니오");
        var task = Task.Run(() => getMsgboxYesOrNoClickedBtn());
        int clickedBtn = await task;

        if (clickedBtn == 1)
        {
            List<int> nextPCPos = getPCPos(scriptSystem.cntNotebook + 1);
            GameObject newPC = Instantiate(prefabPC, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            PC scriptNewPC = newPC.GetComponent<PC>();

            //Set Params
            scriptNewPC.spriteName = spriteName;
            scriptNewPC.pos = nextPCPos;
            scriptSystem.gameBitcoinPerTimeSlice += 0.0000025f;

            scriptSystem.cntNotebook += 1;
        }
    }

    int getMsgboxYesOrNoClickedBtn()
    {
        while (scriptMsgboxYesOrNo.clickedBtn == -1)
        {

        }
        return scriptMsgboxYesOrNo.clickedBtn;
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
