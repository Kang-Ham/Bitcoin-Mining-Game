using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class addPC : MonoBehaviour
{
    system scriptSystem;
    public GameObject prefabPC;
    private msgboxYesOrNo scriptMsgboxYesOrNo;
    private msgbox scriptMsgbox;

    private List<float> listBitcoinPerTimeSlice = new List<float>() {0f, 0.00000001f, 0.0000025f, 0.0000125f, 0.000625f};


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

        if (scriptSystem.PCs.Count >= 0+16*(pcType-1) && scriptSystem.PCs.Count < 16+ 16 * (pcType - 1)
            )
        {
            askForAddPC();
        }
        else
        {
            scriptMsgbox.showMsgbox("You Can't Do That.", "��");
            var task = Task.Run(() => scriptMsgbox.getClickedBtn());
            int clickedBtn = await task;
        }
    }

    async public void askForAddPC()
    {
        if (!scriptMsgboxYesOrNo) scriptMsgboxYesOrNo = GameObject.Find("EventSystem").GetComponent<msgboxYesOrNo>();

        scriptMsgboxYesOrNo.showMsgboxYesOrNo("�ش� PC�� �����Ͻðڽ��ϱ�?", "��", "�ƴϿ�");
        var task = Task.Run(() => scriptMsgboxYesOrNo.getClickedBtn());
        int clickedBtn = await task;

        if (clickedBtn == 1)
        {
            addNewPC();
        }
    }

    public void addNewPC()
    {
        if (!scriptSystem) scriptSystem = GameObject.Find("system").GetComponent<system>();
        int pcType = (int)(scriptSystem.PCs.Count / 16) + 1;

        List<int> nextPCPos = getPCPos(scriptSystem.PCs.Count + 1);
        GameObject newPC = Instantiate(prefabPC, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        PC scriptNewPC = newPC.GetComponent<PC>();

        //Set Params
        scriptNewPC.spriteName = "pc"+ pcType.ToString();
        scriptNewPC.pos = nextPCPos;
        scriptNewPC.level = 1;
        scriptNewPC.bitcoinPerTimeSlice = listBitcoinPerTimeSlice[pcType];

        scriptSystem.PCs.Add(scriptNewPC);
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
