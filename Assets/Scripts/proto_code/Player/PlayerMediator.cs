using UnityEngine;

[RequireComponent(typeof(PlayerInput), typeof(PlayerMovement), typeof(EntityAction))]
public class PlayerMediator : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerMovement playerMovement;
    private EntityAction entityAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        entityAction = GetComponent<EntityAction>();
    }

    private void OnEnable()
    {
        playerInput.OnJumpInput += HandleJumpInput;
        playerInput.OnAttackInput += HandleAttackInput;
    }

    private void OnDisable()
    {
        playerInput.OnJumpInput -= HandleJumpInput;
        playerInput.OnAttackInput -= HandleAttackInput;
    }

    private void Update()
    {
        // Input -> Movement 로 지속적인 데이터 전달
        playerMovement.Move(playerInput.MoveInputX, playerInput.IsWalking);
        playerMovement.Crouch(playerInput.IsCrouching);
    }

    private void HandleJumpInput()
    {
        // Input -> Movement 로 점프 명령 전달
        playerMovement.Jump();
    }

    private void HandleAttackInput()
    {
        // Input -> Action 으로 공격 명령 전달
        entityAction.Attack(null); // (추후 Sense를 이용해 target 지정)
    }
}