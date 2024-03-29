using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSweet : MonoBehaviour
{
    int x;
    int y; 
    public int X 
    { 
        get 
        {
            return x;
        } 
        set 
        { 
            if(CanMove())
            {
                x = value;
            } 
        }
    }  
    public int Y 
    { 
        get 
        {
            return y; 
        }
        set 
        { 
            if(CanMove())
            {
                y = value;
            }
        } 
    }
    GameManager.SweetType type;
    public GameManager.SweetType Type { get => type;}
    CandyMove movedComponent;
    public CandyMove MovedComponent { get => movedComponent;}
    ColorSweet coloredComponent;
    public ColorSweet ColoredComponent { get => coloredComponent;}
    ClearSweet clearComponent;
    public ClearSweet ClearComponent { get => clearComponent;}

    [HideInInspector]
    public GameManager gameManager;
    void Awake()
    {
        movedComponent = GetComponent<CandyMove>();
        coloredComponent = GetComponent<ColorSweet>();
        clearComponent = GetComponent<ClearSweet>();
    }
    public bool CanMove()
    {
        return movedComponent!=null;
    } 
    public bool CanColor()
    {
        return coloredComponent!=null;
    } 
    public bool CanClear()
    {
        return clearComponent!=null;
    }
    public void Init(int x,int y,GameManager gameManager,GameManager.SweetType type)
    {
        this.x = x;
        this.y = y;
        this.gameManager = gameManager;
        this.type = type;
    }
    void OnMouseEnter() 
    {
        gameManager.EnterSweet(this);
    }
    void OnMouseDown() 
    {
        gameManager.PressSweet(this);
    }
    void OnMouseUp() 
    {
        gameManager.ReleaseSweet();
    }
}
