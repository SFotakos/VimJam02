using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushingHammer : MonoBehaviour
{
    InGameSoundManager soundManager;
    Animator animator;
    bool isVisible;

    void Start()
    {
        soundManager = InGameSoundManager.instance;
        animator = GetComponentInChildren<Animator>();
        AnimatorClipInfo[] animationsClipInfo = animator.GetCurrentAnimatorClipInfo(0);
        animator.SetFloat("cycleOffset", Random.Range(0f, animationsClipInfo[0].clip.length));
    }

    private void OnBecameVisible()
    {
        isVisible = true;
    }

    private void OnBecameInvisible()
    {
        isVisible = false;
    }

    void OnStartMotion()
    {
        if (isVisible)
            soundManager.PlaySoundEffect(InGameSoundManager.SoundEffectType.CRUSHING_HAMMER_START);
    }

    void OnHitGround()
    {
        if (isVisible)
            soundManager.PlaySoundEffect(InGameSoundManager.SoundEffectType.CRUSHING_HAMMER_HIT);
    }
}
