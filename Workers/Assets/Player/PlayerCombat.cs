using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    private GameController gameController;

    public int maxStress = 20;
    int currentStress;

    float m_HurtDelay = .3f;
    float m_HurtTimer = 0f;
    bool m_CanBeHurt = true;
    public bool hasSnapped = false;

    private void Awake()
    {
        currentStress = 0;
        gameController = FindObjectOfType<GameController>();
    }

    void Update()
    {
        if (hasSnapped)
            return;

        if (!m_CanBeHurt)
        {
            if (Time.time >= m_HurtTimer)
            {
                m_CanBeHurt = true;
            }
        }
    }

    public void IncreaseStress(int damage = 1)
    {
        if (m_CanBeHurt)
        {

            m_CanBeHurt = false;
            m_HurtTimer = Time.time + m_HurtDelay;
            currentStress += damage;

            animator.SetTrigger("Hurt");

            if (currentStress >= maxStress)
            {
                animator.SetBool("Snapped", true);
                hasSnapped = true;
                GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                animator.SetFloat("Speed", 0f);
                animator.SetBool("IsJumping", false);
            }
        }
    }

    // Called by the end of the snap animation
    public void Snapped() => StartCoroutine(RestartGame());

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Trap"))
            IncreaseStress();
    }

    IEnumerator RestartGame()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        gameController.Restart();
        Reset();
    }

    public void Reset()
    {
        currentStress = maxStress;
        m_HurtDelay = .7f;
        m_HurtTimer = 0f;
        m_CanBeHurt = true;
        hasSnapped = false;
    }
}
