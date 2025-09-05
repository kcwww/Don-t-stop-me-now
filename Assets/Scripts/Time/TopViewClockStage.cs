using UnityEngine;

public class TopViewClockStage : MonoBehaviour
{
    [SerializeField] private Transform center;
    [SerializeField] private Transform hourHand;
    [SerializeField] private Transform minuteHand;
    [SerializeField] private Transform secondHand;

    TimeManager timeManager;

    private void Start()
    {
        timeManager = TimeManager.Instance;
    }

    void Update()
    {
        if (timeManager.isPaused) return;
        // 1초당 각도 계산
        float secondAngle = 6f * Time.deltaTime;       // 360 / 60
        float minuteAngle = 0.1f * Time.deltaTime;     // 6 / 60
        float hourAngle = (1f / 120f) * Time.deltaTime; // 30 / 3600

        // 중심축 기준 회전 (Y+ 방향으로 시계방향 보정)
        secondHand.RotateAround(center.position, Vector3.up, secondAngle);
        minuteHand.RotateAround(center.position, Vector3.up, minuteAngle);
        hourHand.RotateAround(center.position, Vector3.up, hourAngle);
    }
}
