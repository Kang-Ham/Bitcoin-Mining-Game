using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddPcSpawner : MonoBehaviour
{
    public GameObject[] prefabAddPcImages;
    private GameSystem scriptGameSystem;
    private Tabpanel scriptTabpanel;

    void Start()
    {

    }

    public void MakeNewButton(int pcCount)
    {
        if(!scriptGameSystem) scriptGameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();
        if (!scriptTabpanel) scriptTabpanel = GameObject.Find("Canvas").GetComponent<Tabpanel>();


        if (pcCount >= scriptGameSystem.NUMBER_OF_PC_AT_EACH_TABLE * scriptGameSystem.LENGTH_OF_TABLE * scriptGameSystem.NUMBER_OF_MENU - 1)
        {        // 추가 가능한 마지막 PC의 경우 추가 안 함
            return;
        }

        if (pcCount % 2 == 0)
        {
            GameObject[] pcContents = GameObject.FindGameObjectsWithTag("comContent");
            pcContents[pcContents.Length - 1].transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            var spawn_content = Instantiate(prefabAddPcImages[(pcCount + 1) / 16]);
            spawn_content.tag = "comContent";
            spawn_content.transform.SetParent(gameObject.transform);
            spawn_content.transform.localScale = new Vector3(1f, 1f, 1f);
            spawn_content.transform.GetChild(1).gameObject.SetActive(false);
        }
        scriptTabpanel.LoadPcPrice(pcCount + 1);
    }

}