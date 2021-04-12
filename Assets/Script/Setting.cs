using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class Setting : MonoBehaviour
{
    public int clickedButton;
    private GameObject settingPanel, settingButton1, settingButton2;

    // Start is called before the first frame update
    void Start()
    {
        settingPanel = GameObject.Find("PopupPanels").transform.Find("SettingPanel").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    async public void OnClickEvent()
    {
        ShowSetting();
        var task = Task.Run(() => GetClickedButton());
        int clickedButton = await task;

        if(clickedButton == 0)
        {
            //TODO: 세팅 저장
        }
    }

    public void OnClickBoxButton(int index)
    {
        HideYesOrNoMsgbox();
        clickedButton = index;
    }
    public void ShowSetting()
    {
        clickedButton = -1;
        settingPanel.SetActive(true);
    }

    private void HideYesOrNoMsgbox()
    {
        settingPanel.SetActive(false);
    }

    public int GetClickedButton()
    {
        while (clickedButton == -1)
        {

        }
        return clickedButton;
    }
}
