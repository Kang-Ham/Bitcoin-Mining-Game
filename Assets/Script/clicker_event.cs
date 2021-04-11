using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class clicker_event : MonoBehaviour
{
    private system scriptSystem;
    private msgbox scriptMsgbox;
    private tabpanel scriptTabpanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void clicker()
    {
        if (!scriptSystem) scriptSystem = GameObject.Find("system").GetComponent<system>();
        scriptSystem.curBitcoin += Convert.ToInt64(scriptSystem.BITCOIN_AT_FIRST_TOUCH * (Math.Pow(scriptSystem.COEFFICIENT_OF_OVERCLOCK, scriptSystem.BIFURCATION_OF_OVERCLOCK) - 1) * scriptSystem.hitPower / 75) + (20 * scriptSystem.BITCOIN_AT_FIRST_TOUCH*10 / 21);
    }

    public void clicker_upgrade()
    {
        if (!scriptMsgbox) scriptMsgbox = GameObject.Find("EventSystem").GetComponent<msgbox>();
        if (!scriptTabpanel) scriptTabpanel = GameObject.Find("Canvas").GetComponent<tabpanel>();
        if (!scriptSystem) scriptSystem = GameObject.Find("system").GetComponent<system>();

        if (scriptSystem.hitPower < scriptSystem.BIFURCATION_OF_OVERCLOCK+1)
        {
            if (scriptSystem.curMoney >= Convert.ToUInt64(scriptSystem.BITCOIN_AT_FIRST_TOUCH*10 * scriptSystem.curBitcoinPrice * Math.Pow(scriptSystem.COEFFICIENT_OF_OVERCLOCK, scriptSystem.hitPower - scriptSystem.BIFURCATION_OF_OVERCLOCK * Math.Truncate(scriptSystem.hitPower/scriptSystem.BIFURCATION_OF_OVERCLOCK) - 1)))
            {
                scriptSystem.setCurMoney(scriptSystem.curMoney - Convert.ToUInt64(scriptSystem.BITCOIN_AT_FIRST_TOUCH*10 * scriptSystem.curBitcoinPrice * Math.Pow(scriptSystem.COEFFICIENT_OF_OVERCLOCK, scriptSystem.hitPower - scriptSystem.BIFURCATION_OF_OVERCLOCK * Math.Truncate(scriptSystem.hitPower/scriptSystem.BIFURCATION_OF_OVERCLOCK) - 1)));
                scriptSystem.hitPower += 1;
            }
            else
            {
                scriptMsgbox.showMsgbox("You Can't Do That.", "¿¹");
            }
        }
        else
        {
            if (scriptSystem.curMoney >= Convert.ToUInt64(scriptSystem.BITCOIN_AT_FIRST_TOUCH*10 * scriptSystem.curBitcoinPrice * Math.Pow(scriptSystem.COEFFICIENT_OF_OVERCLOCK, scriptSystem.hitPower - scriptSystem.BIFURCATION_OF_OVERCLOCK * Math.Truncate(scriptSystem.hitPower / scriptSystem.BIFURCATION_OF_OVERCLOCK) - 1) + scriptSystem.BITCOIN_AT_FIRST_TOUCH*10 * scriptSystem.curBitcoinPrice * Math.Pow(scriptSystem.COEFFICIENT_OF_OVERCLOCK, scriptSystem.BIFURCATION_OF_OVERCLOCK * Math.Truncate(scriptSystem.hitPower/scriptSystem.BIFURCATION_OF_OVERCLOCK))))
            {
                scriptSystem.setCurMoney(scriptSystem.curMoney - Convert.ToUInt64(scriptSystem.BITCOIN_AT_FIRST_TOUCH*10 * scriptSystem.curBitcoinPrice * Math.Pow(scriptSystem.COEFFICIENT_OF_OVERCLOCK, scriptSystem.hitPower - scriptSystem.BIFURCATION_OF_OVERCLOCK * Math.Truncate(scriptSystem.hitPower / scriptSystem.BIFURCATION_OF_OVERCLOCK) - 1) + scriptSystem.BITCOIN_AT_FIRST_TOUCH*10 * scriptSystem.curBitcoinPrice * Math.Pow(scriptSystem.COEFFICIENT_OF_OVERCLOCK, scriptSystem.BIFURCATION_OF_OVERCLOCK * Math.Truncate(scriptSystem.hitPower/scriptSystem.BIFURCATION_OF_OVERCLOCK))));
                scriptSystem.hitPower += 1;
            }
            else
            {
                scriptMsgbox.showMsgbox("You Can't Do That.", "¿¹");
            }
        }

        scriptTabpanel.loadOverclockPrice();

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
        if (!scriptSystem) scriptSystem = GameObject.Find("system").GetComponent<system>();

        scriptSystem.curBitcoin = 0;
        scriptSystem.setCurMoney(scriptSystem.curMoney - scriptSystem.curMoney);
        scriptSystem.hitPower = 1;
    }
}
