using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class PlayerCollisionHandler : MonoBehaviour
{
    public float knockbackForce = 12f;
    public float strongKnockbackForce = 20f;
    public float knockbackDuration = 0.5f;
    public GameObject enemySpawner;
    public GameObject objectToRotate; // The world

    private CharacterController characterController;
    public AudioSource criticalHit;
    public AudioSource gravityShift;
    private PlayerController playerController;

    public GameObject winScreen;
    public GameObject lossScreen;
    private bool win = false;
    private bool loss = false;
    public AudioSource winSound;
    public AudioSource lossSound;

    void Start()
    {
        GameObject parentObject = transform.parent.gameObject;
        characterController = parentObject.GetComponent<CharacterController>();
        playerController = parentObject.GetComponent<PlayerController>(); // Get the PlayerController component

        if (criticalHit == null || gravityShift == null)
        {
            Debug.LogError("No AudioSource source found for the GameObject");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            Vector3 collisionPoint = other.ClosestPoint(transform.position);
            Vector3 knockbackDirection = (collisionPoint - transform.position).normalized;
            StartCoroutine(KnockbackPlayer(knockbackDirection));
        }
        if (other.gameObject.CompareTag("StrongObstacle"))
        {
            Vector3 collisionPoint = other.ClosestPoint(transform.position);
            Vector3 knockbackDirection = (collisionPoint - transform.position).normalized;
            StartCoroutine(StrongKnockbackPlayer(knockbackDirection));
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            Vector3 collisionPoint = other.ClosestPoint(transform.position);
            Vector3 knockbackDirection = (collisionPoint - transform.position).normalized;
            StartCoroutine(StrongKnockbackPlayer(knockbackDirection));

            StartCoroutine(HalvePlayerSpeed());
        }
        if (other.gameObject.CompareTag("Spawner"))
        {
            GameObject[] spawners = GameObject.FindGameObjectsWithTag("Spawner");
            foreach (GameObject spawner in spawners)
            {
                Destroy(spawner);
            }
            if (gravityShift != null && !gravityShift.isPlaying)
            {
                gravityShift.Play();
            }
            float originalGravity = playerController.gravity;
            playerController.gravity = -originalGravity;
            StartCoroutine(ResetGravity(originalGravity));
            enemySpawner.SetActive(false);
            if (objectToRotate != null)
            {
                StartCoroutine(RotateObject(objectToRotate, 180f, 1.3f));
            }
        }
        if (other.gameObject.CompareTag("Goal"))
        {
            if (!loss && !win)
            {
                win = true;
                winScreen.SetActive(true);
                winSound.Play();
                RestartSceneAfterDelay(4.5f);
            }
            
        }
        if (other.gameObject.CompareTag("Loss"))
        {
            //score 0
            if (!loss && !win)
            {
                loss = true;
                lossScreen.SetActive(true);
                lossSound.Play();
                RestartSceneAfterDelay(3f);
            }

        }
    }
    IEnumerator RestartSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
    }
    IEnumerator ResetGravity(float originalGravity)
    {
        yield return new WaitForSeconds(0.6f);
        playerController.gravity = originalGravity;
    }

    IEnumerator RotateObject(GameObject obj, float targetAngle, float duration)
    {
        Quaternion startRotation = obj.transform.rotation;
        Quaternion endRotation = Quaternion.Euler(targetAngle, 0, 0) * startRotation;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            obj.transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        obj.transform.rotation = endRotation; // Ensure the final rotation is exactly as intended
    }

    IEnumerator KnockbackPlayer(Vector3 knockbackDirection)
    {
        float elapsedTime = 0f;
        float totalDuration = knockbackDuration;

        while (elapsedTime < totalDuration)
        {
            float currentForce = knockbackForce * (1 - (elapsedTime / totalDuration) * (elapsedTime / totalDuration) * (elapsedTime / totalDuration));
            Vector3 force = -knockbackDirection * currentForce * Time.deltaTime;
            characterController.Move(force);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator StrongKnockbackPlayer(Vector3 knockbackDirection)
    {
        float elapsedTime = 0f;
        float totalDuration = knockbackDuration;

        while (elapsedTime < totalDuration)
        {
            float currentForce = strongKnockbackForce * (1 - (elapsedTime / totalDuration) * (elapsedTime / totalDuration) * (elapsedTime / totalDuration));
            Vector3 force = -knockbackDirection * currentForce * Time.deltaTime;
            characterController.Move(force);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator HalvePlayerSpeed()
    {
        PlayerController playerController = transform.parent.GetComponent<PlayerController>();

        playerController.speed = 2.5f;

        if (criticalHit != null && !criticalHit.isPlaying)
        {
            criticalHit.Play();
        }

        yield return new WaitForSeconds(5f);

        playerController.speed = 6f;
    }
}