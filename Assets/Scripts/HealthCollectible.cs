using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        Ruby ruby = collision.GetComponent<Ruby>();

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
