using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class PlayerController1 : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotationSpeed = 0.1f;
    [SerializeField] float cameraRotationSpeed = 0.05f;
    [SerializeField] float minPitch = -30f;
    [SerializeField] float maxPitch = 60f;
    [SerializeField] float cameraDistance = 10f;

    [SerializeField] RawImage RawImage;
    [SerializeField] RenderTexture TopView;
    [SerializeField] RenderTexture SideView;

    [SerializeField] bool isStartStage = false;

    public Camera playerCamera;

    GameManager gameManager;
    PlayerAbility playerAbility;
    Rigidbody rb;
    Vector2 moveInput;
    Vector2 lookInput;
    float cameraPitch = 0f;




    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        playerAbility = GetComponent<PlayerAbility>();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }


    void OnLook(InputValue value)
    {
        if (isStartStage) return;
        lookInput = value.Get<Vector2>();
    }


    void ProcessLook()
    {
        float yaw = transform.eulerAngles.y + lookInput.x * rotationSpeed;

        cameraPitch -= lookInput.y * cameraRotationSpeed;
        cameraPitch = Mathf.Clamp(cameraPitch, minPitch, maxPitch);


        transform.eulerAngles = new Vector3(0f, yaw, 0f);


        Vector3 offset = Quaternion.Euler(cameraPitch, yaw, 0f) * new Vector3(0, 0, -cameraDistance);
        playerCamera.transform.position = transform.position + offset;
        playerCamera.transform.LookAt(transform.position + Vector3.up * 1.5f);
    }




    void FixedUpdate()
    {
        if (gameManager.isRestarting || gameManager.isClearing) return;
        if (playerAbility.isWallClimbing) return;
        ProcessMove();
    }

    private void LateUpdate()
    {
        if (gameManager.isRestarting || gameManager.isClearing) return;
        ProcessLook();
    }




    public Vector3 GetMoveDirection()
    {
        Vector3 camForward = playerCamera.transform.forward;
        Vector3 camRight = playerCamera.transform.right;



        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();



        Vector3 moveDir = camRight * moveInput.x + camForward * moveInput.y;
        return moveDir;
    }


    private void ProcessMove()
    {
        Vector3 moveDir = GetMoveDirection();
        Vector3 targetPos = rb.position + moveDir * moveSpeed * Time.fixedDeltaTime;

        rb.MovePosition(targetPos);
    }


    private void OnCollisionEnter(Collision collision)
    {
        

        if (collision.gameObject.layer == LayerMask.NameToLayer("Clear"))
        {
            GameManager.Instance.StageClear();
        }
        

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            GameManager.Instance.CollectStar(other.gameObject);
        } else if (other.gameObject.CompareTag("ChangeToGlass"))
        {
            if (playerAbility.isShielding) return;
            GameManager.Instance.SpawnPlayer(); // 리스폰 호출
        }


    }

    void OnToggleCamera()
    {
        if (RawImage.texture == TopView)
        {
            RawImage.texture = SideView;
        }
        else
        {
            RawImage.texture = TopView;
        }
    }

    void OnQuit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
