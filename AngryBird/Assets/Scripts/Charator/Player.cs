using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    bool isClick = false;
    public Transform[] BirdPoint;
    public float maxDistance = 3f;
    [HideInInspector]
    public SpringJoint2D springJointBird;
    [HideInInspector]
    public CapsuleCollider2D colliderBird;
    Rigidbody2D rigidBodyBird;
    public LineRenderer[] shootLine;
    public GameObject explosion;
    bool isTouch = false;
    TextMyTrail textMyTrail;
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
    }
    void OnMouseDown()
    {
        if (springJointBird.enabled)
        {
            isClick = true;
            rigidBodyBird.isKinematic = true;
            rigidBodyBird.constraints = RigidbodyConstraints2D.None;
        }
    }
    void OnMouseUp()
    {
        if (springJointBird.enabled)
        {
            isClick = false;
            rigidBodyBird.isKinematic = false;
            Invoke("Fly", 0.1f);
            //禁用劃線
            for (int i = 0; i < shootLine.Length; i++)
            {
                shootLine[i].enabled=false;
            }
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
        springJointBird.enabled = false;
        textMyTrail.TrailStart();
    }
    void Line()
    {
        for (int i = 0; i < shootLine.Length; i++)
        {
            shootLine[i].enabled=true;
            shootLine[i].SetPosition(0, BirdPoint[i].position);
            shootLine[i].SetPosition(1, transform.position);
        }
    }
    void Next()
    {
        textMyTrail.TrailEnd();
        GameManager.Instance.birds.Remove(this);
        Destroy(this.gameObject);
        Instantiate(explosion, this.transform.position, Quaternion.identity);
        GameManager.Instance.NextBird();
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        isTouch = true;
        Invoke("Next",3f);
    }
}
