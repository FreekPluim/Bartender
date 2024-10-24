using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Rigidbody rb;
    [SerializeField] Camera cam;
    [SerializeField] Transform head;

    [Header("BodyMovement")]
    [SerializeField] float Speed;
    [SerializeField] float JumpHeight = 5;

    [Header("Head movement")]
    [SerializeField] float Sensetivity;
    [SerializeField] float lookXLimit;

    bool isGrounded;
    private Vector3 input;
    private float rotX;
    private Vector3 forward;
    private Vector3 wishDirection;

    public void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (cam == null) cam = GetComponentInChildren<Camera>();
        cam.gameObject.SetActive(true);
    }


    private void Update()
    {
        if (!GameStateManager.Instance.paused)
        {
            isGrounded = Physics.Raycast(transform.position + Vector3.up, Vector3.down, 1.1f);
            HeadMovement();
            Movement();
            Jump();
        }
    }

    private void HeadMovement()
    {
        rotX += -Input.GetAxis("Mouse Y") * Sensetivity;
        rotX = Mathf.Clamp(rotX, -lookXLimit, lookXLimit);

        head.localRotation = Quaternion.Euler(rotX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * Sensetivity, 0);
    }

    private void Movement()
    {
        input.x = Input.GetAxisRaw("Vertical");
        input.z = Input.GetAxisRaw("Horizontal");
        input = input.normalized * Speed;

        forward = new Vector3(-cam.transform.right.z, 0f, cam.transform.right.x);
        wishDirection = (forward * input.x + cam.transform.right * input.z + Vector3.up * rb.velocity.y);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, JumpHeight, rb.velocity.z);
        }
    }

    private void FixedUpdate()
    {
        if (!GameStateManager.Instance.paused)
        {
            rb.AddForce(wishDirection, ForceMode.Force);
        }
    }
}
