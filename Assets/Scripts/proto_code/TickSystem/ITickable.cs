// 파일 이름: ITickable.cs

/// <summary>
/// TickManager에 의해 관리되는 모든 틱 기반 객체가 구현해야 할 인터페이스입니다.
/// 객체 스스로 자신의 실행 주기를 정의해야 합니다.
/// </summary>
public interface ITickable
{
    /// <summary>
    /// 이 객체의 OnTick() 메서드가 호출될 주기입니다. (단위: 틱)
    /// 1 = 매 틱마다 실행됩니다.
    /// 10 = 10틱마다 한 번씩 실행됩니다.
    /// </summary>
    uint TickInterval { get; }

    /// <summary>
    /// TickInterval 주기가 도래했을 때 TickManager에 의해 호출될 메서드입니다.
    /// </summary>
    void OnTick();
}