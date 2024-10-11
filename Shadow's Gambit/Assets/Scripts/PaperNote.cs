using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PaperNote : Interactable
{
    public TMP_Text noteText;  // Reference to the Text component that displays the note
    public string tutorialContent = "This is a tutorial note.";  // Default tutorial note content
    public bool isTutorialNote = false;  // Toggle to make this a tutorial note
    public bool isKeycodeNote = false;  // Toggle to make this a keycode note
    public Keypad keypad;

    private void Start()
    {
        HideInteractableUI();  // Ensure the note UI is hidden on start
        UpdateNoteContent();  // Set the content of the note
    }

    public override void Interact()
    {
        ShowInteractableUI();  // Show the note UI when interacting
        UpdateNoteContent();
        Debug.Log("Note interaction triggered.");
    }

    // Set the content of the note in the Text component
    private void UpdateNoteContent()
    {
        if (noteText != null)
        {
            if (isTutorialNote)
            {
                noteText.text = tutorialContent;
            }
            else if (isKeycodeNote && keypad != null)
            {
                noteText.text = "The code for the Keypad is: " + keypad.GetCurrentKeycode();
            }
        }
    }

    public override void OnPlayerExit()
    {
        HideInteractableUI();  // Hide the note UI when the player walks away
        Debug.Log("Player left the note.");
    }
}