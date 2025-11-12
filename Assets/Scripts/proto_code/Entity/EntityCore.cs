using UnityEngine;
using System; 

public class EntityCore : MonoBehaviour
{
    /// <summary>
    /// 이 엔티티의 원본 데이터를 담고 있는 ScriptableObject 입니다. (인스펙터에서 할당)
    /// </summary>
    [Tooltip("이 엔티티의 원본 데이터를 담고 있는 ScriptableObject")]
    [SerializeField] private EntityData data;
    public EntityData Data => data; // 외부에서는 읽기만 가능하도록 설정

    /// <summary>
    /// 엔티티의 현재 체력입니다.
    /// </summary>
    public float CurrentHealth { get; private set; }

    /// <summary>
    /// 엔티티가 살아있는지 여부입니다.
    /// </summary>
    public bool IsDead { get; private set; }

    // --- Events ---

    /// <summary>
    /// 엔티티가 사망했을 때 호출되는 이벤트입니다.
    /// UI, 게임 매니저, 스포너 등이 이 이벤트를 구독하여 후속 처리를 할 수 있습니다.
    /// </summary>
    public event Action OnDie;

    // --- Unity Methods ---

    private void Awake()
    {
        // 데이터가 할당되지 않았을 경우를 대비한 안전장치
        if (data == null)
        {
            Debug.LogError($"{gameObject.name}의 EntityCore에 EntityData가 할당되지 않았습니다!", this);
            return;
        }

        // 초기화
        CurrentHealth = data.maxHealth;
        IsDead = false;
    }

    // --- Public Methods ---

    /// <summary>
    /// 엔티티에게 피해를 입힙니다.
    /// </summary>
    /// <param name="damageAmount">입힐 피해량</param>
    public void TakeDamage(float damageAmount)
    {
        // 이미 죽었다면 아무것도 하지 않음
        if (IsDead) return;

        CurrentHealth -= damageAmount;
        Debug.Log($"{data.entityName}이(가) {damageAmount}의 피해를 입었습니다. 현재 체력: {CurrentHealth}");

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    // --- Private Methods ---

    /// <summary>
    /// 엔티티의 사망 처리를 담당합니다.
    /// 이 메서드는 오직 TakeDamage를 통해서만 호출되어야 합니다.
    /// </summary>
    private void Die()
    {
        // 중복 실행 방지
        if (IsDead) return;

        IsDead = true;
        CurrentHealth = 0;

        Debug.Log($"{data.entityName}이(가) 사망했습니다.");

        // 나를 구독하고 있는 모든 시스템에게 "나 죽었어!" 라고 방송
        OnDie?.Invoke();

        // 오브젝트 풀링을 위해 Destroy 대신 비활성화를 권장
        gameObject.SetActive(false);
    }

        // EntityCore.cs의 OnCollisionEnter2D 수정
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 이 엔티티가 접촉 데미지를 가지고 있지 않다면, 아무 일도 하지 않고 즉시 종료.
        if (!Data.hasContactDamage) return;
    
        if (collision.gameObject.TryGetComponent<EntityCore>(out var otherCore))
        {
            // ... (자기 자신, 같은 편 체크) ...
    
            // 상대방에게 나의 'contactDamage'를 입힙니다.
            otherCore.TakeDamage(this.Data.contactDamage);
        }
    }
}