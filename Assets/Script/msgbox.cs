using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class msgbox : MonoBehaviour
{
    public int clickedBtn;
    private GameObject msgboxPanel, msgboxContent, msgboxBtn1;
    // Start is called before the first frame update
    void Start()
    {
        msgboxPanel = GameObject.Find("Canvas").transform.Find("msgbox_panel").gameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickEventBtn1()
    {
        hideMsgbox();
        clickedBtn = 1;
    }

    public void showMsgbox(string strContent, string strBtn1)
    {
        clickedBtn = -1;
        msgboxPanel.SetActive(true);

        msgboxContent = msgboxPanel.transform.Find("msgbox_content").gameObject;
        msgboxBtn1 = msgboxPanel.transform.Find("msgbox_btn1").gameObject;

        msgboxContent.GetComponent<Text>().text = strContent;
        msgboxBtn1.transform.Find("msgbox_btn1_text").GetComponent<Text>().text = strBtn1;
    }

    private void hideMsgbox()
    {
        msgboxPanel.SetActive(false);
    }
}
