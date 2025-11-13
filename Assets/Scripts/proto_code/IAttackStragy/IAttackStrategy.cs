// 파일 이름: IAttackStrategy.cs (새로 만들기)

using UnityEngine;

/// <summary>
/// '어떻게 공격할 것인가'에 대한 모든 전략이 따라야 할 설계도입니다.
/// ScriptableObject로 구현하여 부품처럼 갈아 끼울 수 있도록 설계되었습니다.
/// </summary>
public interface IAttackStrategy
{
    // 공격을 실행하는 메서드. 공격자(instigator)와 목표물(target) 정보를 받습니다.
    void Execute(EntityAction instigator, EntityCore target);
}