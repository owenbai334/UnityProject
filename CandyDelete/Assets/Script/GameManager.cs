using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public Text TimeText;
    float gameTime = 60;
    bool GameOver;
    public Text ScoreText;
    public int playerScore;
    float addScoreTime = 0;
    float currentScore;
    public GameObject gameOverPanel;
    public Text GameMessager;
    bool IsPause = true;
    public Text LastGameScore;
    void Awake()
    {
        Instance = this;
    }
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
    void Update()
    {
        gameTime -= Time.deltaTime;
        if (gameTime <= 0)
        {
            gameTime = 0;
            //顯示失敗面板
            GameMessager.text = "遊戲結束";
            Time.timeScale = 0;
            LastGameScore.text = "分數:"+playerScore.ToString();
            ScoreText.text = playerScore.ToString();
            IsPause = false;
            gameOverPanel.SetActive(true);           
            GameOver = true;
            return;
        }
        TimeText.text = ((int)gameTime).ToString();
        if (addScoreTime <= 0.05f)
        {
            addScoreTime += Time.deltaTime;
        }
        else
        {
            if (currentScore < playerScore)
            {
                currentScore++;
                ScoreText.text = currentScore.ToString();
                addScoreTime = 0;
            }
        }
        PauseGame();
    }
    #region "填充糖果"
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
        bool needRefill = true;

        while (needRefill)
        {
            yield return new WaitForSeconds(fillTime);
            while (Fill())
            {
                yield return new WaitForSeconds(fillTime);
            }
            //填充後再清除
            needRefill = ClearAllMatchedSweets();
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
    #endregion
    #region "交換糖果"
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

            if (MatchSweets(sweetMe, sweetNeighbor.X, sweetNeighbor.Y) != null || MatchSweets(sweetNeighbor, sweetMe.X, sweetMe.Y) != null)
            {
                int tempX = sweetMe.X;
                int tempY = sweetMe.Y;

                sweetMe.MovedComponent.Move(sweetNeighbor.X, sweetNeighbor.Y, fillTime);
                sweetNeighbor.MovedComponent.Move(tempX, tempY, fillTime);
                ClearAllMatchedSweets();
                StartCoroutine(AllFill());
            }
            else
            {
                sweets[sweetMe.X, sweetMe.Y] = sweetMe;
                sweets[sweetNeighbor.X, sweetNeighbor.Y] = sweetNeighbor;
            }
        }
    }
    #endregion
    #region "滑鼠點擊" 
    public void PressSweet(GameSweet sweet)
    {
        if (GameOver)
        {
            return;
        }
        pressedSweet = sweet;
    }
    public void EnterSweet(GameSweet sweet)
    {
        if (GameOver)
        {
            return;
        }
        enterSweet = sweet;
    }
    public void ReleaseSweet()
    {
        if (GameOver)
        {
            return;
        }
        if (Adjacent(pressedSweet, enterSweet))
        {
            ExchangeSweets(pressedSweet, enterSweet);
        }
    }
    #endregion
    #region "清除糖果"
    //匹配方法
    public List<GameSweet> MatchSweets(GameSweet sweet, int newX, int newY)
    {
        if (sweet.CanColor())
        {
            ColorSweet.ColorType color = sweet.ColoredComponent.Color;
            List<GameSweet> MatchRowSweets = new List<GameSweet>();
            List<GameSweet> MatchColumeSweets = new List<GameSweet>();
            List<GameSweet> FinishedMatchingSweets = new List<GameSweet>();

            //行匹配
            MatchRowSweets.Add(sweet);

            //i=0 左 ,i=1右
            for (int i = 0; i <= 1; i++)
            {
                for (int xDistance = 1; xDistance < GridColume; xDistance++)
                {
                    int x;
                    if (i == 0)
                    {
                        x = newX - xDistance;
                    }
                    else
                    {
                        x = newX + xDistance;
                    }

                    if (x < 0 || x >= GridColume)
                    {
                        break;
                    }

                    if (sweets[x, newY].CanColor() && sweets[x, newY].ColoredComponent.Color == color)
                    {
                        MatchRowSweets.Add(sweets[x, newY]);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            //行匹配加入
            if (MatchRowSweets.Count >= 3)
            {
                for (int i = 0; i < MatchRowSweets.Count; i++)
                {
                    FinishedMatchingSweets.Add(MatchRowSweets[i]);
                }
            }

            //LT型匹配
            //檢查行遍例列表數量是否大於三
            if (MatchRowSweets.Count >= 3)
            {
                for (int i = 0; i < MatchRowSweets.Count; i++)
                {
                    //滿足行匹配後進行列遍例
                    //i=0 上 ,i=1下
                    for (int j = 0; j <= 1; j++)
                    {
                        for (int yDistance = 1; yDistance < GridRow; yDistance++)
                        {
                            int y;
                            if (j == 0)
                            {
                                y = newY - yDistance;
                            }
                            else
                            {
                                y = newY + yDistance;
                            }

                            if (y < 0 || y >= GridRow)
                            {
                                break;
                            }

                            if (sweets[MatchRowSweets[i].X, y].CanColor() && sweets[MatchRowSweets[i].X, y].ColoredComponent.Color == color)
                            {
                                MatchColumeSweets.Add(sweets[MatchRowSweets[i].X, y]);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (MatchColumeSweets.Count < 2)
                    {
                        MatchColumeSweets.Clear();
                    }
                    else
                    {
                        for (int j = 0; j < MatchColumeSweets.Count; j++)
                        {
                            FinishedMatchingSweets.Add(MatchColumeSweets[j]);
                        }
                        break;
                    }
                }
            }

            MatchRowSweets.Clear();
            MatchColumeSweets.Clear();

            if (FinishedMatchingSweets.Count >= 3)
            {
                return FinishedMatchingSweets;
            }

            //列匹配
            MatchColumeSweets.Add(sweet);

            //i=0 上 ,i=1下
            for (int i = 0; i <= 1; i++)
            {
                for (int yDistance = 1; yDistance < GridRow; yDistance++)
                {
                    int y;
                    if (i == 0)
                    {
                        y = newY - yDistance;
                    }
                    else
                    {
                        y = newY + yDistance;
                    }

                    if (y < 0 || y >= GridRow)
                    {
                        break;
                    }

                    if (sweets[newX, y].CanColor() && sweets[newX, y].ColoredComponent.Color == color)
                    {
                        MatchColumeSweets.Add(sweets[newX, y]);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            //列匹配加入
            if (MatchColumeSweets.Count >= 3)
            {
                for (int i = 0; i < MatchColumeSweets.Count; i++)
                {
                    FinishedMatchingSweets.Add(MatchColumeSweets[i]);
                }
            }

            //LT型匹配
            //檢查列遍例列表數量是否大於三
            if (MatchColumeSweets.Count >= 3)
            {
                for (int i = 0; i < MatchColumeSweets.Count; i++)
                {
                    //滿足列匹配後進行行遍例
                    //i=0 上 ,i=1下
                    for (int j = 0; j <= 1; j++)
                    {
                        for (int xDistance = 1; xDistance < GridColume; xDistance++)
                        {
                            int x;
                            if (j == 0)
                            {
                                x = newX - xDistance;
                            }
                            else
                            {
                                x = newX + xDistance;
                            }

                            if (x < 0 || x >= GridColume)
                            {
                                break;
                            }

                            if (sweets[x, MatchColumeSweets[i].Y].CanColor() && sweets[x, MatchColumeSweets[i].Y].ColoredComponent.Color == color)
                            {
                                MatchRowSweets.Add(sweets[x, MatchColumeSweets[i].Y]);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (MatchRowSweets.Count < 2)
                    {
                        MatchRowSweets.Clear();
                    }
                    else
                    {
                        for (int j = 0; j < MatchRowSweets.Count; j++)
                        {
                            FinishedMatchingSweets.Add(MatchRowSweets[j]);
                        }
                        break;
                    }
                }
            }

            MatchRowSweets.Clear();
            MatchColumeSweets.Clear();

            if (FinishedMatchingSweets.Count >= 3)
            {
                return FinishedMatchingSweets;
            }
        }
        return null;
    }
    //清除方法
    public bool ClearCandy(int x, int y)
    {
        if (sweets[x, y].CanClear() && !sweets[x, y].ClearComponent.IsClear)
        {
            sweets[x, y].ClearComponent.Clear();
            CreateCandy(x, y, SweetType.EMPTY);

            return true;
        }
        return false;
    }
    //清除所有完成匹配的糖果
    bool ClearAllMatchedSweets()
    {
        bool needRefill = false;
        for (int y = 0; y < GridRow; y++)
        {
            for (int x = 0; x < GridColume; x++)
            {
                if (sweets[x, y].CanClear())
                {
                    List<GameSweet> matchList = MatchSweets(sweets[x, y], x, y);
                    if (matchList != null)
                    {
                        for (int i = 0; i < matchList.Count; i++)
                        {
                            if (ClearCandy(matchList[i].X, matchList[i].Y))
                            {
                                needRefill = true;
                            }
                        }
                    }
                }
            }
        }

        return needRefill;
    }
    #endregion
    public void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameOverPanel.SetActive(IsPause);
            LastGameScore.text = "分數:"+playerScore.ToString();
            IsPause = !IsPause;
            if (!IsPause)
            {
                GameMessager.text = "遊戲暫停";
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
    }
    public void ReturnGame()
    {
        Time.timeScale=1;
        SceneManager.LoadScene("Game");
    }
    public void QuitMain()
    {
        SceneManager.LoadScene("Title");
    }
}