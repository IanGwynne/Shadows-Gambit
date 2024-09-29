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
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal"); // This is A, D, LeftArrow, and RightArrow
        movement.y = Input.GetAxisRaw("Vertical"); // This is W, S, UpArrow, and DownArrow
        while(playerOnLadder == false) // This is for Crawling
        {
            if(Input.GetKeyDown(KeyCode.S)) // If we continue to do the crawling this way, then we should change the key to something like LeftControl
            {
                Crawl();
            }
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
