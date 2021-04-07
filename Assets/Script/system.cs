using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class system : MonoBehaviour
{
    //Warning: Unity - SampleScene - system - Inspector - System 에서 초기화 해줘야함
    public int NUMBER_OF_PC_AT_EACH_TABLE;
    public int LENGTH_OF_TABLE; //테이블 4*4    
    public int NUMBER_OF_MENU; //메뉴버튼 갯수
    public Vector3 START_SCENE_POS;
    public float SCENE_DISTANCE_BETWEEN_TABLE;
    public float SCENE_DISTANCE_BETWEEN_PC;
    public float VALUE_TIME_SLICE_BITCOIN; //1=> 1초에 1번씩 비트코인 갱신
    public float VALUE_TIME_SLICE_SAVE; //1=> 1초에 1번씩 비트코인 갱신
    public float VALUE_TIME_SLICE_CRAWLING; //60=> 1분에 1번씩 비트코인 크롤링
    public float[,] BITCOIN_PER_SECOND = new float[4, 1] {
        { 0.00000001f },
        { 0.0000025f },
        { 0.0000125f },
        { 0.000625f }
    };

    //TODO: Game Load할 때 불러오기
    public float curBitcoin;
    public UInt64 curMoney;
    public List<PC> PCs;

    //불러오기 않아도 됨        
    public float gameBitcoinPerSecond;
    public int selectedMenu;
    public int curBitcoinPrice;
    private curBitcoin scriptCurBitcoin;
    private curMoney scriptCurMoney;
    private Json scriptJson;

    // Start is called before the first frame update
    void Start()
    {
        scriptCurBitcoin = GameObject.Find("curBitcoin").GetComponent<curBitcoin>();
        scriptCurMoney = GameObject.Find("curMoney").GetComponent<curMoney>();
        scriptJson = GameObject.Find("json").GetComponent<Json>();

        //TODO: BTC, Money 등 로컬 파일에서 불러오기
        scriptJson.load();
        scriptCurMoney.doUpdate();
        scriptCurBitcoin.doUpdate();
        
        StartCoroutine("setCurBitcoinOnRunning", VALUE_TIME_SLICE_BITCOIN);
        StartCoroutine("savePeriodically", VALUE_TIME_SLICE_SAVE);
        StartCoroutine("setCurBitcoinPriceWithCrawling", VALUE_TIME_SLICE_CRAWLING);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnApplicationQuit()
    {
        scriptJson.save();
    }


    IEnumerator setCurBitcoinOnRunning(float delay)
    {
        curBitcoin += gameBitcoinPerSecond*delay;
        scriptCurBitcoin.doUpdate();
        yield return new WaitForSeconds(delay);
        StartCoroutine("setCurBitcoinOnRunning", delay);
    }

    IEnumerator savePeriodically(float delay)
    {
        scriptJson.save();
        yield return new WaitForSeconds(delay);
        StartCoroutine("savePeriodically", delay);
    }

    IEnumerator setCurBitcoinPriceWithCrawling(float delay)
    {
        UnityWebRequest www = UnityWebRequest.Get("https://min-api.cryptocompare.com/data/pricemultifull?fsyms=BTC&tsyms=BTC,KRW");
        www.useHttpContinue = true;
        www.downloadHandler = new DownloadHandlerBuffer();
        yield return www.SendWebRequest();

        string strBTCPrice = "";

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            string htmlText = www.downloadHandler.text;     
            List<int> indexsBTCPrice = getIndexOfBTCFromHTMLText(htmlText, "\"PRICE\":");

            if (indexsBTCPrice.Count >= 2) //해당 html 소스에 PRICE가 여러개 있음... 2번째(인덱스상 1) Price 뒤에 BTC 가격 있음
            {
                int curIndex = indexsBTCPrice[1] + 8; //"PRICE":0000 ... 0000만 추출하기 위해 +8
                while (htmlText[curIndex] != ',') //strBTCPrice에 한 글자씩 넣음
                {
                    strBTCPrice += htmlText[curIndex];
                    curIndex += 1;
                }
                //strBTCPrice 예시: 70175252.53 ... string -> float -> int
                curBitcoinPrice = (int)System.Convert.ToSingle(strBTCPrice);

                try
                {
                    curBitcoinPrice scriptCurBitcoinPrice = GameObject.Find("curBTC_text").GetComponent<curBitcoinPrice>();
                    scriptCurBitcoinPrice.doUpdate(curBitcoinPrice);
                }
                catch
                {
                    Debug.Log("curBitcoinPrice is Loaded, but curBTC_text is inactive");
                }
                
                Debug.Log("BTC Price Loaded: "+strBTCPrice);
            }
            else
            {
                Debug.Log("Cannot Find BTC Price");
            }
        }

        yield return new WaitForSeconds(delay);
        StartCoroutine("setCurBitcoinPriceWithCrawling", delay);
    }

    List<int> getIndexOfBTCFromHTMLText(string htmlText, string target)
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

    public void reset()
    {
        this.PCs = new List<PC>();
        this.gameBitcoinPerSecond = 0f;

        GameObject[] PCs = GameObject.FindGameObjectsWithTag("pc");

        for (int i = 1; i < PCs.Length; i++) Destroy(PCs[i]);
    }

    public void addPC4()
    {
        try
        {
            addPC scriptAddPC = GameObject.Find("EventSystem").GetComponent<addPC>();
            com_menu_spawner scriptComMenuSpawner = GameObject.FindGameObjectWithTag("content").GetComponent<com_menu_spawner>();
            for (int i = 0; i < 4; i++)
            {
                scriptComMenuSpawner.makeNewButton(PCs.Count);
                scriptAddPC.addNewPC(1);
            }
        }
        catch
        {
            Debug.Log("panel - computer 탭 열고 해야함");
        }

    }
}
