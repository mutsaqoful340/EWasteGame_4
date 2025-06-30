using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float acceleration = 20f;
    public float deceleration = 20f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

    [Header("Air")]
    public float airControl = 0.5f;
    public float airDrag = 0.1f;

    [Header("Slope Handling")]
    public float slopeForce = 6f;
    public float slopeRayLength = 1.5f;

    [Header("Look")]
    public Transform cam;
    public float mouseSensitivity = 2f;

    private CharacterController controller;
    private PlayerInputActions inputActions;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 moveVelocity;
    private float verticalVelocity;
    private float xRotation = 0f;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputActions = new PlayerInputActions();

        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += _ => moveInput = Vector2.zero;

        inputActions.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Look.canceled += _ => lookInput = Vector2.zero;

        inputActions.Player.Jump.performed += _ => Jump();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    void Update()
    {
        HandleLook();
        HandleMovement();
    }

    void HandleLook()
    {
        float mouseX = lookInput.x * mouseSensitivity;
        float mouseY = lookInput.y * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement()
    {
        Vector3 inputDir = transform.right * moveInput.x + transform.forward * moveInput.y;
        inputDir.Normalize();

        bool isGrounded = controller.isGrounded;

        if (isGrounded && verticalVelocity < 0)
            verticalVelocity = -2f;

        // Apply gravity
        verticalVelocity += gravity * Time.deltaTime;

        if (isGrounded)
        {
            Vector3 targetVelocity = inputDir * moveSpeed;
            moveVelocity = Vector3.MoveTowards(moveVelocity, targetVelocity, acceleration * Time.deltaTime);

            if (OnSlope() && inputDir != Vector3.zero)
                controller.Move(Vector3.down * slopeForce * Time.deltaTime);
        }
        else
        {
            // In air: apply limited control
            Vector3 airControlVelocity = inputDir * moveSpeed;
            moveVelocity = Vector3.MoveTowards(moveVelocity, airControlVelocity, airControl * acceleration * Time.deltaTime);

            // Apply air drag
            moveVelocity *= (1f - airDrag * Time.deltaTime);
        }

        // Final movement vector
        Vector3 velocity = moveVelocity + Vector3.up * verticalVelocity;
        controller.Move(velocity * Time.deltaTime);
    }

    void Jump()
    {
        if (controller.isGrounded)
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    bool OnSlope()
    {
        if (controller.isGrounded)
        {
            Ray ray = new Ray(transform.position, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, slopeRayLength))
            {
                return hit.normal != Vector3.up;
            }
        }
        return false;
    }
}
