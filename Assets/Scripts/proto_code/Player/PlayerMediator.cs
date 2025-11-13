// 파일 이름: PlayerMediator.cs

using UnityEngine;

[RequireComponent(typeof(PlayerInput), typeof(PlayerMovement), typeof(EntityAction))]
public class PlayerMediator : MonoBehaviour, ITickable
{
    [Header("Player Logic Tick Interval")]
    [SerializeField][Range(1, 10)] private uint tickInterval = 1; // 플레이어 입력은 매 틱마다 처리

    public uint TickInterval => tickInterval;

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
        TickManager.Instance.Register(this);
        playerInput.OnJumpInput += HandleJump;
        playerInput.OnAttackInput += HandleAttack;
    }

    private void OnDisable()
    {
        // TickManager는 자동 처리. 이벤트 해지만 남긴다.
        if (playerInput != null)
        {
            playerInput.OnJumpInput -= HandleJump;
            playerInput.OnAttackInput -= HandleAttack;
        }
    }

    /// <summary>
    /// TickManager가 호출하는 '논리' 업데이트.
    /// 여기서 결정된 사항이 다음 '물리' 업데이트(FixedUpdate)에 반영된다.
    /// </summary>
    public void OnTick()
    {
        float moveX = playerInput.MoveInputX;
        bool isWalking = playerInput.IsWalking;

        // 이동 '명령'을 PlayerMovement에 전달한다.
        playerMovement.SetInputs(new Vector2(moveX, 0), isWalking);
    }

    private void HandleJump()
    {
        // 점프는 즉각적인 반응이 중요하므로, 틱을 기다리지 않고 바로 명령한다.
        playerMovement.Jump();
    }

    private void HandleAttack()
    {
        entityAction.Attack(null);
    }
}