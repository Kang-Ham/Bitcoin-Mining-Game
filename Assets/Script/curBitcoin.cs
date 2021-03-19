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
        textCurBitcoin.text = scriptSystem.curBitcoin.ToString("0."+new string('#', 10));
    }
}
