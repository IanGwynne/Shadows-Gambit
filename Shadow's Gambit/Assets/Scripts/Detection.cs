using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour
{
    [SerializeField] private int detectionCount = 0;
    [SerializeField] private float tickInterval = 1.0f; // Speed at which it ticks up/down, higher is slower

    private Coroutine tickUpCoroutine;
    private Coroutine tickDownCoroutine;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Detection"))
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

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Detection"))
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
}

