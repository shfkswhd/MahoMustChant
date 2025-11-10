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

    [Header("핵심 스탯")]
    public float maxHealth = 100f;
    public float moveSpeed = 3f;

    [Header("상성 관계")]
    [Tooltip("이 엔티티가 천적으로 인식하는 팩션 목록")]
    public List<Faction> predatorFactions = new List<Faction>();

    [Tooltip("이 엔티티가 먹이로 인식하는 팩션 목록")]
    public List<Faction> preyFactions = new List<Faction>();
}