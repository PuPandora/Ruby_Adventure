using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        RubyController ruby = collision.GetComponent<RubyController>();

        if (ruby != null)
        {
            if (ruby.health < ruby.maxHealth)
            {
                ruby.ChangeHealth(1);
                Destroy(gameObject);
            }
        }
    }
}
