// 파일 이름: ChaseState.cs

using UnityEngine;

public class ChaseState : IState
{
    private EntityBrain brain;
    private EntityCore target;

    public void OnEnter(EntityBrain brain)
    {
        this.brain = brain;
        if (brain.Core.Data.preyFactions.Count > 0)
            this.target = brain.Sense.FindClosestTargetByFaction(brain.Core.Data.preyFactions[0]);
    }

    public AIIntent OnTick(out EntityCore outTarget, out Vector2 outDirection)
    {
        outDirection = Vector2.zero; // 이 상태는 방향이 필요 없음

        if (target == null || target.IsDead)
        {
            brain.ChangeState(new WanderState());
            outTarget = null;
            return AIIntent.Wander;
        }

        if (brain.Core.Data.hasMeleeAttack &&
            Vector2.Distance(brain.transform.position, target.transform.position) <= brain.Core.Data.meleeRange)
        {
            // 공격 범위에 들어왔다! 하지만 상태 전환은 AttackState에서 책임지게 할 수도 있다.
            // 여기서는 AttackState로 바로 전환한다.
            brain.ChangeState(new AttackState(target));
            outTarget = this.target;
            return AIIntent.Attack;
        }

        outTarget = this.target;
        return AIIntent.Pursue;
    }

    public void OnExit() { }
}