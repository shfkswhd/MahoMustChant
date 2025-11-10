using UnityEngine;

public class GroundMovement : Locomotion
{
    // Move 메서드 재정의
    public override void Move(Vector2 direction)
    {
        float BasemoveSpeed = entityCore.Data.moveSpeed;
        // 지면 이동 로직 구현
        // 새로운 위치 계산
        Vector2 newPosition = rb.position + (direction * BasemoveSpeed * Time.fixedDeltaTime);       
        // 계산된 위치로 리지드바디 이동
        rb.MovePosition(newPosition);
    }
}