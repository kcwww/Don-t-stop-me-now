using UnityEngine;

public class Player : MonoBehaviour
{
    public float fallThreshold = -10f; // 이 값보다 y좌표가 작아지면 낙사 처리

    Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // 낙사 체크
        if (transform.position.y < fallThreshold)
        {
            rb.useGravity = false; // 중력 비활성화
            rb.linearVelocity = Vector3.zero; // 속도 초기화
            Die();
        }
    }

    void Die()
    {
        GameManager.Instance.SpawnPlayer(); // 리스폰 호출
    }
}
