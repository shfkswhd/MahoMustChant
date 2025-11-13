// 파일 이름: AIIntent.cs

/// <summary>
/// IState가 EntityBrain에게 전달하는 '의도'의 종류를 정의합니다.
/// </summary>
public enum AIIntent
{
    None,            // 아무것도 하지 않음
    Wander,          // 특정 방향으로 배회
    Pursue,          // 특정 대상을 추격
    Flee,            // 특정 대상으로부터 도망
    Attack           // 특정 대상을 공격
}