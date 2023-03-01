using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

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
