using UnityEngine;

public class AnimatorChangeFeedback : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool("HasCollided", true);
        }
    }
}
