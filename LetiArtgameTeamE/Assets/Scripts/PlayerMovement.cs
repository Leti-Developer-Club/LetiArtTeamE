using UnityEngine;
using UnityEngine.SceneManagement;

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

    [Header("References")]
    public Rigidbody rb;
    private CapsuleCollider playerCollider;

    private int lane = 1; 
    private bool isGrounded = true;

    void Start()
    {
        playerCollider = GetComponent<CapsuleCollider>();
        originalColliderHeight = playerCollider.height;
        originalColliderCenter = playerCollider.center;
    }

    void Update()
    {
        if (!alive) return;

        MoveForward();
        HandleLaneSwitching();
        HandleJump();
        HandleSlide();
        CheckFall();
    }

    void MoveForward()
    {
        Vector3 forwardMove = transform.forward * speed * Time.deltaTime;
        rb.MovePosition(rb.position + forwardMove);

        // Smoothly move between lanes
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

    System.Collections.IEnumerator Slide()
    {
        isSliding = true;

        // Shrink collider to simulate ducking
        playerCollider.height = slideHeight;
        playerCollider.center = new Vector3(originalColliderCenter.x, slideHeight / 2f, originalColliderCenter.z);

        yield return new WaitForSeconds(slideDuration);

        // Reset collider
        playerCollider.height = originalColliderHeight;
        playerCollider.center = originalColliderCenter;

        isSliding = false;
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    void CheckFall()
    {
        if (transform.position.y < -5)
            Die();
    }

    public void Die()
    {
        alive = false;
        Invoke(nameof(Restart), 2);
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
