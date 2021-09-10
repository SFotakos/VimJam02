using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public PlayerController controller;
    [SerializeField] private PlayerCombat playerCombat;
    public Animator animator;

    [SerializeField] private float playerSpeed = 350f;
    float horizontalMovement = 0f;
    bool shouldJump = false;

    private void Awake() => controller.groundedCallback = Landed;

    void Update()
    {
        if (playerCombat.hasSnapped)
            return;

        horizontalMovement = Input.GetAxisRaw("Horizontal") * playerSpeed;

        if (Input.GetButtonDown("Jump") && !animator.GetBool("IsJumping"))
            shouldJump = true;
    }

    private void FixedUpdate()
    {
        if (playerCombat.hasSnapped)
            return;

        controller.Move(horizontalMovement * Time.fixedDeltaTime, shouldJump);
        animator.SetFloat("Speed", Mathf.Abs(horizontalMovement));

        if (shouldJump)
            animator.SetBool("IsJumping", true);

        shouldJump = false;
    }

    private void Landed() => animator.SetBool("IsJumping", false);
}
