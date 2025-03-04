using UnityEngine;
using DG.Tweening;


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

    bool isDashing = false;
    [SerializeField] float dashSpeed = 20.0f;
    float dashDuration = 0.2f;
    float dashTime = 0f;
    Vector2 dashDirection;
    public float dashTrailDuration = 0.2f; // Duration of the trail effect
    public float dashTrailFadeDuration = 0.1f; // Fade out duration of the trail
    public int dashTrailSegments = 10; // Number of segments in the trail
    public float dashRotationAmount = 45f; // Amount of rotation during dash
    public Ease dashEase = Ease.OutQuad; // Ease for the movement
    public Ease rotationEase = Ease.OutBack; // Ease for the rotation
    public Ease scaleEase = Ease.OutBack; // Ease for the scale

    bool isFacingRight = true;

    Camera mainCamera;
    Animator animator;
    ParticleSystem particleSystem;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        animator = GetComponentInChildren<Animator>();
        particleSystem = GetComponentInChildren<ParticleSystem>();

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
        Flip();
    }

    void Dash()
    {
        isDashing = true;
        GameManager.instance.audioManager.PlayOneShot(GameManager.instance.audioManager.dashSound);
        dashTime = dashDuration;
        dashDirection = lastMoveVector;
        newSpeed = dashSpeed;
        // Do a cool dash effect with dotween to create illusion of speed.
        transform.DOMove(transform.position + (Vector3)dashDirection, dashDuration / 2).SetEase(dashEase);
        transform.DORotate(new Vector3(0, 0, transform.eulerAngles.z + (dashDirection.x > 0 ? -dashRotationAmount : dashRotationAmount)), dashDuration, RotateMode.FastBeyond360)
            .SetEase(rotationEase)
            .OnComplete(() =>
            {
                transform.DORotate(new Vector3(0, 0, 0), dashDuration * 0.7f).SetEase(rotationEase); // Return to original rotation smoothly after dash.
                isDashing = false;
            });
        transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), dashDuration, 10, 1).SetEase(scaleEase);
        CreateDashTrail();
    }

    void CreateDashTrail()
    {
        for (int i = 0; i < dashTrailSegments; i++)
        {
            GameObject trailSegment = new GameObject("DashTrailSegment");
            SpriteRenderer trailRenderer = trailSegment.AddComponent<SpriteRenderer>();
            SpriteRenderer playerRenderer = GetComponentInChildren<SpriteRenderer>();
            trailRenderer.sprite = playerRenderer.sprite; // Copy the sprite

            // Position and scale the trail segment
            float segmentProgress = (float)i / dashTrailSegments;
            // Adjust the distance and scale
            Vector3 trailPosition = transform.position - (Vector3)(dashDirection * dashSpeed * dashDuration * segmentProgress / dashTrailSegments);
            trailSegment.transform.position = trailPosition;
            trailSegment.transform.localScale = transform.localScale * (1f - segmentProgress * 0.05f); // Larger and less scaling

            // Flip the trail segment's sprite to match the player's facing direction
            if (playerRenderer.flipX)
            {
                trailRenderer.flipX = true;
            }
            else
            {
                trailRenderer.flipX = false;
            }

            // Fade out the trail
            Color startColor = trailRenderer.color;
            startColor.a = 1f;
            trailRenderer.color = startColor;

            trailRenderer.DOFade(0f, dashTrailFadeDuration)
                .SetDelay(dashTrailDuration * segmentProgress)
                .OnComplete(() => Destroy(trailSegment));
        }
    }

    void MovePlayer()
    {
        animator.SetBool("IsMoving", moveVector != Vector2.zero);
        animator.speed = (moveVector != Vector2.zero) ? 1 : 0;
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
            GameManager.instance.audioManager.PlayOneShot(GameManager.instance.audioManager.impact);
            particleSystem.Play();
            // Reset the player position.
            Actions.ResetPlayer?.Invoke();
        }
        else if (collision.gameObject.CompareTag("Goal"))
        {
            // Reset the player position.
            Actions.NextWave?.Invoke();
        }
    }

    private void Flip()
    {
        if (isDashing) return;
        if (!isFacingRight && moveVector.x > 0 || isFacingRight && moveVector.x < 0)
        {
            isFacingRight = !isFacingRight;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
    }
}