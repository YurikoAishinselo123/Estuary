using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float sprintSpeed = 6f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float pitchClamp = 80f;

    [Header("Camera")]
    [SerializeField] private Transform cameraTransform;

    private CharacterController characterController;
    private Vector3 moveDirection;
    private Vector3 velocity;
    private float verticalLookRotation = 0f;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
    }

    private void Update()
    {
        if (!GameStateManager.Instance.IsGameplay)
            return;

        HandleLook();
        HandleMovement();
        HandleJump();
        ApplyGravity();
        MoveCharacter();
    }

    private void HandleLook()
    {
        Vector2 lookInput = InputManager.Instance.LookInput * mouseSensitivity;

        verticalLookRotation -= lookInput.y;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -pitchClamp, pitchClamp);
        cameraTransform.localRotation = Quaternion.Euler(verticalLookRotation, 0f, 0f);

        transform.Rotate(Vector3.up * lookInput.x);
    }

    private void HandleMovement()
    {
        Vector2 moveInput = InputManager.Instance.MoveInput;
        float speed = InputManager.Instance.IsSprinting ? sprintSpeed : walkSpeed;

        moveDirection = (transform.right * moveInput.x + transform.forward * moveInput.y).normalized * speed;
    }

    private void HandleJump()
    {
        if (!characterController.isGrounded)
            return;

        velocity.y = -2f; // Keeps the player grounded

        if (InputManager.Instance.SpaceKey)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
    }

    private void MoveCharacter()
    {
        Vector3 finalMove = moveDirection;
        finalMove.y = velocity.y;

        characterController.Move(finalMove * Time.deltaTime);
    }
}
