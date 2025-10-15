using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("ÒÆ¶¯²ÎÊý")]
    public float moveSpeed = 5f;
    public float jumpForce = 400f;
    public LayerMask groundLayer;
    public float groundCheckDistance = 1f;

    private Rigidbody2D rb;
    private float moveInput;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float horizontal = moveInput;
        if (horizontal < 0 && !GameManager.instance.canMoveLeft) horizontal = 0;
        if (horizontal > 0 && !GameManager.instance.canMoveRight) horizontal = 0;
        rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);

        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<float>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!GameManager.instance.canJump) return;
        if (context.performed && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce);
            EventManager.Trigger(EventType.OnLevel1FirstJump);
        }
    }
}
