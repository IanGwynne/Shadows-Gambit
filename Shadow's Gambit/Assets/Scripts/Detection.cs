using System.Collections;
using UnityEngine;

public class Detection : MonoBehaviour
{
    [SerializeField] private DetectionManager detectionManager; // Reference to DetectionManager

    private PlayerMovement playerMovement;

    void Start()
    {
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
        }
        else
        {
            Debug.LogError("Player GameObject not found!");
        }
        if (detectionManager == null)
        {
            Debug.LogError("DetectionManager not assigned to Detection script!");
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Detection"))
        {
            if (playerMovement != null && !playerMovement.IsHidden())
            {
                detectionManager.SetPlayerDetected(true);
            }
            else
            {
               detectionManager.SetPlayerDetected(false); 
            }
        }
    }
}