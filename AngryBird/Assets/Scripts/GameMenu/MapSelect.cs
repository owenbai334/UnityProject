using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MapSelect : MonoBehaviour
{
    bool IsSelect = false;
    [HideInInspector]
    public int StartsNum;
    //0 鎖 1 星星 2 panel 3星星
    [HideInInspector]
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
                case "map3":
                    MapManager.Instance.SelectMap = 2;
                    break;
                case "map4":
                    MapManager.Instance.SelectMap = 3;
                    break;
                case "map5":
                    MapManager.Instance.SelectMap = 4;
                    break;
                case "map6":
                    MapManager.Instance.SelectMap = 5;
                    break;
                case "map7":
                    MapManager.Instance.SelectMap = 6;
                    break;
                case "mapEgg":
                    MapManager.Instance.SelectMap = 7;
                    break;
            }
            Objects[2].SetActive(true);
            Objects[3].SetActive(false);
        }
    }
}
