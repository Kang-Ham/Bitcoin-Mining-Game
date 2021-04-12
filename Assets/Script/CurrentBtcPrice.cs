using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentBtcPrice : MonoBehaviour
{
    private Text textCurrentBtcPrice;
    // Start is called before the first frame update
    void Start()
    {
        textCurrentBtcPrice = GetComponent<Text>();
        DoUpdate(GameObject.Find("GameSystem").GetComponent<GameSystem>().currentBtcPrice);
    }

    // Update is called once per frame
    public void DoUpdate(int currentBtcPrice)
    {
        string currentBtcString = string.Format("{0:n0}", currentBtcPrice);
        currentBtcString += '¿ø';
        textCurrentBtcPrice.text = currentBtcString;
    }
}
