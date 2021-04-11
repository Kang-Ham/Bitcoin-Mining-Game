using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class settingBtn : MonoBehaviour
{
    public int clickedBtn;
    private GameObject settingPanel, settingBtn1, settingBtn2;

    // Start is called before the first frame update
    void Start()
    {
        settingPanel = GameObject.Find("Canvas").transform.Find("setting_panel").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    async public void OnClickEvent()
    {
        showSetting();
        var task = Task.Run(() => getClickedBtn());
        int clickedBtn = await task;

        if(clickedBtn == 1)
        {
            //TODO: 세팅 저장
        }
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

    public void showSetting()
    {
        clickedBtn = -1;
        settingPanel.SetActive(true);
    }

    private void hideMsgboxYesOrNo()
    {
        settingPanel.SetActive(false);
    }

    public int getClickedBtn()
    {
        while (clickedBtn == -1)
        {

        }
        return clickedBtn;
    }
}
