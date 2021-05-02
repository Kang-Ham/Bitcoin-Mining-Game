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
    public int MAX_GPU_LEVEL;

    public string[] PC_NAMES;
    public float[] PC_BTC_PER_SECOND;
    public UInt64[] PC_PRICES;
    public string[] GPU_NAMES;
    public float[] GPU_RATES;
    public UInt64[] GPU_PRICES;

    //Game Load할 때 불러오기
    public float currentBtc;
    public UInt64 currentMoney;
    public List<Pc> currentPcList;
    public int currentGpuLevel;
    public float currentOverclockLevel;
    public int currentBgmVolume;
    public int currentSoundEffectVolume;

    //불러오기 않아도 됨        
    public float gameBtcPerSecond;
    public int selectedMenu;
    public int currentBtcPrice;

    private TopAreaImage scriptTopAreaImage;
    private Json scriptJson;
    private PcPanel scriptPcPanel;
    private BtcPanel scriptBtcPanel;

    // Start is called before the first frame update
    void Start()
    {
        scriptTopAreaImage = GameObject.Find("TopAreaImage").GetComponent<TopAreaImage>();
        scriptJson = GameObject.Find("Json").GetComponent<Json>();
        scriptPcPanel = GameObject.Find("EventSystem").GetComponent<PcPanel>();

        //Btc, Money 등 로컬 파일에서 불러오기
        scriptJson.Load();
        scriptTopAreaImage.UpdateCurrentBtcText();
        scriptTopAreaImage.UpdateCurrentMoneyText();

        if (currentPcList.Count == 0) scriptPcPanel.AddNewPc();

        StartCoroutine("SetCurrentBtcOnRunning", VALUE_TIME_SLICE_BTC);
        StartCoroutine("SavePeriodically", VALUE_TIME_SLICE_SAVE);
        StartCoroutine("SetcurrentBtcPriceWithCrawling", VALUE_TIME_SLICE_CRAWLING);
    }

    public void SetCurrentMoney(UInt64 money)
    {
        this.currentMoney = money;
        scriptTopAreaImage.UpdateCurrentMoneyText();
    }

    void OnApplicationQuit()
    {
        scriptJson.Save();
    }


    IEnumerator SetCurrentBtcOnRunning(float delay)
    {
        currentBtc += gameBtcPerSecond*delay;
        scriptTopAreaImage.UpdateCurrentBtcText();
        yield return new WaitForSeconds(delay);
        StartCoroutine("SetCurrentBtcOnRunning", delay);
    }

    IEnumerator SavePeriodically(float delay)
    {
        scriptJson.Save();
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

            if (indexsBtcPrice.Count >= 2) //해당 html 소스에 PRICE가 여러개 있음... 2번째(인덱스상 1) Price 뒤에 Btc 가격 있음
            {
                int curIndex = indexsBtcPrice[1] + 8; //"PRICE":0000 ... 0000만 추출하기 위해 +8
                while (htmlText[curIndex] != ',') //strBtcPrice에 한 글자씩 넣음
                {
                    strBtcPrice += htmlText[curIndex];
                    curIndex += 1;
                }
                //strBtcPrice 예시: 70175252.53 ... string -> float -> int
                currentBtcPrice = (int)System.Convert.ToSingle(strBtcPrice);

                try
                {
                    if(!scriptBtcPanel) scriptBtcPanel = GameObject.Find("EventSystem").GetComponent<BtcPanel>();
                    scriptBtcPanel.UpdateCurrentBtcPrice();
                }
                catch
                {
                    Debug.Log("currentBtcPrice is Loaded, but CurrentBtcText is inactive");
                }
                
                Debug.Log("Btc Price Loaded: "+strBtcPrice);
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
        for(int i = 0; i < htmlText.Length-target.Length; i++)
        {
            if(htmlText.Substring(i, target.Length).Equals(target))
            {
                indexs.Add(i);
            }
        }
        return indexs;
    }

    //TEST CODEs
    public void ResetPcGpu()
    {
        this.currentPcList = new List<Pc>();
        this.gameBtcPerSecond = 0f;
        this.currentGpuLevel = 0;

        GameObject[] currentPcList = GameObject.FindGameObjectsWithTag("Pc");

        for (int i = 1; i < currentPcList.Length; i++) Destroy(currentPcList[i]);
    }
    public void ResetOverclockBtcMoney()
    {
        currentBtc = 0;
        SetCurrentMoney(currentMoney - currentMoney);
        currentOverclockLevel = 1;
    }
    public void AddPc4()
    {
        try
        {
            scriptPcPanel = GameObject.Find("EventSystem").GetComponent<PcPanel>();
            Tabpanel scriptTabpanel = GameObject.Find("Canvas").GetComponent<Tabpanel>();
            for (int i = 0; i < 4; i++)
            {
                scriptPcPanel.MakeNewButton(currentPcList.Count);
                scriptTabpanel.SetPcButtonInteractableFalse(true);
                scriptPcPanel.AddNewPc();
            }
        }
        catch
        {
            Debug.Log("ERROR, system-AddPc4: panel - computer 탭 열어야함 또는 PC 갯수 초과");
        }
    }
    public void AddBtcMoney()
    {
        currentBtc += 1000000f;
        SetCurrentMoney(currentMoney + 10000000000000);
    }
}
