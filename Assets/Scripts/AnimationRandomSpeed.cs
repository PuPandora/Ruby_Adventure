using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationRandomSpeed : MonoBehaviour
{
    Animator anim;
    [Tooltip("최소 속도 = X / 최대 속도 Y")]
    public Vector2 randomSpeed = new Vector2(0.5f, 1f);

    [Tooltip("애니메이션의 현재 속도")]
    [SerializeField]
    private float speed;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        speed = Random.Range(randomSpeed.x, randomSpeed.y);
        anim.SetFloat("speed", speed);
    }
}
