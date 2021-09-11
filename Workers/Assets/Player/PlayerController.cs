using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [Space(5)]
    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerCombat playerCombat;

    [Header("Movement Parameters")]
    [Space(5)]
    [Range(150f, 400f)] [SerializeField] private float movementSpeed = 300f;        // Movement multiplier to the horizontal axis.
    [Range(100f, 200f)] [SerializeField] private float climbingSpeed = 200f;        // Climbing multiplier for the climbing movement.
    [Range(200f, 800f)] [SerializeField] private float jumpForce = 400f;            // Amount of force added when the player jumps.
    [Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;        // How much to smooth out the movement.

    private float horizontalMovement = 0f;
    private float verticalMovement = 0f;
    private bool shouldJump = false;

    [Header("Jump Handling Parameters")]
    [Space(5)]
    [SerializeField] private LayerMask whatIsGround;                                // A mask determining what is ground to the character.
    [SerializeField] private Transform groundCheck;                                 // A position marking where to check if the player is grounded.
    [Range(.03f, .15f)] [SerializeField] private float groundCheckRadius = .035f;   // Radius of the overlap circle to determine if grounded.
    private bool isGrounded = true;                                                 // Whether or not the player is grounded.
    private Coroutine keepGroundedCoroutine = null;
    private float keepGroundedTime = .08f;                                          // So the play can jump a few milisseconds after he left the ground.

    [Header("Ladder Handling Parameters")]
    [Space(5)]
    [SerializeField] private LayerMask whatIsLadder;                                // A mask determining what is a ladder to the character.
    [SerializeField] private Transform ladderCheck;                                 // A position marking where to check for a ladder.
    [Range(.1f, .3f)] [SerializeField] private float ladderCheckDistance = .3f;     // A position marking where to check if the player is inside a ladder.
    private bool isNearLadder = false;

    [HideInInspector] public bool facingRight = true;                               // For determining which way the player is currently facing.
    private Vector3 playerVelocity = Vector3.zero;                                  // Reference for SmoothDamp movement.
    private float originalGravityScale;

    private void Awake()
    {
        originalGravityScale = playerRigidbody.gravityScale;
    }

    void Update()
    {
        if (playerCombat.hasSnapped)
            return;

        horizontalMovement = Input.GetAxisRaw("Horizontal");

        verticalMovement = Mathf.Max(0, Input.GetAxisRaw("Vertical"));

        if (Input.GetButtonDown("Jump") && !animator.GetBool("IsJumping"))
            shouldJump = true;
    }

    private void FixedUpdate()
    {
        if (playerCombat.hasSnapped)
            return;

        // The player is grounded if a raycast from the groundcheck position hits anything designated as ground.
        Collider2D groundHit = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        if (groundHit != null && playerRigidbody.velocity.y <= 0f)
            isGrounded = true;
        else if (playerRigidbody.velocity.y < 0 && keepGroundedCoroutine == null) 
            keepGroundedCoroutine = StartCoroutine(KeepGrounded()); // Keep grounded for a few milisseconds

        RaycastHit2D ladderHitInfo = Physics2D.Raycast(ladderCheck.position, Vector2.up, ladderCheckDistance, whatIsLadder);
        isNearLadder = ladderHitInfo.collider != null;

        Move(horizontalMovement * Time.fixedDeltaTime, verticalMovement * Time.fixedDeltaTime, shouldJump);
        shouldJump = false;

        if (Debug.isDebugBuild)
        {
            Debug.DrawLine(groundCheck.position, (Vector2)groundCheck.position + Vector2.down * groundCheckRadius, Color.blue);
            Debug.DrawLine(ladderCheck.position, (Vector2)ladderCheck.position + Vector2.up * ladderCheckDistance, Color.red);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Entry"))
        {
            if (playerCombat.hasSnapped)
            {
                Debug.Log("EntryReached Snapped");
            }
            else
            {
                Debug.Log("EntryReached Not Snapped");
            }
        }
        else if (collision.CompareTag("Exit"))
        {
            //TODO check for finishedTasks
            Debug.Log("ExitReached");
        }
    }

    // Called each FixedUpdate by the PlayerInput class
    // Horizontal and vertical movements are multiplied by Time.fixedDeltaTime
    public void Move(float horizontalMovement, float verticalMovement, bool jump)
    {
        bool climbing = isNearLadder && verticalMovement > 0;
        playerRigidbody.gravityScale = climbing ? 0 : originalGravityScale;
        float _verticalMovement = climbing ? verticalMovement * climbingSpeed : playerRigidbody.velocity.y;
        float _horizontalMovement = horizontalMovement * movementSpeed;

        Vector3 targetVelocity = new Vector2(_horizontalMovement, _verticalMovement);
        playerRigidbody.velocity = Vector3.SmoothDamp(playerRigidbody.velocity, targetVelocity, ref playerVelocity, movementSmoothing);

        if (isGrounded && jump)
        {
            isGrounded = false;
            playerRigidbody.AddForce(new Vector2(0f, jumpForce));
        }

        HandleAnimations(_horizontalMovement, _verticalMovement, jump, climbing);
    }

    private void HandleAnimations(float horizontalMovement, float verticalMovement, bool jumping, bool climbing)
    {
        animator.SetFloat("Speed", Mathf.Abs(horizontalMovement));
        if (jumping) animator.SetBool("IsJumping", true);
        animator.SetBool("IsClimbing", climbing);
        if (isGrounded) animator.SetBool("IsJumping", false);

        //Flip player
        if (horizontalMovement > 0 && !facingRight) Flip();
        else if (horizontalMovement < 0 && facingRight) Flip();
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
