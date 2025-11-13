// 파일 이름: CooldownTimer.cs

using UnityEngine;

/// <summary>
/// '초' 단위 시간 간격을 관리하는 재사용 가능한 타이머 클래스입니다.
/// 쿨타임, 주기적 행동 등 모든 시간 기반 로직에 사용됩니다.
/// </summary>
public class CooldownTimer
{
    private readonly float duration; // 이 타이머의 전체 시간 (초)
    private float currentTime;       // 현재 남은 시간 (초)

    /// <summary>
    /// 지정된 시간(초)을 갖는 새로운 쿨타임 타이머를 생성합니다.
    /// </summary>
    public CooldownTimer(float durationSeconds)
    {
        this.duration = durationSeconds;
        this.currentTime = durationSeconds; // 생성 시 쿨타임이 가득 찬 상태로 시작
    }

    /// <summary>
    /// 타이머가 준비되었는지 (0 이하인지) 확인합니다.
    /// </summary>
    public bool IsReady => currentTime <= 0f;

    /// <summary>
    /// 경과된 시간(초)만큼 타이머를 진행시킵니다.
    /// </summary>
    public void Tick(float elapsedSeconds)
    {
        if (currentTime > 0f)
        {
            currentTime -= elapsedSeconds;
        }
    }

    /// <summary>
    /// 타이머를 원래 설정된 시간으로 다시 초기화합니다.
    /// </summary>
    public void Reset()
    {
        currentTime = duration;
    }

    /// <summary>
    /// 타이머를 즉시 준비된 상태(0)로 만듭니다.
    /// </summary>
    public void ForceReady()
    {
        currentTime = 0f;
    }
}