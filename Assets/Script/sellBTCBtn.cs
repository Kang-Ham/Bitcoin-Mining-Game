using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UI;
using System;

public class sellBTCBtn : MonoBehaviour
{
    private system scriptSystem;
    private curMoney scriptCurMoney;
    private msgboxYesOrNo scriptMsgboxYesOrNo;
    private msgbox scriptMsgbox;
    private InputField sellInputField;

    // Start is called before the first frame update
    void Start()
    {
        scriptSystem = GameObject.Find("system").GetComponent<system>();
        scriptCurMoney = GameObject.Find("curWon").GetComponent<curMoney>();
        scriptMsgboxYesOrNo = GameObject.Find("EventSystem").GetComponent<msgboxYesOrNo>();
        scriptMsgbox = GameObject.Find("EventSystem").GetComponent<msgbox>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void OnClickEvent()
    {
        sellInputField = GameObject.Find("sell_input").GetComponent<InputField>();
        float sellInput;

        try
        {
            sellInput = System.Convert.ToSingle(sellInputField.text);
        }
        catch
        {
            scriptMsgbox.showMsgbox("�Է°��� �߸��Ǿ����ϴ�.", "Ȯ��");
            await Task.Run(() => scriptMsgbox.getClickedBtn());
            return;
        }

        try
        {
            UInt64 moneyToGet = System.Convert.ToUInt64(scriptSystem.curBitcoinPrice * sellInput);

            if (scriptSystem.curBitcoin < sellInput)
            {
                scriptMsgbox.showMsgbox("��Ʈ������ �����մϴ�.", "Ȯ��");
                await Task.Run(() => scriptMsgbox.getClickedBtn());
                return;
            }
            else if (sellInput < 0.00000001)
            {
                scriptMsgbox.showMsgbox("�ּ� �ŷ������� '0.00000001'�Դϴ�.", "Ȯ��");
                await Task.Run(() => scriptMsgbox.getClickedBtn());
                return;
            }

            scriptMsgboxYesOrNo.showMsgboxYesOrNo("��Ʈ������ �ŵ��Ͻðڽ��ϱ�? (" + sellInputField.text + "BTC = " + string.Format("{0:n0}", moneyToGet)+ "��)", "��", "�ƴϿ�");
            var task = Task.Run(() => scriptMsgboxYesOrNo.getClickedBtn());
            int clickedBtn = await task;

            if (clickedBtn == 1)
            {
                scriptSystem.curBitcoin -= sellInput;
                scriptSystem.curMoney += moneyToGet;
                scriptCurMoney.doUpdate();
                sellInputField.text = "";
            }
        }
        catch
        {
            scriptMsgbox.showMsgbox("������ �ʰ��Ͽ����ϴ�.", "Ȯ��");
            await Task.Run(() => scriptMsgbox.getClickedBtn());
            return;
        }

        
    }
}
