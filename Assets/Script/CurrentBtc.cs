using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentBtc : MonoBehaviour
{
    private GameSystem scriptGameSystem;
    private Text textCurBtc;
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    public void DoUpdate()
    {
        if(!scriptGameSystem) scriptGameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();
        if(!textCurBtc) textCurBtc = GetComponent<Text>();

        string strCurBtc = scriptGameSystem.currentBtc.ToString("0."+new string('#', 8));
        if (strCurBtc.Length > 10)
        {
            textCurBtc.text = strCurBtc.Substring(0, 11);
        }
        else
        {

            textCurBtc.text = strCurBtc;
        }
    }
}
