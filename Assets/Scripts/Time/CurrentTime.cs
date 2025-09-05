using UnityEngine;
using TMPro;
public class CurrentTime : MonoBehaviour
{
    [SerializeField] private TextMeshPro timeText;

    // 현재 시간 담을 변수 선언
    private System.DateTime currentTime;

    TimeManager timeManager;

    void Start()
    {
        // HH:mm:ss 형식으로 현재 시간 표시
        timeManager = TimeManager.Instance;
        currentTime = System.DateTime.Now;
        timeText.text = currentTime.ToString("HH:mm:ss");
    }

    void Update()
    {
        if (timeManager.isPaused) return;
        currentTime = System.DateTime.Now;
        timeText.text = currentTime.ToString("HH:mm:ss");
    }

}
