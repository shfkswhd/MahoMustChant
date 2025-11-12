// 파일 이름: FleeState.cs

using UnityEngine;

public class FleeState : IState
{
    private EntityBrain brain;
    private Transform predator;

    private float safeDistance = 8f;

    // 생성자를 제거하고, OnEnter에서 도망칠 대상을 직접 찾습니다.
    public void OnEnter(EntityBrain brain)
    {
        this.brain = brain;

        // 진입 시점에 도망가야 할 가장 가까운 포식자를 찾습니다.
        if (brain.Core.Data.predatorFactions.Count > 0)
        {
            EntityCore predatorEntity = brain.Sense.FindClosestTargetByFaction(brain.Core.Data.predatorFactions[0]);
            if (predatorEntity != null)
            {
                this.predator = predatorEntity.transform;
                Debug.Log($"{brain.Core.Data.entityName}이(가) {predator.name}로부터 도망을 결정!");
            }
        }
    }

    public void OnTick()
    {
        // 틱마다 포식자가 여전히 위협적인지 판단합니다.
        if (predator == null || !predator.gameObject.activeInHierarchy ||
            Vector2.Distance(brain.transform.position, predator.position) > safeDistance)
        {
            // 위협이 사라졌으므로 배회 상태로 전환합니다.
            brain.ChangeState(new WanderState());
            return;
        }

        // 행동 전문가(Action)에게 "저 천적으로부터 도망쳐줘" 라고 틱마다 명령합니다.
        brain.Action.Flee(predator);
    }

    public void OnExit()
    {
        Debug.Log($"{brain.Core.Data.entityName}이(가) 도망을 중단합니다.");
        // 상태를 나갈 때, 반드시 정지 명령을 내립니다.
        brain.Action.Stop();
    }
}