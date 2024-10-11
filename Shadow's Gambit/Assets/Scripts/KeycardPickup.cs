using UnityEngine;

public class KeycardPickup : MonoBehaviour
{
    private bool hasBeenPickedUp = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasBeenPickedUp)
        {
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();
            if (inventory != null)
            {
                inventory.hasKeycard = true;  // Set player's inventory to indicate the keycard has been picked up
                hasBeenPickedUp = true;  // Prevents multiple pickups
                Debug.Log("Keycard picked up.");
                Destroy(gameObject);  // Remove the keycard object from the level
            }
        }
    }
}