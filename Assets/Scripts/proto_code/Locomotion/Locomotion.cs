// 파일 이름: Locomotion.cs (올바른 수정안)

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Locomotion : MonoBehaviour, ITickable
{
    protected Rigidbody2D rb;
    protected EntityCore entityCore;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        entityCore = GetComponent<EntityCore>();
        if (entityCore == null) Debug.LogError("...", this);
    }

    protected virtual void OnEnable()
    {
        TickManager.Instance.Register(this);
    }

    /// <summary>
    /// 모든 자식 이동 클래스가 구현해야 할 틱 기반 업데이트 로직입니다.
    /// </summary>
    public abstract void OnTick();

    /// <summary>
    /// 외부(EntityAction, PlayerMediator 등)에서 호출하여 이번 틱에 적용될 이동 방향을 설정합니다.
    /// 모든 자식 클래스가 이 명령을 이해하고 처리할 수 있도록 추상 메서드로 선언합니다.
    /// </summary>
    public abstract void SetMoveDirection(Vector2 direction);
}