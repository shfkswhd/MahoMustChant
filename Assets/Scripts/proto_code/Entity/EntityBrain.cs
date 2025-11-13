// 파일 이름: EntityBrain.cs

using UnityEngine;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(EntitySense), typeof(EntityAction), typeof(EntityCore))]
public class EntityBrain : MonoBehaviour, ITickable
{
    [Header("AI Logic Tick Interval")]
    [Tooltip("AI의 의사결정 주기 (단위: 틱)")]
    [Range(1, 100)]
    [SerializeField] private uint tickInterval = 5;

    public uint TickInterval => tickInterval;

    public EntitySense Sense { get; private set; }
    public EntityAction Action { get; private set; }
    public EntityCore Core { get; private set; }

    private IState currentState;
    private Dictionary<AIIntent, Action<EntityCore, Vector2>> intentHandlers;

    private void Awake()
    {
        Sense = GetComponent<EntitySense>();
        Action = GetComponent<EntityAction>();
        Core = GetComponent<EntityCore>();
        InitializeIntentHandlers();
    }

    private void OnEnable()
    {
        TickManager.Instance.Register(this);
    }

    private void Start()
    {
        ChangeState(new WanderState());
    }

    /// <summary>
    /// 어떤 의도(AIIntent)가 들어오면, 어떤 행동(메서드)을 실행할지 정의하는 대응표입니다.
    /// </summary>
    private void InitializeIntentHandlers()
    {
        intentHandlers = new Dictionary<AIIntent, Action<EntityCore, Vector2>>
        {
            { AIIntent.None,    (target, direction) => Action.Stop() },
            { AIIntent.Wander,  (target, direction) => Action.MoveInDirection(direction) },
            { AIIntent.Pursue,  (target, direction) => Action.Pursue(target.transform) },
            { AIIntent.Flee,    (target, direction) => Action.Flee(target.transform) },
            { AIIntent.Attack,  (target, direction) => {
                                      Action.Pursue(target.transform); // 공격 시에도 계속 거리를 유지하도록 추격
                                      Action.Attack(target);
                                  }
            }
        };
    }

    public void OnTick()
    {
        if (currentState == null) return;

        // 1. 현재 상태로부터 '의도'와 '데이터'를 보고받는다.
        AIIntent intent = currentState.OnTick(out EntityCore target, out Vector2 direction);

        // 2. 사전을 조회하여 해당 의도를 처리할 행동을 찾는다.
        if (intentHandlers.TryGetValue(intent, out var handler))
        {
            // 3. 찾은 행동을 실행한다.
            handler(target, direction);
        }
    }

    public void ChangeState(IState newState)
    {
        currentState?.OnExit();
        currentState = newState;
        currentState?.OnEnter(this);
    }
}