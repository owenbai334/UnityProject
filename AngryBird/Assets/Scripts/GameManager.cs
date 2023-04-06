using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<Player> birds;
    public List<Enemy> pigs;
    static GameManager instance;
    public static GameManager Instance { get => instance; set => instance = value; }
    Vector3 originPosition;
    //0 背景, 1 贏, 2 輸
    public GameObject[] Menus;
    public GameObject[] Stars;
    int starLength = 0;
    public float starTime;
    void Awake()
    {
        Time.timeScale = 1;
        Instance = this;
        if (birds.Count > 0)
        {
            originPosition = birds[0].transform.position;
        }
    }
    void Start()
    {
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
                Menus[0].SetActive(true);
                Menus[2].SetActive(true);
            }
        }
        else
        {
            Menus[0].SetActive(true);
            Menus[1].SetActive(true);
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
}
