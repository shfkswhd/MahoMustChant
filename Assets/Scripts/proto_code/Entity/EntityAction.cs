using UnityEngine;

[RequireComponent(typeof(Locomotion), typeof(EntityCore))]
public class EntityAction : MonoBehaviour
{
    private Locomotion locomotion;
    private EntityCore entityCore;

    private void Awake()
    {
        locomotion = GetComponent<Locomotion>();
        entityCore = GetComponent<EntityCore>();
    }

    // --- Pursue, Flee, Stop, MoveInDirection은 기존과 동일 ---
    public void Pursue(Transform target) { /* ... */ }
    public void Flee(Transform predator) { /* ... */ }
    public void Stop() { /* ... */ }
    public void MoveInDirection(Vector2 direction) { /* ... */ }

    /// <summary>
    /// 이 엔티티의 공격 행동을 실행합니다.
    /// 데이터에 '고급 공격 전략'이 할당되어 있으면 그것을 우선적으로 사용하고,
    /// 없으면 기본적인 근접 공격을 수행합니다.
    /// </summary>
    public void Attack(EntityCore target)
    {
        // --- 1. 미래를 위한 확장 포인트 ---
        // 만약 EntityData에 'attackStrategy'가 할당되어 있다면,
        if (entityCore.Data.attackStrategy != null)
        {
            // 모든 것을 그 전략에게 위임하고, 여기서 끝낸다.
            entityCore.Data.attackStrategy.Execute(this, target);
            return;
        }

        // --- 2. 현재를 위한 기본 공격 로직 ---
        if (entityCore.Data.hasMeleeAttack)
        {
            if (target != null) // AI처럼 타겟이 지정된 경우
            {
                Debug.Log($"{entityCore.Data.entityName}이(가) {target.Data.entityName}에게 기본 근접 공격!");
                target.TakeDamage(entityCore.Data.meleeDamage);
            }
            else // 플레이어처럼 타겟이 없는 경우 (target == null)
            {
                Debug.Log($"{entityCore.Data.entityName}이(가) 전방을 향해 기본 근접 공격!");
                // TODO: 여기서 전방 범위 안의 적을 찾는 Physics2D.OverlapCircle... 로직을 넣으면 된다.
                // 지금은 그냥 로그만 찍어서 기능이 호출되는지만 확인한다.
            }
        }
    }

    // 다른 스크립트가 이 컴포넌트의 정보에 접근할 수 있도록 도와주는 public 메서드
    public EntityCore GetCore() => entityCore;
}