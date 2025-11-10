// EntityAction.cs
using UnityEngine;

[RequireComponent(typeof(Locomotion), typeof(EntityCore))]
public class EntityAction : MonoBehaviour
{
    private Locomotion locomotion;
    private EntityCore myCore;

    private void Awake()
    {
        locomotion = GetComponent<Locomotion>();
        myCore = GetComponent<EntityCore>();
    }

    /// <summary>
    /// 대상을 향해 이동하도록 Locomotion 컴포넌트에 명령합니다.
    /// </summary>
    public void Pursue(Transform target)
    {
        if (target == null) return;
        Vector2 direction = (target.position - transform.position).normalized;
        locomotion.Move(direction);
    }

    /// <summary>
    /// 대상으로부터 멀어지도록 Locomotion 컴포넌트에 명령합니다.
    /// </summary>
    public void Flee(Transform predator)
    {
        if (predator == null) return;
        Vector2 direction = (transform.position - predator.position).normalized;
        locomotion.Move(direction);
    }

    /// <summary>
    /// 제자리에 멈추도록 Locomotion 컴포넌트에 명령합니다.
    /// </summary>
    public void Stop()
    {
        locomotion.Move(Vector2.zero);
    }

    /// <summary>
    /// 원하는 방향으로 이동하게 Locomotion 컴포넌트에 명령합니다.
    /// </summary>
    public void MoveInDirection(Vector2 direction)
    {
        locomotion.Move(direction);
    }

    /// <summary>
    /// 대상을 공격합니다. (지금은 피해를 주는 간단한 로직)
    /// </summary>
    public void Attack(EntityCore target)
    {
        if (target == null || target.IsDead) return;
        Debug.Log($"{myCore.Data.entityName}이(가) {target.Data.entityName}을(를) 공격!");
        // 여기에 실제 공격 로직(데미지, 애니메이션 등)을 추가할 수 있습니다.
        // 예: target.TakeDamage(myCore.Data.attackPower);
    }

}