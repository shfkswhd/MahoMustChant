using UnityEngine;
public class ChaseState : IState
{
    private EntityBrain brain; // 전문 지휘관
    private Transform target;

    public void OnEnter(EntityBrain brain)
    {
        this.brain = brain;

        // 감각 전문가(Sense)에게 "가장 가까운 먹이를 찾아줘" 라고 요청
        EntityCore prey = brain.Sense.FindClosestTargetByFaction(brain.Core.Data.preyFactions[0]);
        if (prey != null)
        {
            this.target = prey.transform;
        }
    }

    // ChaseState.cs의 OnTick() 메서드
    
    public void OnTick()
    {
        if (target == null || !target.gameObject.activeInHierarchy)
        {
            brain.ChangeState(new WanderState());
            return;
        }
    
        // 목표물과의 거리를 틱마다 계산합니다.
        float distanceToTarget = Vector2.Distance(brain.transform.position, target.position);
        float attackRange = brain.Core.Data.meleeRange;
    
        // --- 상태 전환 판단 ---
        // 거리가 공격 사거리 안으로 들어왔다면,
        if (distanceToTarget <= attackRange)
        {
            // Brain에게 "AttackState"로 변경해달라고 요청합니다.
            // 이때, 누구를 공격할지(target) 정보를 넘겨줍니다.
            brain.ChangeState(new AttackState(target.GetComponent<EntityCore>()));
            return; // 상태가 변경되었으므로 추격 행동은 하지 않습니다.
        }
    
        // 아직 공격 범위 밖이라면, 계속 추격합니다.
        brain.Action.Pursue(target);
    }

    public void OnExit()
    {
        // 상태를 나갈 때, 행동 전문가에게 "이제 멈춰" 라고 요청
        brain.Action.Stop();
    }
}