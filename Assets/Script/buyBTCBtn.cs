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
            scriptMsgbox.showMsgbox("입력값이 잘못되었습니다.", "확인");
            await Task.Run(() => scriptMsgbox.getClickedBtn());
            return;
        }

        try
        {
            UInt64 moneyForBuy = System.Convert.ToUInt64(scriptSystem.curBitcoinPrice * buyInput);

            if (scriptSystem.curMoney < moneyForBuy)
            {
                scriptMsgbox.showMsgbox("현금이 부족합니다.", "확인");
                await Task.Run(() => scriptMsgbox.getClickedBtn());
                return;
            }
            else if (buyInput < 0.00000001)
            {
                scriptMsgbox.showMsgbox("최소 거래단위는 '0.00000001'입니다.", "확인");
                await Task.Run(() => scriptMsgbox.getClickedBtn());
                return;
            }

            scriptMsgboxYesOrNo.showMsgboxYesOrNo("비트코인을 매수하시겠습니까? ("+ buyInputField.text + "BTC = "+ string.Format("{0:n0}", moneyForBuy) + "원)", "예", "아니오");
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
            scriptMsgbox.showMsgbox("범위를 초과하였습니다.", "확인");
            await Task.Run(() => scriptMsgbox.getClickedBtn());
            return;
        }
        
    }
}
