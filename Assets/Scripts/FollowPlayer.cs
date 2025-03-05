using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    float followSpeed = 3f;
    Transform player;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
    }
    private void Update()
    {
        if (player != null)
        {        
            transform.position += (player.position - transform.position).normalized * followSpeed * Time.deltaTime;
        }
    }
}
