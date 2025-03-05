using UnityEngine;
using DG.Tweening;


public class PlayerController : MonoBehaviour
{
    // References and variables.
    Rigidbody2D rb2D;
    Camera mainCamera;
    Animator animator;
    ParticleSystem particleSystem;
    Vector2 moveVector;
    Vector2 lastMoveVector;
    bool isFacingRight = true;
    public int Lives { get { return lives; } set { lives = value; } }
    int lives;
    public SpawnManager spawnManager;

    // Movement speed of the player.
    [Header("Movement Settings")]
    [SerializeField] float moveSpeed = 2.0f;
    [SerializeField] float newSpeed;

    [Header("Dash Settings")]
    [SerializeField] bool isDashing = false;
    [SerializeField] Vector2 dashDirection;
    [SerializeField] float dashSpeed = 20.0f;
    [SerializeField] float dashDuration = 0.2f;
    [SerializeField] float dashTime = 0f;
    [SerializeField] float dashTrailDuration = 0.2f; 
    [SerializeField] float dashTrailFadeDuration = 0.1f; 
    [SerializeField] int dashTrailSegments = 10; 
    [SerializeField] float dashRotationAmount = 45f; 
    [SerializeField] Ease dashEase = Ease.OutQuad; 
    [SerializeField] Ease rotationEase = Ease.OutBack; 
    [SerializeField] Ease scaleEase = Ease.OutBack;


    private void Awake()
    {
        // Get references.
        rb2D = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        animator = GetComponentInChildren<Animator>();
        particleSystem = GetComponentInChildren<ParticleSystem>();
        lives = 9;

        // Subscribe to the events.
        Actions.MoveEvent += GetInputVector;
        Actions.DashEvent += Dash;
    }

    private void FixedUpdate()
    {
        // Handle movement in fixed update for physics calculations.
        MovePlayer();
    }

    private void Update()
    {
        dashTime -= Time.deltaTime;
        if (dashTime <= 0)
        {
            newSpeed = moveSpeed;
        }
        if (moveVector != Vector2.zero)
        {
            // Only handle flip if player is moving.
            Flip();
        }
    }

    void Dash()
    {
        isDashing = true;
        GameManager.instance.audioManager.PlayOneShot(GameManager.instance.audioManager.dashSound);
        dashTime = dashDuration;
        dashDirection = lastMoveVector;
        newSpeed = dashSpeed;

        // Dash movement, rotation, and scale.
        transform.DOMove(transform.position + (Vector3)dashDirection, dashDuration / 2).SetEase(dashEase);
        // Rotate the player in the direction of the dash, then return to original rotation.
        transform.DORotate(new Vector3(0, 0, transform.eulerAngles.z + (dashDirection.x > 0 ? -dashRotationAmount : dashRotationAmount)), dashDuration, RotateMode.FastBeyond360)
            .SetEase(rotationEase)
            .OnComplete(() =>
            {
                transform.DORotate(new Vector3(0, 0, 0), dashDuration * 0.7f).SetEase(rotationEase); // Return to original rotation smoothly after dash.
                isDashing = false;
            });
        // Scale the player up and down during the dash.
        transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), dashDuration, 10, 1).SetEase(scaleEase);

        CreateDashTrail();
    }

    void CreateDashTrail()
    {
        // This was a pain in the butt... U.U but it works.
        for (int i = 0; i < dashTrailSegments; i++)
        {
            // Create a new trail segment. 
            GameObject trailSegment = new GameObject("DashTrailSegment");
            // Add a sprite renderer to the trail segment and copy the player's sprite.
            SpriteRenderer trailRenderer = trailSegment.AddComponent<SpriteRenderer>();
            SpriteRenderer playerRenderer = GetComponentInChildren<SpriteRenderer>();
            trailRenderer.sprite = playerRenderer.sprite; 

            // Set the segment progress based on the current iteration.
            float segmentProgress = (float)i / dashTrailSegments;
            // Set the trail segment's sorting order to be behind the player.
            Vector3 trailPosition = transform.position - (Vector3)(dashDirection * dashSpeed * dashDuration * segmentProgress / dashTrailSegments);

            // Set the segment's position to be behind the player.
            trailSegment.transform.position = trailPosition;

            // Set the segment's scale to be slightly smaller than the player's scale.
            trailSegment.transform.localScale = transform.localScale * (1f - segmentProgress * 0.05f);

            // Flip the trail segment's sprite to match the player's facing direction.
            if (playerRenderer.flipX)
            {
                trailRenderer.flipX = true;
            }
            else
            {
                trailRenderer.flipX = false;
            }

            // Fade out the segment over time and than destroy it.
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
        newPosition = ClampPositionToCameraBounds(newPosition); 
        rb2D.MovePosition(newPosition);
    }

    Vector2 ClampPositionToCameraBounds(Vector2 position)
    {
        if (mainCamera == null) return position;

        // Offset to keep player sprite inside the camera view.
        float offset = 0.3f; 

        // Get the camera bounds.
        Vector3 minBounds = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 maxBounds = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0));

        // Apply offset.
        minBounds.x += offset;
        minBounds.y += offset;
        maxBounds.x -= offset;
        maxBounds.y -= offset;

        // Clamp position with offset.
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
        // Check for collisions unless player is dashing or resetting.
        if (collision.gameObject.CompareTag("Buzzsaw") && !isDashing && spawnManager.IsResetting == false)
        {
            if (GameManager.instance.gameStateManager.currentState == GameStateManager.GameState.Gameplay || 
                GameManager.instance.gameStateManager.currentState == GameStateManager.GameState.Boss)
            {
                GameManager.instance.audioManager.PlayOneShot(GameManager.instance.audioManager.impact);
                particleSystem.Play();
                Actions.ResetPlayer?.Invoke();
                LoseLife();
                UpdatePlayerLives();
                if (lives <= 0)
                {
                    GameOver();
                }

                GameManager.instance.audioManager.PlayOneShot(GameManager.instance.audioManager.impact);
                particleSystem.Play();
                Actions.ResetPlayer?.Invoke();
            }
        }
        else if (collision.gameObject.CompareTag("Laser") && !isDashing && spawnManager.IsResetting == false)
        {
            GameManager.instance.audioManager.PlayOneShot(GameManager.instance.audioManager.impact);
            particleSystem.Play();
            Actions.ResetPlayer?.Invoke();
            LoseLife();
            UpdatePlayerLives();
            if (lives <= 0)
            {
                GameOver();
            }
            
        }
        else if (collision.gameObject.CompareTag("Goal"))
        {
            // Reset the player position if they reach the goal.
            Actions.NextWave?.Invoke();
        }
    }

    private void LoseLife()
    {
        lives--;
        Mathf.Clamp(lives, 0, 9);
    }

    private void UpdatePlayerLives()
    {
        GameManager.instance.gameStateManager.gameplayPanel.GetComponentInChildren<LivesUI>().UpdateLives();
    }

    private void GameOver()
    {
        GameManager.instance.gameStateManager.ChangeState(GameStateManager.GameState.GameOver);
    }

    private void Flip()
    {
        // Check for movement direction and flip the player sprite accordingly.
        if (isDashing) return;
        if (moveVector.x > 0 && !isFacingRight)
        {
            isFacingRight = true;
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (moveVector.x < 0 && isFacingRight)
        {
            isFacingRight = false;
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
}