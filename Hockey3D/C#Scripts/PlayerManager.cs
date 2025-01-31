using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private float speed = 30.0f;
    [SerializeField] private float rotationSpeed = 50.0f;
    [SerializeField] private float drag = 2.0f;
    [SerializeField] private float jetForce = 50.0f;
    [SerializeField] private float maxVelocity = 20.0f;
    [SerializeField] private float maxAngularVelocity = 4.0f;

    [SerializeField] private float angularDegreeMax = 60.0f;
    [SerializeField] private float angularDegreeMin = -60.0f;

    private Vector2 moveInput;
    private Vector2 verticalMoveInput;
    private Vector2 lookInput;
    private bool jetActivated;
    private float currentPitch = 0.0f;

    [SerializeField] private GameObject lightBlade;
    [SerializeField] private Animator attackAnimator;
    private float attackTime = 1.0f;

    private PlayerController playerController;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearDamping = drag;
        rb.maxAngularVelocity = maxAngularVelocity;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        lightBlade.SetActive(false);

        // Input Systemのセットアップ
        playerController = new PlayerController();
        playerController.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playerController.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        playerController.Player.VerticalMove.performed += ctx => verticalMoveInput = ctx.ReadValue<Vector2>();
        playerController.Player.VerticalMove.canceled += ctx => verticalMoveInput = Vector2.zero;

        playerController.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        playerController.Player.Look.canceled += ctx => lookInput = Vector2.zero;

        playerController.Player.Jet.performed += _ => jetActivated = true;
        playerController.Player.Jet.canceled += _ => jetActivated = false;

        playerController.Player.VerticalSlash.performed += _ => PerformSlash(-1);
        playerController.Player.SideSlash.performed += _ => PerformSlash(1);

        playerController.Enable();
    }

    void Update()
    {
        if (playerController.Player.Escape.triggered)
        {
            Application.Quit();
        }

        if (attackTime < 0.5f)
        {
            attackTime += Time.deltaTime;
        }
        else
        {
            lightBlade.SetActive(false);
        }

        RotatePlayer();
    }

    void FixedUpdate()
    {
        MoveFunc();
    }

    private void MoveFunc()
    {
        // 入力から移動ベクトルを生成（ローカル空間での移動）
        Vector3 forwardMovement = transform.forward * moveInput.y;  // 前後の移動
        Vector3 rightMovement = transform.right * moveInput.x;      // 横の移動
        Vector3 verticalMovement = new Vector3(0, verticalMoveInput.y, 0); // 垂直方向の移動

        // ベクトルを合わせて移動ベクトルを作成
        Vector3 movement = forwardMovement + rightMovement + verticalMovement;

        // ベクトルを正規化して速度を適用
        if (movement.magnitude != 0f)
        {
            movement.Normalize();
            movement *= speed;
        }

        // 最大速度制限を追加
        if (rb.linearVelocity.magnitude < maxVelocity)
        {
            rb.AddForce(movement, ForceMode.Force);
        }

        // ジェットを使用している場合、力を追加
        if (jetActivated)
        {
            rb.AddForce(movement * jetForce, ForceMode.Acceleration);
        }
    }

    private void RotatePlayer()
    {
        // 水平方向の回転（Y軸回り）
        float yawRotation = lookInput.x * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, yawRotation, 0);

        // 垂直方向の回転（X軸回り）
        currentPitch -= lookInput.y * rotationSpeed * Time.deltaTime;
        currentPitch = Mathf.Clamp(currentPitch, angularDegreeMin, angularDegreeMax);

        // プレイヤーの角度を設定（X軸の傾きを制御）
        Vector3 currentEuler = transform.eulerAngles;
        currentEuler.x = currentPitch; // 垂直角度を適用
        currentEuler.z = 0;           // プレイヤーを傾かせない
        transform.eulerAngles = currentEuler;
    }


    private void PerformSlash(int mode)
    {
        lightBlade.SetActive(true);

        if (mode == -1)
        {
            attackAnimator.SetTrigger("VerticalSlash");
        }
        else if (mode == 1)
        {
            attackAnimator.SetTrigger("SideSlash");
        }

        attackTime = 0f;
    }
}
