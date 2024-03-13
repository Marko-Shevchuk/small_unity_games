using UnityEngine;
using System.Collections;

public class PlayerCollisionHandler : MonoBehaviour
{
    public float knockbackForce = 12f;
    public float knockbackDuration = 0.5f;

    private CharacterController characterController;

    void Start()
    {
        GameObject parentObject = transform.parent.gameObject;
        characterController = parentObject.GetComponent<CharacterController>();
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Obstacle"))
        {

            // Estimate
            Vector3 collisionPoint = other.ClosestPoint(transform.position);
            Vector3 knockbackDirection = (collisionPoint - transform.position).normalized;
            StartCoroutine(KnockbackPlayer(knockbackDirection));
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
}