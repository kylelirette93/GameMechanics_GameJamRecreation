using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    float followSpeed = 1f;
    Transform player;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
    }
    private void Update()
    {
        Vector3 direction = (player.position - transform.position).normalized;

        transform.position += direction * followSpeed * Time.deltaTime;
    }
}
