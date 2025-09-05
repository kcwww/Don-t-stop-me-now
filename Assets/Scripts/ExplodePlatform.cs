using UnityEngine;
using System.Collections;

public class ExplodePlatform : MonoBehaviour
{
    [SerializeField] private float destroyDelay = 3f;

    private Rigidbody[] pieces;

    void Awake()
    {
        // 하위 조각 Rigidbody 캐싱
        pieces = GetComponentsInChildren<Rigidbody>(true);
    }

    public void Explode(float force, Vector3 center, float radius)
    {
        // 원본 비활성화 (시각적으로 사라짐)
        if (TryGetComponent<Collider>(out var col)) col.enabled = false;

        // 조각 활성화 + 힘 적용
        foreach (var rb in pieces)
        {
            rb.isKinematic = false;
            rb.linearVelocity = Vector3.zero; // 초기화
            rb.angularVelocity = Vector3.zero;
            rb.AddExplosionForce(force, center, radius, 1f, ForceMode.Impulse);
        }

        // 일정 시간 후 조각 삭제
        StartCoroutine(DestroyPiecesAfterDelay(destroyDelay));
    }

    private IEnumerator DestroyPiecesAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (var rb in pieces)
        {
            if (rb != null)
                Destroy(rb.gameObject);
        }

        Destroy(gameObject); // 원본 루트 오브젝트도 정리
    }
}


    

    

    

    
