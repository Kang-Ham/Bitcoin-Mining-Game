using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cartoon : MonoBehaviour
{
    public Sprite[] cartoonSprites;

    private GameObject cartoonPanel;

    private int curCartoonIndex;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCartoonActive(bool flag)
    {
        cartoonPanel = GameObject.Find("PopupPanels").transform.Find("CartoonPanel").gameObject;
        cartoonPanel.SetActive(flag);
        curCartoonIndex = 0;

    }

    public void OnClickNextButton()
    {
        Image cartoonImage = GameObject.Find("CartoonImage").GetComponent<Image>();

        if (curCartoonIndex >= 2)
        {
            cartoonImage.sprite = cartoonSprites[0];
            SetCartoonActive(false);
        }
        else
        {
            curCartoonIndex += 1;
            cartoonImage.sprite = cartoonSprites[curCartoonIndex];
        }
    }
}
