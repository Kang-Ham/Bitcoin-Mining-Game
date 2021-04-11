using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UI;
using System;

public class buyBTCBtn : MonoBehaviour
{
    private system scriptSystem;
    private curMoney scriptCurMoney;
    private msgboxYesOrNo scriptMsgboxYesOrNo;
    private msgbox scriptMsgbox;
    private InputField buyInputField;

    // Start is called before the first frame update
    void Start()
    {
        scriptSystem = GameObject.Find("system").GetComponent<system>();
        scriptCurMoney = GameObject.Find("curMoney").GetComponent<curMoney>();
        scriptMsgboxYesOrNo = GameObject.Find("EventSystem").GetComponent<msgboxYesOrNo>();
        scriptMsgbox = GameObject.Find("EventSystem").GetComponent<msgbox>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public async void OnClickEvent()
    {
        buyInputField = GameObject.Find("buy_input").GetComponent<InputField>();
        float buyInput;

        try
        {
            buyInput = System.Convert.ToSingle(buyInputField.text);
        }
        catch
        {
            scriptMsgbox.showMsgbox("�Է°��� �߸��Ǿ����ϴ�.", "Ȯ��");
            await Task.Run(() => scriptMsgbox.getClickedBtn());
            return;
        }

        try
        {
            UInt64 moneyForBuy = System.Convert.ToUInt64(scriptSystem.curBitcoinPrice * buyInput);

            if (scriptSystem.curMoney < moneyForBuy)
            {
                scriptMsgbox.showMsgbox("������ �����մϴ�.", "Ȯ��");
                await Task.Run(() => scriptMsgbox.getClickedBtn());
                return;
            }
            else if (buyInput < 0.00000001)
            {
                scriptMsgbox.showMsgbox("�ּ� �ŷ������� '0.00000001'�Դϴ�.", "Ȯ��");
                await Task.Run(() => scriptMsgbox.getClickedBtn());
                return;
            }

            scriptMsgboxYesOrNo.showMsgboxYesOrNo("��Ʈ������ �ż��Ͻðڽ��ϱ�? ("+ buyInputField.text + "BTC = "+ string.Format("{0:n0}", moneyForBuy) + "��)", "��", "�ƴϿ�");
            var task = Task.Run(() => scriptMsgboxYesOrNo.getClickedBtn());
            int clickedBtn = await task;

            if (clickedBtn == 1)
            {
                scriptSystem.curBitcoin += buyInput;
                scriptSystem.setCurMoney(scriptSystem.curMoney - moneyForBuy);
                buyInputField.text = "";
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
