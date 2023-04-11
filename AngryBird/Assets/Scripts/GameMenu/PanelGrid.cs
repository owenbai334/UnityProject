using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelGrid : MonoBehaviour
{
   public GameObject gridPrefab;
   public LevilSelect levilSelect;
   public int gridNums;
   void Awake() 
   {
        for (int i = 1; i <= gridNums; i++)
        {
            levilSelect.Num.text = i.ToString();
            GameObject levil = Instantiate(gridPrefab,transform.position, Quaternion.identity);
            levil.transform.SetParent(transform);       
        }
   }
}
