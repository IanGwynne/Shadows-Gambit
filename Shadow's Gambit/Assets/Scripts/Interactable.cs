using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public GameObject interactableUI;  // Reference to the UI object
    protected bool isSolved = false;   // Track whether the interactable has been solved

    // This method is called when the player enters the trigger/collision range
    public virtual void OnPlayerEnter()
    {
        if (!isSolved)
        {
            Debug.Log("Player is near the interactable.");
        }
    }

    // This method is called when the player leaves the trigger/collision range
    public virtual void OnPlayerExit()
    {
        HideInteractableUI();
        Debug.Log("Player left the interactable.");
    }

    // This method shows the UI
    public virtual void ShowInteractableUI()
    {
        if (interactableUI != null && !isSolved)
        {
            interactableUI.SetActive(true);
            Debug.Log("Interactable UI shown.");
        }
    }

    // This method hides the UI
    public virtual void HideInteractableUI()
    {
        if (interactableUI != null)
        {
            interactableUI.SetActive(false);
            Debug.Log("Interactable UI hidden.");
        }
    }

    // This method disables the interactable entirely after it's solved
    protected virtual void DisableInteractable()
    {
        isSolved = true;  // Mark as solved
        HideInteractableUI();  // Hide the UI
        Debug.Log("Interactable has been solved and disabled.");
    }

    // Detect when the player enters the trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerEnter();  // Call OnPlayerEnter when the player enters the trigger
        }
    }

    // Detect when the player exits the trigger
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerExit();  // Call OnPlayerExit when the player exits the trigger
        }
    }

    // Abstract method for interaction logic
    public abstract void Interact();
}