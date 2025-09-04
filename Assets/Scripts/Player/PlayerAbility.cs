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
    [SerializeField] float flashDistance = 5.0f;
    [SerializeField] float wallJumpVerticalForce = 20.0f;
    [SerializeField] float shieldDuration = 3.0f;


    MeshRenderer playerMaterial;
    Rigidbody rb;
    GameManager gameManager;
    ItemType currentAbility = ItemType.Default;

    public bool isWallClimbing = false;
    public bool isShielding = false;
    float wallClimbDuration = 2.0f;
    Coroutine wallClimbCoroutine;








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
            case ItemType.Impulse:
                break;
            case ItemType.Stone:
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


    void OnJump()
    {
        // 현재 가속도 초기화 후 impulse 힘 적용
        // 아이템이 있을때에만 점프 가능
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        playerEffect.TriggerParticle(EffectType.Jump);
        SetDefaultAbility();

    }

    void OnDash()
    {
        Vector3 dashDir = PlayerController.GetMoveDirection();
        if (dashDir == Vector3.zero)
        {
            dashDir = transform.forward;
        }
        // 대시 속도 직접 설정
        rb.linearVelocity = dashDir * jumpForce;
        playerEffect.TriggerParticle(EffectType.Dash);
        SetDefaultAbility();
    }

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
            case ItemType.Impulse:
                break;
            case ItemType.Stone:
                break;
            default:
                playerMaterial.material = abilityMaterials[(int)ItemType.Default];
                break;
        }
    }


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
        StartCoroutine(ShieldOff());
    }

}


