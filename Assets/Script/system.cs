using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class system : MonoBehaviour
{
    //Warning: Unity - SampleScene - system - Inspector - System ���� �ʱ�ȭ �������
    public int NUMBER_OF_PC_AT_EACH_TABLE;
    public int LENGTH_OF_TABLE; //���̺� 4*4    
    public int NUMBER_OF_MENU; //�޴���ư ����
    public Vector3 START_SCENE_POS;
    public float SCENE_DISTANCE_BETWEEN_TABLE;
    public float SCENE_DISTANCE_BETWEEN_PC;
    public float VALUE_TIME_SLICE_SECOND; //1=> 1�ʿ� 1���� ��Ʈ���� ����
    public float VALUE_TIME_SLICE_CRAWLING; //60=> 1�п� 1���� ��Ʈ���� ũ�Ѹ�

    //TODO: Game Load�� �� �ҷ�����
    public float curBitcoin;
    public UInt64 curMoney;
    public int cntNotebook;
    public float gameBitcoinPerTimeSlice;

    //�ҷ����� �ʾƵ� ��
    public int selectedMenu;
    public int curBitcoinPrice;
    private curMoney scriptCurMoney;

    // Start is called before the first frame update
    void Start()
    {
        scriptCurMoney = GameObject.Find("curWon").GetComponent<curMoney>();

        //TODO: BTC, Money �� ���� ���Ͽ��� �ҷ�����
        scriptCurMoney.doUpdate();

        StartCoroutine("setCurBitcoinOnRunning", VALUE_TIME_SLICE_SECOND);
        StartCoroutine("setCurBitcoinPriceWithCrawling", VALUE_TIME_SLICE_CRAWLING);
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator setCurBitcoinOnRunning(float delay)
    {
        curBitcoin += gameBitcoinPerTimeSlice* VALUE_TIME_SLICE_SECOND;
        yield return new WaitForSeconds(delay);
        StartCoroutine("setCurBitcoinOnRunning", VALUE_TIME_SLICE_SECOND);
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
        StartCoroutine("setCurBitcoinPriceWithCrawling", VALUE_TIME_SLICE_CRAWLING);
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


}
