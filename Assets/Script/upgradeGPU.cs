using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class upgradeGPU : MonoBehaviour
{
    private system scriptSystem;
    private msgboxYesOrNo scriptMsgboxYesOrNo;
    private msgbox scriptMsgbox;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    async public void OnClickEvent(int curGPULevel)
    {
        if (!scriptSystem) scriptSystem = GameObject.Find("system").GetComponent<system>();
        if (!scriptMsgbox) scriptMsgbox = GameObject.Find("EventSystem").GetComponent<msgbox>();

        if (curGPULevel == scriptSystem.curGPULevel)
        {
            if (scriptSystem.curMoney >= scriptSystem.GPU_PRICES[curGPULevel+1])
            {
                askForAddPC(curGPULevel);
            }
            else
            {
                scriptMsgbox.showMsgbox("현금이 부족합니다.", "예");
                var task = Task.Run(() => scriptMsgbox.getClickedBtn());
                int clickedBtn = await task;
            }
                
        }
        else
        {
            scriptMsgbox.showMsgbox("You Can't Do That.", "예");
            var task = Task.Run(() => scriptMsgbox.getClickedBtn());
            int clickedBtn = await task;
        }
    }

    async public void askForAddPC(int curGPULevel)
    {
        if (!scriptMsgboxYesOrNo) scriptMsgboxYesOrNo = GameObject.Find("EventSystem").GetComponent<msgboxYesOrNo>();

        scriptMsgboxYesOrNo.showMsgboxYesOrNo("GPU를 강화하시겠습니까?", "예", "아니오");
        var task = Task.Run(() => scriptMsgboxYesOrNo.getClickedBtn());
        int clickedBtn = await task;

        if (clickedBtn == 1)
        {
            scriptSystem.setCurMoney(scriptSystem.curMoney - scriptSystem.GPU_PRICES[curGPULevel+1]);

            upgradeCurrentGPU(curGPULevel);
        }
    }

    public void upgradeCurrentGPU(int curGPULevel)
    {
        if (!scriptSystem) scriptSystem = GameObject.Find("system").GetComponent<system>();
        scriptSystem.gameBitcoinPerSecond *= scriptSystem.GPU_RATES[curGPULevel + 1] / scriptSystem.GPU_RATES[curGPULevel];
        scriptSystem.curGPULevel = curGPULevel + 1;
    }
}
