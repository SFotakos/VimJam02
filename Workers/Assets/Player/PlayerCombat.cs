using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private GameController gameController;

    [Header("Stress Variables")]
    [Space(5)]
    [Range(20, 200)] public float maxStress = 100f;
    [HideInInspector] public float currentStress;
    [Range(.5f, 2.5f)] public float stressPerSecond = .5f;

    private float hurtDelay = .3f;
    private float hurtTimer = 0f;
    private bool canBeHurt = true;
    [HideInInspector] public bool hasSnapped = false;

    [Header("UI")]
    [Space(5)]
    [SerializeField] private Image stressMeter;

    Coroutine increaseStressCoroutine = null;

    private void Awake()
    {
        currentStress = 0;
        gameController = GameController.instance;
    }

    void Update()
    {
        if (hasSnapped)
            return;

        if (!canBeHurt && Time.time >= hurtTimer)
            canBeHurt = true;

        if (increaseStressCoroutine == null)
            increaseStressCoroutine = StartCoroutine(IncreaseStressPerSecond(stressPerSecond));
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Trap"))
            IncreaseStress();
    }

    public void IncreaseStress(float damage = 5, bool respectInvincibility = true, bool visualFeedback = true)
    {
        if (respectInvincibility && !canBeHurt)
            return;

        if (respectInvincibility)
        {
            canBeHurt = false;
            hurtTimer = Time.time + hurtDelay;
        }
        currentStress += damage;

        if (visualFeedback)
            animator.SetTrigger("Hurt");

        if (currentStress >= maxStress)
        {
            animator.SetBool("Snapped", true);
            hasSnapped = true;
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            animator.SetFloat("Speed", 0f);
            animator.SetBool("IsJumping", false);
        }

        stressMeter.fillAmount = currentStress / maxStress;
    }

    // Called by the end of the snap animation
    public void Snapped() => StartCoroutine(RestartGame());

    IEnumerator RestartGame()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        gameController.Restart();
        Reset();
    }

    IEnumerator IncreaseStressPerSecond(float stressPerSecond)
    {
        yield return new WaitForSeconds(.1f);
        IncreaseStress(stressPerSecond / 10f, respectInvincibility: false, visualFeedback: false);
        increaseStressCoroutine = null;
    }

    public void Reset()
    {
        currentStress = maxStress;
        hurtDelay = .7f;
        hurtTimer = 0f;
        canBeHurt = true;
        hasSnapped = false;
    }
}
