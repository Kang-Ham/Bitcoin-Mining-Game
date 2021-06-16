using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using UnityEngine.UI;

public class GameSystem : MonoBehaviour
{
    //Warning: Unity - SampleScene - system - Inspector - System 에서 초기화 해줘야함
    public int NUMBER_OF_PC_AT_EACH_TABLE;
    public int LENGTH_OF_TABLE; //테이블 4*4    
    public int NUMBER_OF_MENU; //메뉴버튼 갯수
    public Vector3 START_SCENE_POS;
    public float SCENE_DISTANCE_BETWEEN_TABLE;
    public float SCENE_DISTANCE_BETWEEN_PC;

    public float VALUE_TIME_SLICE_BTC; //1=> 1초에 1번씩 비트코인 갱신
    public float VALUE_TIME_SLICE_SAVE; //1=> 1초에 1번씩 비트코인 갱신
    public float VALUE_TIME_SLICE_CRAWLING; //60=> 1분에 1번씩 비트코인 크롤링
    public int BIFURCATION_OF_OVERCLOCK;
    public float BTC_AT_FIRST_TOUCH;
    public float COEFFICIENT_OF_OVERCLOCK;
    public float OVERCLOCK_DIFFICULTY;

    public int MAX_BTC_STORING_HOUR; //3 => 게임 껐을 때 최대 3시간 분량의 BTC 저장 가능

    public string[] PC_NAMES;
    public float[] PC_BTC_PER_SECOND;
    public UInt64[] PC_PRICES;
    public string[] GPU_NAMES;
    public float[] GPU_RATES;
    public UInt64[] GPU_PRICES;

    //Game Load할 때 불러오기
    public double currentBtc;
    public UInt64 currentMoney;
    public List<Pc> currentPcList;
    public int currentGpuLevel;
    public float currentOverclockLevel;
    public int currentBgmVolume;
    public int currentSoundEffectVolume;
    public Boolean currentNotificationStatus;

    //불러오기 않아도 됨        
    public float gameBtcPerSecond;
    public int selectedMenu;
    public int currentBtcPrice;
    public UInt64 currentOverclockPrice;
    public UInt64 currentTenOverclockPrice;
    public double currentOverclockPerTouch;
    public double nextOverclockPerTouchIncrement;
    public double nextTenOverclockPerTouchIncrement;
    public GameObject MenuPanel;

    private TopAreaImage scriptTopAreaImage;
    private PcPanel scriptPcPanel;
    private BtcPanel scriptBtcPanel;
    private GooglePlayManager scriptGooglePlayManager;
    private OverclockPanel scriptOverclockPanel;
    private Tabpanel scriptTabpanel;
    private Msgbox scriptMsgbox;

    // Start is called before the first frame update
    void Start()
    {
        Camera.main.orthographicSize = 8.95f * ((Convert.ToSingle(Screen.height) / Convert.ToSingle(Screen.width)) / (16f / 9f));

        scriptTopAreaImage = GameObject.Find("TopAreaImage").GetComponent<TopAreaImage>();
        scriptPcPanel = GameObject.Find("EventSystem").GetComponent<PcPanel>();
        scriptMsgbox = GameObject.Find("EventSystem").GetComponent<Msgbox>();
        scriptGooglePlayManager = GameObject.Find("EventSystem").GetComponent<GooglePlayManager>();

        // 네트워크 연결되지 않았을 시 접속 차단
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            scriptMsgbox.ShowMsgbox("인터넷 연결 유무를 확인하세요,", "확인");
            Application.Quit();
        }
    }

    public void OnDataLoaded()
    {
        StartCoroutine("SetCurrentBtcOnRunning", VALUE_TIME_SLICE_BTC);
        StartCoroutine("SavePeriodically", VALUE_TIME_SLICE_SAVE);
        StartCoroutine("SetcurrentBtcPriceWithCrawling", VALUE_TIME_SLICE_CRAWLING);
    }

    public void SetCurrentMoney(UInt64 money)
    {
        this.currentMoney = money;
        scriptTopAreaImage.UpdateCurrentMoneyText();
    }

    IEnumerator SetCurrentBtcOnRunning(float delay)
    {
        currentBtc += gameBtcPerSecond * delay;
        scriptTopAreaImage.UpdateCurrentBtcText();
        yield return new WaitForSeconds(delay);
        StartCoroutine("SetCurrentBtcOnRunning", delay);
    }

    IEnumerator SavePeriodically(float delay)
    {
        scriptGooglePlayManager.SaveToCloud();
        yield return new WaitForSeconds(delay);
        StartCoroutine("SavePeriodically", delay);
    }

    IEnumerator SetcurrentBtcPriceWithCrawling(float delay)
    {
        UnityWebRequest www = UnityWebRequest.Get("https://min-api.cryptocompare.com/data/pricemultifull?fsyms=Btc&tsyms=Btc,KRW");
        www.useHttpContinue = true;
        www.downloadHandler = new DownloadHandlerBuffer();
        yield return www.SendWebRequest();

        string strBtcPrice = "";

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            string htmlText = www.downloadHandler.text;
            List<int> indexsBtcPrice = GetIndexOfBtcFromHTMLText(htmlText, "\"PRICE\":");

            if (indexsBtcPrice.Count >= 2) // 해당 html 소스에 PRICE가 여러개 있음... 2번째(인덱스상 1) Price 뒤에 Btc 가격 있음
            {
                int curIndex = indexsBtcPrice[1] + 8; // "PRICE":0000 ... 0000만 추출하기 위해 +8
                while (htmlText[curIndex] != ',') // strBtcPrice에 한 글자씩 넣음
                {
                    strBtcPrice += htmlText[curIndex];
                    curIndex += 1;
                }
                // strBtcPrice 예시: 70175252.53 ... string -> float -> int
                currentBtcPrice = (int)System.Convert.ToSingle(strBtcPrice);

                try
                {
                    if (!scriptBtcPanel) scriptBtcPanel = GameObject.Find("EventSystem").GetComponent<BtcPanel>();
                    scriptBtcPanel.UpdateCurrentBtcPrice();
                }
                catch
                {
                    Debug.Log("currentBtcPrice is Loaded, but CurrentBtcText is inactive");
                }

                Debug.Log("Btc Price Loaded: " + strBtcPrice);


                if (MenuPanel) MenuPanel = GameObject.Find("Canvas").transform.Find("MenuPanel").gameObject;
                if (MenuPanel.activeSelf == true && selectedMenu == 0)
                {
                    if (!scriptOverclockPanel) scriptOverclockPanel = GameObject.Find("EventSystem").GetComponent<OverclockPanel>();
                    if (!scriptTabpanel) scriptTabpanel = GameObject.Find("Canvas").GetComponent<Tabpanel>();
                    scriptOverclockPanel.UpdateOverclock();
                    scriptTabpanel.LoadOverclockInformation();
                }
                else
                {
                    if (!scriptOverclockPanel) scriptOverclockPanel = GameObject.Find("EventSystem").GetComponent<OverclockPanel>();
                    scriptOverclockPanel.UpdateOverclock();
                }
            }
            else
            {
                Debug.Log("Cannot Find Btc Price");
            }
        }

        yield return new WaitForSeconds(delay);
        StartCoroutine("SetcurrentBtcPriceWithCrawling", delay);
    }

    List<int> GetIndexOfBtcFromHTMLText(string htmlText, string target)
    {
        List<int> indexs = new List<int>();
        for (int i = 0; i < htmlText.Length - target.Length; i++)
        {
            if (htmlText.Substring(i, target.Length).Equals(target))
            {
                indexs.Add(i);
            }
        }
        return indexs;
    }
}
