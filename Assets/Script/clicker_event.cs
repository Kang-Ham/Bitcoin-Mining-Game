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
        scriptSystem.curBitcoin += 1.002f * 0.0000008f * (scriptSystem.hitPower - 1) + 0.0000008f;
    }

    public void clicker_upgrade()
    {
        if (!scriptMsgbox) scriptMsgbox = GameObject.Find("EventSystem").GetComponent<msgbox>();

        if (scriptSystem.curBitcoin >= 0.000008f * scriptSystem.hitPower)
        {
            scriptSystem.curBitcoin -= 0.000008f * scriptSystem.hitPower;
            scriptSystem.hitPower += 1;
        }
        else
        {
            scriptMsgbox.showMsgbox("You Can't Do That.", "��");
        }
    }

    public void hitPowerReset()
    {
        scriptSystem.curBitcoin = 0;
        scriptSystem.hitPower = 1;
    }
}
