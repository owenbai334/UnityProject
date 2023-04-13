using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevilSelect : MonoBehaviour
{
    public bool IsSelect = false;
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
        else
        {
            Lock();
        }
        ExChange();
    }
    public void ChangeColor()
    {
        Text text = transform.GetChild(0).GetComponent<Text>();
        image.sprite = sprite[MapManager.SelectMap];
        transform.Find("Num").gameObject.SetActive(true);
        var SaveData = SaveJson.LoadFromJson<SaveData>(GameManager.PLAYER_DATA_FILE_NAME);
        int count = SaveData.mapNum[MapManager.SelectMap * PanelGrid.gridNums + System.Convert.ToInt32(text.text) - 1];
        if (count == 3)
        {
            for (int i = 0; i < count; i++)
            {
                stars[i].SetActive(true);
            }
        }
        else if (count == 2)
        {
            stars[2].SetActive(false);
            stars[1].SetActive(true);
            stars[0].SetActive(true);
        }
        else if (count == 1)
        {
            stars[2].SetActive(false);
            stars[1].SetActive(false);
            stars[0].SetActive(true);
        }
        else
        {
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].SetActive(false);
            }
        }
    }
    void Lock()
    {
        image.sprite = sprite[8];
        transform.Find("Num").gameObject.SetActive(false);
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].SetActive(false);
        }
    }
    public void Select()
    {
        if (IsSelect)
        {
            SelectNum = gameObject.transform.GetChild(0).GetComponent<Text>().text;
            GameManager.LevilNum = System.Convert.ToInt32(SelectNum);
            SceneManager.LoadScene("Game");
        }
    }
    void ExChange()
    {
        Text text = transform.GetChild(0).GetComponent<Text>();
        var SaveData = SaveJson.LoadFromJson<SaveData>(GameManager.PLAYER_DATA_FILE_NAME);
        int index = MapManager.SelectMap * PanelGrid.gridNums + System.Convert.ToInt32(text.text) - 2;
        if (index >= 0 && index < 8 * PanelGrid.gridNums)
        {
            if (SaveData.mapNum[index] > 0 || SaveData.mapNum[index + 1] > 0 || (index + 2) % PanelGrid.gridNums == 1)
            {
                IsSelect = true;
            }
            else if (SaveData.mapNum[index] == 0)
            {
                IsSelect = false;
            }
        }
    }
}
