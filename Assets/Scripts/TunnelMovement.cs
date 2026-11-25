using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TunnelMovement : MonoBehaviour
{
    [Header("Настройки скорости")]
    public float forwardSpeed = 15f;
    public float rotationSpeed = 150f;
    public float speedIncreaseRate = 0.2f;

    [Header("Настройки прыжка")]
    public Transform playerModel;
    public float jumpForce = 12f;
    public float gravity = -30f;

    [Header("Настройки рывка")]
    public float dashSpeedMultiplier = 2.5f;
    public float dashDuration = 1f;
    public int dashCost = 32;

    [Header("Настройки уровня")]
    public float winDistance = 4500f;
    public GameManager gameManager;
    public PlayerCollision playerCollision;
    public TextMeshProUGUI progressText;
    public Image progressBarFill;

    [Header("Визуал")]
    public MeshRenderer playerRenderer;
    public Material normalMaterial;
    public Material dashMaterial;

    [HideInInspector] public bool isDashing = false;
    private bool isFinished = false;

    private float currentY;
    private float baseY;
    private float verticalVelocity = 0f;
    private bool isJumping = false;

    private float originalSpeed;
    private float dashTimer = 0f;

    private float startZ;

    private bool isMovingLeft = false;
    private bool isMovingRight = false;

    void Start()
    {
        baseY = playerModel.localPosition.y;
        currentY = baseY;
        originalSpeed = forwardSpeed;
        startZ = transform.position.z;

        if (playerRenderer != null && normalMaterial != null) playerRenderer.material = normalMaterial;
    }

    void Update()
    {
        if (isFinished) return;

        float distanceCovered = transform.position.z - startZ;
        float totalDistance = winDistance - startZ;
        float progressNormalized = Mathf.Clamp01(distanceCovered / totalDistance);

        if (progressText != null)
        {
            progressText.text = Mathf.FloorToInt(progressNormalized * 100f) + "%";
        }

        if (progressBarFill != null)
        {
            progressBarFill.fillAmount = progressNormalized;
        }

        if (transform.position.z >= winDistance)
        {
            isFinished = true;
            if (playerCollision != null && playerCollision.HasEnoughData())
            {
                if (gameManager != null) gameManager.ShowVictory();
            }
            else
            {
                if (gameManager != null) gameManager.ShowGameOver("ВЗЛОМ ПРОВАЛЕН: НЕДОСТАТОЧНО ДАННЫХ!");
            }
            return;
        }

        originalSpeed += speedIncreaseRate * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftShift)) TriggerDash();

        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0) EndDash();
        }
        else
        {
            forwardSpeed = originalSpeed;
        }

        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime, Space.World);

        float horizontalInput = 0f;
        if (isMovingLeft) horizontalInput = -1f;
        if (isMovingRight) horizontalInput = 1f;

        if (horizontalInput == 0f)
        {
            horizontalInput = Input.GetAxis("Horizontal");
        }

        transform.Rotate(Vector3.forward, horizontalInput * rotationSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space)) TriggerJump();

        if (isJumping)
        {
            verticalVelocity += gravity * Time.deltaTime;
            currentY += verticalVelocity * Time.deltaTime;
            if (currentY <= baseY)
            {
                currentY = baseY;
                isJumping = false;
                verticalVelocity = 0f;
            }
            playerModel.localPosition = new Vector3(playerModel.localPosition.x, currentY, playerModel.localPosition.z);
        }
    }

    public void MoveLeftDown() { isMovingLeft = true; }
    public void MoveLeftUp() { isMovingLeft = false; }
    public void MoveRightDown() { isMovingRight = true; }
    public void MoveRightUp() { isMovingRight = false; }
    public void TriggerJump()
    {
        if (!isJumping && !isFinished)
        {
            verticalVelocity = jumpForce;
            isJumping = true;
        }
    }

    public void TriggerDash()
    {
        if (!isDashing && !isFinished && playerCollision != null && playerCollision.TrySpendData(dashCost))
        {
            isDashing = true;
            forwardSpeed = originalSpeed * dashSpeedMultiplier;
            dashTimer = dashDuration;
            if (playerRenderer != null && dashMaterial != null) playerRenderer.material = dashMaterial;
        }
    }

    void EndDash()
    {
        isDashing = false;
        forwardSpeed = originalSpeed;
        if (playerRenderer != null && normalMaterial != null) playerRenderer.material = normalMaterial;
    }
}