// 파일 이름: ITickable.cs

/// <summary>
/// 틱 기반 시스템에 등록되어 주기적인 로직 업데이트를 수신할 수 있는 모든 객체를 위한 인터페이스입니다.
/// 이 인터페이스를 구현하는 클래스는 반드시 OnTick() 메서드를 가져야 합니다.
/// </summary>
public interface ITickable
{
    /// <summary>
    /// TickManager에 의해 고정된 시간 간격(FixedUpdate 주기)마다 호출되는 메서드입니다.
    /// 모든 시뮬레이션 관련 핵심 로직은 이 메서드 안에 구현되어야 합니다.
    /// </summary>
    void OnTick();
}