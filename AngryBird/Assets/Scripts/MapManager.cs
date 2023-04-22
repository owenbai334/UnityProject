using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class MapManager : MonoBehaviour
{
    bool canEnable = true;
    public int[] StartsNum;
    #region "Hide"
    [HideInInspector]
    public Text[] StartsText;
    [HideInInspector]
    public MapSelect[] mapSelect;
    [HideInInspector]
    public GameObject[] Objects;
    [HideInInspector]        
    public Text[] StartCount;
    [HideInInspector]
    public GameObject[] buttons;
    [HideInInspector]
    public Button[] Buttons;
    [HideInInspector]
    public Button[] Configs;
    [HideInInspector]
    public Sprite[] sprites;
    [HideInInspector]
    public Text ConfigText;
    #endregion
    public static int SelectMap=0;
    static MapManager instance;
    public static MapManager Instance { get => instance; set => instance = value; }
    public void Awake()
    {
        instance = this;
        for (int i = 0; i < StartsText.Length; i++)
        {
            StartsText[i].text = StartsNum[i].ToString();
            mapSelect[i].StartsNum = StartsNum[i];
        }
        for (int i = 0; i < StartCount.Length; i++)
        {
            var SaveData = SaveJson.LoadFromJson<SaveData>(GameManager.PLAYER_DATA_FILE_NAME);
            int tempNum = 0;
            for (int j = i*(SaveData.mapNum.Length/StartCount.Length); j < (i+1)*(SaveData.mapNum.Length/StartCount.Length); j++)
            {
                if(SaveData.mapNum[j]!=0)
                {
                    tempNum+=SaveData.mapNum[j];
                }
            }
            StartCount[i].text = $"{tempNum}/{3*PanelGrid.gridNums}";
        }
    }
    public void ReturnLevil()
    {
        Objects[0].SetActive(false);
        Objects[1].SetActive(true);
        buttons[0].SetActive(true);
        buttons[1].SetActive(true);
    }
    public void ReturnMenu()
    {
        SceneManager.LoadScene("Main");
    }
    public void Config()
    {
        Objects[3].SetActive(false);
        Objects[4].SetActive(false);
        Configs[0].image.sprite = sprites[0];
        Configs[1].image.sprite = sprites[0];
        Objects[2].SetActive(canEnable);
        canEnable = !canEnable;
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].enabled = canEnable;
        }
    }
    public void HowToPlay()
    {
        Objects[3].SetActive(false);
        Objects[4].SetActive(true);
        Configs[0].image.sprite = sprites[0];
        Configs[1].image.sprite = sprites[1];
    }
    public void ConfigSetting()
    {
        ConfigText.text = "總星星數:"+ GameManager.Instance.TotalStar.ToString();
        Objects[3].SetActive(true);
        Objects[4].SetActive(false);
        Configs[0].image.sprite = sprites[1];
        Configs[1].image.sprite = sprites[0];
    }
    public void DeleteSave()
    {
        GameManager.realStar = 0;
        for (int i = 0; i < GameManager.mapNum.Length; i++)
        {
            GameManager.mapNum[i]=0;
        }
        GameManager.Instance.SaveStar();
        GameManager.Instance.Awake();
        MapManager.instance.Awake();
        ConfigSetting();
    }
}
