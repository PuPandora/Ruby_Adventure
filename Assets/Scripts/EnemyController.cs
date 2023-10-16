using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    RubyController ruby;
    
    private enum EStartDirection { None, Left, Right, Random }

    // Move
    public float speed = 3.0f;
    public bool vertical;
    public float changeTime;
    public Vector2 randomChangeTime;
    [Tooltip("None : 초기 값. 변경하지 않는다면 왼쪽\nLeft : 왼쪽\nRight : 오른쪽\nRandom : 50% 확률 왼쪽/오른쪽")]
    [SerializeField]
    private EStartDirection startDirection;
    private float timer;
    private int direction = 1;
    // Broken
    private bool broken = true;
    public ParticleSystem smokeEffect;
    // Audio
    public AudioClip[] hitSound;
    public AudioClip fixedSound;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // Set move time
        changeTime = Random.Range(randomChangeTime.x, randomChangeTime.y);
        timer = changeTime;

        SetStartDirection();
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

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (ruby == null)
            {
                ruby = other.gameObject.GetComponent<RubyController>();
            }

            ruby.ChangeHealth(-1);
        }  
    }

    private void SetStartDirection()
    {
        switch (startDirection)
        {
            case EStartDirection.None:
                Debug.LogWarning("로봇의 시작 방향이 설정되지 않았습니다.", gameObject);
                direction = 1;
                break;

            case EStartDirection.Left:
                direction = -1;
                break;

            case EStartDirection.Right:
                direction = 1;
                break;

            case EStartDirection.Random:
                direction = 0;
                while (direction == 0)
                {
                    direction = Random.Range(-1, 2);
                }
                break;
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
