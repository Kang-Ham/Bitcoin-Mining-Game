using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PC : MonoBehaviour
{
    private system scriptSystem;
    private SpriteRenderer spriteR;

    public string spriteName; //"pc1" 
    public List<int> pos = null;

    public int level;
    public float bitcoinPerTimeSlice;

    // Start is called before the first frame update
    void Start()
    {
        scriptSystem = GameObject.Find("system").GetComponent<system>();
        spriteR = gameObject.GetComponent<SpriteRenderer>();

        setSprite(spriteName);
        setPos();

        scriptSystem.gameBitcoinPerTimeSlice += bitcoinPerTimeSlice;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setSprite(string _spriteName)
    {
        spriteR.sprite = Resources.Load<Sprite>("Sprite/" + _spriteName);
    }

    private void setPos()
    {
        if (!scriptSystem) scriptSystem = GameObject.Find("system").GetComponent<system>();

        Vector3 scenePos = new Vector3((float)(scriptSystem.SCENE_DISTANCE_BETWEEN_TABLE * pos[1] + scriptSystem.SCENE_DISTANCE_BETWEEN_PC * pos[3]), (float)(-scriptSystem.SCENE_DISTANCE_BETWEEN_TABLE * pos[0] - scriptSystem.SCENE_DISTANCE_BETWEEN_PC * pos[2]), -1);

        transform.position = scriptSystem.START_SCENE_POS + scenePos;
        if (pos[3] == 1)
        {
            Vector3 prevScale = transform.localScale;
            prevScale[0] *= -1;
            transform.localScale = prevScale;
        }
    }
}
