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
    private Dictionary<SweetType, GameObject> SweetPrefabDictionary;

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
    //填充時間
    public float fillTime;
    //甜品陣列
    GameSweet[,] sweets;
    //要交換的倆甜品
    GameSweet pressedSweet;
    GameSweet enterSweet;
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
            if (!SweetPrefabDictionary.ContainsKey(SweetPrefabs[i].type))
            {
                SweetPrefabDictionary.Add(SweetPrefabs[i].type, SweetPrefabs[i].prefab);
            }
        }
        for (int x = 0; x < GridColume; x++)
        {
            for (int y = 0; y < GridRow; y++)
            {
                GameObject Chocolate = Instantiate(GridPrefab, CorrectPositon(x, y), Quaternion.identity);
                Chocolate.transform.SetParent(transform);
            }
        }
        sweets = new GameSweet[GridColume, GridRow];

        for (int x = 0; x < GridColume; x++)
        {
            for (int y = 0; y < GridRow; y++)
            {
                CreateCandy(x, y, SweetType.EMPTY);
            }
        }

        Destroy(sweets[4, 4].gameObject);
        CreateCandy(4, 4, SweetType.BARRIER);

        StartCoroutine(AllFill());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Vector2 CorrectPositon(int x, int y)
    {
        return new Vector2(transform.position.x - GridColume / 2f + x, transform.position.y + GridRow / 2f - y);
    }

    //產生甜品的方法
    public GameSweet CreateCandy(int x, int y, SweetType type)
    {
        GameObject newSweet = Instantiate(SweetPrefabDictionary[type], CorrectPositon(x, y), Quaternion.identity);
        newSweet.transform.parent = transform;

        sweets[x, y] = newSweet.GetComponent<GameSweet>();
        sweets[x, y].Init(x, y, this, type);

        return sweets[x, y];
    }
    //全部填充的方法
    public IEnumerator AllFill()
    {
        while (Fill())
        {
            yield return new WaitForSeconds(fillTime);
        }
    }
    //部分填充
    public bool Fill()
    {
        bool FilledNotFinished = false;

        for (int y = GridRow - 2; y >= 0; y--)
        {
            for (int x = 0; x < GridColume; x++)
            {
                GameSweet sweet = sweets[x, y];//得到當前元素位置的糖果對象

                if (sweets[x, y].CanMove())//如果無法移動，則無法往下填充
                {
                    GameSweet sweetBelow = sweets[x, y + 1];
                    if (sweetBelow.Type == SweetType.EMPTY)//垂直填充
                    {
                        Destroy(sweetBelow.gameObject);
                        sweet.MovedComponent.Move(x, y + 1, fillTime);
                        sweets[x, y + 1] = sweet;
                        CreateCandy(x, y, SweetType.EMPTY);
                        FilledNotFinished = true;
                    }
                    //斜向填充
                    else
                    {
                        int downX = x + 1;

                        if (downX >= 0 && downX < GridColume)
                        {
                            GameSweet downSweet = sweets[downX, y + 1];
                            if (downSweet.Type == SweetType.EMPTY)
                            {
                                bool canFill = true;//用來判斷是否可以垂直填充

                                for (int aboveY = y; aboveY >= 0; aboveY--)
                                {
                                    GameSweet SweetAbove = sweets[downX, aboveY];
                                    if (SweetAbove.CanMove())
                                    {
                                        break;
                                    }
                                    else if (!SweetAbove.CanMove() && SweetAbove.Type != SweetType.EMPTY)
                                    {
                                        canFill = false;
                                        break;
                                    }
                                }
                                if (!canFill)
                                {
                                    Destroy(downSweet.gameObject);
                                    sweet.MovedComponent.Move(downX, y + 1, fillTime);
                                    sweets[downX, y + 1] = sweet;
                                    CreateCandy(x, y, SweetType.EMPTY);
                                    FilledNotFinished = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        //最上排的特殊情況
        for (int x = 0; x < GridColume; x++)
        {
            GameSweet sweet = sweets[x, 0];
            if (sweet.Type == SweetType.EMPTY)
            {
                GameObject newSweet = Instantiate(SweetPrefabDictionary[SweetType.NORMAL], CorrectPositon(x, -1), Quaternion.identity);
                newSweet.transform.parent = transform;

                sweets[x, 0] = newSweet.GetComponent<GameSweet>();
                sweets[x, 0].Init(x, -1, this, SweetType.NORMAL);
                sweets[x, 0].MovedComponent.Move(x, 0, fillTime);
                sweets[x, 0].ColoredComponent.SetColor((ColorSweet.ColorType)Random.Range(0, sweets[x, 0].ColoredComponent.NumColors));
                FilledNotFinished = true;
            }
        }

        return FilledNotFinished;
    }

    //判斷甜品是否相鄰
    bool Adjacent(GameSweet sweetMe, GameSweet sweetNeighbor)
    {
        return (sweetMe.X == sweetNeighbor.X && Mathf.Abs(sweetMe.Y - sweetNeighbor.Y) == 1) || (sweetMe.Y == sweetNeighbor.Y && Mathf.Abs(sweetMe.X - sweetNeighbor.X) == 1);
    }

    //交換兩個甜品
    void ExchangeSweets(GameSweet sweetMe, GameSweet sweetNeighbor)
    {
        if (sweetMe.CanMove() && sweetNeighbor.CanMove())
        {
            sweets[sweetMe.X, sweetMe.Y] = sweetNeighbor;
            sweets[sweetNeighbor.X, sweetNeighbor.Y] = sweetMe;

            int tempX = sweetMe.X;
            int tempY = sweetMe.Y;

            sweetMe.MovedComponent.Move(sweetNeighbor.X, sweetNeighbor.Y, fillTime);
            sweetNeighbor.MovedComponent.Move(tempX, tempY, fillTime);

        }
    }
    public void PressSweet(GameSweet sweet)
    {
        pressedSweet = sweet;
    }
    public void EnterSweet(GameSweet sweet)
    {
        enterSweet = sweet;
    }
    public void ReleaseSweet()
    {
        if (Adjacent(pressedSweet, enterSweet))
        {
            ExchangeSweets(pressedSweet, enterSweet);
        }
    }
}