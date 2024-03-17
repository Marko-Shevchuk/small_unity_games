using System.Collections;
using UnityEngine;

public class ObstacleKnockback : MonoBehaviour
{
    public float knockbackForce = 5f; 
    public float knockbackDuration = 0.5f; 

    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("e");
            CharacterController playerController = collision.gameObject.GetComponent<CharacterController>();

            if (playerController != null)
            {
                StartCoroutine(KnockbackPlayer(playerController));
            }
        }
    }

    IEnumerator KnockbackPlayer(CharacterController playerController)
    {

        Vector3 knockbackDirection = (transform.position - playerController.transform.position).normalized;

      
        Vector3 knockbackVelocity = knockbackDirection * knockbackForce;

       
        Vector3 originalPosition = playerController.transform.position;

        float elapsedTime = 0f;
        while (elapsedTime < knockbackDuration)
        {
            Vector3 newPosition = originalPosition + knockbackVelocity * elapsedTime;

            
            playerController.Move(newPosition - playerController.transform.position);

          
            elapsedTime += Time.deltaTime;

           
            yield return null;
        }

    }
}