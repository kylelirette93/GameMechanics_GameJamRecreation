using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    float followSpeed = 1f;
    Transform player;
    Animator animator;
    bool isMoving = false;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
        animator = GetComponentInChildren<Animator>();
    }
    private void Update()
    {
        isMoving = ((player.position - transform.position).normalized.sqrMagnitude > 0); // One-liner to check if moving
        animator.SetBool("isMoving", isMoving);

        // Move the object
        transform.position += (player.position - transform.position).normalized * followSpeed * Time.deltaTime;
    }
}
