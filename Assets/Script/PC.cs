using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PC : MonoBehaviour
{
    private system scriptSystem;

    public string spriteName; //"notebook1" 
    private SpriteRenderer spriteR;
    public List<int> pos = null;

    // Start is called before the first frame update
    void Start()
    {
        scriptSystem = GameObject.Find("system").GetComponent<system>();

        spriteR = gameObject.GetComponent<SpriteRenderer>();
        spriteR.sprite = Resources.Load<Sprite>("Sprite/"+spriteName);
        Vector3 scenePos = new Vector3((float)(scriptSystem.SCENE_DISTANCE_BETWEEN_TABLE* pos[1]+ scriptSystem.SCENE_DISTANCE_BETWEEN_PC * pos[3]), (float)(-scriptSystem.SCENE_DISTANCE_BETWEEN_TABLE * pos[0]- scriptSystem.SCENE_DISTANCE_BETWEEN_PC * pos[2]), -1);
        transform.position = scriptSystem.START_SCENE_POS+ scenePos;
        Debug.Log(transform.position.ToString());
        if (pos[3]==1)
        {
            Vector3 prevScale = transform.localScale;
            prevScale[0] *= -1;
            transform.localScale = prevScale;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
