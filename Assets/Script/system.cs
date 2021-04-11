using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using UnityEngine.UI;

public class system : MonoBehaviour
{
    //Warning: Unity - SampleScene - system - Inspector - System ���� �ʱ�ȭ �������
    public int NUMBER_OF_PC_AT_EACH_TABLE;
    public int LENGTH_OF_TABLE; //���̺� 4*4    
    public int NUMBER_OF_MENU; //�޴���ư ����
    public Vector3 START_SCENE_POS;
    public float SCENE_DISTANCE_BETWEEN_TABLE;
    public float SCENE_DISTANCE_BETWEEN_PC;
    public float VALUE_TIME_SLICE_BITCOIN; //1=> 1�ʿ� 1���� ��Ʈ���� ����
    public float VALUE_TIME_SLICE_SAVE; //1=> 1�ʿ� 1���� ��Ʈ���� ����
    public float VALUE_TIME_SLICE_CRAWLING; //60=> 1�п� 1���� ��Ʈ���� ũ�Ѹ�
    public int BIFURCATION_OF_OVERCLOCK;
    public float BITCOIN_AT_FIRST_TOUCH;
    public float COEFFICIENT_OF_OVERCLOCK;

    public float[] PC_BITCOIN_PER_SECOND;
    public UInt64[] PC_PRICES;
    public float[] GPU_RATES;
    public UInt64[] GPU_PRICES;
                                                                                                              
    //Game Load�� �� �ҷ�����
    public float curBitcoin;
    public UInt64 curMoney;
    public List<PC> PCs;
    public int curGPULevel;
    public float hitPower;

    //�ҷ����� �ʾƵ� ��        
    public float gameBitcoinPerSecond;
    public int selectedMenu;
    public int curBitcoinPrice;
    private curBitcoin scriptCurBitcoin;
    private curMoney scriptCurMoney;
    private Json scriptJson;
    private addPC scriptAddPC;

    // Start is called before the first frame update
    void Start()
    {
        scriptCurBitcoin = GameObject.Find("curBitcoin").GetComponent<curBitcoin>();
        scriptCurMoney = GameObject.Find("curMoney").GetComponent<curMoney>();
        scriptJson = GameObject.Find("json").GetComponent<Json>();
        scriptAddPC = GameObject.Find("EventSystem").GetComponent<addPC>();

        //BTC, Money �� ���� ���Ͽ��� �ҷ�����
        scriptJson.load();
        scriptCurMoney.doUpdate();
        scriptCurBitcoin.doUpdate();

        if (PCs.Count == 0) scriptAddPC.addNewPC();

        StartCoroutine("setCurBitcoinOnRunning", VALUE_TIME_SLICE_BITCOIN);
        StartCoroutine("savePeriodically", VALUE_TIME_SLICE_SAVE);
        StartCoroutine("setCurBitcoinPriceWithCrawling", VALUE_TIME_SLICE_CRAWLING);
    }

    public void setCurMoney(UInt64 money)
    {
        this.curMoney = money;
        scriptCurMoney.doUpdate();
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

            if (indexsBTCPrice.Count >= 2) //�ش� html �ҽ��� PRICE�� ������ ����... 2��°(�ε����� 1) Price �ڿ� BTC ���� ����
            {
                int curIndex = indexsBTCPrice[1] + 8; //"PRICE":0000 ... 0000�� �����ϱ� ���� +8
                while (htmlText[curIndex] != ',') //strBTCPrice�� �� ���ھ� ����
                {
                    strBTCPrice += htmlText[curIndex];
                    curIndex += 1;
                }
                //strBTCPrice ����: 70175252.53 ... string -> float -> int
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
        this.curGPULevel = 0;

        GameObject[] PCs = GameObject.FindGameObjectsWithTag("pc");

        for (int i = 1; i < PCs.Length; i++) Destroy(PCs[i]);
    }

    public void addPC4()
    {
        try
        {
            addPC scriptAddPC = GameObject.Find("EventSystem").GetComponent<addPC>();
            com_menu_spawner scriptComMenuSpawner = GameObject.FindGameObjectWithTag("content").GetComponent<com_menu_spawner>();
            tabpanel scriptTabpanel = GameObject.Find("Canvas").GetComponent<tabpanel>();
            for (int i = 0; i < 4; i++)
            {
                scriptComMenuSpawner.makeNewButton(PCs.Count);
                scriptTabpanel.setButtonExceptLastInteractableFalse();
                scriptAddPC.addNewPC();
            }
        }
        catch
        {
            Debug.Log("ERROR, system-addPC4: panel - computer �� ������� �Ǵ� PC ���� �ʰ�");
        }

    }
}
