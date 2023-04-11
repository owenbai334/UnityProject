using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    public static GameManager Instance { get => instance; set => instance = value; }
    Vector3 originPosition;
    int starLength = 0;
    #region "public"
    public List<Player> birds;
    public List<Enemy> pigs;
    //0 背景, 1 贏, 2 輸 3暫停 4音樂
    public GameObject[] Menus;
    [HideInInspector]
    public GameObject[] Stars;
    public float starTime;
    //0背景 1音效
    [HideInInspector]
    public AudioSource[] audios;
    //0背景 1音效
    [HideInInspector]
    public Text[] Nums;
    //0背景 1音效
    [HideInInspector]
    public Slider[] sliders;
    [HideInInspector]
    public Button[] buttons;
    [HideInInspector]
    public bool isClickAudioBtn = false;
    #endregion
    void Awake()
    {
        UseButton();
        Time.timeScale = 1;
        Instance = this;
        if (birds.Count > 0)
        {
            originPosition = birds[0].transform.position;
        }
        Init();
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
        if(birds.Count>2)
        {
            starLength = 2;
        }
        StartCoroutine(Show());
    }
    IEnumerator Show()
    {
        for(int i = 0; i < starLength+1; i++) 
        {
            Stars[i].SetActive(true);
            yield return new WaitForSeconds(starTime);
        }
    }
    public void Replay()
    {
        SceneManager.LoadScene("Game");
    }
    public void ReturnMenu()
    {
        SceneManager.LoadScene("Levil");
    }
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
        Nums[index].text = ((int)(sliders[index].value*100)).ToString();
        audios[index].volume = sliders[index].value;       
    }
    public void AudioPlay(AudioClip audio)
    {
        audios[1].clip = audio;
        audios[1].Play();
    }
}
