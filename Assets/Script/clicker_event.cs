using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class clicker_event : MonoBehaviour
{
    private system scriptSystem;
    private msgbox scriptMsgbox;

    // Start is called before the first frame update
    void Start()
    {
        scriptSystem = GameObject.Find("system").GetComponent<system>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void clicker()
    {
        if(!scriptSystem) GameObject.Find("system").GetComponent<system>();
        scriptSystem.curBitcoin += Convert.ToInt64(0.0000008f * (Math.Pow(1.05, 50) - 1) * scriptSystem.hitPower / 75) + (20 * 0.0000008f / 21);
    }

    public void clicker_upgrade()
    {
        if (!scriptMsgbox) scriptMsgbox = GameObject.Find("EventSystem").GetComponent<msgbox>();

        if (scriptSystem.hitPower < 51)
        {
            if (scriptSystem.curMoney >= Convert.ToUInt64(0.0000008 * scriptSystem.curBitcoinPrice * Math.Pow(1.05, scriptSystem.hitPower - 50 * Math.Truncate(scriptSystem.hitPower/50) - 1)))
            {
                scriptSystem.setCurMoney(scriptSystem.curMoney - Convert.ToUInt64(0.0000008 * scriptSystem.curBitcoinPrice * Math.Pow(1.05, scriptSystem.hitPower - 50 * Math.Truncate(scriptSystem.hitPower / 50) - 1)));
                scriptSystem.hitPower += 1;
            }
            else
            {
                scriptMsgbox.showMsgbox("You Can't Do That.", "¿¹");
            }
        }
        else
        {
            if (scriptSystem.curMoney >= Convert.ToUInt64(0.0000008 * scriptSystem.curBitcoinPrice * Math.Pow(1.05, scriptSystem.hitPower - 50 * Math.Truncate(scriptSystem.hitPower / 50) - 1) + 0.0000008 * scriptSystem.curBitcoinPrice * Math.Pow(1.05, 50 * Math.Truncate(scriptSystem.hitPower/50))))
            {
                scriptSystem.setCurMoney(scriptSystem.curMoney - Convert.ToUInt64(0.0000008 * scriptSystem.curBitcoinPrice * Math.Pow(1.05, scriptSystem.hitPower - 50 * Math.Truncate(scriptSystem.hitPower / 50) - 1) + 0.0000008 * scriptSystem.curBitcoinPrice * Math.Pow(1.05, 50 * Math.Truncate(scriptSystem.hitPower / 50))));
                scriptSystem.hitPower += 1;
            }
            else
            {
                scriptMsgbox.showMsgbox("You Can't Do That.", "¿¹");
            }
        }




    }

    public void clickerUpgrade10()
    {
        for(int i = 1; i < 11; i++)
        {
            this.clicker_upgrade();
        }
    }

    public void hitPowerReset()
    {
        scriptSystem.curBitcoin = 0;
        scriptSystem.setCurMoney(scriptSystem.curMoney - scriptSystem.curMoney);
        scriptSystem.hitPower = 1;
    }
}
