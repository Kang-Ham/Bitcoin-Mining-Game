using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class system : MonoBehaviour
{
    public int NUMBER_OF_PC_AT_EACH_TABLE = 4;
    public int LENGTH_OF_TABLE = 4; //테이블 4*4    
    public Vector3 START_SCENE_POS = new Vector3(-3.525f,7.952f,-1);
    public float SCENE_DISTANCE_BETWEEN_TABLE = 1.985f;
    public float SCENE_DISTANCE_BETWEEN_PC = 0.855f;

    //TODO: Game Load할 때 불러오기
    public float curBitcoin = 0;
    public int cntNotebook = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
}
