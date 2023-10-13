using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    
    // Move
    public float speed = 3.0f;
    public bool vertical;
    public float changeTime = 3.0f;
    private float timer;
    private int direction = 1;
    // Broken
    private bool broken = true;
    public ParticleSystem smokeEffect;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        timer = changeTime;
    }

    void Update()
    {
        if (!broken)
        {
            return;
        }

        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }

        Vector2 position = rigid.position;

        if (vertical)
        {
            position.y += speed * direction * Time.deltaTime;
            anim.SetFloat("Move X", 0);
            anim.SetFloat("Move Y", direction);
        }

        else
        {
            position.x += speed * direction * Time.deltaTime;
            anim.SetFloat("Move X", direction);
            anim.SetFloat("Move Y", 0);
        }

        rigid.MovePosition(position);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        RubyController ruby = collision.gameObject.GetComponent<RubyController>();

        if (ruby != null)
        {
            ruby.ChangeHealth(-1);
        }
    }

    public void Fix()
    {
        broken = false;
        rigid.simulated = false;

        anim.SetTrigger("Fixed");

        smokeEffect.Stop();
    }

}
