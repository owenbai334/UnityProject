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
    public Text[] StartCount;
    #endregion
    public static int SelectMap=0;
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
    }
}
