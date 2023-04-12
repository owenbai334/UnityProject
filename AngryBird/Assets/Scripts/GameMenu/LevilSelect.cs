using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevilSelect : MonoBehaviour
{
    bool IsSelect = false;
    Image image;
    public Sprite[] sprite;
    public Text Num;
    public GameObject[] stars;
    public static string SelectNum = "0"; 
    void Awake()
    {
        image = GetComponent<Image>();
    }
    void Start()
    {
        Text text = transform.parent.GetChild(0).Find("Num").GetComponent<Text>();
        if (text.text == gameObject.transform.Find("Num").GetComponent<Text>().text)
        {
            IsSelect = true;
        }
        if (IsSelect)
        {
            ChangeColor();
        }
    }
    void Update() 
    {
        if (IsSelect)
        {
            ChangeColor();
        }
    }
    public void ChangeColor()
    {
        image.sprite = sprite[MapManager.SelectMap];
        transform.Find("Num").gameObject.SetActive(true);
        var SaveData = SaveJson.LoadFromJson<SaveData>(GameManager.PLAYER_DATA_FILE_NAME);
        int count = SaveData.mapNum[MapManager.SelectMap*55+GameManager.LevilNum-1];
        if(count>0)
        {
            for (int i = 0; i < count; i++)
            {
                stars[i].SetActive(true);
            }
        }
    }
    public void Select()
    {
        if(IsSelect)
        {
            SelectNum = gameObject.transform.GetChild(0).GetComponent<Text>().text;
            GameManager.LevilNum = System.Convert.ToInt32(SelectNum);
            SceneManager.LoadScene("Game");
        }
    } 
}
