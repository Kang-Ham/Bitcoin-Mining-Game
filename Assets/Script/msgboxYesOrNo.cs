using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class msgboxYesOrNo : MonoBehaviour
{
    public int clickedBtn;
    private GameObject msgboxYesOrNoPanel, msgboxContent, msgboxBtn1, msgboxBtn2;
    // Start is called before the first frame update
    void Start()
    {
        msgboxYesOrNoPanel = GameObject.Find("Canvas").transform.Find("msgbox_yes_or_no_panel").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickEventBtn1()
    {
        hideMsgboxYesOrNo();
        clickedBtn = 1;
    }
    public void OnClickEventBtn2()
    {
        hideMsgboxYesOrNo();
        clickedBtn = 2;
    }

    public void showMsgboxYesOrNo(string strContent, string strBtn1, string strBtn2)
    {
        clickedBtn = -1;
        msgboxYesOrNoPanel.SetActive(true);

        msgboxContent = msgboxYesOrNoPanel.transform.Find("msgbox_content").gameObject;
        msgboxBtn1 = msgboxYesOrNoPanel.transform.Find("msgbox_btn1").gameObject;
        msgboxBtn2 = msgboxYesOrNoPanel.transform.Find("msgbox_btn2").gameObject;

        msgboxContent.GetComponent<Text>().text = strContent;
        msgboxBtn1.transform.Find("msgbox_btn1_text").GetComponent<Text>().text = strBtn1;
        msgboxBtn2.transform.Find("msgbox_btn2_text").GetComponent<Text>().text = strBtn2;
    }

    private void hideMsgboxYesOrNo()
    {
        msgboxYesOrNoPanel.SetActive(false);
    }
}
