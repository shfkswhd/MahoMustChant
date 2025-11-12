// 파일 이름: WanderState.cs (최종)

using UnityEngine;

public class WanderState : IState
{
    private EntityBrain brain;

    private float wanderTimer;
    private Vector2 moveDirection;

    public void OnEnter(EntityBrain brain)
    {
        this.brain = brain;
        SetNewWanderPattern();
        Debug.Log($"{brain.Core.Data.entityName}이(가) 배회 상태에 진입합니다.");
    }

    /// <summary>
    /// 배회 상태일 때 틱마다 수행할 로직입니다.
    /// 상태 전환 판단을 최우선으로, 그 외의 경우에만 배회 행동을 수행합니다.
    /// </summary>
    public void OnTick()
    {
        // --- 최우선 순위: 생존 (도망) ---
        // 이 엔티티가 도망갈 포식자 팩션이 정의되어 있는지 먼저 확인합니다.
        if (brain.Core.Data.predatorFactions.Count > 0)
        {
            // Sense를 통해 가장 가까운 포식자를 찾습니다.
            EntityCore predator = brain.Sense.FindClosestTargetByFaction(brain.Core.Data.predatorFactions[0]);
            if (predator != null)
            {
                // 포식자가 감지되면, Brain에게 'FleeState'로 변경해달라고 요청합니다.
                brain.ChangeState(new FleeState());
                return; // 상태가 변경되었으므로, 이번 틱의 나머지 로직(배회)은 실행하지 않습니다.
            }
        }

        // --- 차선 순위: 사냥 (추격) ---
        // 이 엔티티가 사냥할 먹이 팩션이 정의되어 있는지 확인합니다.
        if (brain.Core.Data.preyFactions.Count > 0)
        {
            // Sense를 통해 가장 가까운 먹잇감을 찾습니다.
            EntityCore prey = brain.Sense.FindClosestTargetByFaction(brain.Core.Data.preyFactions[0]);
            if (prey != null)
            {
                // 먹잇감이 감지되면, Brain에게 'ChaseState'로 변경해달라고 요청합니다.
                brain.ChangeState(new ChaseState());
                return; // 상태가 변경되었으므로, 배회 로직은 실행하지 않습니다.
            }
        }

        // --- 아무런 위협이나 기회가 없을 때: 배회 행동 ---
        // 위의 if 문들이 모두 거짓일 때만 이 코드가 실행됩니다.
        wanderTimer -= Time.fixedDeltaTime;
        if (wanderTimer <= 0f)
        {
            SetNewWanderPattern();
        }
        brain.Action.MoveInDirection(moveDirection);
    }

    public void OnExit()
    {
        // 배회 상태를 떠날 때는 이동을 멈추는 것이 안전합니다.
        brain.Action.Stop();
        Debug.Log($"{brain.Core.Data.entityName}이(가) 배회 상태를 종료합니다.");
    }

    /// <summary>
    /// 새로운 무작위 배회 패턴(방향, 시간)을 설정합니다.
    /// </summary>
    private void SetNewWanderPattern()
    {
        float wanderDuration = Random.Range(3f, 8f);
        wanderTimer = wanderDuration;
        moveDirection = Random.insideUnitCircle.normalized;
    }
}