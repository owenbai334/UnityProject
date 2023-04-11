using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevilSelect : MonoBehaviour
{
    bool IsSelect = false;
    bool canSee = false;
    Image image;
    public Sprite[] sprite; 
    public Text Num;
    [HideInInspector]
    void Awake() 
    {
        image = GetComponent<Image>();       
    }
    void Start() 
    {
        Text text = transform.parent.GetChild(0).Find("Num").GetComponent<Text>();
        if(text.text==gameObject.transform.Find("Num").GetComponent<Text>().text)
        {
            IsSelect= true;
        }
        if(IsSelect)
        {
            image.sprite = sprite[MapManager.Instance.SelectMap];
            transform.Find("Num").gameObject.SetActive(true);
        }
    }
    void Update() 
    {
        if(!this.gameObject.activeSelf)
        {
            canSee=false;
        }
        else
        {
            canSee=true;
        }
        if(canSee&&IsSelect)
        {
            image.sprite = sprite[MapManager.Instance.SelectMap];
            transform.Find("Num").gameObject.SetActive(true);
        }
    }
}
