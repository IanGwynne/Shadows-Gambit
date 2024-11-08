using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour
{
    [SerializeField] private int detectionCount = 0;
    [SerializeField] private float tickInterval = 1.0f; // Speed at which it ticks up/down, higher is slower

    private Coroutine tickUpCoroutine;
    private Coroutine tickDownCoroutine;
    private PlayerMovement playerMovement;
    private bool isInDetectionArea = false;

    // Start is called before the first frame update
    void Start()
    {
        // Find the GameObject with the PlayerMovement script
        GameObject player = GameObject.Find("PlayerForTesting");

        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
        }
        else
        {
            Debug.LogError("Player GameObject not found!");
        }
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

    public int GetDetectionCount()
    {
        return detectionCount;
    }
}