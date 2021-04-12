using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class UpgradeGpu : MonoBehaviour
{
    private GameSystem scriptGameSystem;
    private YesOrNoMsgbox scriptYesOrNoMsgbox;
    private Msgbox scriptMsgbox;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    async public void OnClickEvent(int currentGpuLevel)
    {
        if (!scriptGameSystem) scriptGameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();
        if (!scriptMsgbox) scriptMsgbox = GameObject.Find("EventSystem").GetComponent<Msgbox>();

        if (currentGpuLevel == scriptGameSystem.currentGpuLevel)
        {
            if (scriptGameSystem.currentMoney >= scriptGameSystem.GPU_PRICES[currentGpuLevel+1])
            {
                AskForAddPc(currentGpuLevel);
            }
            else
            {
                scriptMsgbox.ShowMsgbox("현금이 부족합니다.", "예");
                var task = Task.Run(() => scriptMsgbox.GetClickedButton());
                int clickedButton = await task;
            }
                
        }
        else
        {
            scriptMsgbox.ShowMsgbox("You Can't Do That.", "예");
            var task = Task.Run(() => scriptMsgbox.GetClickedButton());
            int clickedButton = await task;
        }
    }

    async public void AskForAddPc(int currentGpuLevel)
    {
        if (!scriptYesOrNoMsgbox) scriptYesOrNoMsgbox = GameObject.Find("EventSystem").GetComponent<YesOrNoMsgbox>();

        scriptYesOrNoMsgbox.ShowYesOrNoMsgbox("Gpu를 강화하시겠습니까?", "예", "아니오");
        var task = Task.Run(() => scriptYesOrNoMsgbox.GetClickedButton());
        int clickedButton = await task;

        if (clickedButton == 0)
        {
            scriptGameSystem.SetCurrentMoney(scriptGameSystem.currentMoney - scriptGameSystem.GPU_PRICES[currentGpuLevel+1]);

            UpgradeCurrentGpu(currentGpuLevel);
        }
    }

    public void UpgradeCurrentGpu(int currentGpuLevel)
    {
        if (!scriptGameSystem) scriptGameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();
        scriptGameSystem.gameBtcPerSecond *= scriptGameSystem.GPU_RATES[currentGpuLevel + 1] / scriptGameSystem.GPU_RATES[currentGpuLevel];
        scriptGameSystem.currentGpuLevel = currentGpuLevel + 1;
    }
}
