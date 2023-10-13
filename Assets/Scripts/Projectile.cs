using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rigid;
    public float removeTimer = 5.0f;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        Destroy(gameObject, removeTimer);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyController e = collision.collider.GetComponent<EnemyController>();
        if (e != null)
        {
            e.Fix();
        }

        Destroy(gameObject);
    }

    public void Launch(Vector2 direction, float force)
    {
        rigid.AddForce(direction * force);
    }
}
