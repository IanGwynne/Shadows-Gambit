using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DetectionManager : MonoBehaviour
{
    [Header("Detection Settings")]
    public int detectionCount = 0;
    public int gameOverAmount = 30;
    public float tickInterval = 1.0f;
    public GameObject GameOverScreen;

    private GameObject gameOverScreenInstance;
    private Button retryButton;
    private Coroutine tickCoroutine;
    private bool isPlayerDetected = false;

    public bool IsPlayerDetected => isPlayerDetected;

    void Start()
    {
        // Instantiate the Game Over Screen and set it up
        gameOverScreenInstance = Instantiate(GameOverScreen);
        gameOverScreenInstance.SetActive(false);

        retryButton = gameOverScreenInstance.GetComponentInChildren<Button>();
        retryButton.onClick.AddListener(RestartLevel);
    }
    private void Update()
    {
        CheckGameOver();
    }

    public void SetPlayerDetected(bool detected)
    {
        if (isPlayerDetected == detected) return;

        isPlayerDetected = detected;

        if (tickCoroutine != null)
        {
            StopCoroutine(tickCoroutine);
            tickCoroutine = null;
        }

        tickCoroutine = StartCoroutine(detected ? TickUpDetectionCount() : TickDownDetectionCount());
    }

    private IEnumerator TickUpDetectionCount()
    {
        while (isPlayerDetected)
        {
            detectionCount++;
            Debug.Log("Detection count increased: " + detectionCount);
            yield return new WaitForSeconds(tickInterval);
        }
    }

    private IEnumerator TickDownDetectionCount()
    {
        while (!isPlayerDetected)
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
            gameOverScreenInstance.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    private void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}