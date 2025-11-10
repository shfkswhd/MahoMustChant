using UnityEngine;

public class FleeState : IState
{
    private EntityBrain brain; // 전문 지휘관
    private Transform predator;

    private float safeDistance = 8f; // 이 거리만큼 멀어지면 안전하다고 판단

    // 생성자는 그대로 유지. 도망칠 대상을 외부에서 알려줘야 함
    public FleeState(Transform predator)
    {
        this.predator = predator;
    }

    public void OnEnter(EntityBrain brain)
    {
        this.brain = brain;
        Debug.Log($"{brain.Core.Data.entityName}이(가) {predator.name}로부터 도망을 결정!");
    }

    public void OnUpdate()
    {
        // 천적이 사라졌거나, 충분히 멀어져서 안전해졌는지 판단
        if (predator == null || !predator.gameObject.activeInHierarchy ||
            Vector2.Distance(brain.transform.position, predator.position) > safeDistance)
        {
            // 더 이상 도망칠 필요가 없으므로, 지휘관을 통해 배회 상태로 전환을 명령
            brain.ChangeState(new WanderState()); // WanderState를 사용
            return;
        }

        // 행동 전문가(Action)에게 "저 천적으로부터 도망쳐줘" 라고 요청
        brain.Action.Flee(predator);
    }

    public void OnExit()
    {
        Debug.Log($"{brain.Core.Data.entityName}이(가) 도망을 중단합니다.");
        // 도망 상태를 벗어날 때, 반드시 정지 명령을 내려주는 것이 안전합니다.
        brain.Action.Stop();
    }
}