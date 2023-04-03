using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Player> birds;
    public List<Enemy> pigs;
    static GameManager instance;
    public static GameManager Instance { get => instance; set => instance = value; }
    void Awake() 
    {
        Instance=this;
    }
    void Start() 
    {
        Init();
    }
    void Init()
    {
        for (int i = 0; i < birds.Count; i++)
        {
            if(i==0)
            {
                birds[i].enabled = true;
                birds[i].springJointBird.enabled = true;
            }
            else
            {
                birds[i].enabled = false;
                birds[i].springJointBird.enabled = false;
            }
        }
    }
    public void NextBird()
    {
        if(pigs.Count>0)
        {
            if(birds.Count>0)
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
