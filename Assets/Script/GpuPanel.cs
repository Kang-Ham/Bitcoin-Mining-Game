using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class GpuPanel : MonoBehaviour
{
    private GameSystem scriptGameSystem;
    private YesOrNoMsgbox scriptYesOrNoMsgbox;
    private Msgbox scriptMsgbox;
    private Tabpanel scriptTabpanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickEvent(int currentGpuLevel)
    {
        if (!scriptGameSystem) scriptGameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();
        if (!scriptMsgbox) scriptMsgbox = GameObject.Find("EventSystem").GetComponent<Msgbox>();

        if (currentGpuLevel == scriptGameSystem.currentGpuLevel)
        {
            if (scriptGameSystem.currentMoney >= scriptGameSystem.GPU_PRICES[currentGpuLevel+1])
            {
                AskForAddGpu(currentGpuLevel);
            }
            else
            {
                scriptMsgbox.ShowMsgbox("현금이 부족합니다.", "예");
            }
                
        }
        else
        {
            scriptMsgbox.ShowMsgbox("아직 구입할 수 없습니다.", "예");
        }
    }

    public void AskForAddGpu(int currentGpuLevel)
    {
        if (!scriptYesOrNoMsgbox) scriptYesOrNoMsgbox = GameObject.Find("EventSystem").GetComponent<YesOrNoMsgbox>();
        if (!scriptTabpanel) scriptTabpanel = GameObject.Find("Canvas").GetComponent<Tabpanel>();

        scriptYesOrNoMsgbox.ShowYesOrNoMsgbox("해당 Gpu를 구입하시겠습니까?", "예", "아니오",
            (clickedButton) =>
            {
                if (clickedButton == 0)
                {
                    scriptTabpanel.SetGpuButtonInteractableFalse(false);

                    scriptGameSystem.SetCurrentMoney(scriptGameSystem.currentMoney - scriptGameSystem.GPU_PRICES[currentGpuLevel + 1]);
                    scriptGameSystem.currentGpuLevel = currentGpuLevel + 1;
                }
            }
            );
    }
}
