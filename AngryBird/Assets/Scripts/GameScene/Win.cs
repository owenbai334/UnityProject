using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win : MonoBehaviour
{
    public void Show()
    {
        Debug.Log("成功調用Show");
        GameManager.Instance.ShowStarts();
    }
}
