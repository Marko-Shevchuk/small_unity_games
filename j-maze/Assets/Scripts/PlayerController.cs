using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using System.Linq;
using System;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameObject gameOverImage;
    public AudioSource enemyKill;
    public AudioSource victorySound;
    public AudioSource greenCubeEat;
    public AudioSource enemyAmbiance;
    public AudioSource exitActivate;
    public AudioSource alertSound;
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
    public float lookSpeed = 0.2f;

    public InputActions input;
    private InputAction move;
    private InputAction look;
    private Transform playerTransform;
    public GameObject cautionOverlay;
    void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("LethalEnemy");
        Transform[] enemyTransforms = Array.ConvertAll(enemies, enemy => enemy.transform);

        float smallestDistance = GetSmallestDistanceToEnemy(playerTransform, enemyTransforms);

        if (smallestDistance != float.MaxValue)
        {
            ApplyCameraEffect(Camera.main, smallestDistance);
            UpdateCautionOverlay(smallestDistance);

            // Play the alert sound if the enemy is less than  4 units away
            if (smallestDistance < 4f && (!alertSound.isPlaying) && (alertSound!=null))
            {
                alertSound.Play();
            }
        }
        else
        {
            Camera.main.fieldOfView = 90f;
            // Reset the overlay to fully transparent when no enemy is close
            RawImage rawImage = cautionOverlay.GetComponent<RawImage>();
            rawImage.color = new Color(rawImage.color.r, rawImage.color.g, rawImage.color.b, 0f);
        }
    }
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
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winGoal.SetActive(false);
        winTextObject.SetActive(false);
        gameOverImage.SetActive(false);
        
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
            cautionOverlay.SetActive(false);
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
    private float GetSmallestDistanceToEnemy(Transform player, Transform[] enemies)
    {
        float smallestDistance = float.MaxValue;

        foreach (Transform enemy in enemies)
        {
            Vector3 directionToEnemy = (enemy.position - player.position).normalized;
            float distanceToEnemy = Vector3.Distance(player.position, enemy.position);

            RaycastHit hit;
            if (Physics.Raycast(player.position, directionToEnemy, out hit, distanceToEnemy))
            {
                if (hit.transform == enemy)
                {
                    // Update the smallest distance if the current enemy is closer
                    smallestDistance = Mathf.Min(smallestDistance, distanceToEnemy);
                }
            }
        }

        if (smallestDistance == float.MaxValue)
        {
            return float.MaxValue;
        }

        return smallestDistance;
    }
    private void ApplyCameraEffect(Camera camera, float distanceToEnemy)
    {
        float maxFOV = 50f;
        float baseFOV = 90f;

        // FOV to be maxFOV when the enemy is 5 or more units away,
        // and the FOV should gradually decreases as the enemy gets closer to 1 unit away.
        float fovChange = Mathf.Clamp01((5f - distanceToEnemy) / (5f - 1f));

        float newFOV = Mathf.Lerp(baseFOV, maxFOV, fovChange);

        camera.fieldOfView = newFOV;
    }
    private void UpdateCautionOverlay(float distanceToEnemy)
    {

        RawImage rawImage = cautionOverlay.GetComponent<RawImage>();
        
        float opacity = Mathf.Clamp01((5f - distanceToEnemy) / (5f - 1f));

        rawImage.color = new Color(rawImage.color.r, rawImage.color.g, rawImage.color.b, opacity);
    }

}
