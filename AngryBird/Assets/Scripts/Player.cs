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
    Rigidbody2D rigidBodyBird;
    public LineRenderer[] shootLine;
    public GameObject explosion;
    void Awake()
    {
        springJointBird = GetComponent<SpringJoint2D>();
        rigidBodyBird = GetComponent<Rigidbody2D>();
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
        }
    }
    void OnMouseUp()
    {
        isClick = false;
        rigidBodyBird.isKinematic = false;
        Invoke("Fly", 0.1f);
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
        Invoke("Next",5f);
    }
    void Line()
    {
        for (int i = 0; i < shootLine.Length; i++)
        {
            shootLine[i].SetPosition(0, BirdPoint[i].position);
            shootLine[i].SetPosition(1, transform.position);
        }
    }
    void Next()
    {
        GameManager.Instance.birds.Remove(this);
        Destroy(this.gameObject);
        Instantiate(explosion,this.transform.position,Quaternion.identity);
        GameManager.Instance.NextBird();
    }
}
