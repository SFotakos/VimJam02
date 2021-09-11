using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public PlayerController controller;
    [SerializeField] private PlayerCombat playerCombat;
    public Animator animator;

    float horizontalMovement = 0f;
    bool shouldJump = false;

    float verticalMovement = 0f;

    private void Awake() => controller.groundedCallback = Landed;

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

        controller.Move(horizontalMovement * Time.fixedDeltaTime, verticalMovement * Time.fixedDeltaTime, shouldJump);
        shouldJump = false;
    }

    private void Landed() => animator.SetBool("IsJumping", false);
}
