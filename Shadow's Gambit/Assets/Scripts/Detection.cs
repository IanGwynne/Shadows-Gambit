using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Detection : MonoBehaviour
{
    [SerializeField] private int detectionCount = 0;
    [SerializeField] private int gameOverAmount = 30;
    [SerializeField] private float tickInterval = 1.0f; // Speed at which it ticks up/down, higher is slower
    [SerializeField] private GameObject GameOverScreen; // Reference to the Game Over UI Prefab

    private GameObject gameOverScreenInstance;
    private Button retryButton;
    private Coroutine tickUpCoroutine;
    private Coroutine tickDownCoroutine;
    private PlayerMovement playerMovement;
    private bool isInDetectionArea = false;

    // Start is called before the first frame update
    void Start()
    {
        // Find the GameObject with the PlayerMovement script
        GameObject player = GameObject.Find("Player");

        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
        }
        else
        {
            Debug.LogError("Player GameObject not found!");
        }

        // Instantiate the Game Over Screen and set it up
        gameOverScreenInstance = Instantiate(GameOverScreen);
        gameOverScreenInstance.SetActive(false); // Hide the Game Over Screen at the start

        // Find the Retry Button in the instantiated UI and add a listener to it
        retryButton = gameOverScreenInstance.GetComponentInChildren<Button>();
        retryButton.onClick.AddListener(RestartLevel);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMovement != null)
        {
            if (playerMovement.IsHidden())
            {
                if (tickUpCoroutine != null)
                {
                    StopCoroutine(tickUpCoroutine);
                    tickUpCoroutine = null;
                }

                if (tickDownCoroutine == null)
                {
                    tickDownCoroutine = StartCoroutine(TickDownDetectionCount());
                }
            }
            else if (isInDetectionArea)
            {
                if (tickDownCoroutine != null)
                {
                    StopCoroutine(tickDownCoroutine);
                    tickDownCoroutine = null;
                }

                if (tickUpCoroutine == null)
                {
                    tickUpCoroutine = StartCoroutine(TickUpDetectionCount());
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Detection"))
        {
            isInDetectionArea = true;

            if (playerMovement != null && !playerMovement.IsHidden())
            {
                if (tickDownCoroutine != null)
                {
                    StopCoroutine(tickDownCoroutine);
                    tickDownCoroutine = null;
                }

                if (tickUpCoroutine == null)
                {
                    tickUpCoroutine = StartCoroutine(TickUpDetectionCount());
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Detection"))
        {
            isInDetectionArea = false;

            if (tickUpCoroutine != null)
            {
                StopCoroutine(tickUpCoroutine);
                tickUpCoroutine = null;
            }

            if (tickDownCoroutine == null)
            {
                tickDownCoroutine = StartCoroutine(TickDownDetectionCount());
            }
        }
    }

    private IEnumerator TickUpDetectionCount()
    {
        while (true)
        {
            detectionCount++;
            Debug.Log("Detection count increased: " + detectionCount);
            CheckGameOver();
            yield return new WaitForSeconds(tickInterval);
        }
    }

    private IEnumerator TickDownDetectionCount()
    {
        while (true)
        {
            if (detectionCount > 0)
            {
                detectionCount--;
                Debug.Log("Detection count decreased: " + detectionCount);
            }
            yield return new WaitForSeconds(tickInterval);
        }
    }

    private void CheckGameOver()
    {
        if (detectionCount >= gameOverAmount)
        {
            Debug.Log("Game Over!");
            gameOverScreenInstance.SetActive(true); // Show the Game Over UI
            Time.timeScale = 0f; // Pause the game
        }
    }

    private void RestartLevel()
    {
        Time.timeScale = 1f; // Resume the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }

    public int GetDetectionCount()
    {
        return detectionCount;
    }
}
