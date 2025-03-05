using UnityEngine;
using DG.Tweening;

public class BossHealthController : MonoBehaviour
{
    public int Health { get; private set; } = 7;
    Vector3 endRotation = new Vector3(90, 0, 0);
    ParticleSystem particleSystem;

    private void Start()
    {
        particleSystem = GetComponentInChildren<ParticleSystem>();
    }

    void LoseHealth()
    {
        GameManager.instance.audioManager.PlayOneShot(GameManager.instance.audioManager.impact);
        particleSystem.Play();
        // Do an elastic rotation for visual feedback when boss takes a hit.
        transform.DOPunchRotation(endRotation, 0.5f, 10, 1f)
            .OnComplete(() =>
            {
                transform.DORotate(Vector3.zero, 0.5f);
            });

        Health -= 1;
        if (Health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Game over state, destroy the boss and change state.
        GameManager.instance.gameStateManager.ChangeState(GameStateManager.GameState.GameWin);
        Destroy(transform.parent.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            if (other.gameObject != null)
            {
                LoseHealth();
            }
        }
    }
}
