// 파일 이름: PlayerMovement.cs (수정)

using UnityEngine;

public class PlayerMovement : Locomotion
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeedMultiplier = 0.5f;
    // crouchSpeedMultiplier 등 필요시 추가

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float coyoteTime = 0.1f;
    private float coyoteTimeCounter;

    [Header("Ground Check Settings")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 groundCheckBoxSize = new Vector2(0.5f, 0.1f);
    private bool isGrounded;

    // Mediator로부터 전달받을 현재 틱의 입력 상태
    private Vector2 targetMoveDirection;
    private bool isWalking;
    private bool isCrouching;

    /// <summary>
    /// 외부(PlayerMediator)에서 호출하여 이번 틱에 적용될 이동 입력을 설정합니다.
    /// </summary>
    public void SetInputs(Vector2 moveDirection, bool walking, bool crouching)
    {
        targetMoveDirection = moveDirection;
        isWalking = walking;
        isCrouching = crouching;
    }

    /// <summary>
    /// 부모 클래스 Locomotion의 추상 메서드 계약을 지키기 위해 구현합니다.
    /// 플레이어의 경우, PlayerMediator가 더 구체적인 SetInputs()를 직접 호출하므로
    /// 이 메서드는 일반적으로 사용되지 않습니다.
    /// </summary>
    public override void SetMoveDirection(Vector2 direction)
    {
        // PlayerMediator가 보내는 입력을 우선적으로 사용하기 때문에,
        // 이 메서드는 AI가 플레이어를 조종하는 등의 특수 케이스를 위해 열어둡니다.
        targetMoveDirection = direction;
    }


    /// <summary>
    /// TickManager에 의해 주기적으로 호출됩니다.
    /// 플레이어의 모든 물리 관련 로직이 이곳에서 순서대로 처리됩니다.
    /// </summary>
    public override void OnTick()
    {
        // 1. 상태 확인 (Collision, Ground Check 등)
        CheckIfGrounded();

        // 2. 시간 기반 상태 갱신 (코요테 타임 등)
        UpdateCoyoteTime();

        // 3. 입력에 따른 행동 처리 (이동, 점프 등)
        HandleMovement();
    }

    private void CheckIfGrounded()
    {
        isGrounded = Physics2D.BoxCast(groundCheck.position, groundCheckBoxSize, 0f, Vector2.down, 0.1f, groundLayer);
    }

    private void UpdateCoyoteTime()
    {
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            // Time.deltaTime 대신 TickInterval을 사용하여 틱 기반으로 시간을 차감합니다.
            coyoteTimeCounter -= Time.fixedDeltaTime;
        }
    }

    private void HandleMovement()
    {
        // 현재 상태에 맞는 이동 속도 계산
        float targetSpeed = entityCore.Data.moveSpeed; // 기본 속도
        if (isCrouching) { /* 웅크리기 속도 로직 */ }
        else if (isWalking)
        {
            targetSpeed *= walkSpeedMultiplier;
        }

        // 최종 속도 적용
        rb.linearVelocity = new Vector2(targetMoveDirection.x * targetSpeed, rb.linearVelocity.y);
    }

    public void Jump()
    {
        // 코요테 시간이 남아있을 때만 점프를 실행한다.
        if (coyoteTimeCounter > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            coyoteTimeCounter = 0f;
        }
    }

    private void OnDrawGizmosSelected() { 
        if (groundCheck == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(groundCheck.position + Vector3.down * 0.1f, groundCheckBoxSize);
    }
}