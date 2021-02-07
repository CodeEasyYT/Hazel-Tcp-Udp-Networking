using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerManager manager;

    public Rigidbody rb;
    public float moveSpeed = 6f;
    public float jumpForce = 5f;
    public LayerMask groundCheck;
    public bool grounded;

    public void Initialize(PlayerManager manager)
    {
        this.manager = manager;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (!rb.useGravity)
            rb.useGravity = true;

        float x = Input.GetAxisRaw("Horizontal") * moveSpeed;
        float y = Input.GetAxisRaw("Vertical") * moveSpeed;

        grounded = Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y - 1.5f, transform.position.z), 0.25f, groundCheck);

        if (Input.GetKeyDown(KeyCode.Space) && grounded)
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);

        Vector3 movePos = transform.right * x + transform.forward * y;
        Vector3 newMovePos = new Vector3(movePos.x, rb.velocity.y, movePos.z);

        rb.velocity = newMovePos;
    }
}