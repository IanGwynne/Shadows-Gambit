using UnityEngine;
using UnityEngine.UI;

public class LockPick : Interactable
{
    public Slider[] pinSliders;  // An array of sliders representing each pin
    public float pinSpeed = 0.1f;  // How much the pin rises per press
    public float fallSpeed = 0.05f;  // How much the pin sinks constantly
    public int maxPins = 5;  // Number of pins to be picked
    private int currentPin = 0;  // The current pin the player is working on
    private bool axisInUse = false;  // Tracks if the axis is already pressed
    private bool canStartInput = false;  // Tracks if input can start after opening the UI

    private void Start()
    {
        HideInteractableUI();  // Hide the lock-picking UI on start
        ResetPins();  // Reset all pins to the bottom
    }

    public override void Interact()
    {
        if (!isSolved)
        {
            ShowInteractableUI();  // Show the lock-picking UI when interacting
            canStartInput = false;  // Disable input initially to avoid immediate interaction
            Debug.Log("Lock picking interaction triggered.");
            Invoke("EnableInput", 0.2f);  // Delay input by 0.2 seconds to prevent immediate interaction
        }
        else
        {
            Debug.Log("Lock has already been picked.");
        }
    }

    // Enable input after a short delay
    private void EnableInput()
    {
        canStartInput = true;
    }

    private void Update()
    {
        // Handle pin sinking continuously, even if the UI is not active
        HandlePinSinking();

        if (interactableUI.activeSelf && canStartInput)
        {
            HandleLockPicking();
        }
    }

    // Handle the lock-picking process
    private void HandleLockPicking()
    {
        if (currentPin < maxPins)
        {
            // Handle raising the pin when the player presses the W key
            if (Input.GetAxisRaw("Vertical") > 0 && !axisInUse)
            {
                axisInUse = true;  // Mark the axis as in use

                // Raise the current pin
                pinSliders[currentPin].value += pinSpeed;

                // If the pin is fully raised
                if (pinSliders[currentPin].value >= pinSliders[currentPin].maxValue)
                {
                    pinSliders[currentPin].value = pinSliders[currentPin].maxValue;  // Ensure it stays at max
                    Debug.Log("Pin " + (currentPin + 1) + " picked.");
                    currentPin++;  // Move to the next pin
                }
            }

            // Reset the axisInUse flag when the axis is no longer pressed
            if (Input.GetAxisRaw("Vertical") == 0)
            {
                axisInUse = false;
            }
        }
        else
        {
            OnLockPicked();  // If all pins are picked
        }
    }

    // Handle pin sinking continuously, even when the lock-picking UI is not active
    private void HandlePinSinking()
    {
        if (currentPin < maxPins)
        {
            // Decrease the value of the current pin continuously unless it is fully raised
            if (pinSliders[currentPin].value < pinSliders[currentPin].maxValue)
            {
                pinSliders[currentPin].value -= fallSpeed * Time.deltaTime;
                pinSliders[currentPin].value = Mathf.Max(0, pinSliders[currentPin].value);  // Ensure it doesn't go below 0
            }
        }
    }

    // Called when all pins have been picked successfully
    private void OnLockPicked()
    {
        DisableInteractable();  // Disable the lock interactable
        Debug.Log("Lock picked successfully!");
        HideInteractableUI();  // Hide the lock-picking UI
        // Additional logic to unlock the door or perform other actions
    }

    // Reset all pins to the bottom position
    private void ResetPins()
    {
        foreach (Slider pinSlider in pinSliders)
        {
            pinSlider.value = 0;  // Reset pin to 0
        }
        currentPin = 0;  // Start with the first pin
    }

    public override void OnPlayerExit()
    {
        HideInteractableUI();  // Hide the UI when the player walks away
        Debug.Log("Player left the lock.");
    }
}