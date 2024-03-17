using UnityEngine;
using System.Collections;

public class ProgressTracker : MonoBehaviour
{
    public bool invert = false;
    public GameObject player; 
    public float maxXPosition = -1f;
    public float lossDuration = 8f;
    public float condemnDuration = 5f;

    private float timeNotExceededMaxX = 0f; // Time the player has not exceeded maxXPosition
    private PlayerCollisionHandler playerCollisionHandler;

    void Start()
    {
        playerCollisionHandler = player.GetComponent<PlayerCollisionHandler>();
    }

    void Update()
    {
        // if the player's X position exceeds the maximum
        if (invert ? player.transform.position.x < maxXPosition : player.transform.position.x > maxXPosition)
        {
            playerCollisionHandler.DisableRedOverlay();
            timeNotExceededMaxX = 0f;
            maxXPosition = player.transform.position.x;
        }
        else
        {
           
            timeNotExceededMaxX += Time.deltaTime;

            if(timeNotExceededMaxX >= condemnDuration)
            {
                playerCollisionHandler.EnableRedOverlay();
            }
            if (timeNotExceededMaxX >= lossDuration)
            {
                playerCollisionHandler.TriggerLoss();
            }
        }
    }
}