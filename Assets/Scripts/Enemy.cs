using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed;
    public float health;
    public float maxHealth;
    public RuntimeAnimatorController[] animController;
    public Rigidbody2D target;

    private bool isLive;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private WaitForFixedUpdate wait;
    private float knockBackForce = 3f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
    }

    private void FixedUpdate()
    {
        if (!isLive) // ���� ���� ��쿡 ����
            return;

        Vector2 dirVec = target.position - rb.position;
        Vector2 nextVec = dirVec.normalized * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + nextVec);
        rb.velocity = Vector2.zero; // �ܺ� ���� �̵��� ������ ���� �ʵ��� �ӵ� ����
    }

    private void LateUpdate()
    {
        if (!isLive)
            return;

        sr.flipX = target.position.x < rb.position.x;
    }

    private void OnEnable()
    {
        target = GameManager.Instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        health = maxHealth;
    }

    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animController[data.spriteType];
        moveSpeed = data.moveSpeed;
        maxHealth = data.health;
        health = data.health;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet"))
            return;

        health -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack());

        if (health > 0)
        {
            anim.SetTrigger("Hit");
        }
        else
        {
            Dead();
        }
    }

    private IEnumerator KnockBack()
    {
        yield return wait; // ���� �ϳ��� ���� ������ ������
        Vector3 playerPos = GameManager.Instance.player.transform.position;
        Vector3 dir = transform.position - playerPos;
        rb.AddForce(dir.normalized * knockBackForce, ForceMode2D.Impulse);
    }

    private void Dead()
    {
        gameObject.SetActive(false);
        isLive = false;
    }
}