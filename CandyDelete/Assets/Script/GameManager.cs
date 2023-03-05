using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //糖果的種類
    public enum SweetType
    {
        EMPTY,
        NORMAL,
        BARRIER,
        ROW_CLEAR,
        COLUME_CLEAR,
        RAINBOWCANDY,
        COUNT//標記類型
    } 
    //糖果PREFAB的字典
    private Dictionary<SweetType,GameObject> SweetPrefabDictionary;
    
    [System.Serializable]
    public struct SweetPrefab
    {
        public SweetType type;
        public GameObject prefab;
    }
    public SweetPrefab[] SweetPrefabs;
    private static GameManager _instance;
    public static GameManager Instance { get => _instance; set => _instance = value; }
    public int GridColume;
    public int GridRow;
    public GameObject GridPrefab;
    //甜品陣列
    GameSweet[,] sweets;
    void Awake() 
    {
        Instance = this;
    }
    // Start is called before the first frame update
    //0.65 0.49
    void Start()
    {
        SweetPrefabDictionary = new Dictionary<SweetType, GameObject>();
        for (int i = 0; i < SweetPrefabs.Length; i++)
        {
            if(!SweetPrefabDictionary.ContainsKey(SweetPrefabs[i].type))
            {
                SweetPrefabDictionary.Add(SweetPrefabs[i].type,SweetPrefabs[i].prefab);
            }
        }
        for (int x = 0; x < GridColume; x++)
        {
            for (int y = 0; y < GridRow; y++)
            {
                GameObject Chocolate = Instantiate(GridPrefab,CorrectPositon(x,y),Quaternion.identity);
                Chocolate.transform.SetParent(transform);
            }
        }
        sweets= new GameSweet[GridColume,GridRow];
        for (int x = 0; x < GridColume; x++)
        {
            for (int y = 0; y < GridRow; y++)
            {                                                                           
                GameObject newSweet = Instantiate(SweetPrefabDictionary[SweetType.NORMAL],CorrectPositon(x,y),Quaternion.identity);
                newSweet.transform.SetParent(transform);

                sweets[x,y] = newSweet.GetComponent<GameSweet>();
                sweets[x,y].Init(x,y,this,SweetType.NORMAL);

            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Vector2 CorrectPositon(int x,int y)
    {
        return new Vector2(transform.position.x-GridColume/2f+x,transform.position.y+GridRow/2f-y);
    }
    
}
