using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 movement;
    private SpriteRenderer spriteRenderer;
    private Collider2D playerCollider;
    private Collider2D currentClimbable;

    [SerializeField] public int playerScore = 0;
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float climbingSpeed = 4f;
    [SerializeField] private float crawlingSpeedMultiplier = 0.5f;
    public LayerMask floorLayer;

    private float defaultMovementSpeed;
    private bool isCrawling, isClimbing, isInShadowArea, isNearInteractable, isHidden;
    private Collider2D currentInteractable;
    private float climbableTop, climbableBottom;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultMovementSpeed = movementSpeed;
        rb.gravityScale = 0; // Gravity is disabled as per game design
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        // Allow movement if the player is not hiding in shadows
        if (!isInShadowArea || (isInShadowArea && Input.GetAxisRaw("Vertical") == 0))
        {
            movement.x = !isClimbing ? Input.GetAxisRaw("Horizontal") * movementSpeed : 0;
            movement.y = isClimbing ? Input.GetAxisRaw("Vertical") * climbingSpeed : 0;
        }
        else
        {
            movement = Vector2.zero; // Disable all movement when hiding
        }

        HandleCrawling();
        HandleShadowHiding();

        // Check for interactable objects and interact
        if (isNearInteractable && Input.GetAxisRaw("Vertical") > 0)
        {
            InteractWithObject();
        }
    }

    private void FixedUpdate()
    {
        // Regular horizontal movement when not climbing, otherwise climb handling
        if (!isClimbing)
        {
            rb.velocity = new Vector2(movement.x, rb.velocity.y);
        }
        else
        {
            HandleClimbing();
        }

        // Stop movement if no input
        if (movement == Vector2.zero)
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void HandleCrawling()
    {
        if (!isClimbing && Input.GetAxisRaw("Crawl") > 0)
        {
            isCrawling = true;
            movementSpeed = defaultMovementSpeed * crawlingSpeedMultiplier;
            AdjustPlayerHeight(0.5f); // Make the player crouch (reduce height)
        }
        else
        {
            isCrawling = false;
            movementSpeed = defaultMovementSpeed;
            AdjustPlayerHeight(1f); // Restore player height to normal
        }
    }

    private void HandleShadowHiding()
    {
        // The player can hide only if they are not crawling and inside the shadow area
        if (isInShadowArea && !isCrawling && Input.GetAxisRaw("Vertical") > 0)
        {
            // TODO: Replace with hiding animation
            SetSpriteColor(new Color(0.3f, 0.3f, 0.3f, 1f)); // Darken the sprite to indicate hiding
            // TODO: Reduce player detectability here
            isHidden = true; 
        }
        else if (isInShadowArea && Input.GetAxisRaw("Vertical") == 0)
        {
            // TODO: Replace with leave hiding animation
            SetSpriteColor(new Color(1f, 1f, 1f, 1f)); // Restore original sprite color when no longer hiding
            // TODO: Restore player detectability to normal here
            isHidden = false;
        }
    }

    private void HandleClimbing()
    {
        // End climbing if player exceeds climbable top or bottom
        if ((transform.position.y >= climbableTop && movement.y > 0) || (transform.position.y <= climbableBottom && movement.y < 0))
        {
            EndClimbing();
        }
        else
        {
            rb.velocity = new Vector2(0, movement.y); // Apply vertical climbing velocity
        }

        // Stop movement when no vertical input
        if (Input.GetAxisRaw("Vertical") == 0)
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void InteractWithObject()
    {
        if (currentInteractable != null)
        {
            Interactable interactable = currentInteractable.GetComponent<Interactable>();
            if (interactable != null)
            {
                interactable.Interact(); // Trigger the interactable's specific interaction
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("ShadowArea"))
        {
            isInShadowArea = true; // Enter shadow area, allowing hiding if conditions are met
        }

        // Handle climbing interaction with a climbable object
        if (!isCrawling && other.CompareTag("Climbable"))
        {
            CacheClimbableBounds(other); // Cache the top and bottom bounds of the climbable
            float verticalInput = Input.GetAxisRaw("Vertical");

            // Climb up if vertical input is positive and below the top
            if (verticalInput > 0 && transform.position.y < climbableTop)
            {
                StartClimbing(other);
            }
            // Climb down if vertical input is negative and at or above the top
            else if (verticalInput < 0 && transform.position.y >= climbableTop)
            {
                StartClimbing(other);
                movement.y = -climbingSpeed; // Set downward movement for climbing down
                rb.velocity = new Vector2(0, movement.y);
            }
        }
        if (other.CompareTag("Interactable"))
        {
            isNearInteractable = true;
            currentInteractable = other; // Cache the interactable object
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("ShadowArea"))
        {
            isInShadowArea = false; // Exiting shadow area removes hiding ability
        }

        // End climbing if the player leaves the climbable area
        if (isClimbing && other == currentClimbable)
        {
            EndClimbing();
        }

        if (other.CompareTag("Interactable"))
        {
            isNearInteractable = false; // No longer near an interactable object
            currentInteractable = null; // Clear the cached interactable object
        }
    }

    private void StartClimbing(Collider2D climbable)
    {
        isClimbing = true;
        currentClimbable = climbable;
        rb.velocity = new Vector2(0, rb.velocity.y); // Stop horizontal movement during climbing

        // Ignore collisions with floors while climbing to pass between them
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Floor"), true);
    }

    private void EndClimbing()
    {
        isClimbing = false;
        currentClimbable = null;
        rb.velocity = Vector2.zero; // Stop all movement

        // Re-enable floor collisions once climbing ends
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Floor"), false);
    }

    private void CacheClimbableBounds(Collider2D climbable)
    {
        climbableTop = climbable.bounds.max.y;
        climbableBottom = climbable.bounds.min.y + transform.localScale.y * 0.5f; // Adjust bottom to account for player height
    }

    private void AdjustPlayerHeight(float height)
    {
        // Adjust the player's height and position based on the given height (used for crawling)
        float heightDiff = transform.localScale.y - height;
        transform.localScale = new Vector3(transform.localScale.x, height, transform.localScale.z);
        transform.position = new Vector3(transform.position.x, transform.position.y - heightDiff / 2, transform.position.z);
    }

    private void SetSpriteColor(Color color)
    {
        spriteRenderer.color = color; // Set the player's sprite to the given color
    }

    // Public function to get the isHidden value
    public bool IsHidden()
    {
        return isHidden;
    }
}