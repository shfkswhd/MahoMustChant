// EntitySense.cs
using UnityEngine;
using System.Linq; // 가장 가까운 적을 찾기 위해 Linq를 사용합니다.

[RequireComponent(typeof(EntityCore))]
public class EntitySense : MonoBehaviour
{
    [Header("감지 설정")]
    [SerializeField] private float detectionRadius = 3f; // 감지 반경
    [SerializeField] private LayerMask targetLayer;      // 감지할 대상의 레이어

    private EntityCore myCore;
    public float DetectionRadius => detectionRadius; // 기즈모용 읽기 전용 프로퍼티

    private void Awake()
    {
        myCore = GetComponent<EntityCore>();
    }

    /// <summary>
    /// 지정된 팩션을 가진 가장 가까운 대상을 찾습니다.
    /// </summary>
    /// <param name="targetFaction">찾고자 하는 대상의 팩션</param>
    /// <returns>찾은 대상의 EntityCore. 없으면 null을 반환합니다.</returns>
    public EntityCore FindClosestTargetByFaction(Faction targetFaction)
    {
        // 내 감지 반경 내의 모든 콜라이더를 가져옴
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, targetLayer);

        EntityCore closestTarget = null;
        float minDistance = float.MaxValue;

        foreach (var col in colliders)
        {
            // 감지된 대상이 EntityCore를 가지고 있는지, 그리고 찾는 팩션이 맞는지 확인
            if (col.TryGetComponent<EntityCore>(out var targetCore) && targetCore.Data.factionType == targetFaction)
            {
                float distance = Vector2.Distance(transform.position, col.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestTarget = targetCore;
                }
            }
        }
        return closestTarget;
    }

    /// <summary>
    /// 내 데이터(EntityData)를 기반으로 대상이 천적인지 판단합니다.
    /// </summary>
    public bool IsPredator(EntityCore target)
    {
        if (target == null) return false;
        return myCore.Data.predatorFactions.Contains(target.Data.factionType);
    }

    /// <summary>
    /// 내 데이터(EntityData)를 기반으로 대상이 먹이인지 판단합니다.
    /// </summary>
    public bool IsPrey(EntityCore target)
    {
        if (target == null) return false;
        return myCore.Data.preyFactions.Contains(target.Data.factionType);
    }

#if UNITY_EDITOR
    // 에디터에서 감지 범위를 시각적으로 보여주기 위한 기즈모
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
#endif
}