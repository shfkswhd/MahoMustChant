// 파일 이름: PlayerMediator.cs (최종 수정)

using UnityEngine;

[RequireComponent(typeof(PlayerInput), typeof(PlayerMovement), typeof(EntityAction))]
public class PlayerMediator : MonoBehaviour, ITickable
{
    private PlayerInput playerInput;
    private PlayerMovement playerMovement; // 이제 Locomotion이 아니라 구체적인 PlayerMovement를 참조한다.
    private EntityAction entityAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>(); // 자식 클래스를 직접 참조
        entityAction = GetComponent<EntityAction>();
    }

    private void OnEnable()
    {
        TickManager.Instance.Register(this);
        playerInput.OnJumpInput += HandleJump;
        playerInput.OnAttackInput += HandleAttack;
    }

    private void OnDisable()
    {
        playerInput.OnJumpInput -= HandleJump;
        playerInput.OnAttackInput -= HandleAttack;
    }

    public void OnTick()
    {
        // 1. 이동에 대한 '의도'를 읽어온다.
        float moveX = playerInput.MoveInputX;
        bool isWalking = playerInput.IsWalking;
        bool isCrouching = playerInput.IsCrouching;
        Vector2 moveDirection = new Vector2(moveX, 0);

        // 2. 이동 전문가(PlayerMovement)에게 '의도'를 전달한다.
        //    "이런 입력이 들어왔으니, 네가 알아서 처리해."
        playerMovement.SetInputs(moveDirection, isWalking, isCrouching);
    }

    private void HandleJump()
    {
        // 점프 입력이 들어왔다는 '편지'를 받았다.
        // 나는 점프하는 방법을 모른다. 그냥 이동 전문가에게 "점프하래!" 라고 전달만 한다.
        playerMovement.Jump();
    }

    private void HandleAttack()
    {
        Debug.Log("Attack input received!");
        // entityAction.Attack() -> entityAction.Attack(null) 로 변경
        // "타겟은 없지만, 일단 공격하라는 신호는 보낼게" 라는 의미다.
        entityAction.Attack(null);
    }
}