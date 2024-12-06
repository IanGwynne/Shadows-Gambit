using UnityEngine;

public class Guard : MonoBehaviour
{
    [Header("Patrol Settings")]
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float leftBoundary = -5.0f;
    [SerializeField] private float rightBoundary = 5.0f;
    [SerializeField] private bool movingRight = true;

    [Header("Detection Settings")]
    [SerializeField] private DetectionManager detectionManager; // Reference to the DetectionManager
    [SerializeField] private EnemyVisionCone2D visionCone; // Reference to the vision cone script
    private PlayerMovement playerMovement;

    void Start()
    {
        // Find the Player GameObject and get its movement script
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
        }
        else
        {
            Debug.LogError("Player GameObject not found!");
        }

        // Ensure the vision cone is assigned
        if (visionCone == null)
        {
            Debug.LogError("VisionCone script not assigned to the Guard!");
        }

        // Ensure DetectionManager is assigned
        if (detectionManager == null)
        {
            Debug.LogError("DetectionManager not assigned to the Guard!");
        }
    }

    void Update()
    {
        // Check the detection state using DetectionManager's property
        bool isPlayerDetected = detectionManager != null && detectionManager.IsPlayerDetected;

        if (!isPlayerDetected || (playerMovement != null && playerMovement.IsHidden()))
        {
            Patrol();
        }
        else
        {
            Debug.Log("Player detected! Guard is alert.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Instantly set detection to max when the player enters the guard's trigger
            if (detectionManager != null && !playerMovement.IsHidden())
            {
                detectionManager.detectionCount = detectionManager.gameOverAmount;
                Debug.Log("Player entered the guard's trigger area! Detection maxed out.");
            }
        }
    }

    void Patrol()
    {
        if (movingRight)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            if (transform.position.x >= rightBoundary)
            {
                movingRight = false;
                Flip();
            }
        }
        else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
            if (transform.position.x <= leftBoundary)
            {
                movingRight = true;
                Flip();
            }
        }
    }

    void Flip()
    {
        // Flip the guard's direction
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}