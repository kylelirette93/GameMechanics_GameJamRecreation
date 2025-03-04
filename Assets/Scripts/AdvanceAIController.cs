using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvanceAIController : MonoBehaviour
{
    [SerializeField] Transform leftEyeTransform;
    [SerializeField] Transform rightEyeTransform;
    Transform playerTransform;  // Reference to the player's position
    [SerializeField] GameObject laserPrefab;
    float fireInterval = 0.1f;
    float laserDuration = 0.5f;
    Animator animator;
    private bool isFiring = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(LaserRoutine());
    }

    IEnumerator LaserRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireInterval);
            if (!isFiring)
            {
                isFiring = true;
                animator.SetTrigger("Laser");

                yield return new WaitForSeconds(laserDuration);

                FireLaser();
                yield return new WaitForSeconds(laserDuration);

                isFiring = false;
            }
        }
    }

    void FireLaser()
    {
        // Randomly choose which eye will fire the laser
        Transform eyeTransform = (Random.value > 0.5f) ? leftEyeTransform : rightEyeTransform;

        // Instantiate the laser at the chosen eye's position
        GameObject laser = Instantiate(laserPrefab, eyeTransform.position, Quaternion.identity);

        // Calculate direction to the player
        Vector2 directionToPlayer = (playerTransform.position - eyeTransform.position).normalized;

        // Set laser's velocity toward the player
        laser.GetComponent<Rigidbody2D>().velocity = directionToPlayer * 10f;  // Adjust the speed as needed

        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        laser.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // Optionally destroy the laser after some time to avoid memory issues
        Destroy(laser, 2f);  // Destroy after 2 seconds
    }
}
