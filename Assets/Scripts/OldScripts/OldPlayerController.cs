using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OldPlayerController : MonoBehaviour
{
    public enum PlayerState { Empty, Full }
    public enum PlayerMoveState { Idle, Jumping, Falling }
    [Header("Player State")]
    public PlayerState playerState = PlayerState.Empty;
    private int curSize = 1;
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
    public float enlargeDuration = 0.5f;

    public Vector2Int gridPosition;
    public Vector2Int targetGridPosition;

    // 移动相关
    private bool isMoving = false;
    private int moveDirection = 0;
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

    // 切换状态相关
    private bool isChangingState = false;

    void Start()
    {
        ChangePlayerState(PlayerState.Empty);
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
                    StartCoroutine(ShrinkCo(0.5f));
                    if (!isJumping && !isFalling && moveDirection != 0 && !isMoving)
                    {
                        //isMoving = true;
                        moveStepCount = 0;
                        moveStepTarget = moveDistance;
                        moveDirCache = moveDirection;
                    }
                }
                if (dashProgress < dashDistance)
                {
                    Vector2Int nextPos = new Vector2Int(targetGridPosition.x + dashDir, targetGridPosition.y);
                    bool sideBlocked = false;
                    for (int i = 0; i < curSize; i++)
                    {
                        Vector2Int sideCell = new Vector2Int(nextPos.x + curSize - 1, nextPos.y + i);
                        if (GridPhysicsManager.instance.IsCellOccupied(sideCell))
                        {
                            sideBlocked = true;
                            break;
                        }
                    }
                    if (!sideBlocked)
                    {
                        targetGridPosition.x = nextPos.x;
                        dashProgress++;
                    }
                    else
                    {
                        isDashing = false;
                        StartCoroutine(ShrinkCo(0.5f));
                        if (!isJumping && !isFalling && moveDirection != 0 && !isMoving)
                        {
                            //isMoving = true;
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
            isMoving = true;
            Vector2Int dir = jumpMoveDir > 0 ? Vector2Int.right : Vector2Int.left;
            if (jumpMoveCount == 0)
            {
                int maxStep = moveDistance;
                int actualStep = 0;
                for (int step = 1; step <= maxStep; step++)
                {
                    bool blocked = false;
                    for (int i = 0; i < curSize; i++)
                    {
                        for (int j = 0; j < curSize; j++)
                        {
                            Vector2Int checkCell = new Vector2Int(
                                targetGridPosition.x + dir.x * step + i,
                                targetGridPosition.y + j
                            );
                            if (GridPhysicsManager.instance.IsCellOccupied(checkCell))
                            {
                                blocked = true;
                                break;
                            }
                        }
                        if (blocked) break;
                    }
                    if (blocked) break;
                    actualStep = step;
                }
                if (actualStep > 0)
                {
                    targetGridPosition.x += dir.x * actualStep;
                    jumpMoveCount = moveDistance;
                }
                else
                {
                    jumpMoveCount = moveDistance;
                    isMoving = false;
                }
            }
        }
        else if (!isJumping && !isFalling && moveStepCount < moveStepTarget)
        {
            isMoving = true;
            Vector2Int dir = moveDirCache > 0 ? Vector2Int.right : Vector2Int.left;
            if (moveStepCount == 0)
            {
                int maxStep = moveStepTarget;
                int actualStep = 0;
                for (int step = 1; step <= maxStep; step++)
                {
                    bool blocked = false;
                    for (int i = 0; i < curSize; i++)
                    {
                        for (int j = 0; j < curSize; j++)
                        {
                            Vector2Int checkCell = new Vector2Int(
                                targetGridPosition.x + dir.x * step + i,
                                targetGridPosition.y + j
                            );
                            if (GridPhysicsManager.instance.IsCellOccupied(checkCell))
                            {
                                blocked = true;
                                break;
                            }
                        }
                        if (blocked) break;
                    }
                    if (blocked) break;
                    actualStep = step;
                }
                if (actualStep > 0)
                {
                    targetGridPosition.x += dir.x * actualStep;
                    moveStepCount = moveStepTarget;
                }
                else
                {
                    moveStepCount = moveStepTarget;
                    isMoving = false;
                }
            }
        }
        if (!isJumping && !isFalling && isMoving && moveStepCount >= moveStepTarget)
        {
            Vector3 targetWorld = GridPhysicsManager.instance.GridToWorld(targetGridPosition) + (Vector3)centerOffset;
            float distX = Mathf.Abs(transform.position.x - targetWorld.x);
            if (distX < 0.01f)
            {
                isMoving = false;
            }
            //if (moveDirection != 0)
            //{
            //    moveStepCount = 0;
            //    moveStepTarget = moveDistance;
            //    moveDirCache = moveDirection;
            //}
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
                    bool topBlocked = false;
                    for (int i = 0; i < curSize; i++)
                    {
                        Vector2Int topCell = new Vector2Int(targetGridPosition.x + i, targetGridPosition.y + curSize);
                        if (GridPhysicsManager.instance.IsCellOccupied(topCell))
                        {
                            topBlocked = true;
                            break;
                        }
                    }
                    if (!topBlocked)
                    {
                        targetGridPosition.y += 1;
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
                //isMoving = false;
                moveStepCount = 0;
                moveStepTarget = 0;
            }
            else if (!isMoving && moveDirection != 0)
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
        if (isChangingState) return;
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
        if (isChangingState) return;
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
        if (isChangingState) return;
        if (context.performed && !isDashing && !hasJumpDash && playerState == PlayerState.Full)
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
        for (int i = 0; i < curSize; i++)
        {
            Vector2Int cell = new Vector2Int(gridPosition.x + i, gridPosition.y - 1);
            if (GridPhysicsManager.instance.IsCellOccupied(cell))
                return true;
        }
        return false;
    }

    public void OnEnlarge(InputAction.CallbackContext context)
    {
        if (isChangingState) return;
        if (context.performed && !isDashing && !isJumping && !isFalling && !isMoving && playerState == PlayerState.Empty)
        {
            StartCoroutine(EnlargeCo());
        }
    }

    public void OnShrink(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            StartCoroutine(ShrinkCo(0.5f));
        }
    }

    public IEnumerator EnlargeCo()
    {
        isChangingState = true;
        float gridSize = GridPhysicsManager.instance.GridSize; 
        float originalSize = gridSize; 
        float targetSize = gridSize * 2f;

        Vector3 originalScale = new Vector3(originalSize, originalSize, 1f);
        Vector3 targetScale = new Vector3(targetSize, targetSize, 1f);

        Vector3 anchorPos;
        if (spriteRenderer.flipX)
        {
            anchorPos = transform.position - new Vector3(originalSize / 2f, originalSize / 2f, 0);
        }
        else
        {
            anchorPos = transform.position - new Vector3(-originalSize / 2f, originalSize / 2f, 0);
        }

        float speed = (targetSize - originalSize) / enlargeDuration;
        float curSize = originalSize;

        while (curSize < targetSize)
        {
            curSize += speed * Time.deltaTime;
            if (curSize > targetSize) curSize = targetSize;
            Vector3 curScale = new Vector3(curSize, curSize, 1f);

            if (spriteRenderer.flipX)
            {
                transform.localScale = curScale;
                transform.position = anchorPos + new Vector3(curSize / 2f, curSize / 2f, 0);
            }
            else
            {
                transform.localScale = curScale;
                transform.position = anchorPos + new Vector3(-curSize / 2f, curSize / 2f, 0);
            }

            yield return null;
        }

        transform.localScale = targetScale;
        if (spriteRenderer.flipX)
        {
            transform.position = anchorPos + new Vector3(targetSize / 2f, targetSize / 2f, 0);
        }
        else
        {
            transform.position = anchorPos + new Vector3(-targetSize / 2f, targetSize / 2f, 0);
        }

        ChangePlayerState(PlayerState.Full);
        isChangingState = false;
    }

    public IEnumerator ShrinkCo(float duration)
    {
        isChangingState = true;
        float gridSize = GridPhysicsManager.instance.GridSize;
        float originalSize = gridSize * 2f;
        float targetSize = gridSize;        

        Vector3 originalScale = new Vector3(originalSize, originalSize, 1f);
        Vector3 targetScale = new Vector3(targetSize, targetSize, 1f);

        Vector3 anchorPos;
        if (spriteRenderer.flipX)
        {
            anchorPos = transform.position - new Vector3(originalSize / 2f, originalSize / 2f, 0);
        }
        else
        {
            anchorPos = transform.position + new Vector3(originalSize / 2f, -originalSize / 2f, 0);
        }

        float speed = (originalSize - targetSize) / duration;
        float curSize = originalSize;

        while (curSize > targetSize)
        {
            curSize -= speed * Time.deltaTime;
            if (curSize < targetSize) curSize = targetSize;
            Vector3 curScale = new Vector3(curSize, curSize, 1f);

            if (spriteRenderer.flipX)
            {
                transform.localScale = curScale;
                transform.position = anchorPos + new Vector3(curSize / 2f, curSize / 2f, 0);
            }
            else
            {
                transform.localScale = curScale;
                transform.position = anchorPos + new Vector3(-curSize / 2f, curSize / 2f, 0);
            }

            yield return null;
        }

        transform.localScale = targetScale;
        if (spriteRenderer.flipX)
        {
            transform.position = anchorPos + new Vector3(targetSize / 2f, targetSize / 2f, 0);
        }
        else
        {
            transform.position = anchorPos + new Vector3(-targetSize / 2f, targetSize / 2f, 0);
        }

        ChangePlayerState(PlayerState.Empty);
        isChangingState = false;
    }

    public void ChangePlayerState(PlayerState playerState)
    {
        if (playerState == PlayerState.Empty)
        {
            moveDistance = 1;
            centerOffset = new Vector2(8f, 8f);
            curSize = 1;
            gridPosition = GridPhysicsManager.instance.WorldToGrid(transform.position);
            targetGridPosition = gridPosition;
        }
        else
        {
            moveDistance = 2;
            centerOffset = new Vector2(16f, 16f);
            curSize = 2;
            int curPlayerGridSize = 32;
            Vector3 leftBottomWorld = transform.position - new Vector3(curPlayerGridSize / 2f, curPlayerGridSize / 2f, 0);
            gridPosition = GridPhysicsManager.instance.WorldToGrid(leftBottomWorld);
            targetGridPosition = gridPosition;
        }
        this.playerState = playerState;
    }
}