using UnityEngine;
using UnityEngine.Rendering;


public class PlayerMovement : MonoBehaviour
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

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();

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
        // Move the player using rigidbody position and velocity scaled.
        rb2D.MovePosition(rb2D.position + moveVector * newSpeed * Time.deltaTime);
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