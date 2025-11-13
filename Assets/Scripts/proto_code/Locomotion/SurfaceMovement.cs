// 파일 이름: SurfaceMovement.cs

using UnityEngine;

public class SurfaceMovement : NonPlayerMovement
{
    [Header("표면 탐색 설정")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float surfaceCheckDistance = 0.5f;

    // SetMoveDirection은 부모의 것을 그대로 사용한다.

    protected override void Move()
    {
        // 예전 코드의 로직을 FixedUpdate 대신 Move() 안에 구현한다.
        RaycastHit2D hit = FindSurface();
        float targetSpeed = entityCore.Data.moveSpeed;

        if (hit.collider != null)
        {
            // 표면을 찾았을 때의 로직
            // 1. 오브젝트의 위 방향(up)을 표면의 법선 벡터(normal)와 일치시킨다.
            //    (자석처럼 표면에 착 달라붙는 효과)
            transform.up = hit.normal;
            rb.gravityScale = 0; // 표면에 붙었을 땐 중력을 끈다.

            // 2. 입력받은 moveDirection의 x값(좌/우)에 따라, 오브젝트의 오른쪽(right) 방향으로 이동한다.
            Vector2 moveVector = transform.right * moveDirection.x;

            // 3. 최종 속도를 설정한다.
            rb.linearVelocity = moveVector * targetSpeed;
        }
        else
        {
            // 표면을 찾지 못했을 때의 로직
            // 1. 일반적인 중력을 받도록 되돌린다.
            transform.up = Vector2.up; // 오브젝트의 방향을 원래대로
            rb.gravityScale = 1; // 중력 스케일을 원래대로 (또는 데이터에 정의된 값으로)

            // 2. 공중에서는 좌/우 이동만 제어한다.
            rb.linearVelocity = new Vector2(moveDirection.x * targetSpeed, rb.linearVelocity.y);
        }
    }

    private RaycastHit2D FindSurface()
    {
        Vector2 myPos = transform.position;
        Vector2 myDown = -transform.up;
        Vector2 myRight = transform.right;

        // 우선순위 1: 아래 확인
        RaycastHit2D hit = Physics2D.Raycast(myPos, myDown, surfaceCheckDistance, groundLayer);
        if (hit.collider != null) return hit;

        // 우선순위 2: 바로 앞 확인 (코너 안쪽으로 돌 때)
        hit = Physics2D.Raycast(myPos, myRight * moveDirection.x, surfaceCheckDistance, groundLayer);
        if (hit.collider != null) return hit;

        // 우선순위 3: 앞에서 아래 확인 (코너 바깥쪽으로 돌 때)
        hit = Physics2D.Raycast(myPos + (Vector2)transform.right * moveDirection.x * 0.5f, myDown, surfaceCheckDistance * 1.5f, groundLayer);
        if (hit.collider != null) return hit;

        return new RaycastHit2D();
    }
}