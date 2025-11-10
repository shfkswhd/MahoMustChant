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

    public void OnUpdate()
    {
        if (target == null)
        {
            // 먹이를 놓쳤으면, 지휘관을 통해 상태 전환 요청
            // brain.ChangeState(new WanderState()); // WanderState를 새로 만들고 나면 주석 해제
            return;
        }

        // 행동 전문가(Action)에게 "저 타겟을 추격해줘" 라고 요청
        brain.Action.Pursue(target);
    }

    public void OnExit()
    {
        // 상태를 나갈 때, 행동 전문가에게 "이제 멈춰" 라고 요청
        brain.Action.Stop();
    }
}