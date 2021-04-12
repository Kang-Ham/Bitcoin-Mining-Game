using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YesOrNoMsgbox : MonoBehaviour
{
    public int clickedButton;
    private GameObject yesOrNoMsgboxPanel, msgboxContent, msgboxButton1, msgboxButton2;
    // Start is called before the first frame update
    void Start()
    {
        yesOrNoMsgboxPanel = GameObject.Find("Canvas").transform.Find("YesOrNoMsgboxPanel").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickBoxButton(int index)
    {
        HideYesOrNoMsgbox();
        clickedButton = index;
    }

    public void ShowYesOrNoMsgbox(string strContent, string strButton1, string strButton2)
    {
        clickedButton = -1;
        yesOrNoMsgboxPanel.SetActive(true);

        msgboxContent = yesOrNoMsgboxPanel.transform.Find("MsgboxContent").gameObject;
        msgboxButton1 = yesOrNoMsgboxPanel.transform.Find("MsgboxButton1").gameObject;
        msgboxButton2 = yesOrNoMsgboxPanel.transform.Find("MsgboxButton2").gameObject;

        msgboxContent.GetComponent<Text>().text = strContent;
        msgboxButton1.transform.Find("MsgboxButton1Text").GetComponent<Text>().text = strButton1;
        msgboxButton2.transform.Find("MsgboxButton2Text").GetComponent<Text>().text = strButton2;
    }

    private void HideYesOrNoMsgbox()
    {
        yesOrNoMsgboxPanel.SetActive(false);
    }

    public int GetClickedButton()
    {
        while (clickedButton == -1)
        {

        }
        return clickedButton;
    }
}
