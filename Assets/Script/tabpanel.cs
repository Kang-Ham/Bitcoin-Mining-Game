using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tabpanel : MonoBehaviour
{
    public List<tabbutton> tabButtons;
    public List<GameObject> contentsPanels;
    private system scriptSystem;
    private GameObject[] objMenuTexts;
    private Text[] menuTexts;

    private int isComputerClicked = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void selectMenu(int index)
    {
        if(!scriptSystem) scriptSystem = GameObject.Find("system").GetComponent<system>();

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

            selectMenu(scriptSystem.selectedMenu);
            return;
        }

        scriptSystem.selectedMenu = index;
        Color newColor = new Color();
        newColor.r = 1f;
        newColor.g = 1f;
        newColor.b = 1f;
        for (int i = 0; i < scriptSystem.NUMBER_OF_MENU; i++)
        {
            if (i == index)
            {
                contentsPanels[i].SetActive(true);
                newColor.a = 1f;
                menuTexts[i].color = newColor;

                if (i == 1&&isComputerClicked==0) //Computer 탭 처음으로 눌렀을 때
                {
                    loadAddPCButtons();
                    setButtonExceptLastInteractableFalse();
                    isComputerClicked = 1;
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

    public void loadAddPCButtons()
    {
        int i;
        //PC 갯수만큼 버튼 추가
        com_menu_spawner scriptComMenuSpawner = GameObject.FindGameObjectWithTag("content").GetComponent<com_menu_spawner>();
        for (i = 0; i < scriptSystem.PCs.Count; i++) scriptComMenuSpawner.makeNewButton(i);
    }

    public void setButtonExceptLastInteractableFalse()
    {
        int i;
        //addBtn 마지막 제외하고 전부 비활성화
        GameObject[] addPCBtns = GameObject.FindGameObjectsWithTag("addBtn");
        for (i = 0; i < addPCBtns.Length - 1; i++) addPCBtns[i].GetComponent<Button>().interactable = false;

        if(scriptSystem.PCs.Count >= scriptSystem.NUMBER_OF_PC_AT_EACH_TABLE * scriptSystem.LENGTH_OF_TABLE * scriptSystem.NUMBER_OF_MENU)
        {
            addPCBtns[scriptSystem.NUMBER_OF_PC_AT_EACH_TABLE * scriptSystem.LENGTH_OF_TABLE * scriptSystem.NUMBER_OF_MENU-1].GetComponent<Button>().interactable = false;
        }
    }
}
