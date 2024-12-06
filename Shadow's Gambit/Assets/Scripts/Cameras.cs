using UnityEngine;

public class Cameras : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private float flipInterval = 2f; // Time in seconds between flips
    private float flipTimer;

    [Header("Detection Settings")]
    [SerializeField] private DetectionManager detectionManager; // Reference to the DetectionManager
    [SerializeField] private EnemyVisionCone2D visionCone; // Reference to the vision cone script
    private PlayerMovement playerMovement;

    void Start()
    {
        flipTimer = flipInterval;
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
        }
        else
        {
            Debug.LogError("Player GameObject not found!");
        }
        if (visionCone == null)
        {
            Debug.LogError("VisionCone script not assigned to the Guard!");
        }
        if (detectionManager == null)
        {
            Debug.LogError("DetectionManager not assigned to the Guard!");
        }
    }

    void Update()
    {
        bool isPlayerDetected = detectionManager != null && detectionManager.IsPlayerDetected;

        if (!isPlayerDetected || (playerMovement != null && playerMovement.IsHidden()))
        {
            Flip();
        }
        else
        {
            Debug.Log("Player detected! Camera is alert.");
        }
    }

    void Flip()
    {
        flipTimer -= Time.deltaTime;
        if (flipTimer <= 0f)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1; // Flip on the X-axis
            transform.localScale = scale;
            flipTimer = flipInterval;
        }
    }
}
