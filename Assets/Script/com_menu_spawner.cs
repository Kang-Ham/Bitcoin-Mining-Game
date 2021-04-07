using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class com_menu_spawner: MonoBehaviour
{
    public GameObject com_1content_imgPrefab;
    private system scriptSystem;

    void Start()
    {
        scriptSystem = GameObject.Find("system").GetComponent<system>();
    }

    public void btn_spawn()
    {
        if(scriptSystem.PCs.Count < 15)
        {
            if(scriptSystem.PCs.Count % 2 == 0)
            {
                GameObject[] comContents = GameObject.FindGameObjectsWithTag("comContent");
                comContents[comContents.Length-1].transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                var spawn_content = Instantiate(com_1content_imgPrefab);
                spawn_content.transform.parent = gameObject.transform;
                spawn_content.transform.localScale = new Vector3(1f, 1f, 1f);
                spawn_content.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
        else if(scriptSystem.PCs.Count < 31)
        {
            Debug.Log("������ ��! �� ���� ���ڿ��� �����ϰ� ��");
        }
        else if(scriptSystem.PCs.Count < 47)
        {
            Debug.Log("��� ���� ����� �ʴ� ������ �� ����");
        }
        else
        {
            Debug.Log("Hello Word4");
        }

    }
}
