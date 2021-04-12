using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentMoney : MonoBehaviour
{
    private GameSystem scriptGameSystem;
    private Text textCurrentMoney;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    public void DoUpdate()
    {
        if(!scriptGameSystem) scriptGameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();
        if(!textCurrentMoney) textCurrentMoney = GetComponent<Text>();

        string strcurrentMoney = string.Format("{0:n0}", scriptGameSystem.currentMoney);
        textCurrentMoney.text = strcurrentMoney;
        if (strcurrentMoney.Length > 23)
        {
            textCurrentMoney.fontSize = 22;
        }
        else if (strcurrentMoney.Length > 19)
        {
            textCurrentMoney.fontSize = 24;
        }
        else if (strcurrentMoney.Length > 14)
        {
            textCurrentMoney.fontSize = 28;
        }
        else if (strcurrentMoney.Length > 12)
        {
            textCurrentMoney.fontSize = 36;
        }
        else
        {
            textCurrentMoney.fontSize = 44;
        }
    }
}
