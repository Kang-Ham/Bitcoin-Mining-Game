using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Msgbox : MonoBehaviour
{
    private GameObject msgboxPanel, msgboxContent, msgboxButton1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickBoxButton()
    {
        HideMsgbox();
    }

    public void ShowMsgbox(string contentString, string button1String)
    {
        if(!msgboxPanel) msgboxPanel = GameObject.Find("PopupPanels").transform.Find("MsgboxPanel").gameObject;

        msgboxPanel.SetActive(true);

        msgboxContent = msgboxPanel.transform.Find("MsgboxContent").gameObject;
        msgboxButton1 = msgboxPanel.transform.Find("MsgboxButton1").gameObject;

        msgboxContent.GetComponent<Text>().text = contentString;
        msgboxButton1.transform.Find("MsgboxButton1Text").GetComponent<Text>().text = button1String;
    }

    private void HideMsgbox()
    {
        msgboxPanel.SetActive(false);
    }
}
