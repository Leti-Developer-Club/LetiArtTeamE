using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    bool alive = true;

    [Header("Movement Settings")]
    public float speed = 5f;
    public float jumpForce = 7f;
    public float laneDistance = 2.5f;
    public float laneChangeSpeed = 10f;

    [Header("Sliding Settings")]
    public float slideDuration = 1f;
    public float slideHeight = 0.5f;
    private bool isSliding = false;
    private float originalColliderHeight;
    private Vector3 originalColliderCenter;

    [Header("Invincibility Settings")]
    public bool isInvincible = false;
    public float invincibleDuration = 7f;
    private Renderer playerRenderer;
    private Color originalColor;

    [Header("References")]
    public Rigidbody rb;
    private CapsuleCollider playerCollider;
    private Animator animator;

    private int lane = 1;
    private bool isGrounded = true;
    private bool isJumping = false;

    [Header("Skip Settings")]
    public float skipJumpMultiplier = 1.5f;

    [Header("Start Animation Settings")]
    public float walkTime = 3f;       // How long walking plays before running starts
    private bool startRunEnabled = false;

    void Start()
    {
        playerCollider = GetComponent<CapsuleCollider>();
        playerRenderer = GetComponentInChildren<Renderer>();
        animator = GetComponent<Animator>();

        originalColliderHeight = playerCollider.height;
        originalColliderCenter = playerCollider.center;

        if (playerRenderer != null)
            originalColor = playerRenderer.material.color;

        // Start walking animation
        animator.SetBool("isRunning", false);
        animator.SetBool("isWalking", true);

        // Start delayed running
        StartCoroutine(StartRunningAfterDelay());
    }

    IEnumerator StartRunningAfterDelay()
    {
        yield return new WaitForSeconds(walkTime);

        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", true);
        startRunEnabled = true;
    }

    void Update()
    {
        if (!alive) return;

        MoveForward();
        HandleLaneSwitching();
        HandleJump();
        HandleSlide();
        CheckFall();

        // Only update running after the walking period is done
        if (startRunEnabled)
        {
            animator.SetBool("isRunning", isGrounded && !isSliding);
        }

        animator.SetBool("isJumping", !isGrounded);
    }

    void MoveForward()
    {
        Vector3 forwardMove = transform.forward * speed * Time.deltaTime;
        rb.MovePosition(rb.position + forwardMove);

        Vector3 targetPosition = new Vector3((lane - 1) * laneDistance, rb.position.y, rb.position.z);
        Vector3 moveTo = Vector3.MoveTowards(rb.position, targetPosition, laneChangeSpeed * Time.deltaTime);
        rb.MovePosition(new Vector3(moveTo.x, rb.position.y, rb.position.z + speed * Time.deltaTime));
    }

    void HandleLaneSwitching()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (lane > 0) lane--;
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (lane < 2) lane++;
        }
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isSliding)
        {
            Jump();
        }
    }

    void HandleSlide()
    {
        if (!isSliding && (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && isGrounded)
        {
            StartCoroutine(Slide());
        }
    }

    IEnumerator Slide()
    {
        isSliding = true;

        playerCollider.height = slideHeight;
        playerCollider.center = new Vector3(originalColliderCenter.x, slideHeight / 2f, originalColliderCenter.z);

        animator.SetTrigger("Slide");

        yield return new WaitForSeconds(slideDuration);

        playerCollider.height = originalColliderHeight;
        playerCollider.center = originalColliderCenter;

        isSliding = false;
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
        isJumping = true;

        animator.ResetTrigger("Land");
        animator.SetTrigger("Jump");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isGrounded)
        {
            animator.SetTrigger("Land");
        }

        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platform"))
        {
            isGrounded = true;
            isJumping = false;
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            if (isInvincible)
            {
                Destroy(collision.gameObject);
            }
            else
            {
                Die();
            }
        }
    }

    void CheckFall()
    {
        if (transform.position.y < -5)
            Die();
    }

    public void Die()
    {
        if (!alive) return;

        alive = false;
        animator.SetTrigger("Die");

        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;

        PlayerPrefs.SetString("LastLevel", SceneManager.GetActiveScene().name);

        Invoke(nameof(LoadGameOverScene), 2f);
    }

    void LoadGameOverScene()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void ActivateInvincibility()
    {
        if (!isInvincible)
            StartCoroutine(InvincibilityRoutine());
    }

    IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;
        float timer = 0f;

        while (timer < invincibleDuration)
        {
            if (playerRenderer != null)
                playerRenderer.material.color = Color.Lerp(originalColor, Color.yellow, Mathf.PingPong(Time.time * 5f, 1));

            timer += Time.deltaTime;
            yield return null;
        }

        if (playerRenderer != null)
            playerRenderer.material.color = originalColor;

        isInvincible = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Skip"))
        {
            Destroy(other.gameObject);
            StartCoroutine(SkipJumpDelayed());
        }
    }

    private IEnumerator SkipJumpDelayed()
    {
        yield return new WaitForSeconds(0.3f);

        isGrounded = false;
        isJumping = true;

        animator.ResetTrigger("Land");
        animator.SetBool("isRunning", false);
        animator.SetTrigger("Jump");

        yield return null;

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpForce * skipJumpMultiplier, ForceMode.Impulse);
    }
}
