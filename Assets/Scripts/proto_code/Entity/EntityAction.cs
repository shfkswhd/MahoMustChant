// 파일 이름: EntityAction.cs

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

    public EntityCore GetCore() => entityCore;
}