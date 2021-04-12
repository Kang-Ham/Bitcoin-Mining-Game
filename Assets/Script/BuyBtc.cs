using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UI;
using System;

public class BuyBtc : MonoBehaviour
{
    private GameSystem scriptGameSystem;
    private CurrentMoney scriptCurrentMoney;
    private YesOrNoMsgbox scriptYesOrNoMsgbox;
    private Msgbox scriptMsgbox;
    private InputField buyInputField;

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
        buyInputField = GameObject.Find("BuyInput").GetComponent<InputField>();
        float buyInput;

        try
        {
            buyInput = System.Convert.ToSingle(buyInputField.text);
        }
        catch
        {
            scriptMsgbox.ShowMsgbox("�Է°��� �߸��Ǿ����ϴ�.", "Ȯ��");
            await Task.Run(() => scriptMsgbox.GetClickedButton());
            return;
        }

        try
        {
            UInt64 moneyForBuy = System.Convert.ToUInt64(scriptGameSystem.currentBtcPrice * buyInput);

            if (scriptGameSystem.currentMoney < moneyForBuy)
            {
                scriptMsgbox.ShowMsgbox("������ �����մϴ�.", "Ȯ��");
                await Task.Run(() => scriptMsgbox.GetClickedButton());
                return;
            }
            else if (buyInput < 0.00000001)
            {
                scriptMsgbox.ShowMsgbox("�ּ� �ŷ������� '0.00000001'�Դϴ�.", "Ȯ��");
                await Task.Run(() => scriptMsgbox.GetClickedButton());
                return;
            }

            scriptYesOrNoMsgbox.ShowYesOrNoMsgbox("��Ʈ������ �ż��Ͻðڽ��ϱ�? ("+ buyInputField.text + "Btc = "+ string.Format("{0:n0}", moneyForBuy) + "��)", "��", "�ƴϿ�");
            var task = Task.Run(() => scriptYesOrNoMsgbox.GetClickedButton());
            int clickedButton = await task;

            if (clickedButton == 0)
            {
                scriptGameSystem.currentBtc += buyInput;
                scriptGameSystem.SetCurrentMoney(scriptGameSystem.currentMoney - moneyForBuy);
                buyInputField.text = "";
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
