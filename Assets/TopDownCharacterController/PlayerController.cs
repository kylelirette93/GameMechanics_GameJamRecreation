using UnityEngine;


public class PlayerController : MonoBehaviour
{
    // Reference to rigidbody component.
    Rigidbody2D rb2D;

    // Initialize input vector.
    Vector2 moveVector;
    Vector2 lastMoveVector;

    // Movement speed of the player.
    [SerializeField] float moveSpeed = 2.0f;
    float newSpeed;

    [SerializeField] float dashSpeed = 20.0f;
    float dashDuration = 0.2f;
    float dashTime = 0f;
    Vector2 dashDirection;

    // Reference to the animator component.
    Animator animator;
    Camera mainCamera;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        mainCamera = Camera.main;

        // Subscribe to the move event.
        Actions.MoveEvent += GetInputVector;
        Actions.DashEvent += Dash;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void Update()
    {
        dashTime -= Time.deltaTime;
        if (dashTime <= 0)
        {
            newSpeed = moveSpeed;
        }
    }

    void Dash()
    {
        dashTime = dashDuration;
        dashDirection = lastMoveVector;
        newSpeed = dashSpeed;
    }

    void MovePlayer()
    {
        Vector2 newPosition = rb2D.position + moveVector * newSpeed * Time.deltaTime;
        newPosition = ClampPositionToCameraBounds(newPosition); // Clamp position
        rb2D.MovePosition(newPosition);
    }

    Vector2 ClampPositionToCameraBounds(Vector2 position)
    {
        if (mainCamera == null) return position;

        // Define an offset (margin) to keep the player slightly inside the screen
        float offset = 0.3f; // Adjust this value as needed

        // Get camera world boundaries
        Vector3 minBounds = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 maxBounds = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0));

        // Apply offset
        minBounds.x += offset;
        minBounds.y += offset;
        maxBounds.x -= offset;
        maxBounds.y -= offset;

        // Clamp position with offset
        position.x = Mathf.Clamp(position.x, minBounds.x, maxBounds.x);
        position.y = Mathf.Clamp(position.y, minBounds.y, maxBounds.y);

        return position;
    }

    void GetInputVector(Vector2 inputDirection)
    {
        // If there is movement input, store the new vector. 
        if (inputDirection != Vector2.zero)
        {
            moveVector = inputDirection;
            lastMoveVector = moveVector.normalized;
        }
        else
        {
            // No input, stay idle.
            moveVector = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Buzzsaw"))
        {
            // Reset the player position.
            Actions.ResetPlayer?.Invoke();
        }
        else if (collision.gameObject.CompareTag("Goal"))
        {
            // Reset the player position.
            Actions.NextWave?.Invoke();
        }
    }
}