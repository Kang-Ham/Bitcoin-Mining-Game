using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverclockPanel : MonoBehaviour
{
    private GameSystem scriptGameSystem;
    private Msgbox scriptMsgbox;
    private Tabpanel scriptTabpanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickEvent()
    {
        if (!scriptGameSystem) scriptGameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();
        scriptGameSystem.currentBtc += Convert.ToInt64(scriptGameSystem.BTC_AT_FIRST_TOUCH * (Math.Pow(scriptGameSystem.COEFFICIENT_OF_OVERCLOCK, scriptGameSystem.BIFURCATION_OF_OVERCLOCK) - 1) * scriptGameSystem.currentOverclockLevel / 75) + (20 * scriptGameSystem.BTC_AT_FIRST_TOUCH*10 / 21);
    }

    public void UpgradeOverclock()
    {
        if (!scriptMsgbox) scriptMsgbox = GameObject.Find("EventSystem").GetComponent<Msgbox>();
        if (!scriptTabpanel) scriptTabpanel = GameObject.Find("Canvas").GetComponent<Tabpanel>();
        if (!scriptGameSystem) scriptGameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();

        if (scriptGameSystem.currentOverclockLevel < scriptGameSystem.BIFURCATION_OF_OVERCLOCK+1)
        {
            if (scriptGameSystem.currentMoney >= Convert.ToUInt64(scriptGameSystem.BTC_AT_FIRST_TOUCH*10 * scriptGameSystem.currentBtcPrice * Math.Pow(scriptGameSystem.COEFFICIENT_OF_OVERCLOCK, scriptGameSystem.currentOverclockLevel - scriptGameSystem.BIFURCATION_OF_OVERCLOCK * Math.Truncate(scriptGameSystem.currentOverclockLevel/scriptGameSystem.BIFURCATION_OF_OVERCLOCK) - 1)))
            {
                scriptGameSystem.SetCurrentMoney(scriptGameSystem.currentMoney - Convert.ToUInt64(scriptGameSystem.BTC_AT_FIRST_TOUCH*10 * scriptGameSystem.currentBtcPrice * Math.Pow(scriptGameSystem.COEFFICIENT_OF_OVERCLOCK, scriptGameSystem.currentOverclockLevel - scriptGameSystem.BIFURCATION_OF_OVERCLOCK * Math.Truncate(scriptGameSystem.currentOverclockLevel/scriptGameSystem.BIFURCATION_OF_OVERCLOCK) - 1)));
                scriptGameSystem.currentOverclockLevel += 1;
            }
            else
            {
                scriptMsgbox.ShowMsgbox("You Can't Do That.", "¿¹");
            }
        }
        else
        {
            if (scriptGameSystem.currentMoney >= Convert.ToUInt64(scriptGameSystem.BTC_AT_FIRST_TOUCH*10 * scriptGameSystem.currentBtcPrice * Math.Pow(scriptGameSystem.COEFFICIENT_OF_OVERCLOCK, scriptGameSystem.currentOverclockLevel - scriptGameSystem.BIFURCATION_OF_OVERCLOCK * Math.Truncate(scriptGameSystem.currentOverclockLevel / scriptGameSystem.BIFURCATION_OF_OVERCLOCK) - 1) + scriptGameSystem.BTC_AT_FIRST_TOUCH*10 * scriptGameSystem.currentBtcPrice * Math.Pow(scriptGameSystem.COEFFICIENT_OF_OVERCLOCK, scriptGameSystem.BIFURCATION_OF_OVERCLOCK * Math.Truncate(scriptGameSystem.currentOverclockLevel/scriptGameSystem.BIFURCATION_OF_OVERCLOCK))))
            {
                scriptGameSystem.SetCurrentMoney(scriptGameSystem.currentMoney - Convert.ToUInt64(scriptGameSystem.BTC_AT_FIRST_TOUCH*10 * scriptGameSystem.currentBtcPrice * Math.Pow(scriptGameSystem.COEFFICIENT_OF_OVERCLOCK, scriptGameSystem.currentOverclockLevel - scriptGameSystem.BIFURCATION_OF_OVERCLOCK * Math.Truncate(scriptGameSystem.currentOverclockLevel / scriptGameSystem.BIFURCATION_OF_OVERCLOCK) - 1) + scriptGameSystem.BTC_AT_FIRST_TOUCH*10 * scriptGameSystem.currentBtcPrice * Math.Pow(scriptGameSystem.COEFFICIENT_OF_OVERCLOCK, scriptGameSystem.BIFURCATION_OF_OVERCLOCK * Math.Truncate(scriptGameSystem.currentOverclockLevel/scriptGameSystem.BIFURCATION_OF_OVERCLOCK))));
                scriptGameSystem.currentOverclockLevel += 1;
            }
            else
            {
                scriptMsgbox.ShowMsgbox("You Can't Do That.", "¿¹");
            }
        }

        scriptTabpanel.LoadOverclockPrice();

    }

    public void UpgradeOverclock10()
    {
        for(int i = 1; i < 11; i++)
        {
            this.UpgradeOverclock();
        }
    }
}
