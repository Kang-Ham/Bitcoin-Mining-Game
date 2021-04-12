using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UI;
using System;

public class SellBtc : MonoBehaviour
{
    private GameSystem scriptGameSystem;
    private CurrentMoney scriptCurrentMoney;
    private YesOrNoMsgbox scriptYesOrNoMsgbox;
    private Msgbox scriptMsgbox;
    private InputField sellInputField;

    // Start is called before the first frame update
    void Start()
    {
        scriptGameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();
        scriptCurrentMoney = GameObject.Find("CurrentMoneyText").GetComponent<CurrentMoney>();
        scriptYesOrNoMsgbox = GameObject.Find("EventSystem").GetComponent<YesOrNoMsgbox>();
        scriptMsgbox = GameObject.Find("EventSystem").GetComponent<Msgbox>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void OnClickEvent()
    {
        sellInputField = GameObject.Find("SellInput").GetComponent<InputField>();
        float sellInput;

        try
        {
            sellInput = System.Convert.ToSingle(sellInputField.text);
        }
        catch
        {
            scriptMsgbox.ShowMsgbox("�Է°��� �߸��Ǿ����ϴ�.", "Ȯ��");
            await Task.Run(() => scriptMsgbox.GetClickedButton());
            return;
        }

        try
        {
            UInt64 moneyToGet = System.Convert.ToUInt64(scriptGameSystem.currentBtcPrice * sellInput);

            if (scriptGameSystem.currentBtc < sellInput)
            {
                scriptMsgbox.ShowMsgbox("��Ʈ������ �����մϴ�.", "Ȯ��");
                await Task.Run(() => scriptMsgbox.GetClickedButton());
                return;
            }
            else if (sellInput < 0.00000001)
            {
                scriptMsgbox.ShowMsgbox("�ּ� �ŷ������� '0.00000001'�Դϴ�.", "Ȯ��");
                await Task.Run(() => scriptMsgbox.GetClickedButton());
                return;
            }

            scriptYesOrNoMsgbox.ShowYesOrNoMsgbox("��Ʈ������ �ŵ��Ͻðڽ��ϱ�? (" + sellInputField.text + "Btc = " + string.Format("{0:n0}", moneyToGet)+ "��)", "��", "�ƴϿ�");
            var task = Task.Run(() => scriptYesOrNoMsgbox.GetClickedButton());
            int clickedButton = await task;

            if (clickedButton == 0)
            {
                scriptGameSystem.currentBtc -= sellInput;
                scriptGameSystem.SetCurrentMoney(scriptGameSystem.currentMoney + moneyToGet);
                sellInputField.text = "";
            }
        }
        catch
        {
            scriptMsgbox.ShowMsgbox("������ �ʰ��Ͽ����ϴ�.", "Ȯ��");
            await Task.Run(() => scriptMsgbox.GetClickedButton());
            return;
        }
    }
}
