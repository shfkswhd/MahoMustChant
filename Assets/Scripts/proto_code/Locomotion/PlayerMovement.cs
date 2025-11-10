using UnityEngine;

public class PlayerMovement : Locomotion
{
    [Header("점프 설정")]
    [SerializeField] private float jumpForce = 10f;

    [Header("바닥 체크 (박스캐스트)")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 groundCheckBoxSize = new Vector2(0.5f, 0.1f); // 체크할 박스의 크기
    private bool isGrounded;

    [Header("코요테 타임")]
    [SerializeField] private float coyoteTime = 0.1f;
    private float coyoteTimeCounter;

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

    private void FixedUpdate()
    {
        // OverlapCircle 대신 BoxCast 사용!
        // groundCheck 위치에서, groundCheckBoxSize 크기의 박스를, 0도 회전으로, 아래쪽으로 0.1f만큼 쏴서 groundLayer와 충돌하는지 확인
        isGrounded = Physics2D.BoxCast(groundCheck.position, groundCheckBoxSize, 0f, Vector2.down, 0.1f, groundLayer);
    }

    public override void Move(Vector2 direction)
    {
        rb.linearVelocity = new Vector2(direction.x * entityCore.Data.moveSpeed, rb.linearVelocity.y);
    }

    public void Move(float moveX, bool isWalking)
    {
        float walkSpeedMultiplier = 0.5f;
        float targetSpeed = isWalking ? entityCore.Data.moveSpeed * walkSpeedMultiplier : entityCore.Data.moveSpeed;
        rb.linearVelocity = new Vector2(moveX * targetSpeed, rb.linearVelocity.y);
    }

    public void Jump()
    {
        if (coyoteTimeCounter > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            coyoteTimeCounter = 0f;
        }
    }

    public void Crouch(bool isCrouching)
    {
        // 수그리기 로직
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.green;
        // 이제 원 대신 박스를 그립니다. BoxCast의 파라미터와 동일하게 그리는 것이 중요합니다.
        Gizmos.DrawWireCube(groundCheck.position + Vector3.down * 0.1f, groundCheckBoxSize);
    }
}