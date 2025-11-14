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

#if UNITY_EDITOR // 이 코드는 유니티 에디터에서만 포함되도록 한다.

    /// <summary>
    /// 씬(Scene) 뷰에서 AI의 현재 상태와 행동 반경을 시각적으로 표시합니다.
    /// </summary>
    protected virtual void OnDrawGizmos()
    {
        // Awake()가 호출되기 전일 수 있으므로, Core와 Sense가 null인지 확인
        if (Core == null) Core = GetComponent<EntityCore>();
        if (Sense == null) Sense = GetComponent<EntitySense>();
        if (Core == null || Sense == null) return;

        // --- 1. 감지 범위(Sense Radius) 그리기 ---
        UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, Sense.DetectionRadius);

        // --- 2. 현재 상태에 따른 기즈모 그리기 ---
        if (currentState is ChaseState chaseState)
        {
            // [ChaseState일 때] 공격 범위를 '예고'하고 타겟에게 선을 긋는다.
            float attackRange = Core.Data.meleeRange;

            // Gizmos 대신 Handles를 사용하면 더 깔끔하게 그릴 수 있다.
            UnityEditor.Handles.color = new Color(1f, 0f, 1f, 0.2f); // 반투명 마젠타
            UnityEditor.Handles.DrawSolidArc(transform.position, Vector3.forward, transform.right, 360f, attackRange);

            if (chaseState.Target != null) // GetTarget() 메서드가 있다고 가정
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, chaseState.Target.transform.position);
            }
        }
        else if (currentState is AttackState attackState)
        {
            // [AttackState일 때] 실제 공격 범위를 그리고, 타겟과의 상태를 시각화한다.
            float attackRange = Core.Data.meleeRange; // Brain이 직접 Core의 데이터에 접근!
            EntityCore target = attackState.Target; // 공격 대상은 AttackState만 알고 있다.

            // 공격 범위 안에 유효한 타겟이 있는지 확인
            bool isTargetInRange = (target != null && !target.IsDead &&
                                   Vector2.Distance(transform.position, target.transform.position) <= attackRange);

            // 조건에 따라 색상 결정
            UnityEditor.Handles.color = isTargetInRange ? new Color(0f, 1f, 1f, 0.2f) : new Color(1f, 0f, 1f, 0.2f); // 시안 또는 마젠타

            // 공격 범위를 채워진 원호(Arc)로 그리기
            UnityEditor.Handles.DrawSolidArc(transform.position, Vector3.forward, transform.right, 360f, attackRange);

            // 타겟에게 선 긋기
            if (target != null)
            {
                Gizmos.color = isTargetInRange ? Color.cyan : Color.magenta;
                Gizmos.DrawLine(transform.position, target.transform.position);
            }
        }

        // --- 3. 현재 상태 이름 표시 ---
        if (currentState != null)
        {
            UnityEditor.Handles.color = Color.white;
            UnityEditor.Handles.Label(transform.position + Vector3.up * 1.5f, $"<size=14><b>{currentState.GetType().Name}</b></size>",
                new GUIStyle { alignment = TextAnchor.MiddleCenter, normal = { textColor = Color.white } });
        }
    }

#endif
}