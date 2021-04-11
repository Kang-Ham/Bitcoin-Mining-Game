using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;
using UnityEngine.UI;

public class addPC : MonoBehaviour
{
    private system scriptSystem;
    private curMoney scriptCurMoney;

    public GameObject prefabPC;
    private msgboxYesOrNo scriptMsgboxYesOrNo;
    private msgbox scriptMsgbox;
    private com_menu_spawner scriptComMenuSpawner;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
                                    
    async public void OnClickEvent(int pcType)
    {
        if (!scriptSystem) scriptSystem = GameObject.Find("system").GetComponent<system>();
        if (!scriptMsgbox) scriptMsgbox = GameObject.Find("EventSystem").GetComponent<msgbox>();

        if (scriptSystem.PCs.Count >= 0 + 16 * (pcType - 1) && scriptSystem.PCs.Count < 16 + 16 * (pcType - 1)
            )
        {
            if(scriptSystem.curMoney >= scriptSystem.PC_PRICES[pcType-1])
            {
                askForAddPC();
            }
            else
            {
                scriptMsgbox.showMsgbox("현금이 부족합니다.", "예");
                var task = Task.Run(() => scriptMsgbox.getClickedBtn());
                int clickedBtn = await task;
            }
            
        }
        else
        {
            scriptMsgbox.showMsgbox("You Can't Do That.", "예");
            var task = Task.Run(() => scriptMsgbox.getClickedBtn());
            int clickedBtn = await task;
        }
    }

    async public void askForAddPC()
    {
        if (!scriptMsgboxYesOrNo) scriptMsgboxYesOrNo = GameObject.Find("EventSystem").GetComponent<msgboxYesOrNo>();
        if (!scriptComMenuSpawner) scriptComMenuSpawner = GameObject.FindGameObjectWithTag("content").GetComponent<com_menu_spawner>();

        int pcType = (int)(scriptSystem.PCs.Count / 16) + 1;

        scriptMsgboxYesOrNo.showMsgboxYesOrNo("해당 PC를 구입하시겠습니까?", "예", "아니오");
        var task = Task.Run(() => scriptMsgboxYesOrNo.getClickedBtn());
        int clickedBtn = await task;

        if (clickedBtn == 1)
        {
            GameObject[] addPCBtns = GameObject.FindGameObjectsWithTag("addBtn");
            addPCBtns[addPCBtns.Length - 1].GetComponent<Button>().interactable = false;
            scriptComMenuSpawner.makeNewButton(scriptSystem.PCs.Count);

            scriptSystem.setCurMoney(scriptSystem.curMoney - scriptSystem.PC_PRICES[pcType - 1]);

            addNewPC();
        }
    }

    public PC addNewPC()
    {
        if (!scriptSystem) scriptSystem = GameObject.Find("system").GetComponent<system>();
        int pcType = (int)(scriptSystem.PCs.Count / 16) + 1;

        List<int> nextPCPos = getPCPos(scriptSystem.PCs.Count + 1);
        GameObject newPC = Instantiate(prefabPC, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        PC scriptNewPC = newPC.GetComponent<PC>();

        //Set Params
        scriptNewPC.spriteName = "pc" + pcType.ToString();
        scriptNewPC.pos = nextPCPos;
        scriptNewPC.bitcoinPerSecond = scriptSystem.PC_BITCOIN_PER_SECOND[pcType - 1];

        scriptSystem.PCs.Add(scriptNewPC);
        return scriptNewPC;
    }

    List<int> getPCPos(int cnt)
    {
        if (!scriptSystem) scriptSystem = GameObject.Find("system").GetComponent<system>();

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
        if (!scriptSystem) scriptSystem = GameObject.Find("system").GetComponent<system>();

        List<int> RET = new List<int>();
        RET.Add((cnt - 1) / (scriptSystem.LENGTH_OF_TABLE * scriptSystem.NUMBER_OF_PC_AT_EACH_TABLE));
        RET.Add(((cnt - 1) % (scriptSystem.LENGTH_OF_TABLE * scriptSystem.NUMBER_OF_PC_AT_EACH_TABLE)) / scriptSystem.NUMBER_OF_PC_AT_EACH_TABLE);
        return RET;
    }
}
