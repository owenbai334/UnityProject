using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    public Animator anima;
    bool IsPause = false;
    public GameObject[] Menus;
    public void Pause()
    {
        GameManager.Instance.isClickAudioBtn = false;
        GameManager.Instance.Menus[4].SetActive(false);
        IsPause =!IsPause;
        if(IsPause)
        {
            Menus[0].SetActive(IsPause);
            Menus[1].SetActive(IsPause);
        }
        else
        {
            Time.timeScale = 1; 
        }
        anima.SetBool("IsPause",IsPause);
        Invoke("PauseTime",0.4f);
    }
    void PauseTime()
    {
        if(IsPause)
        {
            Time.timeScale = 0;           
        }
        else
        {
            Menus[0].SetActive(IsPause);
            Menus[1].SetActive(IsPause);
        }
    }
}
