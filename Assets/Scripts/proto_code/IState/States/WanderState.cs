// 파일 이름: WanderState.cs

using UnityEngine;

public class WanderState : IState
{
    private EntityBrain brain;
    private CooldownTimer wanderTimer;
    private Vector2 moveDirection;

    public void OnEnter(EntityBrain brain)
    {
        this.brain = brain;
        SetNewWanderPattern();
    }

    public AIIntent OnTick(out EntityCore outTarget, out Vector2 outDirection)
    {
        // 최우선: 상태 전환 판단
        if (brain.Core.Data.predatorFactions.Count > 0)
        {
            EntityCore predator = brain.Sense.FindClosestTargetByFaction(brain.Core.Data.predatorFactions[0]);
            if (predator != null)
            {
                brain.ChangeState(new FleeState());
                outTarget = predator;
                outDirection = Vector2.zero;
                return AIIntent.Flee; // 즉시 도망 의도로 전환하여 보고
            }
        }
        if (brain.Core.Data.preyFactions.Count > 0)
        {
            EntityCore prey = brain.Sense.FindClosestTargetByFaction(brain.Core.Data.preyFactions[0]);
            if (prey != null)
            {
                brain.ChangeState(new ChaseState());
                outTarget = prey;
                outDirection = Vector2.zero;
                return AIIntent.Pursue; // 즉시 추격 의도로 전환하여 보고
            }
        }

        // 배회 행동 타이머
        float elapsedSeconds = brain.TickInterval * TickManager.Instance.TickIntervalSeconds;
        wanderTimer.Tick(elapsedSeconds);

        if (wanderTimer.IsReady)
        {
            SetNewWanderPattern();
        }

        // 최종 보고
        outTarget = null;
        outDirection = this.moveDirection; // 계산된 방향을 out 파라미터로 넘겨준다.
        return AIIntent.Wander;
    }

    public void OnExit() { }

    private void SetNewWanderPattern()
    {
        float wanderDurationSeconds = Random.Range(3f, 8f);
        wanderTimer = new CooldownTimer(wanderDurationSeconds);
        moveDirection = Random.insideUnitCircle.normalized;
    }
}