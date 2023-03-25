using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearRainbow : ClearSweet
{
    ColorSweet.ColorType willClearAllSweet;
    public ColorSweet.ColorType WillClearAllSweet { get => willClearAllSweet; set => willClearAllSweet = value; }

    public override void Clear()
    {
        base.Clear();
        sweet.gameManager.ClearRainbow(willClearAllSweet);
    }
}
