// EntityBrain.cs
using UnityEngine;

[RequireComponent(typeof(EntitySense), typeof(EntityAction))]
public class EntityBrain : MonoBehaviour
{
    // 상태(State)들이 이 Brain을 통해 다른 컴포넌트에 쉽게 접근할 수 있도록 공개
    public EntitySense Sense { get; private set; }
    public EntityAction Action { get; private set; }
    public EntityCore Core { get; private set; }

    // 상태 관리용 변수
    private IState currentState;

    private void Awake()
    {
        // 내게 붙어있는 전문가 컴포넌트들을 찾아서 저장
        Sense = GetComponent<EntitySense>();
        Action = GetComponent<EntityAction>();
        Core = GetComponent<EntityCore>();
    }

    private void Start()
    {
        // 게임이 시작되면 기본 상태(예: 배회 상태)로 전환
        // 실제 게임에서는 WanderState를 만들어 new WanderState()로 교체해야 합니다.
        // 지금은 임시로 null 상태로 둡니다.
        ChangeState(null); // TODO: new WanderState()로 교체
    }

    private void Update()
    {
        // 현재 상태가 있다면, 매 프레임 상태의 로직을 실행
        currentState?.OnUpdate();
    }

    /// <summary>
    /// AI의 상태를 새로운 상태로 안전하게 전환합니다.
    /// </summary>
    public void ChangeState(IState newState)
    {
        // 기존 상태가 있었다면, 마무리(OnExit) 처리
        currentState?.OnExit();

        // 새로운 상태로 교체하고, 초기화(OnEnter) 처리
        currentState = newState;
        currentState?.OnEnter(this); // 'this' (EntityBrain 자신)를 넘겨줌
    }
}