using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbility : MonoBehaviour
{
    [SerializeField] PlayerEffect playerEffect;
    [SerializeField] PlayerController PlayerController;
    [SerializeField] Material[] abilityMaterials;
    [SerializeField] GameObject shieldObject;

    [SerializeField] float jumpForce = 20.0f;
    [SerializeField] float dashForce = 20.0f;
    [SerializeField] float flashDistance = 5.0f;
    [SerializeField] float wallJumpVerticalForce = 20.0f;
    [SerializeField] float shieldDuration = 3.0f;
    [SerializeField] float groundImpactForce = 30.0f;
    [SerializeField] float groundImpactRadius = 4.0f;
    [SerializeField] float groundImpactExplosionForce = 100.0f;
    [SerializeField] float ironDuration = 3.0f;

    MeshRenderer playerMaterial;
    Rigidbody rb;
    GameManager gameManager;
    ItemType currentAbility = ItemType.Default;

    public bool isWallClimbing = false;
    public bool isShielding = false;
    public bool isImpacting = false;
    public bool isIron = false;

    float wallClimbDuration = 2.0f;

    Coroutine wallClimbCoroutine;
    Coroutine shieldCoroutine;
    Coroutine ironCoroutine;








    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMaterial = GetComponent<MeshRenderer>();
        gameManager = GameManager.Instance;
    }

    void OnAbility()
    {
        if (gameManager.isRestarting || gameManager.isClearing) return;

        switch (currentAbility)
        {
            case ItemType.DoubleJump:
                OnJump();
                break;
            case ItemType.Dash:
                OnDash();
                break;
            case ItemType.Flash:
                OnFlash();
                break;
            case ItemType.WallClimbing:
                OnClimbing();
                break;
            case ItemType.Shield:
                OnShield();
                break;
            case ItemType.GroundImpact:
                OnGroundImpact();
                break;
            case ItemType.Iron:
                break;
            default:
                break;
        }
    }



    void SetDefaultAbility()
    {
        currentAbility = ItemType.Default;
        playerMaterial.material = abilityMaterials[(int)ItemType.Default];
    }


    // 벽 타기 기능

    IEnumerator WallClimbTimer()
    {
        yield return new WaitForSeconds(wallClimbDuration);

        if (!isWallClimbing) yield break;

        isWallClimbing = false;
        rb.useGravity = true;
        SetDefaultAbility();
    }

    private void OnCollisionStay(Collision collision)
    {

        if (collision.gameObject.CompareTag("Wall") && currentAbility == ItemType.WallClimbing)
        {

            // 벽 타기 시작
            isWallClimbing = true;


            rb.linearVelocity = Vector3.zero;
            rb.useGravity = false;

            if (wallClimbCoroutine != null)
            {
                StopCoroutine(wallClimbCoroutine);
            }
            wallClimbCoroutine = StartCoroutine(WallClimbTimer());
        }
    }

    void OnClimbing()
    {
        if (!isWallClimbing) return;
        // 벽 타기 중일 때 반대 방향으로 점프

        isWallClimbing = false;
        rb.useGravity = true;


        rb.linearVelocity = Vector3.zero;
        rb.AddForce(Vector3.up * wallJumpVerticalForce, ForceMode.Impulse);



        //playerEffect.TriggerParticle(EffectType.WallClimbing);

        SetDefaultAbility();
    }



    // 더블 점프 기능

    void OnJump()
    {
        // 현재 가속도 초기화 후 impulse 힘 적용
        // 아이템이 있을때에만 점프 가능
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        playerEffect.TriggerParticle(EffectType.Jump);
        SetDefaultAbility();

    }


    // 대시 기능

    void OnDash()
    {
        Vector3 dashDir = PlayerController.GetMoveDirection();
        if (dashDir == Vector3.zero)
        {
            dashDir = transform.forward;
        }
        // 대시 속도 직접 설정
        rb.linearVelocity = dashDir * dashForce;
        playerEffect.TriggerParticle(EffectType.Dash);
        SetDefaultAbility();
    }

    // 점멸 기능
    void OnFlash()
    {
        Vector3 flashDir = PlayerController.GetMoveDirection();
        if (flashDir == Vector3.zero)
        {
            flashDir = transform.forward;
        }

        // 순간이동
        transform.position += flashDir * flashDistance;
        playerEffect.TriggerParticle(EffectType.Flash);
        SetDefaultAbility();
    }

    // 쉴드 기능
    IEnumerator ShieldOff()
    {
        Renderer shieldRenderer = shieldObject.GetComponent<Renderer>();
        Material mat = shieldRenderer.material;

        float initialMetallic = mat.GetFloat("_Metallic");
        float initialSmoothness = mat.GetFloat("_Smoothness");

        float elapsedTime = 0f;
        while (elapsedTime < shieldDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / shieldDuration;
            mat.SetFloat("_Metallic", Mathf.Lerp(initialMetallic, 0f, t));
            mat.SetFloat("_Smoothness", Mathf.Lerp(initialSmoothness, 0f, t));
            yield return null;
        }



        isShielding = false;
        shieldObject.SetActive(false);
        mat.SetFloat("_Metallic", initialMetallic);
        mat.SetFloat("_Glossiness", initialSmoothness);
    }

    void OnShield()
    {
        isShielding = true;
        shieldObject.SetActive(true);
        SetDefaultAbility();
        if (shieldCoroutine != null)
        {
            StopCoroutine(shieldCoroutine);
        }
        shieldCoroutine = StartCoroutine(ShieldOff());
    }

    // 폭발 기능

    void OnGroundImpact()
    {
        // 땅을 강하게 내려찍는 기능
        // 현재 속도 초기화 후 아래 방향으로 Impulse 힘 적용
        isImpacting = true;
        rb.linearVelocity = Vector3.zero;
        rb.AddForce(Vector3.down * groundImpactForce, ForceMode.Impulse);
        // 바운시 코드에서 변수 확인 후 충격파 이펙트 발생 및 코드 실행
        SetDefaultAbility();
    }

    public void ExplodeImpact()
    {
        
        isImpacting = false;
        //playerEffect.TriggerParticle(EffectType.GroundImpact);
        



        Collider[] hits = Physics.OverlapSphere(transform.position, groundImpactRadius);


        // 부술 수 있는 오브젝트들에 힘 적용
        // 힘 적용 후 코루틴을 실행하여 저장한 오브젝트들 일정 시간 후 삭제
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Breakable"))
            {
                ExplodePlatform b = hit.GetComponent<ExplodePlatform>();
                if (b != null)
                {
                    b.Explode(groundImpactExplosionForce, transform.position, groundImpactRadius * 2);
                }
            }
        }

    }

    void OnIron()
    {
        isIron = true;
        if (ironCoroutine != null)
        {
            StopCoroutine(ironCoroutine);
        }
        ironCoroutine = StartCoroutine(IronOff());
    }

    IEnumerator IronOff()
    {
        yield return new WaitForSeconds(ironDuration);
        SetDefaultAbility();
        isIron = false;
        rb.linearVelocity = Vector3.zero; // 현재 속도 초기화
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        playerEffect.TriggerParticle(EffectType.Jump);
    }


    public void GiveAbility(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.DoubleJump:
                currentAbility = ItemType.DoubleJump;
                playerMaterial.material = abilityMaterials[(int)ItemType.DoubleJump];
                break;
            case ItemType.Dash:
                currentAbility = ItemType.Dash;
                playerMaterial.material = abilityMaterials[(int)ItemType.Dash];
                break;
            case ItemType.Flash:
                currentAbility = ItemType.Flash;
                playerMaterial.material = abilityMaterials[(int)ItemType.Flash];
                break;
            case ItemType.WallClimbing:
                currentAbility = ItemType.WallClimbing;
                playerMaterial.material = abilityMaterials[(int)ItemType.WallClimbing];
                break;
            case ItemType.Shield:
                currentAbility = ItemType.Shield;
                playerMaterial.material = abilityMaterials[(int)ItemType.Shield];
                break;
            case ItemType.GroundImpact:
                currentAbility = ItemType.GroundImpact;
                playerMaterial.material = abilityMaterials[(int)ItemType.GroundImpact];
                break;
            case ItemType.Iron:
                currentAbility = ItemType.Iron;
                OnIron();
                playerMaterial.material = abilityMaterials[(int)ItemType.Iron];
                break;
            default:
                playerMaterial.material = abilityMaterials[(int)ItemType.Default];
                break;
        }
    }



}


