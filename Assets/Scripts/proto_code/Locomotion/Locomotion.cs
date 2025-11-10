using UnityEngine;

// 이 스크립트가 붙은 오브젝트는 반드시 Rigidbody 2D를 가지도록 강제합니다.
[RequireComponent(typeof(Rigidbody2D))]
public abstract class Locomotion : MonoBehaviour
{
    // 이 클래스를 상속받는 자식 클래스들만 접근 가능한(protected) 공통 변수들입니다.
    protected Rigidbody2D rb;
    protected EntityCore entityCore; 

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        entityCore = GetComponent<EntityCore>(); // EntityBase 대신 EntityCore를 찾습니다.

        if (entityCore == null)
        {
            Debug.LogError("Locomotion 컴포넌트는 EntityCore 컴포넌트가 있는 오브젝트에 붙어야 합니다.", this);
        }
    }

    // 반드시 구현
    public abstract void Move(Vector2 direction);
}