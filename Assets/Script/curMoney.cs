using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class curMoney : MonoBehaviour
{
    private system scriptSystem;
    private Text textCurMoney;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    public void doUpdate()
    {
        if(!scriptSystem) scriptSystem = GameObject.Find("system").GetComponent<system>();
        if(!textCurMoney) textCurMoney = GetComponent<Text>();

        string strCurMoney = string.Format("{0:n0}", scriptSystem.curMoney);
        textCurMoney.text = strCurMoney;
        if (strCurMoney.Length > 23)
        {
            textCurMoney.fontSize = 22;
        }
        else if (strCurMoney.Length > 19)
        {
            textCurMoney.fontSize = 24;
        }
        else if (strCurMoney.Length > 14)
        {
            textCurMoney.fontSize = 28;
        }
        else if (strCurMoney.Length > 12)
        {
            textCurMoney.fontSize = 36;
        }
        else
        {
            textCurMoney.fontSize = 44;
        }
    }
}
