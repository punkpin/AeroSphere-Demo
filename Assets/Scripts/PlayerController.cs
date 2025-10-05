using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState { Empty, Full }
    public enum PlayerMoveState { Idle, Jumping, Falling }
    [Header("Player State")]
    public PlayerState playerSate = PlayerState.Empty;
    public PlayerMoveState playerMoveSate = PlayerMoveState.Idle;
    public Vector2 centerOffset;
    private SpriteRenderer spriteRenderer;

    [Header("Movement Settings")]
    public int moveDistance = 1;
    public float moveDuration = 0.4f;
    public int jumpHeight = 2;
    public int dashDistance = 5;
    public float jumpStepTime = 0.5f;
    public float fallStepTime = 0.25f;
    public float dashStepTime = 0.1f;

    private Vector2Int gridPosition;
    private Vector2Int targetGridPosition;

    // 移动相关
    public bool isMoving = false;
    public int moveDirection = 0;
    private int moveStepCount = 0;
    private int moveStepTarget = 0;
    private int moveDirCache = 0;

    // 跳跃相关
    private bool isJumping = false;
    private int jumpProgress = 0;
    private int jumpMoveCount = 0;
    private int jumpMoveDir = 0;
    private bool jumpMoveLocked = false;
    private bool hasJumpDash = false;

    // 下落相关
    private bool isFalling = false;

    // 冲刺相关
    private bool isDashing = false;
    private int dashProgress = 0;
    private int dashDir = 1;

    void Start()
    {
        gridPosition = GridPhysicsManager.instance.WorldToGrid(transform.position);
        targetGridPosition = gridPosition;
        centerOffset = new Vector2(8f, 8f);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        // 冲刺过程
        if (isDashing)
        {
            Vector3 targetWorld = GridPhysicsManager.instance.GridToWorld(targetGridPosition) + (Vector3)centerOffset;
            float distX = Mathf.Abs(transform.position.x - targetWorld.x);
            if (distX < 0.01f)
            {
                if (dashProgress == dashDistance)
                {
                    isDashing = false;
                    if (!isJumping && !isFalling && moveDirection != 0 && !isMoving)
                    {
                        isMoving = true;
                        moveStepCount = 0;
                        moveStepTarget = moveDistance;
                        moveDirCache = moveDirection;
                    }
                }
                if (dashProgress < dashDistance)
                {
                    Vector2Int nextPos = new Vector2Int(targetGridPosition.x + dashDir, targetGridPosition.y);
                    if (!GridPhysicsManager.instance.IsCellOccupied(nextPos))
                    {
                        targetGridPosition.x = nextPos.x;
                        dashProgress++;
                    }
                    else
                    {
                        isDashing = false;
                        if (!isJumping && !isFalling && moveDirection != 0 && !isMoving)
                        {
                            isMoving = true;
                            moveStepCount = 0;
                            moveStepTarget = moveDistance;
                            moveDirCache = moveDirection;
                        }
                    }
                }
            }
        }

        // 左右移动
        if ((isJumping || isFalling) && jumpMoveCount < moveDistance && jumpMoveDir != 0)
        {
            Vector2Int dir = jumpMoveDir > 0 ? Vector2Int.right : Vector2Int.left;
            Vector2Int nextPos = new Vector2Int(targetGridPosition.x + dir.x, targetGridPosition.y);

            Vector3 targetWorld = GridPhysicsManager.instance.GridToWorld(targetGridPosition) + (Vector3)centerOffset;
            float distX = Mathf.Abs(transform.position.x - targetWorld.x);
            if (distX < 0.01f)
            {
                if (!GridPhysicsManager.instance.IsCellOccupied(nextPos))
                {
                    targetGridPosition.x = nextPos.x;
                    jumpMoveCount++;
                }
                else
                {
                    jumpMoveCount = moveDistance;
                }
            }
        }
        else if (!isJumping && !isFalling && isMoving && moveStepCount < moveStepTarget)
        {
            Vector2Int dir = moveDirCache > 0 ? Vector2Int.right : Vector2Int.left;
            Vector2Int nextPos = new Vector2Int(targetGridPosition.x + dir.x, targetGridPosition.y);

            Vector3 targetWorld = GridPhysicsManager.instance.GridToWorld(targetGridPosition) + (Vector3)centerOffset;
            float distX = Mathf.Abs(transform.position.x - targetWorld.x);
            if (distX < 0.01f)
            {
                if (!GridPhysicsManager.instance.IsCellOccupied(nextPos))
                {
                    targetGridPosition.x = nextPos.x;
                    moveStepCount++;
                }
                else
                {
                    moveStepCount = moveStepTarget;
                }
            }
        }
        if (!isJumping && !isFalling && isMoving && moveStepCount >= moveStepTarget)
        {
            if (moveDirection == 0)
            {
                isMoving = false;
            }
            else
            {
                moveStepCount = 0;
                moveStepTarget = moveDistance;
                moveDirCache = moveDirection;
            }
        }

        // 跳跃过程
        if (isJumping)
        {
            Vector3 targetWorld = GridPhysicsManager.instance.GridToWorld(targetGridPosition) + (Vector3)centerOffset;
            float distY = Mathf.Abs(transform.position.y - targetWorld.y);
            if (distY < 0.01f)
            {
                if (jumpProgress == jumpHeight)
                {
                    isJumping = false;
                    isFalling = true;
                }
                if (jumpProgress < jumpHeight)
                {
                    Vector2Int nextPos = new Vector2Int(targetGridPosition.x, targetGridPosition.y + 1);
                    if (!GridPhysicsManager.instance.IsCellOccupied(nextPos))
                    {
                        targetGridPosition.y = nextPos.y;
                        jumpProgress++;
                    }
                    else
                    {
                        if (jumpProgress > 0)
                        {
                            isJumping = false;
                            isFalling = true;
                        }
                        else
                        {
                            isJumping = false;
                        }
                    }
                }
            }
        }
        // 下落过程
        else if (isFalling)
        {
            Vector3 targetWorld = GridPhysicsManager.instance.GridToWorld(targetGridPosition) + (Vector3)centerOffset;
            float distY = Mathf.Abs(transform.position.y - targetWorld.y);
            if (distY < 0.01f)
            {
                Vector2Int nextPos = new Vector2Int(targetGridPosition.x, targetGridPosition.y - 1);
                if (!IsGrounded() && !GridPhysicsManager.instance.IsCellOccupied(nextPos))
                {
                    targetGridPosition.y = nextPos.y;
                }
                else
                {
                    isFalling = false;
                }
            }
        }
        else if (!IsGrounded())
        {
            isFalling = true;
        }

        // 插值到目标位置
        Vector3 finalTargetWorld = GridPhysicsManager.instance.GridToWorld(targetGridPosition) + (Vector3)centerOffset;
        float xDuration = isDashing ? dashStepTime : moveDuration;
        float newX = Mathf.MoveTowards(transform.position.x, finalTargetWorld.x, Time.deltaTime / xDuration * GridPhysicsManager.instance.GridSize);
        float yDuration = isJumping ? jumpStepTime : (isFalling ? fallStepTime : moveDuration);
        float newY = Mathf.MoveTowards(transform.position.y, finalTargetWorld.y, Time.deltaTime / yDuration * GridPhysicsManager.instance.GridSize);
        transform.position = new Vector3(newX, newY, transform.position.z);

        // 更新gridPosition
        gridPosition = GridPhysicsManager.instance.WorldToGrid(transform.position - (Vector3)centerOffset);

        if (!isJumping && !isFalling && !isDashing)
        {
            jumpMoveCount = 0;
            jumpMoveDir = 0;
            jumpMoveLocked = false;
            hasJumpDash = false;

            if (moveDirection == 0)
            {
                isMoving = false;
                moveStepCount = 0;
                moveStepTarget = 0;
            }
            else if (!isMoving)
            {
                isMoving = true;
                moveStepCount = 0;
                moveStepTarget = moveDistance;
                moveDirCache = moveDirection;
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        float input = context.ReadValue<float>();
        int inputDir = Mathf.Abs(input) > 0.01f ? (input > 0 ? 1 : -1) : 0;

        if (isJumping || isFalling)
        {
            bool facingLeft = spriteRenderer.flipX;
            if ((hasJumpDash || jumpMoveLocked) && inputDir != 0 && inputDir != jumpMoveDir)
            {
                return;
            }
            if (!hasJumpDash && inputDir != 0)
            {
                jumpMoveDir = inputDir;
                if (spriteRenderer != null)
                    spriteRenderer.flipX = inputDir < 0;
            }
            if (!jumpMoveLocked && inputDir != 0)
            {
                jumpMoveLocked = true;
            }
        }
        else
        {
            if (spriteRenderer != null)
            {
                if (moveDirection > 0)
                    spriteRenderer.flipX = false;
                else if (moveDirection < 0)
                    spriteRenderer.flipX = true;
            }
        }

        if (inputDir != 0 && !isMoving)
        {
            isMoving = true;
            moveStepCount = 0;
            moveStepTarget = moveDistance;
            moveDirCache = inputDir;
        }
        moveDirection = inputDir;
    }

    public void OnJump(InputAction.CallbackContext context)
    {   
        if (isDashing) return;
        if (context.performed && IsGrounded() && !isJumping && !isFalling)
        {
            isJumping = true;
            jumpProgress = 0;
            jumpMoveCount = 0;
            jumpMoveDir = 0;
            jumpMoveLocked = false;

            if (moveDirection != 0)
            {
                jumpMoveDir = moveDirection;
                jumpMoveLocked = true;
            }

            isMoving = false;
            moveStepCount = 0;
            moveStepTarget = 0;
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed && !isDashing && !hasJumpDash)
        {
            isDashing = true;
            dashProgress = 0;
            dashDir = spriteRenderer.flipX ? -1 : 1;
            if (isJumping || isFalling)
            {
                hasJumpDash = true;
            }
        }
    }

    public bool IsGrounded()
    {
        Vector2Int below = new Vector2Int(gridPosition.x, gridPosition.y - 1);
        return GridPhysicsManager.instance.IsCellOccupied(below);
    }
}
