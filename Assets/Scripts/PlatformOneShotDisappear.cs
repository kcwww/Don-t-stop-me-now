using UnityEngine;
using System.Collections;

/// <summary>
/// 플레이어가 닿으면 일정 시간 후 사라지고, 일정 시간 후 다시 나타나는 일회용 플랫폼
/// </summary>
public class PlatformOneShotDisappear : MonoBehaviour
{
    [SerializeField] float disappearDelay = 0.6f;   // 사라지기까지의 지연 시간
    [SerializeField] float destroyDelay = 0.1f;     // 오브젝트 삭제까지의 추가 지연 시간 (현재 미사용)
    [SerializeField] string playerTag = "Player";   // 플레이어 태그

    bool triggered;                                 // 이미 트리거됐는지 여부
    Collider col;                                   // 플랫폼 콜라이더
    Renderer[] renderers;                           // 플랫폼 렌더러들

    void Awake()
    {
        col = GetComponent<Collider>();                         // 콜라이더 컴포넌트 캐싱
        renderers = GetComponentsInChildren<Renderer>(true);    // 모든 자식 렌더러 캐싱
    }

    // 플레이어가 충돌했을 때 호출
    void OnCollisionEnter(Collision other)
    {
        if (!triggered && other.gameObject.CompareTag(playerTag))
        {
            triggered = true;
            Invoke(nameof(DisappearNow), disappearDelay); // 일정 시간 후 사라짐
        }
    }

    // 플레이어가 트리거로 닿았을 때 호출
    void OnTriggerEnter(Collider other)
    {
        if (!triggered && other.gameObject.CompareTag(playerTag))
        {
            triggered = true;
            Invoke(nameof(DisappearNow), disappearDelay); // 일정 시간 후 사라짐
        }
    }

    // 플랫폼 사라짐 처리
    void DisappearNow()
    {
        // 오브젝트 비활성화
        gameObject.SetActive(false);

        // 일정 시간 후 다시 활성화 (respawn)
        //StartCoroutine(Respawn());
    }

/*    [SerializeField] float respawnTime = 3.0f; // 다시 나타나기까지의 시간

    // 일정 시간 후 플랫폼을 다시 활성화
    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime);

        // 오브젝트 활성화
        gameObject.SetActive(true);
    }*/
}
