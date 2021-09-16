using System.Collections;
using UnityEngine;

public class Mop : MonoBehaviour
{
    public float moppingDuration = 2.5f;
    public bool hasMopped = false;
    private Coroutine moppingCoroutine = null;

    public void StartMopping(System.Action finishedMoppingCallback)
    {
        if (moppingCoroutine == null)
        {
            moppingCoroutine = StartCoroutine(Mopping(finishedMoppingCallback));
        }
    }

    IEnumerator Mopping(System.Action finishedMoppingCallback)
    {
        yield return new WaitForSeconds(moppingDuration);
        hasMopped = true;
        moppingCoroutine = null;
        finishedMoppingCallback.Invoke();
    }
}
