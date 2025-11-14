// 파일 이름: PlayerMovement.cs

using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : Locomotion
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeedMultiplier = 0.5f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float coyoteTime = 0.1f;
    private float coyoteTimeCounter;

    [Header("Ground Check")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 groundCheckBoxSize = new Vector2(0.5f, 0.1f);
    private bool isGrounded;

    // Mediator로부터 전달받을 입력 상태
    private bool isWalking;

    public void SetInputs(Vector2 direction, bool walking)
    {
        this.moveDirection = direction;
        this.isWalking = walking;
    }

    // Unity의 일반 Update는 이제 코요테 타임 계산 같은 비-물리 로직에만 사용한다.
    private void Update()
    {
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    /// <summary>
    /// FixedUpdate마다 호출되며, 모든 물리 관련 로직을 처리합니다.
    /// </summary>
    protected override void Move() // 또는 ApplyMovement()
    {
        // 1. 상태 확인 (Ground Check)
        isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckBoxSize, 0f, groundLayer);

        // 2. 이동 실행
        float targetSpeed = isWalking ? entityCore.Data.moveSpeed * walkSpeedMultiplier : entityCore.Data.moveSpeed;
        rb.linearVelocity = new Vector2(moveDirection.x * targetSpeed, rb.linearVelocity.y);

        // 3. 좌우 반전 로직
        if (moveDirection.x != 0)
        {
            // 목표 방향 (오른쪽: 1, 왼쪽: -1)
            float targetDirection = Mathf.Sign(moveDirection.x);

            // 현재 바라보고 있는 방향
            float currentDirection = Mathf.Sign(transform.localScale.x);

            // 목표 방향과 현재 방향이 다를 때만 뒤집는다.
            if (targetDirection != currentDirection)
            {
                // 현재 localScale 값을 가져와서 x 값에만 -1을 곱한다.
                Vector3 newScale = transform.localScale;
                newScale.x *= -1;
                transform.localScale = newScale;
            }
        }
    }

    public void Jump()
    {
        if (coyoteTimeCounter > 0f)
        {
            coyoteTimeCounter = 0f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (groundCheck == null) return;
        Gizmos.color = isGrounded ? Color.cyan : Color.magenta;
        Gizmos.DrawWireCube(groundCheck.position, groundCheckBoxSize);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)(moveDirection.normalized * 1f));
    }
#endif
}