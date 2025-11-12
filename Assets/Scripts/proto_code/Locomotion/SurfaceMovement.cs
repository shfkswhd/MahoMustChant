// 파일 이름: SurfaceMovement.cs

using UnityEngine;

// 상속 대상을 NonPlayerMovement로 변경합니다.
public class SurfaceMovement : NonPlayerMovement
{
    [Header("표면 탐색 설정")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float surfaceCheckDistance = 0.5f;

    private Vector2 targetMoveDirection;
    
    public override void SetMoveDirection(Vector2 direction)
    {
        targetMoveDirection = direction;
    }

    public override void OnTick()
    {
        // 부모의 보간 변수를 사용합니다.
        previousPosition = rb.position;

        RaycastHit2D hit = FindSurface();
        float targetSpeed = entityCore.Data.moveSpeed;
        Vector2 newPosition;

        if (hit.collider != null)
        {
            transform.up = hit.normal;
            Vector2 moveVector = transform.right * targetMoveDirection.x;
            newPosition = rb.position + (moveVector * targetSpeed * Time.fixedDeltaTime);
        }
        else
        {
            Vector2 gravityDirection = Vector2.down;
            newPosition = rb.position + (gravityDirection * targetSpeed * Time.fixedDeltaTime);
        }
        rb.MovePosition(newPosition);

        // 부모의 보간 변수를 사용합니다.
        currentPosition = newPosition;
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