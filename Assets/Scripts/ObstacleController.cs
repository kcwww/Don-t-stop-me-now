using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public bool isReverse = false; // 반전 여부
    public bool isMoveX = false;
    public bool isMoveY = false;
    public bool isMoveZ = false;
    public bool isRotate = false;
    public bool isScale = false;
    public bool isPush = false;
    public bool isPushUp = false;

    public float moveSpeed = 1f; // 이동 속도
    public float moveRange = 5f; // 이동 범위
    public float rotateSpeed = 50f; // 회전 속도
    public float scaleSpeed = 0.1f; // 크기 변경 속도
    public float scaleRange = 2f; // 크기 변경 범위
    public float pushForce = 20f; // 밀치는 힘

    private Vector3 startPosition; // 시작 위치 저장용

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //오브젝트 배치 초기 위치 저장
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        isMoveObstacle(); // 이동 처리
        isRotateObstacle(); // 회전 처리
        isScaleObstacle(); // 크기 변경 처리
    }

    void isMoveObstacle()
    {
        if (isMoveX)
        {
            // 시작 위치 기준 좌우로 이동
            Vector3 newPosition = startPosition;
            float moveX = Mathf.PingPong(Time.time * moveSpeed, moveRange) - (moveRange / 2);
            newPosition.x += moveX;
            transform.position = newPosition;

            if (isReverse)
            {
                // 반전 처리
                newPosition.x = startPosition.x - moveX;
                transform.position = newPosition;
            }
        }
        else if (isMoveY)
        {
            // 시작 위치 기준 상하로 이동
            float moveY = Mathf.PingPong(Time.time * moveSpeed, moveRange) - (moveRange / 2);
            Vector3 newPosition = startPosition;
            newPosition.y += moveY;
            transform.position = newPosition;

            if (isReverse)
            {
                // 반전 처리
                newPosition.y = startPosition.y - moveY;
                transform.position = newPosition;
            }
        }
        else if (isMoveZ)
        {
            // 시작 위치 기준 앞뒤로 이동
            float moveZ = Mathf.PingPong(Time.time * moveSpeed, moveRange); // - (moveRange / 2) 제거
            Vector3 newPosition = startPosition;
            newPosition.z += moveZ;
            transform.position = newPosition;

            if (isReverse)
            {
                // 반전 처리
                newPosition.z = startPosition.z - moveZ;
                transform.position = newPosition;
            }
        }
    }

    void isRotateObstacle()
    {
        if (isRotate)
        {
            transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
        }
    }

    void isScaleObstacle()
    {
        if (isScale)
        {
            // 크기 변경
            float scaleValue = Mathf.PingPong(Time.time * scaleSpeed, scaleRange) + 1; // 1부터 scaleRange까지
            transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue); // x, y, z 모두 동일하게 변경
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isPush && collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                Vector3 pushDirection = (collision.transform.position - transform.position).normalized;
                playerRb.AddForce(pushDirection * pushForce, ForceMode.Impulse);
            }
        }
        else if (isPushUp && collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                Vector3 pushDirection = Vector3.up; // 위로 밀기
                playerRb.AddForce(pushDirection * pushForce, ForceMode.Impulse);
            }
        }
    }
}
