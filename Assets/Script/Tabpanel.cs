using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tabpanel : MonoBehaviour
{
    public List<GameObject> contentsPanels;
    private GameObject[] objMenuTexts;
    private Text[] menuTexts;

    private PcPanel scriptPcPanel;
    private GameSystem scriptGameSystem;
    private BtcPanel scriptBtcPanel;

    private int isOverclockClicked = 0;
    private int isComputerClicked = 0;
    private int isGpuClicked = 0;
    private int isBtcClicked = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SelectMenu(int index)
    {
        if(!scriptGameSystem) scriptGameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();

        if (index == -1)
        {
            if (objMenuTexts == null)
            {
                objMenuTexts = GameObject.FindGameObjectsWithTag("MenuText");
                menuTexts = new Text[objMenuTexts.Length];
                for (int i = 0; i < objMenuTexts.Length; i++)
                {
                    menuTexts[i] = objMenuTexts[i].GetComponent<Text>();
                }
            }

            SelectMenu(scriptGameSystem.selectedMenu);
            return;
        }

        scriptGameSystem.selectedMenu = index;
        Color newColor = new Color();
        newColor.r = 1f;
        newColor.g = 1f;
        newColor.b = 1f;
        for (int i = 0; i < scriptGameSystem.NUMBER_OF_MENU; i++)
        {
            if (i == index)
            {
                contentsPanels[i].SetActive(true);
                newColor.a = 1f;
                menuTexts[i].color = newColor;

                if (i == 0&&isOverclockClicked == 0) //Overclock 탭
                {
                    LoadOverclockPrice();
                    isOverclockClicked = 1;
                }
                else if (i == 1&&isComputerClicked == 0) //Computer 탭
                {
                    LoadAddPcButtons();
                    SetPcButtonInteractableFalse(true);
                    LoadPcInformation(0);
                    isComputerClicked = 1;
                }
                else if(i == 2&&isGpuClicked == 0) //Gpu 탭
                {
                    LoadGpuInformation();
                    SetGpuButtonInteractableFalse(true);
                    isGpuClicked = 1;
                }else if(i == 3&&isBtcClicked == 0) //Btc 탭
                {
                    if (!scriptBtcPanel) scriptBtcPanel = GameObject.Find("EventSystem").GetComponent<BtcPanel>();
                    scriptBtcPanel.UpdateCurrentBtcPrice();
                    isBtcClicked = 1;
                }
            }
            else
            {
                newColor.a = 0.5f;
                menuTexts[i].color = newColor;
                contentsPanels[i].SetActive(false);
            }
        }
    }

    public void LoadAddPcButtons()
    {
        if (!scriptPcPanel) scriptPcPanel = GameObject.Find("EventSystem").GetComponent<PcPanel>();
        for (int i = 0; i < scriptGameSystem.currentPcList.Count; i++) scriptPcPanel.MakeNewButton(i);
    }

    public void SetPcButtonInteractableFalse(Boolean flagIfSetAllButtons)
    {
        GameObject[] buyPcButtons = GameObject.FindGameObjectsWithTag("BuyPcButton");

        if(flagIfSetAllButtons)
        {        //addButton 마지막 제외하고 전부 비활성화
            for (int i = 0; i < buyPcButtons.Length - 1; i++)
            {
                buyPcButtons[i].GetComponent<Button>().interactable = false;
            }

            if (scriptGameSystem.currentPcList.Count >= scriptGameSystem.NUMBER_OF_PC_AT_EACH_TABLE * scriptGameSystem.LENGTH_OF_TABLE * scriptGameSystem.NUMBER_OF_MENU)
            {
                buyPcButtons[scriptGameSystem.NUMBER_OF_PC_AT_EACH_TABLE * scriptGameSystem.LENGTH_OF_TABLE * scriptGameSystem.NUMBER_OF_MENU - 1].GetComponent<Button>().interactable = false;
            }
        }
        else
        {
            buyPcButtons[buyPcButtons.Length - 1].GetComponent<Button>().interactable = false;
        }
    }

    public void LoadPcInformation(int pcCount)
    {
        int pcType = pcCount / 16;
        GameObject addPcButton = GameObject.FindGameObjectsWithTag("AddButton")[pcCount];

        Text currentPcBpsText = addPcButton.transform.GetChild(2).GetComponent<Text>();
        Text currentPcPriceText = addPcButton.transform.GetChild(3).GetComponent<Text>();

        currentPcBpsText.text = scriptGameSystem.PC_BTC_PER_SECOND[pcType].ToString("0." + new string('#', 8))+"BPS";
        currentPcPriceText.text = string.Format("{0:n0}", scriptGameSystem.PC_PRICES[pcType])+"원";
    }


    public void SetGpuButtonInteractableFalse(Boolean flagIfSetAllButtons)
    {
        GameObject[] buyGpuButtons = GameObject.FindGameObjectsWithTag("BuyGpuButton");

        if (flagIfSetAllButtons)
        {        
            for (int i = 0; i < scriptGameSystem.currentGpuLevel; i++)
            {
                buyGpuButtons[i].GetComponent<Button>().interactable = false;
            }
        }
        else
        {
            buyGpuButtons[scriptGameSystem.currentGpuLevel].GetComponent<Button>().interactable = false;
        }
    }

    public void LoadGpuInformation()
    {
        GameObject[] GpuNameTexts = GameObject.FindGameObjectsWithTag("GpuNameText");
        GameObject[] GpuRateTexts = GameObject.FindGameObjectsWithTag("GpuRateText");
        GameObject[] GpuPriceTexts = GameObject.FindGameObjectsWithTag("GpuPriceText");

        for (int i = 1; i < GpuNameTexts.Length+1; i++)
        {
            GpuNameTexts[i - 1].GetComponent<Text>().text = scriptGameSystem.GPU_NAMES[i].ToString();
            GpuRateTexts[i-1].GetComponent<Text>().text = "BPS 증가율: " + scriptGameSystem.GPU_RATES[i].ToString()+"배";
            GpuPriceTexts[i - 1].GetComponent<Text>().text = string.Format("{0:n0}", scriptGameSystem.GPU_PRICES[i]) + "원";
        }
    }


    public void LoadOverclockPrice()
    {
        GameObject overclockText = GameObject.Find("OverclockText");
        
        if (scriptGameSystem.currentOverclockLevel < scriptGameSystem.BIFURCATION_OF_OVERCLOCK+1)
        {
            overclockText.GetComponent<Text>().text = Convert.ToUInt64(scriptGameSystem.BTC_AT_FIRST_TOUCH * 10 * scriptGameSystem.currentBtcPrice * Math.Pow(scriptGameSystem.COEFFICIENT_OF_OVERCLOCK, scriptGameSystem.currentOverclockLevel - scriptGameSystem.BIFURCATION_OF_OVERCLOCK * Math.Truncate(scriptGameSystem.currentOverclockLevel / scriptGameSystem.BIFURCATION_OF_OVERCLOCK) - 1)).ToString();
        }
        else
        {
            overclockText.GetComponent<Text>().text = Convert.ToUInt64(scriptGameSystem.BTC_AT_FIRST_TOUCH * 10 * scriptGameSystem.currentBtcPrice * Math.Pow(scriptGameSystem.COEFFICIENT_OF_OVERCLOCK, scriptGameSystem.currentOverclockLevel - scriptGameSystem.BIFURCATION_OF_OVERCLOCK * Math.Truncate(scriptGameSystem.currentOverclockLevel / scriptGameSystem.BIFURCATION_OF_OVERCLOCK) - 1) + scriptGameSystem.BTC_AT_FIRST_TOUCH * 10 * scriptGameSystem.currentBtcPrice * Math.Pow(scriptGameSystem.COEFFICIENT_OF_OVERCLOCK, scriptGameSystem.BIFURCATION_OF_OVERCLOCK * Math.Truncate(scriptGameSystem.currentOverclockLevel / scriptGameSystem.BIFURCATION_OF_OVERCLOCK))).ToString();
        }  
    }
}
