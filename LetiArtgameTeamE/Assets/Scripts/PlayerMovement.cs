using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    bool alive = true;

    public float speed = 5f;
    public float jumpForce = 7f;
    public Rigidbody rb;

    private int lane = 1; // 0 = left, 1 = middle, 2 = right
    public float laneDistance = 2.5f; // distance between lanes
    public float laneChangeSpeed = 10f; // how fast to slide between lanes

    bool isGrounded = true;

    void Update()
    {
        if (!alive) return;

        // forward movement
        Vector3 forwardMove = transform.forward * speed * Time.deltaTime;
        rb.MovePosition(rb.position + forwardMove);

        // lane switching
        if (Input.GetKeyDown(KeyCode.LeftArrow) && lane > 0)
            lane--;
        if (Input.GetKeyDown(KeyCode.RightArrow) && lane < 2)
            lane++;

        // jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            Jump();

        // move smoothly to target lane
        Vector3 targetPosition = new Vector3((lane - 1) * laneDistance, rb.position.y, rb.position.z);
        Vector3 moveTo = Vector3.MoveTowards(rb.position, targetPosition, laneChangeSpeed * Time.deltaTime);
        rb.MovePosition(new Vector3(moveTo.x, rb.position.y, rb.position.z + speed * Time.deltaTime));

        // fall check
        if (transform.position.y < -5)
            Die();
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

    public void Die()
    {
        alive = false;
        Invoke("Restart", 2);
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}