// 파일 이름: AttackState.cs

using UnityEngine;

public class AttackState : IState
{
    private EntityBrain brain;
    private EntityCore target;

    public AttackState(EntityCore target)
    {
        this.target = target;
    }

    public void OnEnter(EntityBrain brain)
    {
        this.brain = brain;
    }

    public AIIntent OnTick(out EntityCore outTarget, out Vector2 outDirection)
    {
        outDirection = Vector2.zero;

        // 최우선: 상태 전환 판단
        if (target == null || target.IsDead ||
            Vector2.Distance(brain.transform.position, target.transform.position) > brain.Core.Data.meleeRange)
        {
            // 공격을 중단하고 다시 추격해야 한다.
            brain.ChangeState(new ChaseState());
            outTarget = this.target; // 놓쳤더라도 마지막 타겟 정보를 넘겨줌
            return AIIntent.Pursue;
        }

        // 상태 유효. 나의 의도는 '공격'이다.
        outTarget = this.target;
        return AIIntent.Attack;
    }

    public void OnExit() { }
}