using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    public GameObject gameOverImage;
    public AudioSource enemyKill;
    public AudioSource victorySound;
    public AudioSource greenCubeEat;
    public AudioSource enemyAmbiance;
    public AudioSource exitActivate;
    private Rigidbody rb;

    private int count;
    private float movementX;
    private float movementY;
    private float lookX;
    private float lookY;
    public float speed = 0;
    public TextMeshProUGUI countText;
    public GameObject winGoal;
    public GameObject winTextObject;
    public Camera mainCamera; 
    public float lookSpeed = 2f;

    public InputActions input;
    private InputAction move;
    private InputAction look;

    private void Awake()
    {
        input = new InputActions();
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

        // Lock the cursor
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


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winGoal.SetActive(false);
        winTextObject.SetActive(false);
        gameOverImage.SetActive(false);
    }
    void OnMove(InputAction.CallbackContext context)
    {
        // Check the phase of the input action
        if (context.phase == InputActionPhase.Performed)
        {
            // Button was pressed, read the movement value from the context.
            Vector2 movementVector = context.ReadValue<Vector2>();

            // Store the X and Y components of the movement.
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

        // Store the X and Y components of the look.
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
        movement.Normalize(); // Normalize to prevent faster diagonal movement

        rb.AddForce(movement * speed);
        // Add constant gravity force.
        rb.AddForce(Physics.gravity * rb.mass);
    }


    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("PickUp"))
        {

            other.gameObject.SetActive(false);
            if ((greenCubeEat != null) && (!greenCubeEat.isPlaying))
                greenCubeEat.Play();
            count = count + 1;

            SetCountText();
        }
        else if (other.gameObject.CompareTag("LethalEnemy"))
        {
            
            if ((enemyKill != null) && (!enemyKill.isPlaying))
                enemyKill.Play();
            winGoal.SetActive(false);
            winTextObject.SetActive(false);
            gameOverImage.SetActive(true);
            StartCoroutine(ShowGameOverImage());
            

        }
        else if (other.gameObject.CompareTag("VictoryGoal"))
        {
            
            if ((victorySound != null) && (!victorySound.isPlaying))
                victorySound.Play();
            winTextObject.SetActive(true);
        }
    }
    System.Collections.IEnumerator ShowGameOverImage()
    {

        
        yield return new WaitForSeconds(3f);
        
        ResetLevel();
    }
    void ResetLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        
        count = 0;

        SetCountText();
    }
    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();

        if (count >= 8)
        {
            // ENABLE EXIT
            if ((exitActivate != null) && (!exitActivate.isPlaying))
                exitActivate.Play();
            winGoal.SetActive(true);
        }
    }
    
}
