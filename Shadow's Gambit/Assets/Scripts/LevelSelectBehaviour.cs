using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectBehaviour : MonoBehaviour
{
    public GameObject StartMenuCanvas; // Assign the level select canvas in the inspector
    public void LoadLevel(int levelNum)
    {
        switch (levelNum)
        {
            case 1:
            {
                SceneManager.LoadScene("Level 1 shadow gambit");
                break;
            }
            case 2:
            {
                SceneManager.LoadScene("Level 2");
                break;
            }
            case 3:
            {
                SceneManager.LoadScene("Level 3");
                break;
            }
        }
    }
    public void LoadStartMenu()
    {
        gameObject.SetActive(false); 
        if (StartMenuCanvas != null)
        {
            StartMenuCanvas.SetActive(true);
        }
    }
}