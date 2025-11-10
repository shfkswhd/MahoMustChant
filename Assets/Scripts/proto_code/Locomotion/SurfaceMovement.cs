using UnityEngine;

// 표면을 붙어서 기어다니는 로직
public class SurfaceMovement : Locomotion
{
    [Header("표면 탐색 설정")]
    [SerializeField] private LayerMask groundLayer; //지면 레이어 마스크
    [SerializeField] private float surfaceCheckDistance = 0.5f; //레이캐스트 거리
    
    // Move 메서드 재정의
    public override void Move(Vector2 direction)
    {
        RaycastHit2D hit = FindSurface();
        float BasemoveSpeed = entityCore.Data.moveSpeed;
        if (hit.collider != null)
        {
            // 1. 오브젝트의 위 방향을 표면의 법선 벡터로 설정
            transform.up = hit.normal;

            // 2. 오브젝트의 오른쪽 방향을 이동 방향으로 설정
            Vector2 moveVector = transform.right * direction.x;

            // 3. 새로운 위치 계산
            Vector2 newPosition = rb.position + (moveVector * BasemoveSpeed * Time.fixedDeltaTime);
            rb. MovePosition(newPosition);
        }
        else
        {
            // 1. 표면을 찾지 못한 경우, 기본 중력 방향으로 떨어지도록 설정
            Vector2 gravityDirection = Vector2.down;
            // 2. 새로운 위치 계산
            Vector2 newPosition = rb.position + (gravityDirection * BasemoveSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPosition);
        }
    }

    private RaycastHit2D FindSurface()
    {
        // 표면을 찾기 위한 레이캐스트 로직 구현
        Vector2 myPos = transform.position;
        Vector2 myDown = -transform.up;
        Vector2 myRight = transform.right;

        // 우선순위 1 : 아래 확인
        RaycastHit2D hit = Physics2D.Raycast(myPos, myDown, surfaceCheckDistance, groundLayer);
        if (hit.collider != null) return hit;
        
        // 우선순위 2 : 바로 앞 확인(내회전 전환)
        hit = Physics2D.Raycast(myPos, myRight, surfaceCheckDistance, groundLayer);
        if (hit.collider != null) return hit;

        // 우선순위 3 : 앞에서 아래 확인(외회전 전환)
        hit = Physics2D.Raycast(myPos + myRight * 0.4f, myDown, surfaceCheckDistance, groundLayer);
        if (hit.collider != null) return hit;
        
        return new RaycastHit2D();
    }
}