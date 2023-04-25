using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class WhiteEgg : MonoBehaviour
{
    Player player;
    SpriteRenderer spriteRenderer;
    public GameObject egg;
    public float speed;
    public Sprite image;
    void Awake() 
    {
        player = GetComponent<Player>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void UseEgg()
    {
        spriteRenderer.sprite = image;
        Instantiate(egg,this.transform.position,Quaternion.identity);
        transform.Translate(new Vector2(speed*Time.deltaTime,speed*Time.deltaTime),Space.World);
    } 
}
