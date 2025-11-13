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

    public void SetInputs(Vector2 direction, bool walking)
    {
        this.moveDirection = direction;
        this.isWalking = walking;
    }

    protected override void Move()
    {
        // 1. 상태 확인 (Ground Check) - 물리 업데이트 직전에 하는 것이 가장 정확하다.
        isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckBoxSize, 0f, groundLayer);

        // 2. 이동 실행
        float targetSpeed = isWalking ? entityCore.Data.moveSpeed * walkSpeedMultiplier : entityCore.Data.moveSpeed;
        rb.linearVelocity = new Vector2(moveDirection.x * targetSpeed, rb.linearVelocity.y);
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

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireCube(groundCheck.position, groundCheckBoxSize);
    }
}