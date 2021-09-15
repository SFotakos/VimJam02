using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushingHammer : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        AnimatorClipInfo[] animationsClipInfo = animator.GetCurrentAnimatorClipInfo(0);
        animator.SetFloat("cycleOffset", Random.Range(0f, animationsClipInfo[0].clip.length));
    }
}
