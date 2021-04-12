using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pc : MonoBehaviour
{
    private GameSystem scriptGameSystem;
    private SpriteRenderer spriteR;

    public string spriteName; //"pc1" 
    public List<int> pos = null;

    public float btcPerSecond;

    // Start is called before the first frame update
    void Start()
    {
        scriptGameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();
        spriteR = gameObject.GetComponent<SpriteRenderer>();

        SetSprite(spriteName);
        SetPos();

        scriptGameSystem.gameBtcPerSecond += btcPerSecond;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSprite(string spriteName)
    {
        spriteR.sprite = Resources.Load<Sprite>("Sprite/" + spriteName);
    }

    private void SetPos()
    {
        if (!scriptGameSystem) scriptGameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();

        Vector3 scenePos = new Vector3((float)(scriptGameSystem.SCENE_DISTANCE_BETWEEN_TABLE * pos[1] + scriptGameSystem.SCENE_DISTANCE_BETWEEN_PC * pos[3]), (float)(-scriptGameSystem.SCENE_DISTANCE_BETWEEN_TABLE * pos[0] - scriptGameSystem.SCENE_DISTANCE_BETWEEN_PC * pos[2]), -1);

        transform.position = scriptGameSystem.START_SCENE_POS + scenePos;
        if (pos[3] == 1)
        {
            Vector3 prevScale = transform.localScale;
            prevScale[0] *= -1;
            transform.localScale = prevScale;
        }
    }
}
