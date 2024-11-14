using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LootableBehaviour : MonoBehaviour
{
    bool isActive = true;
    [SerializeField] int scoreValue = 2000;

    // Reference to the PlayerMovement script
    private PlayerMovement playerMovement;
    
    // Reference to the tilemap component
    private Tilemap tilemap;
    private Color color = new Color(0.5f, 0.5f, 0.5f);
    void Start()
    {
        // Find the Player GameObject and get the PlayerMovement script from it
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
        }

        // Get the tilemap component attached to the Lootable object
        tilemap = GetComponent<Tilemap>();
    }
    private void OnTriggerStay2D(Collider2D other) 
    {
        if (isActive && playerMovement != null && Input.GetKey(KeyCode.W)) // Ensure playerMovement is assigned, feels overcomplicated I need to compress it later
        {
            Debug.Log("Player is in Contact with Lootable Object"); // Remove this when ready to submit
            if (other.gameObject.CompareTag("Player"))
            {
                Debug.Log("Player has looted Lootable Object"); // Remove this when ready to submit
                isActive = false;
                playerMovement.playerScore += scoreValue;
                Debug.Log("Player score is: " + playerMovement.playerScore); // Remove this when ready to submit

                // Change the color of the object to red for testing purposes
                if (tilemap != null)
                {
                    tilemap.GetComponent<TilemapRenderer>().material.color = color;
                }
                // Change sprite
            }
        }
    }
}
