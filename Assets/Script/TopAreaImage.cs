using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopAreaImage : MonoBehaviour
{
    private GameSystem scriptGameSystem;
    private Text textCurrentBtc;
    private Text textCurrentMoney;
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    public void UpdateCurrentBtcText()
    {
        if(!scriptGameSystem) scriptGameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();
        if(!textCurrentBtc) textCurrentBtc = GameObject.Find("CurrentBtcText").GetComponent<Text>();

        string strCurrentBtc = scriptGameSystem.currentBtc.ToString("0."+new string('#', 8));
        if (strCurrentBtc.Length > 10)
        {
            textCurrentBtc.text = strCurrentBtc.Substring(0, 11);
        }
        else
        {
            textCurrentBtc.text = strCurrentBtc;
        }
    }

    public void UpdateCurrentMoneyText()
    {
        if (!scriptGameSystem) scriptGameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();
        if (!textCurrentMoney) textCurrentMoney = GameObject.Find("CurrentMoneyText").GetComponent<Text>();

        string strCurrentMoney = string.Format("{0:n0}", scriptGameSystem.currentMoney);
        textCurrentMoney.text = strCurrentMoney;
        if (strCurrentMoney.Length > 23)
        {
            textCurrentMoney.fontSize = 22;
        }
        else if (strCurrentMoney.Length > 19)
        {
            textCurrentMoney.fontSize = 24;
        }
        else if (strCurrentMoney.Length > 14)
        {
            textCurrentMoney.fontSize = 28;
        }
        else if (strCurrentMoney.Length > 12)
        {
            textCurrentMoney.fontSize = 36;
        }
        else
        {
            textCurrentMoney.fontSize = 44;
        }
    }
}
