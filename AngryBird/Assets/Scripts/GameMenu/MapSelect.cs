using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MapSelect : MonoBehaviour
{
    [HideInInspector]
    public bool IsSelect = false;
    [HideInInspector]
    public int StartsNum;
    //0 鎖 1 星星
    public GameObject[] Objects;
    void Start()
    {
        CheckStart();
    }
    void CheckStart()
    {
        if (PlayerPrefs.GetInt("TatalStar", 0) >= StartsNum)
        {
            IsSelect = true;
        }
        if (IsSelect)
        {
            Objects[0].SetActive(false);
            Objects[1].SetActive(true);
        }
    }
    public void Select()
    {
        if (IsSelect)
        {
            var button = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
            switch (button.name)
            {
                case "map1":
                    MapManager.Instance.SelectMap = 0;
                    break;
                case "map2":
                    MapManager.Instance.SelectMap = 1;
                    break;
            }
            Objects[2].SetActive(true);
            Objects[3].SetActive(false);
        }
    }
}
