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
    public struct SweetPreab
    {
        public SweetType type;
        public GameObject prefab;
    }
    public SweetPreab[] SweetPrefabs;
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
        set
        {
            _instance = value;
        }
    }
    public int GridColume;
    public int GridRow;
    public GameObject GridPrefab;
    void Awake() 
    {
        _instance = this;
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
