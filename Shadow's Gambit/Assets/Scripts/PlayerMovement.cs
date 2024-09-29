using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 movement = Vector2.zero;
    bool isCrawling = false;
    bool playerOnLadder = false; // Currently unused, however we will add it when we have ladders.
    [SerializeField] float moveSpeed = 200;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Debug.Log("Player Script has Started"); // Remove this when ready to submit
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal"); // This is A, D, LeftArrow, and RightArrow
        movement.y = Input.GetAxisRaw("Vertical"); // This is W, S, UpArrow, and DownArrow
        if(!playerOnLadder && Input.GetKeyDown(KeyCode.S)) // This is for Crawling
        {
            Debug.Log("Player is Crawling"); // Remove this when ready to submit
            Crawl();
        }
    }

    private void FixedUpdate()
    {
        if (movement.sqrMagnitude > 0)
        {
            rb.velocity = movement * moveSpeed * Time.deltaTime;
        }
    }

    private void Crawl()
    {
        // Change player animation / sprite
        // Change player movement speed
        // Change player height (For fitting in tunnels. Might be unnecessary)
        // Lower detection multiplier
    }
}
