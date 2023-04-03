using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Player> birds;
    public List<Enemy> pigs;
    static GameManager instance;
    public static GameManager Instance { get => instance; set => instance = value; }
    Vector3 originPosition;
    void Awake()
    {
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
                Debug.Log("輸了");
            }
        }
        else
        {
            Debug.Log("贏了");
        }
    }
}