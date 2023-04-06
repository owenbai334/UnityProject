using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float MaxSpeed = 10f;
    public float MinSpeed = 5f;
    SpriteRenderer spriteRenderer;
    public Sprite hurt;
    public GameObject Explosion;
    public GameObject Score;
    public Transform ScorePosition;
    public bool isPig;
    void Awake() 
    {
        spriteRenderer = GetComponent<SpriteRenderer>();    
    }
    void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.relativeVelocity.magnitude > MaxSpeed&&other.gameObject.tag=="Bird")
        {
            Die();
        }
        else if(other.relativeVelocity.magnitude <= MaxSpeed&&other.relativeVelocity.magnitude >=MinSpeed&&other.gameObject.tag=="Bird")
        {
            spriteRenderer.sprite = hurt;
        }
    }
    void Die()
    {
        if(isPig)
        {
            GameManager.Instance.pigs.Remove(this);
        }
        Instantiate(Explosion,this.transform.position,Quaternion.identity);
        GameObject tempScore = Instantiate(Score,ScorePosition.transform.position,Quaternion.identity);
        Destroy(this.gameObject);
        Destroy(tempScore,1.5f);
    }
}
