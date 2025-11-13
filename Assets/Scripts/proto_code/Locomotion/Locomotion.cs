// 파일 이름: Locomotion.cs

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(EntityCore))]
public abstract class Locomotion : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected EntityCore entityCore;
    protected Vector2 moveDirection; // 모든 자식들이 공유해서 쓸 이동 방향

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        entityCore = GetComponent<EntityCore>();
        if (entityCore == null)
            Debug.LogError($"{gameObject.name}의 Locomotion 컴포넌트가 EntityCore를 찾을 수 없습니다.", this);
    }

    private void FixedUpdate()
    {
        Move();
    }

    protected abstract void Move();

    /// <summary>
    /// 외부에서 이동 방향을 설정합니다.
    /// virtual로 선언하여, 대부분의 자식 클래스는 이 메서드를 수정할 필요 없이 그대로 사용하고,
    /// 특별한 처리가 필요한 클래스만 override하여 재정의할 수 있습니다.
    /// </summary>
    public virtual void SetMoveDirection(Vector2 direction)
    {
        this.moveDirection = direction.normalized; // 방향 벡터는 정규화해서 저장하는 것이 안전하다.
    }
}