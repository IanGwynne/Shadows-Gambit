using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 movement = Vector2.zero;
    bool isCrawling = false;
    bool isClimbing = false;
    [SerializeField] float moveSpeed = 200;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Debug.Log("Player Script has Started"); // Remove this when ready to submit
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal"); // A, D, LeftArrow, and RightArrow
        movement.y = Input.GetAxisRaw("Vertical");   // W, S, UpArrow, and DownArrow

        // Replace this with switch case
        // Worth trying to change the `Input.GetKey(KeyCode.(LeftControl and W))` to `Input.GetAxisRaw("")` for control compatibility
        if (!isClimbing && Input.GetKey(KeyCode.LeftControl)) // Start crawling
        {
            isCrawling = true;
            moveSpeed = 100; // Change speed when crawling
            Debug.Log("Player is Crawling"); // Remove this when ready to submit
        }
        else
        {
            isCrawling = false;
            moveSpeed = 200; // Reset speed when not crawling
            // Debug.Log("Player has Stopped Crawling"); // Remove this when ready to submit
        }
        // Need to change this to only work while in contact with object with tag "Climbable" or something to that effect        
    }
    private void FixedUpdate()
    {
        if (movement.sqrMagnitude > 0)
        {
            rb.velocity = movement * moveSpeed * Time.deltaTime;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision) // This code doesn't work very well. It has issues recognizing contact with Climbables (It has to do with pressing W key to climb). It does not work if BoxCollider2D on object is set to "Is Trigger".
    {
        if (collision.gameObject.CompareTag("Climbable"))
        {
            if (!isCrawling && Input.GetKey(KeyCode.W)) // Start climbing
            {
                isClimbing = true;
                rb.gravityScale = 0; // Disables gravity when climbing
                Debug.Log("Player is Climbing"); // Remove this when ready to submit
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Climbable"))
        {
            isClimbing = false;
            rb.gravityScale = 1; // Enables gravity when not climbing
            Debug.Log("Player has Stopped Climbing"); // Remove this when ready to submit
        }
    }
}