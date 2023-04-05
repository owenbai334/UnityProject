using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMyTrail : MonoBehaviour
{
    public WeaponTrail myTrail;
    Animator animator;
    float t = 0.033f;
    float tempT = 0;
    float animationIncrement = 0.003f;
    void Start()
    {
        animator = GetComponent<Animator>();
        myTrail.SetTime(0.0f, 0.0f, 1.0f);
    }
    //開始拖尾
    public void TrailStart()
    {      
        myTrail.SetTime(2.0f, 0.0f, 1.0f);
        myTrail.StartTrail(0.5f, 0.4f);
    }
    //清除拖尾
    public void TrailEnd()
    {
        myTrail.ClearTrail();
    }
    void LateUpdate()
    {
        t = Mathf.Clamp(Time.deltaTime, 0, 0.066f);

        if (t > 0)
        {
            while (tempT < t)
            {
                tempT += animationIncrement;

                if (myTrail.time > 0)
                {
                    myTrail.Itterate(Time.time - t + tempT);
                }
                else
                {
                    myTrail.ClearTrail();
                }
            }

            tempT -= t;

            if (myTrail.time > 0)
            {
                myTrail.UpdateTrail(Time.time, t);
            }
        }
    }
}
