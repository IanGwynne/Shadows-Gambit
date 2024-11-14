using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject levelSelectCanvas; // Assign the level select canvas in the inspector

    public void StartGame()
    {
        SceneManager.LoadScene("Level 1 shadow gambit");
    }

    public void LoadLevel()
    {
        // Disable the main menu canvas and enable the level select canvas
        gameObject.SetActive(false); 
        if (levelSelectCanvas != null)
        {
            levelSelectCanvas.SetActive(true);
        }
    }

    public void QuitGame()
    {
        // Quit the application
        Debug.Log("Quit Game"); // Debug message for testing in the editor
        Application.Quit();
    }
}