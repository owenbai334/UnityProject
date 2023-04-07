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
    //0 受傷 1死亡
    public AudioClip[] audios;
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.relativeVelocity.magnitude > MaxSpeed)
        {
            Die();
        }
        else if (other.relativeVelocity.magnitude <= MaxSpeed && other.relativeVelocity.magnitude >= MinSpeed)
        {
            spriteRenderer.sprite = hurt;
            if (audios != null)
            {
                GameManager.Instance.AudioPlay(audios[0]);
            }
        }
    }
    void Die()
    {
        if (audios != null)
        {
            GameManager.Instance.AudioPlay(audios[1]);
        }
        if (isPig)
        {
            GameManager.Instance.pigs.Remove(this);
        }
        Instantiate(Explosion, this.transform.position, Quaternion.identity);
        GameObject tempScore = Instantiate(Score, ScorePosition.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
        Destroy(tempScore, 1.5f);
    }
}
