using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class curBitcoin : MonoBehaviour
{
    private system scriptSystem;
    private Text textCurBitcoin;
    // Start is called before the first frame update
    void Start()
    {
        scriptSystem = GameObject.Find("system").GetComponent<system>();
        textCurBitcoin = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        string strCurBitCoin = scriptSystem.curBitcoin.ToString("0."+new string('#', 8));
        if (strCurBitCoin.Length > 10)
        {
            textCurBitcoin.text = strCurBitCoin.Substring(0, 11);
        }
        else
        {

            textCurBitcoin.text = strCurBitCoin;
        }
    }
}
