using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    // Component
    Rigidbody2D rigid;
    Animator anim;
    AudioSource audioSource;

    // Projectile
    public GameObject projectilePrefab;
    public float projectileForce = 300.0f;
    // Move
    public float speed = 3.0f;
    Vector2 lookDirection = new Vector2(0, 1);
    // HP
    public int maxHealth = 5;
    public int health { get { return currentHealth; } }
    private int currentHealth;
    // Invincible
    public float timeInvincible = 2.0f;
    private bool isInvincible;
    private float invincibleTimer;
    // Audio
    public AudioClip hitSound;
    public AudioClip throwCogSound;
    public AudioClip footstepSound;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        currentHealth = maxHealth;
    }

    void Update()
    {
        // Input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Animation
        Vector2 move = new Vector3(horizontal, vertical);
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
        anim.SetFloat("Look X", lookDirection.x);
        anim.SetFloat("Look Y", lookDirection.y);
        anim.SetFloat("Speed", move.magnitude);

        // Move
        Vector2 position = rigid.position;
        position += move.normalized * speed * Time.deltaTime;
        rigid.MovePosition(position);

        // Invincible
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        // Attack, Throw a cog
        if (Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }

        // Interact, Dialogue
        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(
                rigid.position + Vector2.up * 0.2f,
                lookDirection,
                1.5f,
                LayerMask.GetMask("NPC")
                );

            NonPlayerCharacter npc = hit.collider.GetComponent<NonPlayerCharacter>();
            if (npc != null)
            {
                npc.DisplayDialog();
            }
        }
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;

            anim.SetTrigger("Hit");

            PlaySound(hitSound);
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        // Health Bar UI Update
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }

    private void Launch()
    {
        GameObject projectileObject =
            Instantiate(
                projectilePrefab,
                rigid.position + Vector2.up * 0.5f,
                Quaternion.identity
                );

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, projectileForce);

        PlaySound(throwCogSound);
        
        anim.SetTrigger("Launch");
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
