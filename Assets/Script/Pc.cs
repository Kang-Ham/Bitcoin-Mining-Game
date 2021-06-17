using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pc : MonoBehaviour
{
    private GameSystem scriptGameSystem;
    private SpriteRenderer spriteR;

    public int spriteType; //1 
    public List<int> pos = null;

    public float btcPerSecond;

    // Start is called before the first frame update
    void Start()
    {
        spriteR = gameObject.GetComponent<SpriteRenderer>();

        SetSprite(spriteType);
        SetPos();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSprite(int spriteType)
    {
        //TODO: PC234 구분해서 적용
        Animator animator = gameObject.GetComponent<Animator>();
        animator.runtimeAnimatorController = Resources.Load("Animation/Pc"+ spriteType.ToString() + "AnimatorController") as RuntimeAnimatorController;
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
