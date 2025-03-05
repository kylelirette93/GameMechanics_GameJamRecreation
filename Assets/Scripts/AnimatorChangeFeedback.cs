using UnityEngine;

public class AnimatorChangeFeedback : MonoBehaviour
{
    // Feedback for if the player has collided with a saw, it turns redish.
    Animator animator;
    TrailRenderer trailRenderer;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool("HasCollided", true);
            trailRenderer.startColor = Color.red;
            trailRenderer.endColor = Color.clear;
        }
    }
}
