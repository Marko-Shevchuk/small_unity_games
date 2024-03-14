using UnityEngine;
using System.Collections;

public class PlayerCollisionHandler : MonoBehaviour
{
    public float knockbackForce = 12f;
    public float strongKnockbackForce = 20f;
    public float knockbackDuration = 0.5f;

    private CharacterController characterController;
    private AudioSource audioSource; 

    void Start()
    {
        GameObject parentObject = transform.parent.gameObject;
        characterController = parentObject.GetComponent<CharacterController>();

        PlayerController playerController = parentObject.GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("No AudioSource component found on the GameObject. Please add an AudioSource component.");
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
            Vector3 collisionPoint = other.ClosestPoint(transform.position);
            Vector3 knockbackDirection = (collisionPoint - transform.position).normalized;
            StartCoroutine(KnockbackPlayer(knockbackDirection));
            StartCoroutine(HalvePlayerSpeed());
            //
        }
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

 
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }

        yield return new WaitForSeconds(5f);

        playerController.speed = 5f;
    }
}