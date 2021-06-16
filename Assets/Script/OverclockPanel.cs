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

        //Btc 증가
        scriptGameSystem.currentBtc += scriptGameSystem.currentOverclockPerTouch;

        //효과
        mousePosition = Input.mousePosition;
        GameObject instantObject = Instantiate(itemBtc, this.mousePosition, transform.rotation) as GameObject;
        instantObject.transform.SetParent(GameObject.Find("ClickerButton").transform);

        Rigidbody2D itemRigid = instantObject.GetComponent<Rigidbody2D>();
        X_VELOCITY = Random.Range(-14.0f, 14.0f);
        Y_VELOCITY = Random.Range(160.0f, 220.0f);
        Vector2 velocityVector = new Vector2(X_VELOCITY, Y_VELOCITY);
        itemRigid.AddForce(velocityVector, ForceMode2D.Impulse);
        Destroy(instantObject.gameObject, 1.5f);

        //소리 재생
        audioPlayer.PlayOneShot(coinSoundEffect);
        audioPlayer.volume = 0.2f * Convert.ToSingle(scriptGameSystem.currentSoundEffectVolume);
    }

    public void UpgradeOverclock(Boolean isTenUpgrade)
    {
        if (!scriptMsgbox) scriptMsgbox = GameObject.Find("EventSystem").GetComponent<Msgbox>();
        if (!scriptTabpanel) scriptTabpanel = GameObject.Find("Canvas").GetComponent<Tabpanel>();
        if (!scriptGameSystem) scriptGameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();

        // 네트워크 확인
        if (scriptGameSystem.currentBtcPrice == 0)
        {
            scriptMsgbox.ShowMsgbox("비트코인 가격이 정상적으로 로드되지 않았습니다. 인터넷 연결 유무를 확인하세요,", "확인");
            return;
        }

        // 1회, 10회 구입인지 확인하고 가격 및 강화횟수 대입
        ulong overclockPrice;
        int upgradeCount;
        if(isTenUpgrade == false)
        {
            overclockPrice = scriptGameSystem.currentOverclockPrice;
            upgradeCount = 1;
        }
        else
        {
            overclockPrice = scriptGameSystem.currentTenOverclockPrice;
            upgradeCount = 10;
        }

        // 구입 연산
        if (scriptGameSystem.currentMoney >= overclockPrice)
        {
            scriptGameSystem.SetCurrentMoney(scriptGameSystem.currentMoney - overclockPrice);
            scriptGameSystem.currentOverclockLevel += upgradeCount;
            UpdateOverclock();
        }
        else
        {
            scriptMsgbox.ShowMsgbox("현금이 부족합니다.", "예");
        }

        scriptTabpanel.LoadOverclockInformation();
    }

    public void UpdateOverclock()
    {
        if (!scriptGameSystem) scriptGameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();

        scriptGameSystem.currentOverclockPerTouch = Convert.ToDouble(scriptGameSystem.BTC_AT_FIRST_TOUCH * 50 * Math.Pow((scriptGameSystem.currentOverclockLevel / scriptGameSystem.COEFFICIENT_OF_OVERCLOCK), 2) + scriptGameSystem.BTC_AT_FIRST_TOUCH);
        scriptGameSystem.currentOverclockPrice = Convert.ToUInt64(8 * scriptGameSystem.currentOverclockLevel * (Math.Pow(Math.Truncate((scriptGameSystem.currentOverclockLevel - 1) / scriptGameSystem.BIFURCATION_OF_OVERCLOCK) + 1, 2) - Math.Pow(Math.Truncate((scriptGameSystem.currentOverclockLevel - 1) / scriptGameSystem.BIFURCATION_OF_OVERCLOCK), 2)) / scriptGameSystem.BIFURCATION_OF_OVERCLOCK + 8 * Math.Pow(Math.Truncate((scriptGameSystem.currentOverclockLevel - 1) / scriptGameSystem.BIFURCATION_OF_OVERCLOCK), 2) + scriptGameSystem.OVERCLOCK_DIFFICULTY * Math.Truncate((scriptGameSystem.currentOverclockLevel - 1) / scriptGameSystem.BIFURCATION_OF_OVERCLOCK) + scriptGameSystem.currentOverclockLevel);
        scriptGameSystem.currentTenOverclockPrice = 0;
        
        for (int i = 0; i < 10; i++)
        {
            scriptGameSystem.currentTenOverclockPrice += Convert.ToUInt64(8 * scriptGameSystem.currentOverclockLevel * (Math.Pow(Math.Truncate((scriptGameSystem.currentOverclockLevel + i) / scriptGameSystem.BIFURCATION_OF_OVERCLOCK) + 1, 2) - Math.Pow(Math.Truncate((scriptGameSystem.currentOverclockLevel + i) / scriptGameSystem.BIFURCATION_OF_OVERCLOCK), 2)) / scriptGameSystem.BIFURCATION_OF_OVERCLOCK + 8 * Math.Pow(Math.Truncate((scriptGameSystem.currentOverclockLevel + i) / scriptGameSystem.BIFURCATION_OF_OVERCLOCK), 2) + scriptGameSystem.OVERCLOCK_DIFFICULTY * Math.Truncate((scriptGameSystem.currentOverclockLevel + i) / scriptGameSystem.BIFURCATION_OF_OVERCLOCK) + scriptGameSystem.currentOverclockLevel + i);
        }

        scriptGameSystem.nextOverclockPerTouchIncrement = Convert.ToDouble(scriptGameSystem.BTC_AT_FIRST_TOUCH * 50 * Math.Pow(((scriptGameSystem.currentOverclockLevel + 1) / scriptGameSystem.COEFFICIENT_OF_OVERCLOCK), 2)) - Convert.ToDouble(scriptGameSystem.BTC_AT_FIRST_TOUCH * 50 * Math.Pow((scriptGameSystem.currentOverclockLevel / scriptGameSystem.COEFFICIENT_OF_OVERCLOCK), 2));
        scriptGameSystem.nextTenOverclockPerTouchIncrement = Convert.ToDouble(scriptGameSystem.BTC_AT_FIRST_TOUCH * 50 * Math.Pow(((scriptGameSystem.currentOverclockLevel + 10) / scriptGameSystem.COEFFICIENT_OF_OVERCLOCK), 2)) - Convert.ToDouble(scriptGameSystem.BTC_AT_FIRST_TOUCH * 50 * Math.Pow((scriptGameSystem.currentOverclockLevel / scriptGameSystem.COEFFICIENT_OF_OVERCLOCK), 2));
    }
}
