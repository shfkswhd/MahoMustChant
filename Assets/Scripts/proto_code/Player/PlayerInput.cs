using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerInput : MonoBehaviour
{
    // Input Action Editor에서 생성한 클래스 (사용자 이름 반영)
    private PlayerKeysetting playerkeysetting;

    // --- 1. 지속적인 값 (CCTV 방식) ---
    public float MoveInputX { get; private set; } // 수평 이동 값 (-1, 0, 1)
    public bool IsCrouching { get; private set; } // 수그리기 키를 누르고 있는지 여부
    public bool IsWalking { get; private set; }   // 걷기(쉬프트) 키를 누르고 있는지 여부

    // --- 2. 단발성 이벤트 (초인종 방식) ---
    public event Action OnJumpInput;
    public event Action OnAttackInput;

    private void Awake()
    {
        playerkeysetting = new PlayerKeysetting();
    }

    private void OnEnable()
    {
        playerkeysetting.Player.Enable();

        // 단발성 액션 이벤트 구독
        playerkeysetting.Player.Jump.performed += HandleJump;
        playerkeysetting.Player.Attack.performed += HandleAttack;
    }

    private void OnDisable()
    {
        playerkeysetting.Player.Disable();
        playerkeysetting.Player.Jump.performed -= HandleJump;
        playerkeysetting.Player.Attack.performed -= HandleAttack;
    }

    private void Update()
    {
        // 지속적으로 확인해야 하는 값들을 매 프레임 갱신
        MoveInputX = playerkeysetting.Player.Move.ReadValue<float>();
        IsCrouching = playerkeysetting.Player.Crouch.IsPressed();
        IsWalking = playerkeysetting.Player.Walk.IsPressed();
    }

    private void HandleJump(InputAction.CallbackContext context)
    {
        OnJumpInput?.Invoke();
    }

    private void HandleAttack(InputAction.CallbackContext context)
    {
        OnAttackInput?.Invoke();
    }
}