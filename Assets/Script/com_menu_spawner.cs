using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class com_menu_spawner : MonoBehaviour
{
    public GameObject[] com_content_imgPrefabs;
    private system scriptSystem;

    void Start()
    {

    }

    public void makeNewButton(int PCCount)
    {
        if(!scriptSystem) scriptSystem = GameObject.Find("system").GetComponent<system>();

        if (PCCount >= scriptSystem.NUMBER_OF_PC_AT_EACH_TABLE * scriptSystem.LENGTH_OF_TABLE * scriptSystem.NUMBER_OF_MENU - 1)
        {        // 추가 가능한 마지막 PC의 경우 추가 안 함
            return;
        }

        if (PCCount % 2 == 0)
        {
            GameObject[] comContents = GameObject.FindGameObjectsWithTag("comContent");
            comContents[comContents.Length - 1].transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            var spawn_content = Instantiate(com_content_imgPrefabs[(PCCount+1) / 16]);
            spawn_content.tag = "comContent";
            spawn_content.transform.SetParent(gameObject.transform);
            spawn_content.transform.localScale = new Vector3(1f, 1f, 1f);
            spawn_content.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

}