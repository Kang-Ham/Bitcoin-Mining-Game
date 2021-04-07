using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class com_menu_spawner : MonoBehaviour
{
    public GameObject[] com_content_imgPrefabs;

    void Start()
    {

    }

    public void makeNewButton(int PCCount)
    {
        if (PCCount % 2 == 0)
        {
            GameObject[] comContents = GameObject.FindGameObjectsWithTag("comContent");
            comContents[comContents.Length - 1].transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {

            Debug.Log(PCCount);
            var spawn_content = Instantiate(com_content_imgPrefabs[(PCCount+1) / 16]);
            spawn_content.tag = "comContent";
            spawn_content.transform.SetParent(gameObject.transform);
            spawn_content.transform.localScale = new Vector3(1f, 1f, 1f);
            spawn_content.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

}