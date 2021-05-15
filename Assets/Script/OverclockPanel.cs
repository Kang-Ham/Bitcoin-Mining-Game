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
    private float X_VELOCITY;
    private float Y_VELOCITY;
    public GameObject itemBtc;
    public Vector3 mousePosition;
    public AudioSource audioPlayer;
    public AudioClip coinSoundEffect;
    public UInt64 currentOverclockPrice;
    public double currentOverclockPerTouch;

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

        //Btc ����
        scriptGameSystem.currentBtc += currentOverclockPerTouch;

        //ȿ��
        mousePosition = Input.mousePosition;
        GameObject instantObject = Instantiate(itemBtc, this.mousePosition, transform.rotation) as GameObject;
        instantObject.transform.SetParent(GameObject.Find("ClickerButton").transform);

        Rigidbody2D itemRigid = instantObject.GetComponent<Rigidbody2D>();
        X_VELOCITY = Random.Range(-7.0f, 7.0f);
        Y_VELOCITY = Random.Range(80.0f, 110.0f);
        Vector2 velocityVector = new Vector2(X_VELOCITY, Y_VELOCITY);
        itemRigid.AddForce(velocityVector, ForceMode2D.Impulse);
        Destroy(instantObject.gameObject, 1.5f);

        //�Ҹ� ���
        audioPlayer.PlayOneShot(coinSoundEffect);
        audioPlayer.volume = 0.2f * Convert.ToSingle(scriptGameSystem.currentSoundEffectVolume);
    }

    public void UpgradeOverclock()
    {
        if (!scriptMsgbox) scriptMsgbox = GameObject.Find("EventSystem").GetComponent<Msgbox>();
        if (!scriptTabpanel) scriptTabpanel = GameObject.Find("Canvas").GetComponent<Tabpanel>();
        if (!scriptGameSystem) scriptGameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();

        

        if (scriptGameSystem.currentMoney >= currentOverclockPrice)
        {
            scriptGameSystem.SetCurrentMoney(scriptGameSystem.currentMoney - currentOverclockPrice);
            scriptGameSystem.currentOverclockLevel += 1;
            UpdateOverclock();
        }
        else
        {
            scriptMsgbox.ShowMsgbox("������ �����մϴ�.", "��");
        }

        scriptTabpanel.LoadOverclockInformation();

    }

    public void UpdateOverclock()
    {
        currentOverclockPrice = Convert.ToUInt64(10 * scriptGameSystem.BTC_AT_FIRST_TOUCH * scriptGameSystem.currentBtcPrice * Math.Pow(scriptGameSystem.COEFFICIENT_OF_OVERCLOCK, scriptGameSystem.currentOverclockLevel - scriptGameSystem.BIFURCATION_OF_OVERCLOCK * Math.Truncate((scriptGameSystem.currentOverclockLevel - 1) / scriptGameSystem.BIFURCATION_OF_OVERCLOCK)) + 10 * scriptGameSystem.BTC_AT_FIRST_TOUCH * scriptGameSystem.currentBtcPrice * (Math.Pow(scriptGameSystem.COEFFICIENT_OF_OVERCLOCK, scriptGameSystem.BIFURCATION_OF_OVERCLOCK) - 1) * Math.Truncate((scriptGameSystem.currentOverclockLevel - 1) / scriptGameSystem.BIFURCATION_OF_OVERCLOCK));
        currentOverclockPerTouch = Convert.ToDouble((1 / scriptGameSystem.OVERCLOCK_DIFFICULTY) * scriptGameSystem.BTC_AT_FIRST_TOUCH * (Math.Pow(scriptGameSystem.COEFFICIENT_OF_OVERCLOCK, scriptGameSystem.BIFURCATION_OF_OVERCLOCK) - scriptGameSystem.COEFFICIENT_OF_OVERCLOCK) * scriptGameSystem.currentOverclockLevel / 50 + scriptGameSystem.BTC_AT_FIRST_TOUCH * scriptGameSystem.COEFFICIENT_OF_OVERCLOCK);
    }


    public void UpgradeOverclock10()
    {
        for(int i = 1; i < 11; i++)
        {
            this.UpgradeOverclock();
        }
    }
}
