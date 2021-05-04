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
                scriptMsgbox.ShowMsgbox("������ �����մϴ�.", "��");
            }
                
        }
        else
        {
            scriptMsgbox.ShowMsgbox("���� ������ �� �����ϴ�.", "��");
        }
    }

    public void AskForAddGpu(int currentGpuLevel)
    {
        if (!scriptYesOrNoMsgbox) scriptYesOrNoMsgbox = GameObject.Find("EventSystem").GetComponent<YesOrNoMsgbox>();
        if (!scriptTabpanel) scriptTabpanel = GameObject.Find("Canvas").GetComponent<Tabpanel>();

        scriptYesOrNoMsgbox.ShowYesOrNoMsgbox("�ش� Gpu�� �����Ͻðڽ��ϱ�?", "��", "�ƴϿ�",
            (clickedButton) =>
            {
                if (clickedButton == 0)
                {
                    scriptTabpanel.SetGpuButtonInteractableFalse(false);

                    scriptGameSystem.SetCurrentMoney(scriptGameSystem.currentMoney - scriptGameSystem.GPU_PRICES[currentGpuLevel + 1]);

                    UpgradeCurrentGpu(currentGpuLevel);
                }
            }
            );
    }

    public void UpgradeCurrentGpu(int currentGpuLevel)
    {
        if (!scriptGameSystem) scriptGameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();
        scriptGameSystem.gameBtcPerSecond *= scriptGameSystem.GPU_RATES[currentGpuLevel + 1] / scriptGameSystem.GPU_RATES[currentGpuLevel];
        scriptGameSystem.currentGpuLevel = currentGpuLevel + 1;
    }
}
