using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSweet : MonoBehaviour
{
    public enum ColorType
    {
        BLUE,
        GREEN,
        PINK,
        PURPLE,
        RED,
        YELLOW,
        RAINBOW,
        COUNT
    }
    [System.Serializable]
    public struct ColorSprite
    {
        public ColorType color;
        public Sprite sprite;
    }
    public ColorSprite[] colorSprites;
    Dictionary<ColorType,Sprite> colorSpriteDictionary;
    SpriteRenderer sprite;
    public int NumColors
    {
        get => colorSprites.Length;
    }
    public ColorType Color { get => color; set => color = value; }
    ColorType color;
    void Awake()
    {
        sprite = transform.Find("Sweet").GetComponent<SpriteRenderer>();
        
        colorSpriteDictionary = new Dictionary<ColorType, Sprite>();
        
        for (int i = 0; i < colorSprites.Length; i++)
        {
            if(!colorSpriteDictionary.ContainsKey(colorSprites[i].color))
            {
                colorSpriteDictionary.Add(colorSprites[i].color,colorSprites[i].sprite);
            }
        }
    }
    public void SetColor(ColorType newColor)
    {
        color = newColor;
        if(colorSpriteDictionary.ContainsKey(newColor))
        {
            sprite.sprite = colorSpriteDictionary[newColor];
        }
    }
}
