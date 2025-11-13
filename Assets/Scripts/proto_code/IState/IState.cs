// 파일 이름: IState.cs

using UnityEngine;

/// <summary>
/// AI의 각 상태가 구현해야 할 행동 및 전환 규칙의 설계도입니다.
/// </summary>
public interface IState
{
    void OnEnter(EntityBrain brain);

    /// <summary>
    /// 매 턴마다 호출되어, 현재 상태의 '의도'와 그에 필요한 '데이터'를 반환합니다.
    /// 상태 전환 판단도 이 안에서 이루어집니다.
    /// </summary>
    /// <param name="outTarget">의도에 필요한 대상(EntityCore)</param>
    /// <param name="outDirection">의도에 필요한 방향(Vector2)</param>
    /// <returns>이번 턴의 AI 의도</returns>
    AIIntent OnTick(out EntityCore outTarget, out Vector2 outDirection);

    void OnExit();
}