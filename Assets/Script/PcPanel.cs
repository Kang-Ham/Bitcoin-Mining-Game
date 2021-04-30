using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;
using UnityEngine.UI;

public class PcPanel : MonoBehaviour
{
    public GameObject prefabPc;
    public GameObject[] prefabAddPcImages;

    private GameSystem scriptGameSystem;
    private Tabpanel scriptTabpanel;
    private YesOrNoMsgbox scriptYesOrNoMsgbox;
    private Msgbox scriptMsgbox;

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
        if (!scriptGameSystem) scriptGameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();
        if (!scriptMsgbox) scriptMsgbox = GameObject.Find("EventSystem").GetComponent<Msgbox>();

        if (scriptGameSystem.currentPcList.Count >= 0 + 16 * (pcType - 1) && scriptGameSystem.currentPcList.Count < 16 + 16 * (pcType - 1)
            )
        {
            if(scriptGameSystem.currentMoney >= scriptGameSystem.PC_PRICES[pcType-1])
            {
                AskForAddPc();
            }
            else
            {
                scriptMsgbox.ShowMsgbox("현금이 부족합니다.", "예");
                var task = Task.Run(() => scriptMsgbox.GetClickedButton());
                int clickedButton = await task;
            }
            
        }
        else
        {
            scriptMsgbox.ShowMsgbox("You Can't Do That.", "예");
            var task = Task.Run(() => scriptMsgbox.GetClickedButton());
            int clickedButton = await task;
        }
    }

    async public void AskForAddPc()
    {
        if (!scriptYesOrNoMsgbox) scriptYesOrNoMsgbox = GameObject.Find("EventSystem").GetComponent<YesOrNoMsgbox>();
        if (!scriptTabpanel) scriptTabpanel = GameObject.Find("Canvas").GetComponent<Tabpanel>();

        int pcType = (int)(scriptGameSystem.currentPcList.Count / 16) + 1;

        scriptYesOrNoMsgbox.ShowYesOrNoMsgbox("해당 PC를 구입하시겠습니까?", "예", "아니오");
        var task = Task.Run(() => scriptYesOrNoMsgbox.GetClickedButton());
        int clickedButton = await task;

        if (clickedButton == 0)                                                                 
        {
            scriptTabpanel.SetPcButtonInteractableFalse(false);

            MakeNewButton(scriptGameSystem.currentPcList.Count);

            scriptGameSystem.SetCurrentMoney(scriptGameSystem.currentMoney - scriptGameSystem.PC_PRICES[pcType - 1]);

            AddNewPc();
        }
    }
    public void MakeNewButton(int pcCount)
    {
        if (!scriptGameSystem) scriptGameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();
        if (!scriptTabpanel) scriptTabpanel = GameObject.Find("Canvas").GetComponent<Tabpanel>();


        if (pcCount >= scriptGameSystem.NUMBER_OF_PC_AT_EACH_TABLE * scriptGameSystem.LENGTH_OF_TABLE * scriptGameSystem.NUMBER_OF_MENU - 1)
        {        // 추가 가능한 마지막 PC의 경우 추가 안 함
            return;
        }

        if (pcCount % 2 == 0)
        {
            GameObject[] AddPcImages = GameObject.FindGameObjectsWithTag("AddPcImage");
            AddPcImages[AddPcImages.Length - 1].transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            GameObject PcContent = GameObject.Find("PcContent");

            var newAddPcImage = Instantiate(prefabAddPcImages[(pcCount + 1) / 16]);
            newAddPcImage.transform.SetParent(PcContent.transform);
            newAddPcImage.transform.localScale = new Vector3(1f, 1f, 1f);
            newAddPcImage.transform.GetChild(1).gameObject.SetActive(false);
        }
        scriptTabpanel.LoadPcInformation(pcCount + 1);
    }

    public Pc AddNewPc()
    {
        if (!scriptGameSystem) scriptGameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();
        int pcType = (int)(scriptGameSystem.currentPcList.Count / 16) + 1;

        List<int> nextPcPos = GetPcPos(scriptGameSystem.currentPcList.Count + 1);
        GameObject newPc = Instantiate(prefabPc, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        newPc.transform.parent = GameObject.Find("PcList").transform;
        Pc scriptnewPc = newPc.GetComponent<Pc>();

        //Set Params
        scriptnewPc.spriteName = "pc" + pcType.ToString();
        scriptnewPc.pos = nextPcPos;
        scriptnewPc.btcPerSecond = scriptGameSystem.PC_BTC_PER_SECOND[pcType - 1];

        scriptGameSystem.currentPcList.Add(scriptnewPc);
        return scriptnewPc;
    }

    List<int> GetPcPos(int pcCount)
    {
        if (!scriptGameSystem) scriptGameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();

        List<int> RET = new List<int>();
        List<int> tablePos = GetTablePos(pcCount);
        int eachTableIndex = pcCount - (tablePos[0] * scriptGameSystem.NUMBER_OF_PC_AT_EACH_TABLE * scriptGameSystem.LENGTH_OF_TABLE + tablePos[1] * scriptGameSystem.NUMBER_OF_PC_AT_EACH_TABLE) - 1;
        RET.Add(tablePos[0]);
        RET.Add(tablePos[1]);
        RET.Add((int)(eachTableIndex / Math.Sqrt(scriptGameSystem.NUMBER_OF_PC_AT_EACH_TABLE)));
        RET.Add((int)(eachTableIndex % Math.Sqrt(scriptGameSystem.NUMBER_OF_PC_AT_EACH_TABLE)));
        return RET;
    }

    List<int> GetTablePos(int pcCount)
    {
        if (!scriptGameSystem) scriptGameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();

        List<int> RET = new List<int>();
        RET.Add((pcCount - 1) / (scriptGameSystem.LENGTH_OF_TABLE * scriptGameSystem.NUMBER_OF_PC_AT_EACH_TABLE));
        RET.Add(((pcCount - 1) % (scriptGameSystem.LENGTH_OF_TABLE * scriptGameSystem.NUMBER_OF_PC_AT_EACH_TABLE)) / scriptGameSystem.NUMBER_OF_PC_AT_EACH_TABLE);
        return RET;
    }
}
