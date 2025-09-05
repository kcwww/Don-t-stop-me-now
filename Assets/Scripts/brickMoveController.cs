using UnityEngine;
using System.Collections;

/// <summary>
/// 플레이어가 닿으면 일정 시간 후 앞으로 이동하는 벽(브릭) 컨트롤러
/// </summary>
public class brickMoveController : MonoBehaviour
{
    public float distance = 2f;                // 이동 거리
    [SerializeField] float WaitTime = 0.2f;    // 이동 전 대기 시간

    // 초기화 (필요 시 사용)
    void Start()
    {
        // 필요 시 초기화 코드 작성
    }

    // 매 프레임 호출 (현재 미사용)
    void Update()
    {
        // 필요 시 업데이트 코드 작성
    }

    /// <summary>
    /// 플레이어가 트리거에 닿았을 때 호출
    /// </summary>
    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.tag == "Player")
    //    {
    //        // 플레이어와 충돌 시 일정 시간 후 앞으로 이동
    //        StartCoroutine(MoveBrick());
    //    }
    //}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // 플레이어와 충돌 시 일정 시간 후 앞으로 이동
            StartCoroutine(MoveBrick());
        }
    }

    /// <summary>
    /// 일정 시간 대기 후 벽을 앞으로 이동
    /// </summary>
    IEnumerator MoveBrick()
    {
        yield return new WaitForSeconds(WaitTime);
        transform.Translate(Vector3.forward * distance);
    }
}
