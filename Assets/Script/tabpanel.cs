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

                if (i == 1&&isComputerClicked==0)
                {
                    loadAddPCButtons();
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
        com_menu_spawner scriptComMenuSpawner = GameObject.FindGameObjectWithTag("content").GetComponent<com_menu_spawner>();
        for(int i=0;i< scriptSystem.PCs.Count; i++) 
            scriptComMenuSpawner.makeNewButton(i);
    }
}
