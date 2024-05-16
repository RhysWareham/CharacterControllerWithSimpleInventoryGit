using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region playerComponents
    private Rigidbody2D rb2d;
    private CapsuleCollider2D capsuleCollider;
    private Animator animator;
    #endregion

    #region playerInput
    private PlayerInput playerInput;
    private InputAction jumpAction;
    private InputAction moveAction;
    #endregion

    private float startingScaleX;

    [Header("Movement")]
    [SerializeField] private float baseMoveSpeed = 5;

    [Header("Jumping")]
    [SerializeField] private float jumpForce = 2;
    [SerializeField] private LayerMask jumpableLayers;
    private bool isJumping = false;

    public Vector2 RawMoveInput { get; private set; }

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        jumpAction = playerInput.actions["Jump"];
        moveAction = playerInput.actions["Move"];
    }

    private void OnEnable()
    {
        jumpAction.started += JumpAction_started;
        moveAction.performed += MoveAction_performed;
        moveAction.canceled += MoveAction_cancelled;
    }

    private void OnDisable()
    {
        jumpAction.started -= JumpAction_started;
        moveAction.performed -= MoveAction_performed;
        moveAction.canceled -= MoveAction_cancelled;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        animator = GetComponentInChildren<Animator>();
        startingScaleX = transform.localScale.x;
    }

    private void FixedUpdate()
    {
        Move();

        bool playerHasHorziontalSpeed = Mathf.Abs(rb2d.velocity.x) > Mathf.Epsilon;
        SetMoveAnimation(playerHasHorziontalSpeed);
        Flip(playerHasHorziontalSpeed);
    }

    private void Move()
    {
        rb2d.velocity = new Vector2((RawMoveInput.x * baseMoveSpeed), rb2d.velocity.y);
    }

    private void SetMoveAnimation(bool hasHorizontalSpeed)
    {
        animator.SetBool("IsMoving", hasHorizontalSpeed);
    }

    private void TryJump()
    {
        if (!capsuleCollider.IsTouchingLayers(jumpableLayers))
        {
            return;
        }

        animator.SetBool("IsJumping", true);
        rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
        isJumping = true;
    }

    private void Flip(bool hasHorizontalSpeed)
    {
        transform.localScale = new Vector2(Mathf.Sign(rb2d.velocity.x) * startingScaleX, 1);
    }

    private void MoveAction_performed(InputAction.CallbackContext obj)
    {
        RawMoveInput = obj.ReadValue<Vector2>();
    }

    private void MoveAction_cancelled(InputAction.CallbackContext obj)
    {
        RawMoveInput = new Vector2(0, 0);
        animator.SetBool("IsMoving", false);
    }

    private void JumpAction_started(InputAction.CallbackContext obj)
    {
        TryJump();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Ground>())
        {
            if (isJumping)
            {
                isJumping = false;
                animator.SetBool("IsJumping", false);
            }
        }
    }
}
