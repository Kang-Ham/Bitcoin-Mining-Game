using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UI;
using System;

public class BtcPanel : MonoBehaviour
{
    private GameSystem scriptGameSystem;
    private YesOrNoMsgbox scriptYesOrNoMsgbox;
    private Msgbox scriptMsgbox;
    private InputField buyInputField;
    private InputField sellInputField;

    private Text textCurrentBtcPrice;

    // Start is called before the first frame update
    void Start()
    {
        scriptGameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();
        scriptYesOrNoMsgbox = GameObject.Find("EventSystem").GetComponent<YesOrNoMsgbox>();
        scriptMsgbox = GameObject.Find("EventSystem").GetComponent<Msgbox>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void OnClickBuyButtonEvent()
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
            UInt64 moneyForBuy = Convert.ToUInt64(scriptGameSystem.currentBtcPrice * buyInput);

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

            scriptYesOrNoMsgbox.ShowYesOrNoMsgbox("��Ʈ������ �ż��Ͻðڽ��ϱ�? (" + buyInputField.text + "BTC = " + string.Format("{0:n0}", moneyForBuy) + "��)", "��", "�ƴϿ�");
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

    public async void OnClickBuyAllButtonEvent()
    {
        try
        {
            UInt64 moneyForBuy = scriptGameSystem.currentMoney;
            UInt64 btcToGet = moneyForBuy / Convert.ToUInt64(scriptGameSystem.currentBtcPrice);

            if (btcToGet < 0.00000001)
            {
                scriptMsgbox.ShowMsgbox("�ּ� �ŷ������� '0.00000001'�Դϴ�.", "Ȯ��");
                await Task.Run(() => scriptMsgbox.GetClickedButton());
                return;
            }

            scriptYesOrNoMsgbox.ShowYesOrNoMsgbox("��Ʈ������ ���� �ż��Ͻðڽ��ϱ�? (" + btcToGet.ToString("0." + new string('#', 8)) + "BTC = " + string.Format("{0:n0}", moneyForBuy) + "��)", "��", "�ƴϿ�");
            var task = Task.Run(() => scriptYesOrNoMsgbox.GetClickedButton());
            int clickedButton = await task;

            if (clickedButton == 0)
            {
                scriptGameSystem.currentBtc += btcToGet;
                scriptGameSystem.SetCurrentMoney(0);
            }
        }
        catch
        {
            scriptMsgbox.ShowMsgbox("������ �ʰ��Ͽ����ϴ�.", "Ȯ��");
            await Task.Run(() => scriptMsgbox.GetClickedButton());
            return;
        }

    }

    public async void OnClickSellButtonEvent()
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

            scriptYesOrNoMsgbox.ShowYesOrNoMsgbox("��Ʈ������ �ŵ��Ͻðڽ��ϱ�? (" + sellInputField.text + "BTC = " + string.Format("{0:n0}", moneyToGet)+ "��)", "��", "�ƴϿ�");
            var task = Task.Run(() => scriptYesOrNoMsgbox.GetClickedButton());
            int clickedButton = await task;

            if (clickedButton == 0)
            {
                scriptGameSystem.SetCurrentMoney(scriptGameSystem.currentMoney + moneyToGet);
                scriptGameSystem.currentBtc -= sellInput;
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

    public async void OnClickSellAllButtonEvent()
    {
        try
        {
            float btcForBuy = scriptGameSystem.currentBtc;
            UInt64 moneyToGet = Convert.ToUInt64(scriptGameSystem.currentBtcPrice* btcForBuy);

            if (btcForBuy < 0.00000001)
            {
                scriptMsgbox.ShowMsgbox("�ּ� �ŷ������� '0.00000001'�Դϴ�.", "Ȯ��");
                await Task.Run(() => scriptMsgbox.GetClickedButton());
                return;
            }

            scriptYesOrNoMsgbox.ShowYesOrNoMsgbox("��Ʈ������ ���� �ŵ��Ͻðڽ��ϱ�? (" + btcForBuy.ToString("0." + new string('#', 8)) + "BTC = " + string.Format("{0:n0}", moneyToGet) + "��)", "��", "�ƴϿ�");
            var task = Task.Run(() => scriptYesOrNoMsgbox.GetClickedButton());
            int clickedButton = await task;

            if (clickedButton == 0)
            {
                scriptGameSystem.SetCurrentMoney(scriptGameSystem.currentMoney + moneyToGet);
                scriptGameSystem.currentBtc = 0;
            }
        }
        catch
        {
            scriptMsgbox.ShowMsgbox("������ �ʰ��Ͽ����ϴ�.", "Ȯ��");
            await Task.Run(() => scriptMsgbox.GetClickedButton());
            return;
        }
    }

    public void UpdateCurrentBtcPrice()
    {
        try
        {
            if (!textCurrentBtcPrice) textCurrentBtcPrice = GameObject.Find("CurrentBtcPriceText").GetComponent<Text>();
            int currentBtcPrice = GameObject.Find("GameSystem").GetComponent<GameSystem>().currentBtcPrice;

            string currentBtcString = string.Format("{0:n0}", currentBtcPrice);
            currentBtcString += '��';
            textCurrentBtcPrice.text = currentBtcString;
        }
        catch
        {
            Debug.Log("Btc ���� ������ ���� ä�� UpdateCurrentBtcPrice()�� ������ �� �����ϴ�.");
        }
    }
}
