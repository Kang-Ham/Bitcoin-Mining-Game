using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tabpanel : MonoBehaviour
{
    public List<GameObject> contentsPanels;
    private GameSystem scriptGameSystem;
    private GameObject[] objMenuTexts;
    private Text[] menuTexts;

    private int isOverclockClicked = 0;
    private int isComputerClicked = 0;
    private int isGpuClicked = 0;

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

                if (i == 0&&isOverclockClicked == 0)
                {
                    LoadOverclockPrice();
                    isOverclockClicked = 1;
                }
                else if (i == 1&&isComputerClicked == 0) //Computer �� ó������ ������ ��
                {
                    LoadAddPcButtons();
                    SetButtonExceptLastInteractableFalse();
                    LoadPcPrice(0);
                    isComputerClicked = 1;
                }
                else if(i == 2&&isGpuClicked == 0) //Gpu ��
                {
                    LoadGpuPrices();
                    isGpuClicked = 1;
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
        AddPcSpawner scriptAddPcSpawner = GameObject.FindGameObjectWithTag("content").GetComponent<AddPcSpawner>();
        for (int i = 0; i < scriptGameSystem.currentPcList.Count; i++) scriptAddPcSpawner.MakeNewButton(i);
    }

    public void SetButtonExceptLastInteractableFalse()
    {
        int i;
        //addButton ������ �����ϰ� ���� ��Ȱ��ȭ
        GameObject[] addPcButtons = GameObject.FindGameObjectsWithTag("addBtn");
        for (i = 0; i < addPcButtons.Length - 1; i++) addPcButtons[i].GetComponent<Button>().interactable = false;

        if(scriptGameSystem.currentPcList.Count >= scriptGameSystem.NUMBER_OF_PC_AT_EACH_TABLE * scriptGameSystem.LENGTH_OF_TABLE * scriptGameSystem.NUMBER_OF_MENU)
        {
            addPcButtons[scriptGameSystem.NUMBER_OF_PC_AT_EACH_TABLE * scriptGameSystem.LENGTH_OF_TABLE * scriptGameSystem.NUMBER_OF_MENU-1].GetComponent<Button>().interactable = false;
        }
    }
    public void LoadPcPrice(int pcCount)
    {
        int pcType = pcCount / 16;
        GameObject addPcButton = GameObject.FindGameObjectsWithTag("addBtn")[pcCount];
        
        Text currentPcText = addPcButton.transform.GetChild(0).GetComponent<Text>();
        currentPcText.text = scriptGameSystem.PC_PRICES[pcType].ToString();
    }

    public void LoadGpuPrices()
    {
        GameObject[] GpuTexts = GameObject.FindGameObjectsWithTag("GPU_text");
        for(int i = 1; i < GpuTexts.Length+1; i++)
        {
            GpuTexts[i-1].GetComponent<Text>().text = scriptGameSystem.GPU_PRICES[i].ToString();
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
