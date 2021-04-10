using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class clicker_event : MonoBehaviour
{
    private system scriptSystem;

    // Start is called before the first frame update
    void Start()
    {
        scriptSystem = GameObject.Find("system").GetComponent<system>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void clicker()
    {
        scriptSystem.curBitcoin += scriptSystem.hitPower;
    }

    public void clicker_upgrade()
    {
        scriptSystem.hitPower = 1.002f * scriptSystem.hitPower + 0.0000008f;
    }
}
