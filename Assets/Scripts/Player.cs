using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    public float baseMoveSpeed;
    public Vector2 inputVec;
    public Scanner scanner;
    public Hand[] hands;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;

    private void Awake()
    {
        baseMoveSpeed = moveSpeed;
        scanner = GetComponent<Scanner>();
        hands = GetComponentsInChildren<Hand>(true);
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Vector2 nextVec = inputVec * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + nextVec); // ���� ��ġ�� ���͸� ���� ��ġ�� �̵�
    }

    private void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }

    private void LateUpdate()
    {
        anim.SetFloat("Speed", inputVec.magnitude); // ������ ũ�⸸ ����

        if (inputVec.x != 0)
        {
            sr.flipX = inputVec.x < 0;
        }
    }
}