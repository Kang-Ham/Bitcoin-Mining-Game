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

    // Start is called before the first frame update
    void Start()
    {
        scriptSystem = GameObject.Find("system").GetComponent<system>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ClickTab(int id)
    {
        for(int i = 0; i < contentsPanels.Count; i++)
        {
            if(i==id)
            {
                contentsPanels[i].SetActive(true);
                selectMenu(i);
            }
            else
            {
                contentsPanels[i].SetActive(false);
            }
            
        }
    }

    public void selectMenu(int index)
    {
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
                newColor.a = 1f;
                menuTexts[i].color = newColor;
            }
            else
            {
                newColor.a = 0.5f;
                menuTexts[i].color = newColor;
            }
        }
    }
}
