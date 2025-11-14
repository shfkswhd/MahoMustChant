// 파일 이름: EntityAction.cs

using UnityEngine;

[RequireComponent(typeof(Locomotion), typeof(EntityCore))]
public class EntityAction : MonoBehaviour
{
    private Locomotion locomotion;
    private EntityCore entityCore;
    public EntityCore GetCore() => entityCore;
    private void Awake()
    {
        locomotion = GetComponent<Locomotion>();
        entityCore = GetComponent<EntityCore>();
    }

    public void Pursue(Transform target)
    {
        if (target == null) return;
        Vector2 direction = (target.position - transform.position).normalized;
        locomotion.SetMoveDirection(direction);
    }

    public void Flee(Transform predator)
    {
        if (predator == null) return;
        Vector2 direction = (transform.position - predator.position).normalized;
        locomotion.SetMoveDirection(direction);
    }

    public void Stop()
    {
        locomotion.SetMoveDirection(Vector2.zero);
    }

    public void MoveInDirection(Vector2 direction)
    {
        locomotion.SetMoveDirection(direction);
    }

    public void Attack(EntityCore target)
    {
        if (entityCore.Data.attackStrategy != null)
        {
            entityCore.Data.attackStrategy.Execute(this, target);
            return;
        }

        if (entityCore.Data.hasMeleeAttack)
        {
            if (target != null)
            {
                Debug.Log($"{entityCore.Data.entityName}이(가) {target.Data.entityName}에게 기본 근접 공격!");
                target.TakeDamage(entityCore.Data.meleeDamage);
            }
            else
            {
                Debug.Log($"{entityCore.Data.entityName}이(가) 전방을 향해 기본 근접 공격!");
                // TODO: 플레이어의 전방 범위 공격 로직
            }
        }
    }


#if UNITY_EDITOR // 유니티 에디터에서만 포함

    protected virtual void OnDrawGizmos()
    {
        if (GetComponent<PlayerInput>() == null) return;
        if (entityCore == null) entityCore = GetComponent<EntityCore>();
        if (entityCore == null) return;

        if (entityCore.Data.hasMeleeAttack)
        {
            // --- 1. 공격 범위 데이터 가져오기 ---
            float range = entityCore.Data.meleeRange;
            // 직사각형의 '폭'을 위한 값을 추가할 수 있다. 없으면 range의 절반을 사용.
            float attackWidth = range * 0.5f;
            Vector2 attackSize = new Vector2(range, attackWidth * 2);

            // --- 2. 바라보는 방향 및 공격 중심점 계산 ---
            // 캐릭터의 현재 스케일(좌우 반전)을 확인하여 방향을 결정한다.
            float facingDirection = Mathf.Sign(transform.localScale.x);
            Vector2 center = (Vector2)transform.position + new Vector2(facingDirection * (range / 2f), 0f);

            // --- 3. 직사각형 범위 안에 있는 모든 적을 실제로 찾아본다. ---
            Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(center, attackSize, 0f);

            bool canHitEnemy = false;
            foreach (var enemy in hitEnemies)
            {
                if (enemy.gameObject == this.gameObject) continue;
                if (enemy.TryGetComponent<EntityCore>(out var enemyCore))
                {
                    canHitEnemy = true;
                    break;
                }
            }

            // --- 4. 조건에 따라 기즈모 색상 결정 및 그리기 ---
            Gizmos.color = canHitEnemy ? Color.cyan : Color.magenta;

            // Gizmos.matrix를 사용하면 로컬 좌표계처럼 회전/이동된 기즈모를 그릴 수 있다.
            // 여기서는 월드 좌표계에서 직접 계산했으므로 필요 없다.
            DrawWireBox(center, attackSize, 0f);
        }
    }

    // Gizmos.DrawWireBox는 회전을 지원하지 않으므로, 직접 그려주는 헬퍼 메서드
    private void DrawWireBox(Vector2 center, Vector2 size, float angle)
    {
        var halfSize = size / 2;
        Vector2 p1 = new Vector2(-halfSize.x, -halfSize.y);
        Vector2 p2 = new Vector2(halfSize.x, -halfSize.y);
        Vector2 p3 = new Vector2(halfSize.x, halfSize.y);
        Vector2 p4 = new Vector2(-halfSize.x, halfSize.y);

        if (angle != 0)
        {
            var rotation = Quaternion.Euler(0, 0, angle);
            p1 = rotation * p1;
            p2 = rotation * p2;
            p3 = rotation * p3;
            p4 = rotation * p4;
        }

        Gizmos.DrawLine(center + p1, center + p2);
        Gizmos.DrawLine(center + p2, center + p3);
        Gizmos.DrawLine(center + p3, center + p4);
        Gizmos.DrawLine(center + p4, center + p1);
    }

#endif
}