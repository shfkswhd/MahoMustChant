// 파일 이름: TickManager.cs

using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 게임의 모든 틱 기반 로직을 총괄하는 중앙 관리자입니다.
/// 자동 생성 싱글톤이며, 등록된 모든 ITickable 객체의 생명주기와 실행 주기를 관리합니다.
/// </summary>
public class TickManager : MonoBehaviour
{
    #region Singleton
    private static TickManager _instance;
    private static bool applicationIsQuitting = false;
    public static bool IsQuitting => applicationIsQuitting;

    public static TickManager Instance
    {
        get
        {
            if (applicationIsQuitting) return null;
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<TickManager>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject($"@{nameof(TickManager)} (Auto-Generated)");
                    _instance = singletonObject.AddComponent<TickManager>();
                }
            }
            return _instance;
        }
    }
    #endregion

    [Header("Tick Settings")]
    [Tooltip("초당 실행될 틱의 횟수 (Ticks Per Second)")]
    [Range(1, 120)]
    public ushort tickRate = 20;
    public float TickIntervalSeconds { get; private set; }

    // --- 내부 변수 ---
    private readonly List<ITickable> tickables = new List<ITickable>();
    private float accumulator = 0f;
    private ulong currentTickCount = 0; // 전체 틱 카운트 (ulong으로 오버플로우 방지)

    #region Unity Lifecycle
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
        UpdateTickInterval();
    }

    private void OnDestroy()
    {
        applicationIsQuitting = true;
        tickables?.Clear();
    }

    private void OnValidate()
    {
        UpdateTickInterval();
    }

    private void Update()
    {
        accumulator += Time.deltaTime;
        while (accumulator >= TickIntervalSeconds)
        {
            accumulator -= TickIntervalSeconds;
            BroadcastTick(); // 틱 실행
        }
    }
    #endregion

    /// <summary>
    /// 한 번의 틱을 실행하고, 등록된 모든 객체의 주기를 검사하며, 파괴된 객체를 정리합니다.
    /// </summary>
    private void BroadcastTick()
    {
        currentTickCount++; // 전체 틱 카운트 증가

        // 리스트를 뒤에서부터 순회하여 삭제 시 인덱스 문제를 방지합니다.
        for (int i = tickables.Count - 1; i >= 0; i--)
        {
            ITickable tickable = tickables[i];

            // 1. 안전한 생명주기 관리: 객체가 파괴되었는지(null) 확인합니다.
            if (tickable == null)
            {
                tickables.RemoveAt(i);
                continue;
            }

            // 2. 주기 검사: 현재 틱 카운트를 객체의 TickInterval로 나눈 나머지가 0인지 확인합니다.
            //    (TickInterval이 0인 경우는 없도록 uint로 방지했지만, 만일을 대비해 체크)
            if (tickable.TickInterval > 0 && currentTickCount % tickable.TickInterval == 0)
            {
                // 3. 주기가 도래한 객체의 OnTick()만 호출합니다.
                tickable.OnTick();
            }
        }
    }

    private void UpdateTickInterval()
    {
        if (tickRate > 0) TickIntervalSeconds = 1.0f / tickRate;
    }

    /// <summary>
    /// 틱 시스템에 객체를 등록합니다.
    /// </summary>
    public void Register(ITickable tickable)
    {
        if (tickable != null && !tickables.Contains(tickable))
        {
            tickables.Add(tickable);
        }
    }
}