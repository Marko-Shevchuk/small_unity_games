using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    public GameObject player;
    public float disappearTime = 5f;
    private bool isPlayerOnPlatform = false;
    private AudioSource audioSource;
    private float elapsedTime = 0f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("No AudioSource component found on the platform. Please add an AudioSource component.");
        }
    }

    void Update()
    {
        if (isPlayerOnPlatform)
        {
            elapsedTime += Time.deltaTime; 
            if (elapsedTime >= 1.1f && !audioSource.isPlaying)
            {
                audioSource.Play();
            }
            if (elapsedTime >= disappearTime)
            {
                

                gameObject.SetActive(false);
            }
        }
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerOnPlatform = true;
            elapsedTime = 0f;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerOnPlatform = false;
            elapsedTime = 0f;
        }
    }
}