using UnityEngine;
using TMPro;

public class Keypad : Interactable
{
    public TMP_Text displayText;  // Display for the keypad code
    private string inputCode = "";  // Holds the entered digits
    private string currentCode;
    public int maxDigits = 4;  // Maximum number of digits

    private void Start()
    {
        HideInteractableUI();  // Ensure the keypad UI is hidden on start
        GenerateRandomCode();
    }

    private void GenerateRandomCode()
    {
        int currentMinDigits = (int)Mathf.Pow(10, maxDigits - 1);
        int currentMaxDigits = (int)Mathf.Pow(10, maxDigits) - 1;
        currentCode = Random.Range(currentMinDigits, currentMaxDigits + 1).ToString();
        Debug.Log("Generated Keycode: " + currentCode);
    }

    public string GetCurrentKeycode()
    {
        return currentCode;
    }
    public override void Interact()
    {
        if (!isSolved)
        {
            ShowInteractableUI();
            ResetKeypad();  // Reset the keypad input when the player interacts
            Debug.Log("Keypad interaction triggered.");
        }
        else
        {
            Debug.Log("Keypad already solved.");
        }
    }

    public override void OnPlayerExit()
    {
        HideInteractableUI();  // Hide the keypad UI when the player leaves
    }

    public void OnButtonPress(string value)
    {
        if (value == "*")
        {
            ResetKeypad();  // Reset the input
        }
        else if (value == "#")
        {
            CheckCode();  // Submit the code
        }
        else if (inputCode.Length < maxDigits)
        {
            inputCode += value;  // Add the digit to the input
            UpdateDisplay();
        }
    }

    private void UpdateDisplay()
    {
        displayText.text = inputCode.PadRight(maxDigits, '_');  // Display the entered code
    }

    private void CheckCode()
    {
        if (inputCode == currentCode)
        {
            Debug.Log("Access Granted.");
            DisableInteractable();  // Disable the keypad after the correct code is entered
        }
        else
        {
            Debug.Log("Incorrect Code.");
            ResetKeypad();  // Reset the keypad if the code is wrong
        }
    }

    private void ResetKeypad()
    {
        inputCode = "";
        UpdateDisplay();
    }
}