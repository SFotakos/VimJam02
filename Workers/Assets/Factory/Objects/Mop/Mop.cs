using System.Collections;
using UnityEngine;

public class Mop : MonoBehaviour
{
    public bool hasMopped = false;
    private System.Action finishedMoppingCallback;

    Animator animator;
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
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
}
