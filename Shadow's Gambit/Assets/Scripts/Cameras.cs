using UnityEngine;

public class Cameras : MonoBehaviour
{
    [SerializeField] private float flipInterval = 2f; // Time in seconds between flips
    private bool isPlayerDetected = false;
    private float flipTimer;
    private PlayerMovement playerMovement;

    void Start()
    {
        flipTimer = flipInterval;

        // Find the GameObject with the PlayerMovement script
        GameObject player = GameObject.Find("PlayerForTesting");

        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
        }
        else
        {
            Debug.LogError("Player GameObject not found!");
        }
    }

    void Update()
    {
        if (!isPlayerDetected || playerMovement.IsHidden())
        {
            flipTimer -= Time.deltaTime;
            if (flipTimer <= 0f)
            {
                FlipDirection();
                flipTimer = flipInterval;
            }
        }
    }

    private void FlipDirection()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1; // Flip on the X-axis
        transform.localScale = scale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerDetected = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerDetected = false;
        }
    }
}
