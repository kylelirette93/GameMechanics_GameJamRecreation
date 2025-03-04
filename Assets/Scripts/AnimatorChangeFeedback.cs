using UnityEngine;

public class AnimatorChangeFeedback : MonoBehaviour
{
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
