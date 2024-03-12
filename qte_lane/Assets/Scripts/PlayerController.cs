using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
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
    public float speed = 5f;
    public float jumpPower = 1f;
    public float gravity = 9.81f; 

    CharacterController characterController;

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
            // Apply jump force
            Vector3 jumpDirection = new Vector3(0, jumpPower, 0);
            characterController.Move(jumpDirection);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            
        }
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
        movement.Normalize();

       
            // Adjust for gravity
        movement.y -= gravity * Time.deltaTime;
        

        // Apply movement using CharacterController
        characterController.Move(movement * speed * Time.deltaTime);
    }
}