using UnityEngine;
using UnityEngine.UI;

public class KeycardReader : Interactable
{
    public Scrollbar keycardSwipeBar;  // Scroll bar representing the keycard swipe
    private bool playerHasKeycard = false;
    private PlayerInventory playerInventory;

    private void Start()
    {
        HideInteractableUI();  // Ensure the UI is hidden on start
        keycardSwipeBar.gameObject.SetActive(false);  // Disable the swipe bar at the start
    }

    public override void Interact()
    {
        if (!isSolved)
        {
            ShowInteractableUI();  // Show the UI for interaction
            if (playerHasKeycard)
            {
                keycardSwipeBar.gameObject.SetActive(true);  // Enable the swipe bar
                keycardSwipeBar.value = 1;  // Reset the scroll bar to the top position
                Debug.Log("Keycard reader interaction triggered.");
            }
        }
        else if (isSolved)
        {
            Debug.Log("Keycard reader already solved.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInventory = other.GetComponent<PlayerInventory>();
            if (playerInventory != null && playerInventory.hasKeycard)
            {
                playerHasKeycard = true;
                Debug.Log("Player has the keycard.");
            }
        }
    }

    private void Update()
    {
        if (keycardSwipeBar.gameObject.activeSelf && keycardSwipeBar.value <= 0)
        {
            OnSwipeCard();  // Trigger swipe completion when the scroll bar is fully swiped
        }
    }

    private void OnSwipeCard()
    {
        Debug.Log("Keycard swiped successfully.");
        keycardSwipeBar.gameObject.SetActive(false);  // Hide the swipe bar
        DisableInteractable();  // Disable the interactable once solved
        // Additional logic for unlocking the door can be added here
    }
}