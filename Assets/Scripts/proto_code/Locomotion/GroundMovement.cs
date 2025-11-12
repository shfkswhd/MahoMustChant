// 파일 이름: GroundMovement.cs

using UnityEngine;

// 상속 대상을 NonPlayerMovement로 변경합니다.
public class GroundMovement : NonPlayerMovement
{
    private Vector2 targetMoveDirection;

    public override void SetMoveDirection(Vector2 direction)
    {
        targetMoveDirection = direction;
    }

    public override void OnTick()
    {
        // 부모의 보간 변수를 사용합니다.
        previousPosition = rb.position;

        float targetSpeed = entityCore.Data.moveSpeed;
        rb.linearVelocity = new Vector2(targetMoveDirection.x * targetSpeed, rb.linearVelocity.y);
        
        // 부모의 보간 변수를 사용합니다.
        currentPosition = rb.position + rb.linearVelocity * Time.fixedDeltaTime;
    }
}