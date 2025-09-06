using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.UI; // 버튼 제어
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private bool shouldCountTime = false; // 시간 카운트 여부

    // === 변경: 별 카운트 구조 ===
    public int StarCount { get; private set; } = 0;   // 표시용(저장 + 현재 스테이지)
    private int savedStars = 0;                       // 이전 스테이지들에서 누적(유지 대상)
    private int currentStageStars = 0;                // 현재 스테이지에서 먹은 별(해당 스테이지 내에서만 초기화됨)

    private bool resetStarsOnNextStart = false;       // 최종 클리어 후 Start 진입 때 전체 초기화

    // 인스펙터 할당 제거
    private GameObject finalClearPanel;
    private Button homeButton;
    private Button exitButton;

    public int DeathCount { get; private set; } = 0;   // 죽음 횟수
    public float PlayTime { get; private set; } = 0f;  // 현재 씬의 플레이 시간(초)

    public bool isRestarting = false;
    public bool isClearing = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1f;
        isRestarting = false;
        isClearing = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        var sceneName = scene.name;

        // === 중요: 씬 진입 시 별 로직 ===
        if (sceneName == "Start")
        {
            // Start에서는 시간 리셋 + 카운트 중지
            PlayTime = 0f;
            shouldCountTime = false;

            // 최종 클리어 후 Start로 올 때만 전체 초기화
            if (resetStarsOnNextStart)
            {
                savedStars = 0;
                currentStageStars = 0;
                UpdateStarCount();
                resetStarsOnNextStart = false;
            }
        }
        else if (sceneName == "Stage1")
        {
            // Stage1 시작 시: 이전 누적/현재 모두 0 (처음부터)
            savedStars = 0;
            currentStageStars = 0;
            UpdateStarCount();
            shouldCountTime = true;
        }
        else if (sceneName == "Stage2" || sceneName == "Stage3")
        {
            // Stage2/3 시작 시: 이전 누적(savedStars)은 유지, 현재 스테이지 별은 0부터
            currentStageStars = 0;
            UpdateStarCount();
            shouldCountTime = true;
        }
        else
        {
            // 기타 씬은 기본적으로 카운트 (필요 시 조정)
            shouldCountTime = true;
        }

        EnsureEventSystem();

        finalClearPanel = GameObject.Find("UI 3/Canvas/Clear Text/ClearPanel");
        if (finalClearPanel != null)
        {
            finalClearPanel.SetActive(false); // 기본은 꺼두기
            var hb = GameObject.Find("UI 3/Canvas/Clear Text/ClearPanel/MainButton");
            var eb = GameObject.Find("UI 3/Canvas/Clear Text/ClearPanel/ExitButton");
            homeButton = hb ? hb.GetComponent<Button>() : null;
            exitButton = eb ? eb.GetComponent<Button>() : null;

            if (homeButton != null)
            {
                homeButton.onClick.RemoveAllListeners();
                homeButton.onClick.AddListener(OnClickHomeButton);
            }
            if (exitButton != null)
            {
                exitButton.onClick.RemoveAllListeners();
                exitButton.onClick.AddListener(OnClickExitButton);
            }
        }
    }

    private void Update()
    {
        if (shouldCountTime && !isRestarting && !isClearing)
        {
            PlayTime += Time.deltaTime;
        }
    }

    public void SpawnPlayer()
    {
        if (isRestarting || isClearing) return;

        if (finalClearPanel != null) finalClearPanel.SetActive(false);

        if (UI.Instance != null)
            UI.Instance.ShowFailText("Fail...");

        DeathCount++;
        ExplodePlayer();

        // === 중요: 사망 시 현재 스테이지 별만 초기화
        ResetCurrentStageStars();

        // 바로 카운트 중지되도록 즉시 플래그 설정
        isRestarting = true;

        StartCoroutine(RestartCurrentSceneAfterRealtime(2f));
    }

    private void ExplodePlayer()
    {
        PlayerEffect effect = FindFirstObjectByType<PlayerEffect>();
        if (effect != null)
        {
            effect.TriggerParticle(EffectType.Explosion);
        }
    }

    private void NextEffect()
    {
        PlayerEffect effect = FindFirstObjectByType<PlayerEffect>();
        if (effect != null)
        {
            effect.TriggerParticle(EffectType.NextStage);
        }
    }

    private void ClearEffect()
    {
        PlayerEffect effect = FindFirstObjectByType<PlayerEffect>();
        if (effect != null)
        {
            effect.TriggerParticle(EffectType.Clear);
        }
    }

    private IEnumerator RestartCurrentSceneAfterRealtime(float delay)
    {
        isRestarting = true;
        yield return new WaitForSecondsRealtime(delay);

        if (finalClearPanel != null) finalClearPanel.SetActive(false);

        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.name); // 시간 유지
    }

    public void StageClear()
    {
        if (isClearing || isRestarting) return;

        // === 중요: 다음 스테이지로 넘어가기 전에
        // 현재 스테이지에서 먹은 별을 저장 별에 합산하고 현재 스테이지 별은 초기화
        savedStars += currentStageStars;
        currentStageStars = 0;
        UpdateStarCount();

        NextEffect();
        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.name == "Stage1") StartCoroutine(LoadNextStageAfterRealtime("Stage2", 2f));
        else if (currentScene.name == "Stage2") StartCoroutine(LoadNextStageAfterRealtime("Stage3", 2f));
    }

    private IEnumerator LoadNextStageAfterRealtime(string stageName, float delay)
    {
        isClearing = true;

        if (UI.Instance != null)
            UI.Instance.ShowClearText("Clear!");

        yield return new WaitForSecondsRealtime(delay);

        SceneManager.LoadScene(stageName);
    }

    public void FinalClear()
    {
        if (isClearing || isRestarting) return;
        isClearing = true;

        var playerInput = FindFirstObjectByType<PlayerInput>();
        if (playerInput != null) playerInput.enabled = false;

        var playerController = FindFirstObjectByType<PlayerController>();
        if (playerController != null) playerController.enabled = false;

        if (UI.Instance != null)
            UI.Instance.ShowClearText("Finish!");

        // === 중요: Stage3 최종 클리어 → Start 진입 시 전체 초기화 표시
        resetStarsOnNextStart = true;

        // Stage3 최종 클리어 후, 카운트 중지
        ClearEffect();
        shouldCountTime = false;

        if (finalClearPanel != null) finalClearPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // === 변경: 아이템(별) 획득 시 현재 스테이지 별만 증가
    public void CollectStar(GameObject item)
    {
        currentStageStars++;
        UpdateStarCount();

        if (item != null)
            Destroy(item);
    }

    // 버튼 핸들러
    public void OnClickHomeButton()
    {
        Time.timeScale = 1f;

        // Start로 갈 때 명시적으로 시간/별 초기화 + 카운트 중지
        PlayTime = 0f;
        shouldCountTime = false;

        // 홈으로 가면 별도 초기화하고 시작(요구사항에 맞게 유지하고 싶으면 이 줄 제거)
        savedStars = 0;
        currentStageStars = 0;
        UpdateStarCount();

        SceneManager.LoadScene("Start");
    }

    public void OnClickExitButton()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // === 유틸: 현재 스테이지 별만 초기화
    private void ResetCurrentStageStars()
    {
        currentStageStars = 0;
        UpdateStarCount();
    }

    // === 유틸: 표시용 총합 업데이트
    private void UpdateStarCount()
    {
        StarCount = savedStars + currentStageStars;
        // 필요 시 UI 반영 로직이 있다면 여기서 갱신
        // 예: UI.Instance?.SetStar(StarCount);
    }

    // EventSystem이 없으면 만들어줌 (Input System 환경 대응)
    private void EnsureEventSystem()
    {
        if (FindFirstObjectByType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            var es = new GameObject("EventSystem");
            es.AddComponent<UnityEngine.EventSystems.EventSystem>();
#if ENABLE_INPUT_SYSTEM
            es.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
#else
            es.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
#endif
        }
    }

    
}
