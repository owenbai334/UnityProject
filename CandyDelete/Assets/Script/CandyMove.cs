using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyMove : MonoBehaviour
{
    GameSweet sweet;
    IEnumerator moveCoroutine;

    void Awake()
    {
        sweet = GetComponent<GameSweet>();
    } 
    //開啟或結束移動
    public void Move(int X,int Y,float time)
    {
       if(moveCoroutine!=null)
       {
            StopCoroutine(moveCoroutine);
       }
       moveCoroutine = MoveCoroutine(X,Y,time);
       StartCoroutine(moveCoroutine);
    }
    //負責移動
    IEnumerator MoveCoroutine(int x,int y,float time)
    {
        sweet.X = x;
        sweet.Y = y;

        Vector2 startPos = transform.position;
        Vector2 endPos = sweet.gameManager.CorrectPositon(x,y);

        for (float t = 0; t < time; t+=Time.deltaTime)
        {
            sweet.transform.position = Vector2.Lerp(startPos,endPos,t/time);
            yield return 0;
        }

        sweet.transform.position = endPos;
    }
}
