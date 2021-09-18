using System.Collections;
using UnityEngine;

public class Mop : MonoBehaviour
{
    public bool hasMopped = false;
    private System.Action finishedMoppingCallback;

    Animator animator;
    InGameSoundManager soundManager;
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        soundManager = InGameSoundManager.instance;
    }

    public void StartMopping(System.Action finishedMoppingCallback)
    {
        this.finishedMoppingCallback = finishedMoppingCallback;
        animator.SetTrigger("mopping");
    }

    private void FinishedMopping()
    {
        hasMopped = true;
        finishedMoppingCallback.Invoke();
    }

    void MopSound()
    {
        soundManager.PlaySoundEffect(InGameSoundManager.SoundEffectType.MOPPING);
    }
}
