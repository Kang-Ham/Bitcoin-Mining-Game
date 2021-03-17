using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnAddNoteBook : MonoBehaviour
{
    GameObject system;
    system scriptSystem;
    // Start is called before the first frame update
    void Start()
    {
        GameObject system = GameObject.Find("system");
        system scriptSystem = system.GetComponent<system>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void onClick()
    {
        

    }

}
