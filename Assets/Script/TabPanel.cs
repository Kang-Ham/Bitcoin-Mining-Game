using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabPanel : MonoBehaviour
{
    public List<Tabbutton> tabButtons;
    public List<GameObject> contentPanels;

    public void ClickTab(int id)
    {
        for(int i = 0; i<contentPanels.Count; i++)
        {
            if(i==id)
            {
                contentPanels[i].SetActive(true);
            }
            else
            {
                contentPanels[i].SetActive(false);
            }
        }
    }
}
