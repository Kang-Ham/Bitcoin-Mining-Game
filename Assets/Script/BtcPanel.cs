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

    public void OnClickBuyButtonEvent()
    {
        buyInputField = GameObject.Find("BuyInput").GetComponent<InputField>();
        float buyInput;

        try
        {
            buyInput = System.Convert.ToSingle(buyInputField.text);
        }
        catch
        {
            scriptMsgbox.ShowMsgbox("입력값이 잘못되었습니다.", "확인");
            return;
        }

        try
        {
            UInt64 moneyForBuy = Convert.ToUInt64(scriptGameSystem.currentBtcPrice * buyInput);

            if (scriptGameSystem.currentMoney < moneyForBuy)
            {
                scriptMsgbox.ShowMsgbox("현금이 부족합니다.", "확인");
                return;
            }
            else if (buyInput < 0.00000001)
            {
                scriptMsgbox.ShowMsgbox("최소 거래단위는 '0.00000001'입니다.", "확인");
                return;
            }

            scriptYesOrNoMsgbox.ShowYesOrNoMsgbox("비트코인을 매수하시겠습니까? (" + buyInputField.text + "BTC = " + string.Format("{0:n0}", moneyForBuy) + "원)", "예", "아니오",
                (clickedButton) =>
                {
                    if (clickedButton == 0)
                    {
                        scriptGameSystem.currentBtc += buyInput;
                        scriptGameSystem.SetCurrentMoney(scriptGameSystem.currentMoney - moneyForBuy);
                        buyInputField.text = "";
                    }
                }
                );
        }
        catch
        {
            scriptMsgbox.ShowMsgbox("범위를 초과하였습니다.", "확인");
            return;
        }

    }

    public void OnClickBuyAllButtonEvent()
    {
        try
        {
            UInt64 moneyForBuy = scriptGameSystem.currentMoney;
            UInt64 btcToGet = moneyForBuy / Convert.ToUInt64(scriptGameSystem.currentBtcPrice);

            if (btcToGet < 0.00000001)
            {
                scriptMsgbox.ShowMsgbox("최소 거래단위는 '0.00000001'입니다.", "확인");
                return;
            }

            scriptYesOrNoMsgbox.ShowYesOrNoMsgbox("비트코인을 전량 매수하시겠습니까? (" + btcToGet.ToString("0." + new string('#', 8)) + "BTC = " + string.Format("{0:n0}", moneyForBuy) + "원)", "예", "아니오",
                (clickedButton) =>
                {
                    if (clickedButton == 0)
                    {
                        scriptGameSystem.currentBtc += btcToGet;
                        scriptGameSystem.SetCurrentMoney(0);
                    }
                }
                );
        }
        catch
        {
            scriptMsgbox.ShowMsgbox("범위를 초과하였습니다.", "확인");
            return;
        }

    }

    public void OnClickSellButtonEvent()
    {
        sellInputField = GameObject.Find("SellInput").GetComponent<InputField>();
        float sellInput;

        try
        {
            sellInput = System.Convert.ToSingle(sellInputField.text);
        }
        catch
        {
            scriptMsgbox.ShowMsgbox("입력값이 잘못되었습니다.", "확인");
            return;
        }

        try
        {
            UInt64 moneyToGet = System.Convert.ToUInt64(scriptGameSystem.currentBtcPrice * sellInput);

            if (scriptGameSystem.currentBtc < sellInput)
            {
                scriptMsgbox.ShowMsgbox("비트코인이 부족합니다.", "확인");
                return;
            }
            else if (sellInput < 0.00000001)
            {
                scriptMsgbox.ShowMsgbox("최소 거래단위는 '0.00000001'입니다.", "확인");
                return;
            }

            scriptYesOrNoMsgbox.ShowYesOrNoMsgbox("비트코인을 매도하시겠습니까? (" + sellInputField.text + "BTC = " + string.Format("{0:n0}", moneyToGet)+ "원)", "예", "아니오",
                (clickedButton) =>
                {
                    if (clickedButton == 0)
                    {
                        scriptGameSystem.SetCurrentMoney(scriptGameSystem.currentMoney + moneyToGet);
                        scriptGameSystem.currentBtc -= sellInput;
                        sellInputField.text = "";
                    }
                }
                );
        }
        catch
        {
            scriptMsgbox.ShowMsgbox("범위를 초과하였습니다.", "확인");
            return;
        }
    }

    public void OnClickSellAllButtonEvent()
    {
        try
        {
            float btcForBuy = scriptGameSystem.currentBtc;
            UInt64 moneyToGet = Convert.ToUInt64(scriptGameSystem.currentBtcPrice* btcForBuy);

            if (btcForBuy < 0.00000001)
            {
                scriptMsgbox.ShowMsgbox("최소 거래단위는 '0.00000001'입니다.", "확인");
                return;
            }

            scriptYesOrNoMsgbox.ShowYesOrNoMsgbox("비트코인을 전량 매도하시겠습니까? (" + btcForBuy.ToString("0." + new string('#', 8)) + "BTC = " + string.Format("{0:n0}", moneyToGet) + "원)", "예", "아니오",
                (clickedButton) =>
                {
                    if (clickedButton == 0)
                    {
                        scriptGameSystem.SetCurrentMoney(scriptGameSystem.currentMoney + moneyToGet);
                        scriptGameSystem.currentBtc = 0;
                    }
                }
                );
        }
        catch
        {
            scriptMsgbox.ShowMsgbox("범위를 초과하였습니다.", "확인");
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
            currentBtcString += '원';
            textCurrentBtcPrice.text = currentBtcString;
        }
        catch
        {
            Debug.Log("Btc 탭이 열리지 않은 채로 UpdateCurrentBtcPrice()를 수행할 수 없습니다.");
        }
    }
}
