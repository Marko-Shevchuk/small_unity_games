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
    /*private float lookX;
    private float lookY;*/
    private float movementX;
    private float movementY;
    /*public float lookSpeed = 2f;*/


    public float moveSpeed = 30f;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float acceleration = movementX * moveSpeed;

        Vector3 force = transform.forward * acceleration;
        rb.AddForce(force);
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

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        move.Disable();
        move.performed -= OnMove;
        move.canceled -= OnMove; 
        

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Vector2 movementVector = context.ReadValue<Vector2>();

            movementX = -movementVector.x;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            movementX = 0;
        }
    }


    /* void OnLook(InputAction.CallbackContext context)
     {
         // Read the look value from the context.
         Vector2 lookVector = context.ReadValue<Vector2>();

         lookX = lookVector.x;
         lookY = lookVector.y;

         // Adjust the camera rotation based on look input
         float rotationX = mainCamera.transform.localEulerAngles.y + lookX * lookSpeed;
         float rotationY = mainCamera.transform.localEulerAngles.x - lookY * lookSpeed;
         mainCamera.transform.localEulerAngles = new Vector3(rotationY, rotationX, 0);
     }*/
}
