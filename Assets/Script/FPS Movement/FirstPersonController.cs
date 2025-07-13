using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("References")]
    public Transform cameraTransform;

    [Header("Movement")]
    public float walkSpeed = 4f;
    public float runSpeed = 8f;
    public float crouchSpeed = 2f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;
    [Range(0f, 1f)] public float airControlFactor = 0.2f; // 0 = no air control, 1 = full

    [Header("Crouch Settings")]
    public float standingHeight = 2f;   // Match your controller's default
    public float crouchingHeight = 1f;  // Shorter height
    public float crouchCameraOffset = -0.5f; // How much lower the camera goes

    [Header("Look")]
    public float mouseSensitivity = 1f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private CharacterController controller;
    private PlayerControls inputActions;

    private Vector2 moveInput;
    private Vector2 lookInput;

    private Vector3 horizontalVelocity; // keeps your momentum
    private Vector3 verticalVelocity;   // handles jumping & gravity
    private Vector3 originalCameraLocalPos;

    private bool isGrounded;
    private bool isRunning = false;
    private bool isCrouching = false;

    private float xRotation = 0f;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        originalCameraLocalPos = cameraTransform.localPosition;

        inputActions = new PlayerControls();

        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        inputActions.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Look.canceled += ctx => lookInput = Vector2.zero;

        inputActions.Player.Run.performed += ctx => isRunning = true;
        inputActions.Player.Run.canceled += ctx => isRunning = false;

        inputActions.Player.Crouch.performed += ctx => isCrouching = !isCrouching;

        inputActions.Player.Jump.performed += ctx => Jump();
    }

    void OnEnable()
    {
        inputActions.Enable();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    void Update()
    {
        // Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && verticalVelocity.y < 0)
        {
            verticalVelocity.y = gravity * Time.deltaTime; // keeps grounded
        }

        // Calculate desired speed
        float targetSpeed = isCrouching ? crouchSpeed : (isRunning ? runSpeed : walkSpeed);
        Vector3 inputDirection = (transform.right * moveInput.x + transform.forward * moveInput.y).normalized;

        if (isGrounded)
        {
            // Full control on ground
            horizontalVelocity = inputDirection * targetSpeed;
        }
        else
        {
            // Limited air control
            Vector3 desiredVelocity = inputDirection * targetSpeed;
            horizontalVelocity = Vector3.Lerp(horizontalVelocity, desiredVelocity, airControlFactor * Time.deltaTime * 5f);
        }

        HandleCrouch();

        // Apply horizontal movement
        controller.Move(horizontalVelocity * Time.deltaTime);

        // Gravity & vertical movement
        verticalVelocity.y += gravity * Time.deltaTime;
        controller.Move(verticalVelocity * Time.deltaTime);

        // Look rotation
        float mouseX = lookInput.x * mouseSensitivity;
        float mouseY = lookInput.y * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void Jump()
    {
        if (isGrounded)
        {
            verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
        }
    }


    void HandleCrouch()
    {
        if (isCrouching)
        {
            controller.height = crouchingHeight;
            controller.center = new Vector3(0, crouchingHeight / 2f, 0);

            cameraTransform.localPosition = new Vector3(
               originalCameraLocalPos.x,
                originalCameraLocalPos.y + crouchCameraOffset,
               originalCameraLocalPos.z
            );
        }
        else
        {
            controller.height = standingHeight;
            controller.center = new Vector3(0, standingHeight / 2f, 0);

            cameraTransform.localPosition = originalCameraLocalPos;
        }
    }


}
