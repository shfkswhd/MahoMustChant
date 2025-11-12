// 파일 이름: AttackState.cs

using UnityEngine;

public class AttackState : IState
{
    private EntityBrain brain;
    private EntityCore target; // 공격 대상은 EntityCore로 받는 것이 더 명확합니다.

    // --- 공격 관련 설정 ---
    private float meleeRange;      // 이 엔티티의 공격 사거리
    private float meleeCooldown;   // 공격 사이의 대기 시간 (초)
    private float meleeTimer;      // 다음 공격까지 남은 시간을 재는 타이머

    /// <summary>
    /// 공격 상태에 진입할 때, 공격할 대상을 반드시 알려줘야 합니다.
    /// </summary>
    public AttackState(EntityCore target)
    {
        this.target = target;
    }

    public void OnEnter(EntityBrain brain)
    {
        this.brain = brain;
        Debug.Log($"{brain.Core.Data.entityName}이(가) {target.Data.entityName}에 대한 공격 상태에 진입!");

        // EntityData로부터 이 엔티티의 공격 능력치를 가져옵니다.
        this.meleeRange = brain.Core.Data.meleeRange;
        this.meleeCooldown = brain.Core.Data.meleeCooldown;

        // 상태에 진입하자마자 바로 공격할 수 있도록 타이머를 0으로 설정합니다.
        this.meleeTimer = 0f;

        // 공격 위치에 도착했으므로, 일단 정지합니다.
        brain.Action.Stop();
    }

    public void OnTick()
    {
        // --- 최우선: 목표 유효성 검사 ---
        // 목표가 죽었거나, 너무 멀어지면 상태를 변경해야 합니다.
        if (target == null || target.IsDead ||
            Vector2.Distance(brain.transform.position, target.transform.position) > meleeRange)
        {
            // 목표를 잃었으므로, 다시 추격 상태로 돌아갑니다.
            brain.ChangeState(new ChaseState());
            return;
        }

        // --- 공격 쿨타임 계산 ---
        // 틱마다 쿨타임 타이머를 감소시킵니다.
        meleeTimer -= Time.fixedDeltaTime;

        // --- 공격 실행 ---
        // 타이머가 0 이하로 떨어지면 공격할 시간입니다.
        if (meleeTimer <= 0f)
        {
            // Action 전문가에게 "이 타겟을 공격해!" 라고 명령합니다.
            brain.Action.Attack(target);

            // 타이머를 다시 쿨타임으로 초기화합니다.
            meleeTimer = meleeCooldown;
        }
    }

    public void OnExit()
    {
        Debug.Log($"{brain.Core.Data.entityName}이(가) 공격 상태를 종료합니다.");
        // 상태를 나갈 때 특별히 할 일은 없지만, 로그를 남겨두면 디버깅에 좋습니다.
    }
}