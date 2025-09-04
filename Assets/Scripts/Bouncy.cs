using UnityEngine;

/// <summary>
/// 바운스(튕김) 효과를 주는 컴포넌트
/// </summary>
public class Bouncy : MonoBehaviour
{
    [SerializeField] PlayerEffect playerEffect; // 이펙트 처리용 컴포넌트
    [SerializeField] float bounceForce = 20f;   // 튕기는 힘

    Rigidbody rb; // 플레이어 Rigidbody
    GameManager gameManager;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameManager = GameManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Platform") && !(gameManager.isRestarting || gameManager.isClearing))
        {
            rb.linearVelocity = Vector3.zero; // 현재 속도 초기화
            rb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
            playerEffect.TriggerParticle(EffectType.Jump);
        }
    }
}
