// 파일 이름: NonPlayerMovement.cs

using UnityEngine;

/// <summary>
/// 플레이어가 아닌 모든 엔티티(AI, NPC 등)를 위한 이동 클래스의 부모입니다.
/// </summary>
public abstract class NonPlayerMovement : Locomotion
{
    // 추후 AI가 공유하는 이동 로직이 
    // 여기에 추가될 수 있습니다.
}