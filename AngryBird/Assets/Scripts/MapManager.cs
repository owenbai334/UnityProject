using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public int[] StartsNum;
    #region "Hide"
    [HideInInspector]
    public Text[] StartsText;
    [HideInInspector]
    public MapSelect[] mapSelect;
    [HideInInspector]
    public GameObject[] Objects;
    [HideInInspector]
    public int SelectMap=-1;
    #endregion
    static MapManager instance;
    public static MapManager Instance { get => instance; set => instance = value; }
    void Awake()
    {
        instance = this;
        for (int i = 0; i < StartsText.Length; i++)
        {
            StartsText[i].text = StartsNum[i].ToString();
            mapSelect[i].StartsNum = StartsNum[i];
        }
    }
    public void ReturnLevil()
    {
        Objects[0].SetActive(false);
        Objects[1].SetActive(true);
    }
}
