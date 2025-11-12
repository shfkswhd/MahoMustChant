// EntityData.cs
using UnityEngine;
using System.Collections.Generic; // List를 사용하기 위함

// 이 메뉴를 통해 Unity 에디터에서 쉽게 에셋 파일을 생성할 수 있습니다.
[CreateAssetMenu(fileName = "New EntityData", menuName = "Entity/Entity Data")]
public class EntityData : ScriptableObject
{
    [Header("기본 정보")]
    public string entityName = "새 엔티티";
    public Faction factionType = Faction.Green;

    [Header("주 스탯")]
    public float maxHealth = 100f;
    public float moveSpeed = 3f;

    [Header("접촉 공격 스탯")]
    public bool hasContactDamage = false;   // 접촉 데미지가 있는가?
    public float contactDamage = 0f;        // 있다면, 데미지는 얼마인가?

    [Header("근접 공격 스탯")]
    public bool hasMeleeAttack = false;      // 근접 공격(AttackState)을 사용하는가?
    public float meleeDamage = 0f;         // 사용한다면, 데미지는 얼마인가?
    public float meleeRange = 1f;     // 공격 사거리
    public float meleeCooldown = 2.0f;  // 공격 쿨타임 (2초에 한 번)

    [Header("고급 공격 전략")]
    [Tooltip("여기에 공격 전략 에셋을 할당하면, 기본 근접/원거리 공격 대신 이 전략을 사용합니다.")]
    public IAttackStrategy attackStrategy;

    [Header("상성 관계")]
    [Tooltip("이 엔티티가 천적으로 인식하는 팩션 목록")]
    public List<Faction> predatorFactions = new List<Faction>();

    [Tooltip("이 엔티티가 먹이로 인식하는 팩션 목록")]
    public List<Faction> preyFactions = new List<Faction>();

}