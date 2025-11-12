// 파일 이름: NonPlayerMovement.cs

using UnityEngine;

/// <summary>
/// 플레이어가 아닌 모든 엔티티(AI, NPC 등)를 위한 이동 클래스의 부모입니다.
/// Locomotion을 상속받아 틱 기반 시스템에 참여하며,
/// 시각적 보간(Interpolation) 기능을 자식 클래스들에게 제공합니다.
/// </summary>
public abstract class NonPlayerMovement : Locomotion
{
    // --- 보간을 위한 공통 변수들 ---
    protected Vector2 previousPosition;
    protected Vector2 currentPosition;

    protected override void Awake()
    {
        base.Awake(); // Locomotion의 Awake() 실행
        // 보간을 위해 초기 위치 설정
        previousPosition = rb.position;
        currentPosition = rb.position;
    }

    /// <summary>
    /// 매 프레임 호출되어 렌더링될 위치를 부드럽게 보간합니다.
    /// 이 클래스를 상속받는 모든 자식들은 이 기능을 자동으로 갖게 됩니다.
    /// </summary>
    protected virtual void Update()
    {
        // 틱 시간과 렌더링 시간 사이의 진행률(alpha)을 계산합니다.
        // 이 값은 0.0(이전 틱 시점)과 1.0(다음 틱 시점) 사이를 오갑니다.
        float alpha = (Time.time - Time.fixedTime) / Time.fixedDeltaTime;

        // transform.position을 이전 틱 위치와 현재(이번 틱) 위치 사이로 부드럽게 이동시킵니다.
        transform.position = Vector2.Lerp(previousPosition, currentPosition, alpha);
    }
}