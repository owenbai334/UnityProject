using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearSweet : MonoBehaviour
{
    public AnimationClip animationClear;
    bool isClear;
    public bool IsClear { get => isClear; }
    protected GameSweet sweet;
    public AudioClip DestroyAudio;
    public virtual void Clear()
    {
        isClear = true;
        StartCoroutine(ClearCoroutine());
    }
    IEnumerator ClearCoroutine()
    {
        Animator animator = GetComponent<Animator>();

        if (animator != null)
        {
            animator.Play(animationClear.name);
            GameManager.Instance.playerScore++;
            AudioSource.PlayClipAtPoint(DestroyAudio,transform.position);
            yield return new WaitForSeconds(animationClear.length);
            Destroy(gameObject);
        }
    }
}
