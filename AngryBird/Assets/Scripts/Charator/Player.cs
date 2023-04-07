using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    bool isClick = false;
    bool isTouch = false;
    float maxDistance = 1.8f;
    float CameraSpeed = 3f; 
    TextMyTrail textMyTrail;
    Rigidbody2D rigidBodyBird;
    #region "public
    public Transform[] BirdPoint;
    [HideInInspector]
    public SpringJoint2D springJointBird;
    [HideInInspector]
    public CapsuleCollider2D colliderBird;
    public LineRenderer[] shootLine;
    [HideInInspector]
    public GameObject explosion;
    //0 選擇 1飛行 2死亡
    [HideInInspector]
    public AudioClip[] audios;
    #endregion
    void Awake()
    {
        springJointBird = GetComponent<SpringJoint2D>();
        rigidBodyBird = GetComponent<Rigidbody2D>();
        colliderBird = GetComponent<CapsuleCollider2D>();
        textMyTrail = GetComponent<TextMyTrail>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isClick)
        {
            BirdClicked();
        }
        CameraFollow();
    }
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
        if (Vector3.Distance(this.transform.position, BirdPoint[0].position) > maxDistance)
        {
            Vector3 tempPos = (this.transform.position - BirdPoint[0].position).normalized;//獲得向量的方向
            tempPos *= maxDistance;//最大長度的向量
            this.transform.position = tempPos + BirdPoint[0].position;
        }
        Line();
    }
    void Fly()
    {
        GameManager.Instance.AudioPlay(audios[1]);
        springJointBird.enabled = false;
        textMyTrail.TrailStart();
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
        GameManager.Instance.DisAudio();
        isTouch = true;
        Invoke("Next", 3f);
    }
    public void CameraFollow()
    {
        float posX = this.transform.position.x;
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position,new Vector3(Mathf.Clamp(posX,0,17),Camera.main.transform.position.y,Camera.main.transform.position.z),CameraSpeed*Time.deltaTime);
    }
}
