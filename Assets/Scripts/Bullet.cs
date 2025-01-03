using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public int per; // �����

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;
        this.per = per;

        if (per > -1) // ������ ������ �ƴ� ��(���Ÿ� ������ ���)
        {
            rb.velocity = dir * 15f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {   // ���� �ƴϰų� ���� ������ ��� ����
        if (!collision.CompareTag("Enemy") || per == -1)
            return;

        per--; // ���� �����ϸ� ����� -1

        if (per == -1)
        {
            rb.velocity = Vector3.zero; // ���� Ǯ������ ������ ���� ���� �ӵ� �ʱ�ȭ
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {   // ���� �Ÿ����� �־����� ��Ȱ��ȭ
        if (other.CompareTag("Area"))
        {
            rb.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }
}