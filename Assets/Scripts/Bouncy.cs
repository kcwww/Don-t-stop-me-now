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
    PlayerAbility playerAbility;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerAbility = GetComponent<PlayerAbility>();
        gameManager = GameManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameManager.isRestarting || gameManager.isClearing) return;
        if (playerAbility.isIron) return;

        if (other.gameObject.CompareTag("Platform") || other.gameObject.CompareTag("Breakable"))
        {
            if (playerAbility.isImpacting) playerAbility.ExplodeImpact();

            rb.linearVelocity = Vector3.zero; // 현재 속도 초기화
            rb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
            playerEffect.TriggerParticle(EffectType.Jump);
        }
    }
}
