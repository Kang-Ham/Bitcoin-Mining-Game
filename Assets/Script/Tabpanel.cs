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

    public int isOverclockClicked = 0;
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
        if (!scriptGameSystem) scriptGameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();

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

                if (i == 0 && isOverclockClicked == 0) //Overclock ��
                {
                    LoadOverclockInformation();
                    isOverclockClicked = 1;
                }
                else if (i == 1 && isComputerClicked == 0) //Computer ��
                {
                    LoadAddPcButtons();
                    SetPcButtonInteractableFalse(true);
                    LoadPcInformation(0);
                    isComputerClicked = 1;
                }
                else if (i == 2 && isGpuClicked == 0) //Gpu ��
                {
                    LoadAllGpuInformation();
                    SetGpuButtonInteractableFalse(true);
                    isGpuClicked = 1;
                }
                else if (i == 3 && isBtcClicked == 0) //Btc ��
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

        if (flagIfSetAllButtons)
        {        //addButton ������ �����ϰ� ���� ��Ȱ��ȭ
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

        Text PcNameText = GameObject.FindGameObjectsWithTag("PcNameText")[pcCount].GetComponent<Text>();
        Text PcBpsText = GameObject.FindGameObjectsWithTag("PcBpsText")[pcCount].GetComponent<Text>();
        Text PcPriceText = GameObject.FindGameObjectsWithTag("PcPriceText")[pcCount].GetComponent<Text>();

        PcNameText.text = scriptGameSystem.PC_NAMES[pcType];
        PcBpsText.text = scriptGameSystem.PC_BTC_PER_SECOND[pcType].ToString("0." + new string('#', 8)) + "BPS";
        PcPriceText.text = string.Format("{0:n0}", scriptGameSystem.PC_PRICES[pcType]) + "��";
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

    public void LoadAllGpuInformation()
    {
        GameObject[] gpuNameTextGameObjects = GameObject.FindGameObjectsWithTag("GpuNameText");
        GameObject[] gpuRateTextGameObjects = GameObject.FindGameObjectsWithTag("GpuRateText");
        GameObject[] gpuPriceTextGameObjects = GameObject.FindGameObjectsWithTag("GpuPriceText");

        for (int i = 1; i < gpuNameTextGameObjects.Length + 1; i++)
        {
            gpuNameTextGameObjects[i - 1].GetComponent<Text>().text = scriptGameSystem.GPU_NAMES[i].ToString();
            gpuRateTextGameObjects[i - 1].GetComponent<Text>().text = "BPS ������: " + scriptGameSystem.GPU_RATES[i].ToString() + "��";
            gpuPriceTextGameObjects[i - 1].GetComponent<Text>().text = string.Format("{0:n0}", scriptGameSystem.GPU_PRICES[i]) + "��";
        }
    }


    public void LoadOverclockInformation()
    {
        if (!scriptGameSystem) scriptGameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();

        Text currentOverclockLevelText = GameObject.Find("currentOverclockLevelText").GetComponent<Text>();
        Text currentOverclockBtcText = GameObject.Find("currentOverclockBtcText").GetComponent<Text>();

        currentOverclockLevelText.text = "���� ����Ŭ�� " + scriptGameSystem.currentOverclockLevel.ToString() + "����";
        currentOverclockBtcText.text = "��ġ�� " + scriptGameSystem.currentOverclockPerTouch.ToString("0." + new string('#', 9)) + "BTC";

        Text overclockBtcText = GameObject.Find("OverclockBtcText").GetComponent<Text>();
        Text overclockPriceText = GameObject.Find("OverclockPriceText").GetComponent<Text>();

        overclockBtcText.text = "��ġ�� + " + scriptGameSystem.nextOverclockPerTouch.ToString("0." + new string('#', 10)) + "BTC";
        overclockPriceText.text = scriptGameSystem.currentOverclockPrice.ToString() + "��";

        Text tenOverclockBtcText = GameObject.Find("tenOverclockBtcText").GetComponent<Text>();
        Text tenOverclockPriceText = GameObject.Find("tenOverclockPriceText").GetComponent<Text>();

        tenOverclockBtcText.text = "��ġ�� + " + scriptGameSystem.nextTenOverclockPerTouch.ToString("0." + new string('#', 8)) + "BTC";
        tenOverclockPriceText.text = scriptGameSystem.currentTenOverclockPrice.ToString() + "��";
    }
}
