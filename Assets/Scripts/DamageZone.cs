using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D collision)
    {
        Ruby ruby = collision.GetComponent<Ruby>();

        if (ruby != null)
        {
            ruby.ChangeHealth(-1);
        }
    }
}
