using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class system : MonoBehaviour
{
    //Warning: Unity - SampleScene - system - Inspector - System 에서 초기화 해줘야함
    public int NUMBER_OF_PC_AT_EACH_TABLE;
    public int LENGTH_OF_TABLE; //테이블 4*4    
    public int NUMBER_OF_MENU; //메뉴버튼 갯수
    public Vector3 START_SCENE_POS;
    public float SCENE_DISTANCE_BETWEEN_TABLE;
    public float SCENE_DISTANCE_BETWEEN_PC;
    public float VALUE_TIME_SLICE_SECOND; //1=> 1초에 1번씩 비트코인 갱신

    //TODO: Game Load할 때 불러오기
    public float curBitcoin;
    public int cntNotebook;
    public float gameBitcoinPerTimeSlice;

    public int selectedMenu;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("setCurBitcoinOnRunning", VALUE_TIME_SLICE_SECOND);
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator setCurBitcoinOnRunning(float delay)
    {
        curBitcoin += gameBitcoinPerTimeSlice* VALUE_TIME_SLICE_SECOND;
        yield return new WaitForSeconds(delay);
        StartCoroutine("setCurBitcoinOnRunning", VALUE_TIME_SLICE_SECOND);
    }
}
