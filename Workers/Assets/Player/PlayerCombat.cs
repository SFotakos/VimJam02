using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem snappedParticles;
    private ParticleSystemRenderer snappedParticlesSystemRenderer;

    [Header("Stress Variables")]
    [Space(5)]
    [Range(20, 200)] public float maxStress = 100f;
    [HideInInspector] public float currentStress;
    [Range(.5f, 2.5f)] public float stressPerSecond = .5f;
    
    private float hurtDelay = .6f;
    private float hurtTimer = 0f;
    private bool canBeHurt = true;

    [Header("UI")]
    [Space(5)]
    [SerializeField] private Image stressMeter;
    [SerializeField] private RectTransform snapped;

    Coroutine increaseStressCoroutine = null;

    private GameController gameController;

    private void Awake()
    {
        snappedParticlesSystemRenderer = snappedParticles.GetComponent<ParticleSystemRenderer>();
        currentStress = 0;
    }

    private void Start()
    {
        gameController = GameController.instance;
    }

    void Update()
    {
        if (gameController.snapped)
            return;

        if (!canBeHurt && Time.time >= hurtTimer)
            canBeHurt = true;

        if (increaseStressCoroutine == null)
            increaseStressCoroutine = StartCoroutine(IncreaseStressPerSecond(stressPerSecond));
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Trap") && !gameController.snapped)
            IncreaseStress();
    }

    public void IncreaseStress(float damage = 5, bool respectInvincibility = true, bool visualFeedback = true)
    {
        if (gameController.disableStress)
            return;

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
            snapped.gameObject.SetActive(true);
            
            snappedParticles.Play();
            gameController.snapped = true;
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            animator.SetFloat("Speed", 0f);
            animator.SetBool("IsJumping", false);
        }

        stressMeter.fillAmount = currentStress / maxStress;
    }

    IEnumerator IncreaseStressPerSecond(float stressPerSecond)
    {
        yield return new WaitForSeconds(.1f);
        IncreaseStress(stressPerSecond / 10f, respectInvincibility: false, visualFeedback: false);
        increaseStressCoroutine = null;
    }

    public void FlipParticles()
    {
        snappedParticlesSystemRenderer.flip = snappedParticlesSystemRenderer.flip.x == 0 ? Vector3.right : Vector3.zero;
    }
}
