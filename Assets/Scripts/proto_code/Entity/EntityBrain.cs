// 파일 이름: EntityBrain.cs (수정)

using UnityEngine;

[RequireComponent(typeof(EntitySense), typeof(EntityAction), typeof(EntityCore))]
public class EntityBrain : MonoBehaviour, ITickable // ITickable 구현
{
    public EntitySense Sense { get; private set; }
    public EntityAction Action { get; private set; }
    public EntityCore Core { get; private set; }

    private IState currentState;

    private void Awake()
    {
        Sense = GetComponent<EntitySense>();
        Action = GetComponent<EntityAction>();
        Core = GetComponent<EntityCore>();
    }

    private void OnEnable()
    {
        TickManager.Instance.Register(this); // 틱 시스템에 등록
    }

    private void Start()
    {
        // WanderState를 기본 상태로 설정하는 예시
        ChangeState(new WanderState());
    }

    /// <summary>
    /// Update() 대신 TickManager에 의해 주기적으로 호출됩니다.
    /// </summary>
    public void OnTick()
    {
        // 현재 상태의 틱 기반 업데이트 메서드를 호출합니다.
        currentState?.OnTick();
    }

    public void ChangeState(IState newState)
    {
        currentState?.OnExit();

        currentState = newState;

        // 새로운 상태에 진입할 때, 자신(Brain)의 참조를 넘겨줍니다.
        currentState?.OnEnter(this);
    }
}