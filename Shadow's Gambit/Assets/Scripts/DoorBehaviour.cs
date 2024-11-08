using UnityEngine;
using System.Collections;

public class DoorBehaviour : MonoBehaviour
{
    public DoorBehaviour linkedDoor; // Reference to the corresponding door
    public float cooldownDuration = 1f; // Time before the player can teleport again
    [SerializeField] Vector2 teleportOffset = new Vector2(0f, -0.85f);
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();

            if (!player.teleportCooldown) // Check if player is on cooldown
            {
                if (Input.GetAxisRaw("Vertical") == 1)
                {
                    StartCoroutine(TeleportPlayer(player));
                }
            }
        }
    }

    private IEnumerator TeleportPlayer(PlayerMovement player)
    {
        player.teleportCooldown = true; // Activate cooldown on player
        player.transform.position = (Vector2)linkedDoor.transform.position + teleportOffset;

        yield return new WaitForSeconds(cooldownDuration); // Wait before cooldown resets
        player.teleportCooldown = false; // Reset cooldown
    }
}