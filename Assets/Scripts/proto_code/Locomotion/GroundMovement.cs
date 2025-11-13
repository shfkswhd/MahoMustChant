// 파일 이름: GroundMovement.cs

using UnityEngine;

public class GroundMovement : NonPlayerMovement
{
    // SetMoveDirection 메서드는 부모(Locomotion)의 것을 그대로 사용하므로,
    // 이 클래스에서 재정의(override)할 필요가 없다. 코드가 훨씬 깔끔해진다.

    protected override void Move()
    {
        // 예전 코드의 로직과 동일하다.
        // 부모로부터 물려받은 moveDirection 변수를 사용하여 속도를 설정한다.
        float targetSpeed = entityCore.Data.moveSpeed;
        rb.linearVelocity = new Vector2(moveDirection.x * targetSpeed, rb.linearVelocity.y);
    }
}