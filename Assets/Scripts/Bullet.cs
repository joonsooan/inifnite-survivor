using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public int per; // 관통력

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;
        this.per = per;

        if (per > -1) // 관통이 무한이 아닐 때(원거리 무기일 경우)
        {
            rb.velocity = dir * 15f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {   // 적이 아니거나 근접 무기일 경우 리턴
        if (!collision.CompareTag("Enemy") || per == -1)
            return;

        per--; // 적과 접촉하면 관통력 -1

        if (per == -1)
        {
            rb.velocity = Vector3.zero; // 추후 풀링으로 재사용할 때를 위해 속도 초기화
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {   // 일정 거리보다 멀어지면 비활성화
        if (other.CompareTag("Area"))
        {
            rb.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }
}