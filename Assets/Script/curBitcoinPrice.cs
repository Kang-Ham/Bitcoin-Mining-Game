using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class curBitcoinPrice : MonoBehaviour
{
    private Text textCurBitcoinPrice;
    // Start is called before the first frame update
    void Start()
    {
        textCurBitcoinPrice = GetComponent<Text>();
        doUpdate(GameObject.Find("system").GetComponent<system>().curBitcoinPrice);
    }

    // Update is called once per frame
    public void doUpdate(int curBitcoinPrice)
    {
        string strCurBitCoin = string.Format("{0:n0}", curBitcoinPrice);
        strCurBitCoin += '¿ø';
        textCurBitcoinPrice.text = strCurBitCoin;
    }
}
