using UnityEngine;

public class WanderState : IState
{
    private EntityBrain brain; // 전문 지휘관

    private float wanderDuration;
    private float wanderTimer;
    private Vector2 moveDirection;

    public void OnEnter(EntityBrain brain)
    {
        this.brain = brain;
        SetNewWanderPattern();
    }

    public void OnUpdate()
    {
        // ... (천적/먹이 탐지 로직은 동일) ...

        // 배회 로직 부분
        wanderTimer -= Time.deltaTime;

        if (wanderTimer <= 0)
        {
            SetNewWanderPattern();
        }

        // EntityAction의 새로운 능력을 사용! 훨씬 깔끔하고 직관적입니다.
        brain.Action.MoveInDirection(moveDirection);
    }

    public void OnExit()
    {
        brain.Action.Stop();
        // GameObject.Destroy(virtualTarget); // 더 이상 필요 없으므로 삭제!
    }

    private void SetNewWanderPattern()
    {
        wanderDuration = Random.Range(3f, 8f);
        wanderTimer = wanderDuration;
        moveDirection = Random.insideUnitCircle.normalized;

        Debug.Log($"{brain.Core.Data.entityName}이(가) {moveDirection} 방향으로 {wanderDuration:F1}초 동안 배회 시작!");
    }

}