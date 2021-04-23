using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class OverclockPanel : MonoBehaviour
{
    private GameSystem scriptGameSystem;
    private Msgbox scriptMsgbox;
    private Tabpanel scriptTabpanel;
    private Vector3 BTCPosition;
    private float X_VELOCITY;
    private float Y_VELOCITY;
    public GameObject itemBTC;
    public Vector3 mousePosition;
    public Rigidbody2D rigid;

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
        scriptGameSystem.currentBtc += Convert.ToInt64(scriptGameSystem.BTC_AT_FIRST_TOUCH * (Math.Pow(scriptGameSystem.COEFFICIENT_OF_OVERCLOCK, scriptGameSystem.BIFURCATION_OF_OVERCLOCK) - 1) * scriptGameSystem.currentOverclockLevel / 75) + (20 * scriptGameSystem.BTC_AT_FIRST_TOUCH * 10 / 21);
        mousePosition = Input.mousePosition;
        GameObject instantObject = Instantiate(itemBTC, this.mousePosition, transform.rotation) as GameObject;
        instantObject.transform.parent = GameObject.Find("ClickerButton").transform;

        Rigidbody2D itemRigid = instantObject.GetComponent<Rigidbody2D>();
        X_VELOCITY = Random.Range(-5.0f, 5.0f);
        Y_VELOCITY = Random.Range(30.0f, 50.0f);
        Vector2 velocityVector = new Vector2(X_VELOCITY, Y_VELOCITY);
        itemRigid.AddForce(velocityVector, ForceMode2D.Impulse);
        Destroy(instantObject.gameObject, 1f);
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
                scriptMsgbox.ShowMsgbox("You Can't Do That.", "��");
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
                scriptMsgbox.ShowMsgbox("You Can't Do That.", "��");
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
