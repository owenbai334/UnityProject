using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
class SaveData
{
    public int TotalStar;
    public int[] mapNum = new int[8 * PanelGrid.gridNums];
}
public class GameManager : MonoBehaviour
{
    public const string PLAYER_DATA_FILE_NAME = "player.game";
    static GameManager instance;
    public static GameManager Instance { get => instance; set => instance = value; }
    Vector3 originPosition;
    int starLength = 0;
    int starsNum;
    public static int[] mapNum = new int[8 * PanelGrid.gridNums];
    #region "Hide"
    [HideInInspector]
    public GameObject[] Stars;
    [HideInInspector]
    public AudioSource[] audios;
    [HideInInspector]
    public Text[] Nums;
    [HideInInspector]
    public Slider[] sliders;
    [HideInInspector]
    public Button[] buttons;
    [HideInInspector]
    public List<Player> birds;
    [HideInInspector]
    public List<Enemy> pigs;
    [HideInInspector]
    public bool isClickAudioBtn = false;
    //0 背景, 1 贏, 2 輸 3暫停 4音樂
    [HideInInspector]
    public GameObject[] Menus;
    [HideInInspector]
    public float starTime;
    #endregion
    #region "public"
    public int TotalStar = 0;
    #endregion
    public static int LevilNum = 1;
    public static int realStar = 0;
    void Awake()
    {
        Time.timeScale = 1;
        Instance = this;
        Scene scene = SceneManager.GetActiveScene();
        if(scene.name=="Levil")
        {
            Load();
        }
        if (scene.name == "Game")
        {
            Instantiate(Resources.Load($"{MapManager.SelectMap}/{LevilNum}"));
            UseButton();
            for (int i = 0; i < GameObject.Find("Bird").transform.childCount; i++)
            {
                birds.Add(GameObject.Find("Bird").transform.GetChild(i).GetComponent<Player>());
            }
            for (int i = 0; i < GameObject.Find("Pig").transform.childCount; i++)
            {
                pigs.Add(GameObject.Find("Pig").transform.GetChild(i).GetComponent<Enemy>());
            }
            if (birds.Count > 0)
            {
                originPosition = birds[0].transform.position;
            }
            Init();
        }
    }
    void Init()
    {
        birds[0].transform.position = originPosition;
        birds[0].enabled = true;
        birds[0].springJointBird.enabled = true;
        birds[0].colliderBird.enabled = true;
    }
    public void NextBird()
    {
        if (pigs.Count > 0)
        {
            if (birds.Count > 0)
            {
                Init();
            }
            else
            {
                DisButton();
                Menus[0].SetActive(true);
                Menus[2].SetActive(true);
            }
        }
        else
        {            
            DisButton();
            Menus[0].SetActive(true);
            Menus[1].SetActive(true);
            realStar = birds.Count+1;
        }
    }
    void DisButton()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].enabled = false;
        }
    }
    void UseButton()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].enabled = true;
        }
    }
    public void ShowStarts()
    {
        starLength = birds.Count;
        if (birds.Count > 2)
        {
            starLength = 2;
        }
        StartCoroutine(Show());
    }
    IEnumerator Show()
    {
        for (starsNum = 0; starsNum < starLength + 1; starsNum++)
        {
            Stars[starsNum].SetActive(true);
            yield return new WaitForSeconds(starTime);
        }
    }
    public void Replay()
    {
        SaveStar();
        SceneManager.LoadScene("Game");
    }
    public void ReturnMenu()
    {
        SaveStar();
        SceneManager.LoadScene("Levil");
    }
    #region "Audio"
    public void AudioPlay()
    {
        isClickAudioBtn = !isClickAudioBtn;
        Menus[4].SetActive(isClickAudioBtn);
    }
    public void BackGroundAudio()
    {
        VolumeAdjustment(0);
    }
    public void EffectAudio()
    {
        VolumeAdjustment(1);
    }
    void VolumeAdjustment(int index)
    {
        Nums[index].text = ((int)(sliders[index].value * 100)).ToString();
        audios[index].volume = sliders[index].value;
    }
    public void AudioPlay(AudioClip audio)
    {
        audios[1].clip = audio;
        audios[1].Play();
    }
    #endregion
    #region "Json"
    public void SaveStar()
    {
        SaveJson.SaveByjson(PLAYER_DATA_FILE_NAME, Saveing());
    }
    public void Load()
    {
        LoadJson();
    }
    void LoadJson()
    {
        var SaveData = SaveJson.LoadFromJson<SaveData>(PLAYER_DATA_FILE_NAME);
        LoadData(SaveData);
    }
    SaveData Saveing()
    {
        var SaveData = new SaveData();
        int index = MapManager.SelectMap * PanelGrid.gridNums + LevilNum - 1;
        if(index<0)
        {
            index=0;
        }
        if(realStar>mapNum[index])
        {
            SaveData.mapNum[index] = realStar;
            mapNum[index] = realStar;
        }
        else
        {
            SaveData.mapNum[index] = mapNum[index];
        }
        int tempTotalStar = 0;
        for (int i = 0; i < mapNum.Length; i++)
        {
            if (mapNum[i] != 0)
            {
                SaveData.mapNum[i]=mapNum[i];
                tempTotalStar += SaveData.mapNum[i];           
            }
        }
        SaveData.TotalStar = tempTotalStar;
        TotalStar = tempTotalStar;
        return SaveData;
    }
    void LoadData(SaveData saveData)
    {       
        int tempTotalStar = 0;
        for (int i = 0; i < saveData.mapNum.Length; i++)
        {
            if (saveData.mapNum[i] != 0)
            {
                tempTotalStar += saveData.mapNum[i];
                mapNum[i] = saveData.mapNum[i];
            }
        }
        TotalStar = tempTotalStar;
    }
    #endregion
}
