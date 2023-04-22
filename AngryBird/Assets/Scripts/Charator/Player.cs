using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region "Internal"
    bool isFly = false;
    bool isClick = false;
    bool isTouch = false;
    float CameraSpeed = 3f;
    float maxDistance = 1.8f;
    SpriteRenderer render;
    Rigidbody2D rigidBodyBird;
    Transform[] BirdPoint = new Transform[2];
    LineRenderer[] shootLine = new LineRenderer[2];
    List<Enemy> blocks = new List<Enemy>();
    #endregion
    #region "Hide"
    [HideInInspector]
    public SpringJoint2D springJointBird;
    [HideInInspector]
    public CapsuleCollider2D colliderBird;
    [HideInInspector]
    //0 選擇 1飛行 2死亡
    public AudioClip[] audios;
    [HideInInspector]
    public GameObject explosion;
    TextMyTrail textMyTrail;
    #endregion
    #region "public
    public Sprite Hurt;
    [System.Serializable]
    public enum BirdType
    {
        RED,
        YELLOW,
        WHITE,
        BLACK,
        GREEN,
        BLUE,
    }
    public BirdType color;
    #endregion
    
    void Awake()
    {
        springJointBird = GetComponent<SpringJoint2D>();
        rigidBodyBird = GetComponent<Rigidbody2D>();
        colliderBird = GetComponent<CapsuleCollider2D>();
        textMyTrail = GetComponent<TextMyTrail>();
        render = GetComponent<SpriteRenderer>();
        BirdPoint[0] = transform.parent.parent.Find("Shot").GetChild(1).GetChild(0).GetComponent<Transform>();
        BirdPoint[1] = transform.parent.parent.Find("Shot").GetChild(0).GetChild(0).GetComponent<Transform>();
        shootLine[0] = transform.parent.parent.Find("Shot").GetChild(1).GetComponent<LineRenderer>();
        shootLine[1] = transform.parent.parent.Find("Shot").GetChild(0).GetComponent<LineRenderer>();
    }
    void Update()
    {
        if (isClick)
        {
            BirdClicked();
        }
        if (isFly||(color==BirdType.BLACK&&isTouch))
        {
            UseSkill();
        }
        CameraFollow();
    }
    #region "點擊鳥"
    void OnMouseDown()
    {
        if (springJointBird.enabled)
        {
            GameManager.Instance.AudioPlay(audios[0]);
            isClick = true;
            rigidBodyBird.isKinematic = true;
            rigidBodyBird.constraints = RigidbodyConstraints2D.None;
        }
    }
    void OnMouseUp()
    {
        isClick = false;
        rigidBodyBird.isKinematic = false;
        Invoke("Fly", 0.1f);
        //禁用劃線
        for (int i = 0; i < shootLine.Length; i++)
        {
            shootLine[i].enabled = false;
        }
    }
    void BirdClicked()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position += new Vector3(0, 0, -Camera.main.transform.position.z);
        if (Vector3.Distance(transform.position, BirdPoint[0].position) > maxDistance)
        {
            Vector3 tempPos = (transform.position - BirdPoint[0].position).normalized;//獲得向量的方向
            tempPos *= maxDistance;//最大長度的向量
            transform.position = tempPos + BirdPoint[0].position;
        }
        Line();
    }
    void Line()
    {
        for (int i = 0; i < shootLine.Length; i++)
        {
            shootLine[i].enabled = true;
            shootLine[i].SetPosition(0, BirdPoint[i].position);
            shootLine[i].SetPosition(1, transform.position);
        }
    }
    #endregion
    #region "技能"
    void UseSkill()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShowSkill();
        }
    }
    public void ShowSkill()
    {
        isFly = false;
        switch (color)
        {
            case BirdType.YELLOW:
                rigidBodyBird.velocity *= 2;
                break;
            case BirdType.GREEN:
                Vector3 speed = rigidBodyBird.velocity;
                speed.x*=-1;
                speed.y *= Mathf.Atan(-speed.y);
                rigidBodyBird.velocity = speed;
                break;
            case BirdType.BLACK:
                if(blocks.Count >0 && blocks!=null)
                {
                    for (int i = 0; i < blocks.Count; i++)
                    {
                        blocks[i].Die();                   
                    }
                }
                OnClear();
                break;
            default:
                break;
        }

    }
    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag=="Enemy"||other.gameObject.tag=="Barrier")
        {
            blocks.Add(other.gameObject.GetComponent<Enemy>());
        }
    }
    void OnTriggerExit2D(Collider2D other) 
    {
        if(other.gameObject.tag=="Enemy"||other.gameObject.tag=="Barrier")
        {
            blocks.Remove(other.gameObject.GetComponent<Enemy>());
        }
    }
    void OnClear()
    {
        rigidBodyBird.velocity = Vector3.zero;
        Next();
    }
    #endregion
    void Fly()
    {
        isFly = true;
        GameManager.Instance.AudioPlay(audios[1]);
        springJointBird.enabled = false;
        textMyTrail.TrailStart();
    }
    void Next()
    {
        GameManager.Instance.AudioPlay(audios[2]);
        textMyTrail.TrailEnd();
        GameManager.Instance.birds.Remove(this);
        Destroy(this.gameObject);
        Instantiate(explosion, this.transform.position, Quaternion.identity);
        GameManager.Instance.NextBird();
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        isTouch = true;
        isFly = false;
        Invoke("Next", 3f);
    }
    public void CameraFollow()
    {
        float posX = this.transform.position.x;
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(Mathf.Clamp(posX, 0, 17), Camera.main.transform.position.y, Camera.main.transform.position.z), CameraSpeed * Time.deltaTime);
    }
    public void BirdHurt()
    {
        render.sprite = Hurt;
    }
}
