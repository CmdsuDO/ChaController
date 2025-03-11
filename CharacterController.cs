using UnityEngine;

public class FPSController : MonoBehaviour
{
    public CharacterController controller;
    public Transform playerCamera;
    
    public float moveSpeed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;
    public float mouseSensitivity = 2f;
    
    private float xRotation = 0f;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Move();
        Look();
        ApplyGravity();
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal"); // A/D
        float z = Input.GetAxis("Vertical");   // W/S

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void ApplyGravity()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0) velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
