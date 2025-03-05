using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvanceAIController : MonoBehaviour
{
    [SerializeField] Transform leftEyeTransform;
    [SerializeField] Transform rightEyeTransform;
    [SerializeField] GameObject laserPrefab;
    [SerializeField] GameObject bossTargetPickupPrefab;

    Transform playerTransform;
    Animator animator;
    float fireInterval = 0.15f;
    float laserDuration = 0.5f;
    bool isFiring = false;
    float pickupDelay = 5f;
    int maxPickups = 50;
    int pickupsSpawned = 0;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(LaserRoutine());
        StartCoroutine(SpawnPickup());
    }

    IEnumerator LaserRoutine()
    {
        // Fires a laser based on interval and duration.
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
        // Randomly choose which eye will fire the laser.
        Transform eyeTransform = (Random.value > 0.5f) ? leftEyeTransform : rightEyeTransform;

        // Instantiate the laser at the chosen eye's position.
        GameObject laser = Instantiate(laserPrefab, eyeTransform.position, Quaternion.identity);


        Vector2 directionToPlayer = (playerTransform.position - eyeTransform.position).normalized;
        laser.GetComponent<Rigidbody2D>().velocity = directionToPlayer * 10f;  

        // Rotate laser toward player.
        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        laser.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // Destroy laser after 2 seconds to avoid memory issues.
        Destroy(laser, 2f);  
    }

    IEnumerator SpawnPickup()
    {
        while (pickupsSpawned < maxPickups)
        {
            yield return new WaitForSeconds(pickupDelay);
            GameObject pickupInstance = Instantiate(bossTargetPickupPrefab, new Vector3(4, 4, 4), transform.rotation);
            pickupsSpawned++;
        }
    }
}
