// 파일 이름: TickManager.cs (FixedUpdate 최종 완성 버전)

using UnityEngine;
using System.Collections.Generic;

public class TickManager : MonoBehaviour
{
    // --- 싱글톤 ---
    private static TickManager _instance;
    private static bool applicationIsQuitting = false;
    public static bool IsQuitting => applicationIsQuitting;
    public static TickManager Instance
    {
        get
        {
            // 게임이 종료되는 중이라면, 아무것도 찾거나 만들지 말고 그냥 null을 반환한다.
            if (applicationIsQuitting)
            {
                return null;
            }

            // _instance가 비어있는지(null) 확인한다.
            if (_instance == null)
            {
                // 씬에 이미 TickManager 타입의 오브젝트가 있는지 찾아본다. (가장 빠른 방식)
                _instance = FindAnyObjectByType<TickManager>();

                // 찾아봤는데도 없다면, 직접 만들어서 씬에 추가한다.
                if (_instance == null)
                {
                    // "@TickManager (Auto-Generated)" 라는 이름의 새 게임 오브젝트를 생성한다.
                    GameObject singletonObject = new GameObject($"@{nameof(TickManager)} (Auto-Generated)");
                    // 생성된 오브젝트에 TickManager 컴포넌트를 붙인다.
                    _instance = singletonObject.AddComponent<TickManager>();
                }
            }
            // 최종적으로 찾거나, 새로 만든 _instance를 반환한다.
            return _instance;
        }
    }

    // --- 틱 설정 ---
    [Header("Tick Settings")]
    [Tooltip("초당 실행될 물리 틱의 횟수 (Ticks Per Second)")]
    [Range(1, 120)]
    public int tickRate = 50; // 정수로 변경

    // TickInterval은 이제 Time.fixedDeltaTime을 통해 관리되므로, public 프로퍼티는 없어도 된다.

    // --- 내부 변수 ---
    private List<ITickable> tickables = new List<ITickable>();

    // --- Unity 생명주기 메서드 ---
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
        ApplyTickSettings(); // Awake에서 호출
    }

    private void OnDestroy()
    {
        applicationIsQuitting = true;
        tickables?.Clear();
    }

    private void OnValidate()
    {
        ApplyTickSettings(); // OnValidate에서 호출
    }

    /// <summary>
    /// Unity의 물리 업데이트 주기에 맞춰 고정적으로 호출됩니다.
    /// 이것이 우리 시스템의 새로운 '심장 박동기'입니다.
    /// </summary>
    private void FixedUpdate()
    {
        // 리스트를 순회하며 죽은 객체를 정리하고, 살아있는 객체의 'OnTick()'을 호출합니다.
        for (int i = tickables.Count - 1; i >= 0; i--)
        {
            if (tickables[i] == null)
            {
                tickables.RemoveAt(i);
                continue;
            }
            tickables[i].OnTick(); // <-- '약속'된 OnTick()을 호출!
        }
    }

    // --- 설정 적용 ---
    private void ApplyTickSettings()
    {
        if (tickRate > 0)
        {
            // Unity의 물리 시간 단위를 우리의 tickRate에 맞춰 설정합니다.
            Time.fixedDeltaTime = 1.0f / tickRate;
        }
    }

    // --- Public API ---
    public void Register(ITickable tickable)
    {
        if (!tickables.Contains(tickable))
        {
            tickables.Add(tickable);
        }
    }
}