using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using UnityEngine.UI;

public class GameSystem : MonoBehaviour
{
    //Warning: Unity - SampleScene - system - Inspector - System ���� �ʱ�ȭ �������
    public int NUMBER_OF_PC_AT_EACH_TABLE;
    public int LENGTH_OF_TABLE; //���̺� 4*4    
    public int NUMBER_OF_MENU; //�޴���ư ����
    public Vector3 START_SCENE_POS;
    public float SCENE_DISTANCE_BETWEEN_TABLE;
    public float SCENE_DISTANCE_BETWEEN_PC;
    public float VALUE_TIME_SLICE_BTC; //1=> 1�ʿ� 1���� ��Ʈ���� ����
    public float VALUE_TIME_SLICE_SAVE; //1=> 1�ʿ� 1���� ��Ʈ���� ����
    public float VALUE_TIME_SLICE_CRAWLING; //60=> 1�п� 1���� ��Ʈ���� ũ�Ѹ�
    public int BIFURCATION_OF_OVERCLOCK;
    public float BTC_AT_FIRST_TOUCH;
    public float COEFFICIENT_OF_OVERCLOCK;

    public float[] PC_BTC_PER_SECOND;
    public UInt64[] PC_PRICES;
    public float[] GPU_RATES;
    public UInt64[] GPU_PRICES;

    //Game Load�� �� �ҷ�����
    public float currentBtc;
    public UInt64 currentMoney;
    public List<Pc> currentPcList;
    public int currentGpuLevel;
    public float currentOverclockLevel;

    //�ҷ����� �ʾƵ� ��        
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

        //Btc, Money �� ���� ���Ͽ��� �ҷ�����
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

            if (indexsBtcPrice.Count >= 2) //�ش� html �ҽ��� PRICE�� ������ ����... 2��°(�ε����� 1) Price �ڿ� Btc ���� ����
            {
                int curIndex = indexsBtcPrice[1] + 8; //"PRICE":0000 ... 0000�� �����ϱ� ���� +8
                while (htmlText[curIndex] != ',') //strBtcPrice�� �� ���ھ� ����
                {
                    strBtcPrice += htmlText[curIndex];
                    curIndex += 1;
                }
                //strBtcPrice ����: 70175252.53 ... string -> float -> int
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
                scriptTabpanel.SetButtonInteractableFalse(true);
                scriptPcPanel.AddNewPc();
            }
        }
        catch
        {
            Debug.Log("ERROR, system-AddPc4: panel - computer �� ������� �Ǵ� PC ���� �ʰ�");
        }
    }
    public void AddBtcMoney()
    {
        currentBtc += 1000000f;
        SetCurrentMoney(currentMoney + 10000000000000);
    }
}
