using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

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

    public float speed = 0;

    public TextMeshProUGUI countText;

    public GameObject winGoal;
    public GameObject winTextObject;

    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winGoal.SetActive(false);
        winTextObject.SetActive(false);
        gameOverImage.SetActive(false);
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        // Store the X and Y components of the movement.
        movementX = movementVector.x;
        movementY = movementVector.y;
    }


    private void FixedUpdate()
    {

        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        rb.AddForce(movement * speed);
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
