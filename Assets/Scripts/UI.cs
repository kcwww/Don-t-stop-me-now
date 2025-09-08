using System.Collections;
using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{
    public static UI Instance; // 싱글톤 인스턴스

    [Header("큰 버전 UI")]
    public TextMeshProUGUI deathCountText;
    public TextMeshProUGUI timeText;
    // === 추가: 큰 별 카운트 ===
    public TextMeshProUGUI starCountText;

    [Header("작은 버전 UI (기본 비활성화)")]
    public TextMeshProUGUI deathCountTextSmall;
    public TextMeshProUGUI timeTextSmall;
    // === 추가: 작은 별 카운트 ===
    public TextMeshProUGUI starCountTextSmall;

    [Header("기타 UI")]
    public TextMeshProUGUI stageOneText;
    public TextMeshProUGUI clearText;
    public TextMeshProUGUI failText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환에도 UI 유지
        }
        else
        {
            Destroy(gameObject); // 중복 UI 제거
        }
    }

    void Start()
    {
        StartCoroutine(ShowStageOneText());
    }

    void Update()
    {
        var gm = GameManager.Instance;
        if (gm == null) return;

        string formattedTime = gm.PlayTime.ToString("F3");

        // 큰 버전 텍스트 갱신
        if (deathCountText != null)
            deathCountText.text = " Deaths: " + gm.DeathCount;
        if (timeText != null)
            timeText.text = " Time: " + formattedTime;
        // === 추가: 큰 별 카운트 표시 ===
        if (starCountText != null)
            starCountText.text = $" Star: {gm.StarCount}";   // ★: n / 10 형태

        // 작은 버전 텍스트 갱신
        if (deathCountTextSmall != null)
            deathCountTextSmall.text = "Deaths: " + gm.DeathCount;
        if (timeTextSmall != null)
            timeTextSmall.text = "Time: " + formattedTime;
        // === 추가: 작은 별 카운트 표시 ===
        if (starCountTextSmall != null)
            starCountTextSmall.text = $" Star: {gm.StarCount}"; // ★: n / 10 형태
    }

    IEnumerator ShowStageOneText() // 스테이지 알림 텍스트 코루틴
    {
        if (stageOneText != null)
        {
            stageOneText.gameObject.SetActive(true);
            yield return new WaitForSeconds(2);
            if (stageOneText != null)
                stageOneText.gameObject.SetActive(false);
        }
    }

    public void ShowClearText(string message) // 클리어 텍스트 활성화
    {
        if (clearText != null)
        {
            // 큰 버전 숨기기
            if (deathCountText != null) deathCountText.gameObject.SetActive(false);
            if (timeText != null) timeText.gameObject.SetActive(false);
            if (stageOneText != null) stageOneText.gameObject.SetActive(false);
            if (starCountText != null) starCountText.gameObject.SetActive(false);   // === 추가

            // 작은 버전 켜기
            if (deathCountTextSmall != null) deathCountTextSmall.gameObject.SetActive(true);
            if (timeTextSmall != null) timeTextSmall.gameObject.SetActive(true);
            if (starCountTextSmall != null) starCountTextSmall.gameObject.SetActive(true); // === 추가

            // 클리어 텍스트 표시
            clearText.text = message;
            clearText.gameObject.SetActive(true);
        }
    }

    public void ShowFailText(string message) // 게임오버 텍스트 활성화
    {
        if (failText != null)
        {
            // 큰 버전 숨기기
            if (deathCountText != null) deathCountText.gameObject.SetActive(false);
            if (timeText != null) timeText.gameObject.SetActive(false);
            if (stageOneText != null) stageOneText.gameObject.SetActive(false);
            if (clearText != null) clearText.gameObject.SetActive(false);
            if (starCountText != null) starCountText.gameObject.SetActive(false);   // === 추가

            // 작은 버전도 숨기기 (FAIL에서는 작은 카운터 표시 안 함)
            if (deathCountTextSmall != null) deathCountTextSmall.gameObject.SetActive(false);
            if (timeTextSmall != null) timeTextSmall.gameObject.SetActive(false);
            if (starCountTextSmall != null) starCountTextSmall.gameObject.SetActive(false); // === 추가

            // Fail 텍스트 표시
            failText.text = message;
            failText.gameObject.SetActive(true);
        }
    }

}
