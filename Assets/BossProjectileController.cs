using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectileController : MonoBehaviour
{
    GameObject bossCat;
    string bossTag = "Boss";

    private void Start()
    {
        bossCat = GameObject.FindWithTag(bossTag);
    }

    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ShootAtBoss();
        }
    }

    void ShootAtBoss()
    {
        transform.position = Vector3.Lerp(transform.position, bossCat.transform.position, 1f);
    }
}
