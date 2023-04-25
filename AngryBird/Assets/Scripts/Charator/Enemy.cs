using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    #region "Hide"
    [HideInInspector]
    public Transform ScorePosition;
    [HideInInspector]
    public GameObject Explosion;
    #endregion
    public float MaxSpeed = 10f;
    public float MinSpeed = 5f;
    public Sprite hurt;
    public GameObject Score;
    public bool isPig;
    public int score = 0;
    //0 受傷 1死亡
    public AudioClip[] audios;
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag=="Bird")
        {
            other.transform.GetComponent<Player>().BirdHurt();
        }
        if(other.gameObject.tag=="Egg")
        {
            other.transform.GetComponent<EggCollision>().Die();
        }
        if (other.relativeVelocity.magnitude > MaxSpeed||other.gameObject.tag=="Egg")
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
    public void Die()
    {
        GameManager.Instance.TotalScore+=score;
        GameManager.Instance.scores[0].text = "Score:"+GameManager.Instance.TotalScore.ToString();
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
