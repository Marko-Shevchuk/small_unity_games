using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public Camera mainCamera;

    public PlayerControls input;
    private InputAction move;
    private InputAction look;
    private InputAction jump; 
    private float lookX;
    private float lookY;
    private float movementX;
    private float movementY;
    public float lookSpeed = 0.15f;
    public float speed = 6f;
    public float jumpPower = 10f;
    public float gravity = 0.981f;
    private Vector3 gravityForce = Vector3.zero;
    CharacterController characterController;

    private bool jumping = false;
    private float jumpTime = 0f;
    private const float jumpDuration = 0.75f;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Awake()
    {
        input = new PlayerControls();
        mainCamera = Camera.main; // Assigning main camera.
    }

    private void OnEnable()
    {
        move = input.Player.Move;
        move.Enable();
        move.performed += OnMove;
        move.canceled += OnMove;
        look = input.Player.Look;
        look.Enable();
        look.performed += OnLook;
        jump = input.Player.Jump; 
        jump.Enable();
        jump.performed += OnJump;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        move.Disable();
        move.performed -= OnMove;
        move.canceled -= OnMove;
        look.Disable();
        look.performed -= OnLook;
        jump.Disable();
        jump.performed -= OnJump;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Vector2 movementVector = context.ReadValue<Vector2>();
            movementX = movementVector.x;
            movementY = movementVector.y;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            movementX = 0;
            movementY = 0;
        }
    }

    void OnLook(InputAction.CallbackContext context)
    {
        Vector2 lookVector = context.ReadValue<Vector2>();
        lookX = lookVector.x;
        lookY = lookVector.y;

        float rotationX = mainCamera.transform.localEulerAngles.y + lookX * lookSpeed;
        float rotationY = mainCamera.transform.localEulerAngles.x - lookY * lookSpeed;
        mainCamera.transform.localEulerAngles = new Vector3(rotationY, rotationX, 0);
    }

    void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && characterController.isGrounded)
        {
            StartCoroutine(JumpCoroutine());
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            StopCoroutine(JumpCoroutine());
            jumping = false;
        }
    }

    IEnumerator JumpCoroutine()
    {
        jumping = true;
        jumpTime = 0f;

        while (jumpTime < jumpDuration)
        {
            jumpTime += Time.deltaTime;
            yield return null;
        }

        jumping = false;
    }


    void Update()
    {
        Vector3 cameraForward = mainCamera.transform.forward;
        Vector3 cameraRight = mainCamera.transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 movement = cameraForward * movementY + cameraRight * movementX;
        Vector3 movementJump = Vector3.zero;
        
        if (characterController.isGrounded)
        {
            gravityForce = Vector3.zero;
        }
        if (jumping)
        {
            float jumpProgress = jumpTime / jumpDuration;
            movementJump.y += jumpPower * (1 - jumpProgress * jumpProgress - jumpProgress * jumpProgress * jumpProgress); 
        }
        gravityForce.y -= gravity * Time.deltaTime;
        movement.Normalize();

        characterController.Move((movement * speed * Time.deltaTime) + (movementJump) + gravityForce);
    }
}