using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClicksystem()
    {
        Debug.Log("시스템");
    }
    public void OnClickcomputer()
    {
        Debug.Log("컴퓨터");
    }
    public void OnClickGPU()
    {
        Debug.Log("그래픽카드");
    }
    public void OnClickskill()
    {
        Debug.Log("특전");
    }
    public void OnClickQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
