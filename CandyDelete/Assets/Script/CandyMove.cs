using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyMove : MonoBehaviour
{
    GameSweet sweet;

    void Awake()
    {
        sweet = GetComponent<GameSweet>();
    } 
    public void Move(int X,int Y)
    {
        sweet.X=X;
        sweet.Y=Y;
        sweet.transform.position = sweet.gameManager.CorrectPositon(X,Y);
    }
}
