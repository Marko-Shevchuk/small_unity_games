using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    public Camera mainCamera;

    public PlayerControls input;
    private InputAction move;
    private InputAction look;
    private float lookX;
    private float lookY;
    private float movementX;
    private float movementY;
    public float lookSpeed = 2f;


    public float speed = 3f;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
   
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
        move.canceled += OnMove;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        move.Disable();
        move.performed -= OnMove;
        move.canceled -= OnMove; // Unsubscribe from the 'canceled' event
        look.Disable();
        look.performed -= OnLook;
        look.canceled -= OnLook;

        // Reset cursor state
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
 

    void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            // Button was pressed, read the movement value from the context.
            Vector2 movementVector = context.ReadValue<Vector2>();

            movementX = movementVector.x;
            movementY = movementVector.y;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            // Button was released, reset the movement values.
            movementX = 0;
            movementY = 0;
        }
    }


    void OnLook(InputAction.CallbackContext context)
    {
        // Read the look value from the context.
        Vector2 lookVector = context.ReadValue<Vector2>();

        lookX = lookVector.x;
        lookY = lookVector.y;

        // Adjust the camera rotation based on look input
        float rotationX = mainCamera.transform.localEulerAngles.y + lookX * lookSpeed;
        float rotationY = mainCamera.transform.localEulerAngles.x - lookY * lookSpeed;
        mainCamera.transform.localEulerAngles = new Vector3(rotationY, rotationX, 0);
    }

    void FixedUpdate()
    {
        // Transform movement input relative to camera rotation
        Vector3 cameraForward = mainCamera.transform.forward;
        Vector3 cameraRight = mainCamera.transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 movement = cameraForward * movementY + cameraRight * movementX;
        movement.Normalize();

        rb.AddForce(movement * speed);
        // Double constant gravity force.
        rb.AddForce(Physics.gravity * rb.mass);
    }
}
