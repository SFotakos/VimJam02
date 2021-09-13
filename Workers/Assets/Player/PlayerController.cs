using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [Space(5)]
    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerCombat playerCombat;
    private GameController gameController;

    [Header("Movement Variables")]
    [Space(5)]
    [Range(150f, 400f)] [SerializeField] private float movementSpeed = 300f;        // Movement multiplier to the horizontal axis.
    [Range(100f, 200f)] [SerializeField] private float climbingSpeed = 200f;        // Climbing multiplier for the climbing movement.
    [Range(200f, 800f)] [SerializeField] private float jumpForce = 400f;            // Amount of force added when the player jumps.
    
    private float horizontalMovement = 0f;
    private float verticalMovement = 0f;
    private bool shouldJump = false;

    [Header("Jump Handling Variables")]
    [Space(5)]
    [SerializeField] private LayerMask whatIsGround;                                // A mask determining what is ground to the character.
    [SerializeField] private Transform groundCheck;                                 // A position marking where to check if the player is grounded.
    [Range(.03f, .15f)] [SerializeField] private float groundCheckRadius = .035f;   // Radius of the overlap circle to determine if grounded.
    private bool isGrounded = true;                                                 // Whether or not the player is grounded.
    private bool disableGroundCheck = false;
    private Coroutine keepGroundedCoroutine = null;
    private float keepGroundedTime = .08f;                                          // So the play can jump a few milisseconds after he left the ground.
    Collider2D groundHit;

    [Header("Ladder Handling Variables")]
    [Space(5)]
    [SerializeField] private LayerMask whatIsLadder;                                // A mask determining what is a ladder to the character.
    [SerializeField] private Transform ladderCheck;                                 // A position marking where to check for a ladder.
    [Range(.1f, .3f)] [SerializeField] private float ladderCheckDistance = .3f;     // A position marking where to check if the player is inside a ladder.
    private bool isNearLadder = false;

    [HideInInspector] public bool facingRight = true;                               // For determining which way the player is currently facing.
    private float originalGravityScale;

    [Header("Snapped AI Variables")]
    [Space(5)]
    private NavMeshAgent agent;
    private AgentLinkMover agentLinkMover;
    [SerializeField] private Transform snappedDestination;
    private Coroutine offMeshLinkMoveCoroutine = null;

    [Header("UI Variables")]
    [Space(5)]
    [SerializeField] private Image carriedItemImage;

    // Inventory
    private GameObject box = null;

    private void Awake()
    {
        originalGravityScale = playerRigidbody.gravityScale;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.autoTraverseOffMeshLink = false;
        agentLinkMover = GetComponent<AgentLinkMover>();
    }

    private void Start()
    {
        gameController = GameController.instance;
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
            gameController.Restart();

        if (playerCombat.hasSnapped && isGrounded)
        {
            if (!agent.enabled)
            {
                playerRigidbody.simulated = false;
                agent.enabled = true;
                agent.SetDestination(snappedDestination.position);
            }
            HandleNavMeshMovement();
            return;
        }

        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Mathf.Max(0, Input.GetAxisRaw("Vertical"));

        if (Input.GetButtonDown("Jump") && !animator.GetBool("IsJumping"))
            shouldJump = true;
    }

    private void FixedUpdate()
    {
        // The player is grounded if a raycast from the groundcheck position hits anything designated as ground.
        groundHit = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        if (groundHit != null && !disableGroundCheck)
            isGrounded = true;

        else if (playerRigidbody.velocity.y < 0 && keepGroundedCoroutine == null)
            keepGroundedCoroutine = StartCoroutine(KeepGrounded()); // Keep grounded for a few milisseconds

        RaycastHit2D ladderHitInfo = Physics2D.Raycast(ladderCheck.position, Vector2.up, ladderCheckDistance, whatIsLadder);
        isNearLadder = ladderHitInfo.collider != null;

        if (Debug.isDebugBuild)
        {
            Debug.DrawLine(groundCheck.position, (Vector2)groundCheck.position + Vector2.down * groundCheckRadius, Color.blue);
            Debug.DrawLine(ladderCheck.position, (Vector2)ladderCheck.position + Vector2.up * ladderCheckDistance, Color.red);
        }

        if (playerCombat.hasSnapped)
            return;

        Move(horizontalMovement * Time.fixedDeltaTime, verticalMovement * Time.fixedDeltaTime, shouldJump);
        shouldJump = false;
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
        } else if (collision.CompareTag("Box"))
        {
            if (box == null) 
            {
                box = collision.gameObject;
                box.GetComponent<Collider2D>().enabled = false;
                carriedItemImage.sprite = box.GetComponentInChildren<SpriteRenderer>().sprite;
                carriedItemImage.enabled = true;
                box.SetActive(false);
            }
            
        } else if (collision.CompareTag("BoxUnloadingArea"))
        {
            if (box != null)
            {
                carriedItemImage.enabled = false;
                BoxUnloadingArea boxUnloadingArea = collision.GetComponent<BoxUnloadingArea>();
                boxUnloadingArea.AddBox(box);
                box = null;
            }
        }
    }

    // Called each FixedUpdate by the PlayerInput class
    // Horizontal and vertical movements are multiplied by Time.fixedDeltaTime
    private void Move(float horizontalMovement, float verticalMovement, bool jump)
    {
        bool climbing = isNearLadder && verticalMovement > 0;
        playerRigidbody.gravityScale = climbing ? 0 : originalGravityScale;
        float _verticalMovement = climbing ? verticalMovement * climbingSpeed : playerRigidbody.velocity.y;
        float _horizontalMovement = horizontalMovement * movementSpeed;

        playerRigidbody.velocity = new Vector2(_horizontalMovement, _verticalMovement);

        if (isGrounded && jump)
        {
            isGrounded = false;
            playerRigidbody.AddForce(new Vector2(0f, jumpForce));
        }

        HandleAnimations(_horizontalMovement, jump, climbing);
    }

    private void HandleNavMeshMovement()
    {
        bool jump = agent.isOnOffMeshLink && isGrounded;

        bool climbing = isNearLadder && agent.velocity.y > 0.15f;
        HandleAnimations(agent.velocity.x, jump, climbing);
    }

    private void HandleAnimations(float horizontalMovement, bool jumping, bool climbing)
    {
        if (jumping)
        {
            animator.SetBool("IsJumping", true);
            if (agent.enabled && offMeshLinkMoveCoroutine == null)
            {
                offMeshLinkMoveCoroutine = StartCoroutine(StartOffMeshLinkMove());
            }
        }
        animator.SetBool("IsClimbing", climbing);

        if (!climbing) 
            animator.SetFloat("Speed", Mathf.Abs(horizontalMovement));

        if (isGrounded) 
            animator.SetBool("IsJumping", false);

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

    private void Reset()
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

    IEnumerator StartOffMeshLinkMove()
    {
        disableGroundCheck = true;
        isGrounded = false;
        if (agentLinkMover.navigationMethod == OffMeshLinkMoveMethod.NormalSpeed)
            yield return StartCoroutine(agentLinkMover.NormalSpeed(agent));
        else if (agentLinkMover.navigationMethod == OffMeshLinkMoveMethod.Parabola)
            yield return StartCoroutine(agentLinkMover.Parabola(agent, 2.0f, 0.5f));
        else if (agentLinkMover.navigationMethod == OffMeshLinkMoveMethod.Curve)
            yield return StartCoroutine(agentLinkMover.Curve(agent, 0.5f));

        agent.CompleteOffMeshLink();
        offMeshLinkMoveCoroutine = null;
        disableGroundCheck = false;
        yield return null;
    }
}
