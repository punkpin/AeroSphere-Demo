using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("移动参数")]
    public float moveSpeed = 5f;
    public float jumpForce = 400f;
    public LayerMask groundLayer;
    public float groundCheckDistance = 1f;

    [HideInInspector]public Rigidbody2D rb;
    private float moveInput;
    private bool isGrounded;
    public float HP;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float horizontal = moveInput;
        if (GameManager.instance.currentLevel.hasFirstJump)
        {
            if (GameManager.instance.esc.canMove)
            {
                if (GameManager.instance.canMoveRight && !GameManager.instance.canMoveLeft)
                {
                    horizontal = horizontal > 0 ? horizontal : 0;
                }
                else if (GameManager.instance.canMoveLeft && !GameManager.instance.canMoveRight)
                {
                    horizontal = horizontal > 0 ? -Mathf.Abs(horizontal) : 0;
                }
                else
                {
                    horizontal = 0;
                }
            }

            if (!GameManager.instance.canMoveLeft && horizontal < 0)
            {
                horizontal = 0;
            }
            if (!GameManager.instance.canMoveRight && horizontal > 0)
            {
                horizontal = 0;
            }
        }

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

    public void OnEsc(InputAction.CallbackContext context)
    {
        if (GameManager.instance.esc.canMove)
        {
            GameManager.instance.esc.canMove = false;
            GameManager.instance.esc.escText.text = "Esc";
        }
        else
        {
            GameManager.instance.esc.canMove = true;
            GameManager.instance.esc.escText.text = "Right";
        }
    }
    #region 角色与敌人碰撞
    void OnCollisionEnter2D(Collision2D collision)//检测敌人
    {
        GameObject enemy = collision.gameObject;
        int layer = collision.gameObject.layer;
        string layerName = LayerMask.LayerToName(layer);
        string objectName = collision.gameObject.name;


        if (layerName == "Enemy")//层次判断
        {
            HPChange();
            Debug.Log("eee");
        }

    }
    public void HPChange()
    {
        HP -= 1;
        Debug.Log(HP);
    }
    #endregion

}
