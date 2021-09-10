using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerCombat playerCombat;
    [SerializeField] private Animator animator;

    [SerializeField] private float jumpForce = 400f;                            // Amount of force added when the player jumps.
    [Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;    // How much to smooth out the movement
    [SerializeField] private LayerMask whatIsGround;                            // A mask determining what is ground to the character
    [SerializeField] private Transform groundCheck;                             // A position marking where to check if the player is grounded.

    const float groundedRadius = .05f;                                           // Radius of the overlap circle to determine if grounded
    private bool isGrounded;                                                    // Whether or not the player is grounded.

    public Rigidbody2D playerRigidbody2D;
    public bool facingRight = true;                                             // For determining which way the player is currently facing.
    private Vector3 playerVelocity = Vector3.zero;

    private Coroutine keepGroundedCoroutine = null;
    private float keepGroundedTime = .08f;
    public System.Action groundedCallback;

    private void FixedUpdate()
    {
        if (playerCombat.hasSnapped)
            return;

        // Keep grounded for a few milisseconds
        if (playerRigidbody2D.velocity.y != 0 && keepGroundedCoroutine == null)
            keepGroundedCoroutine = StartCoroutine(KeepGrounded());
        
        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        Collider2D[] grounds = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround);
        if (!isGrounded)
        {
            if (keepGroundedCoroutine != null)
            {
                StopCoroutine(keepGroundedCoroutine);
                keepGroundedCoroutine = null;
            }
            if (grounds.Length != 0)
                isGrounded = true;
        }

        if (isGrounded && playerRigidbody2D.velocity.y < 0f && animator.GetBool("IsJumping"))
            groundedCallback();
    }

    public void Move(float move, bool jump)
    {
        Vector3 targetVelocity = new Vector2(move, playerRigidbody2D.velocity.y);
        playerRigidbody2D.velocity = Vector3.SmoothDamp(playerRigidbody2D.velocity, targetVelocity, ref playerVelocity, movementSmoothing);

        //Flip player
        if (move > 0 && !facingRight) Flip();
        else if (move < 0 && facingRight) Flip();

        if (isGrounded && jump)
        {
            isGrounded = false;
            playerRigidbody2D.AddForce(new Vector2(0f, jumpForce));
        }
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void InstantKill() => playerCombat.IncreaseStress(playerCombat.maxStress); 

    public void Reset()
    {
        // Reset game
        transform.position = Vector2.zero;
        animator.SetBool("Snapped", false);
    }

    IEnumerator KeepGrounded()
    {
        yield return new WaitForSeconds(keepGroundedTime);
        isGrounded = false;
        keepGroundedCoroutine = null;
    }
}
