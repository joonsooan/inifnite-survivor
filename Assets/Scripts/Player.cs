using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
        if (!GameManager.Instance.gameLive)
            return;

        Vector2 nextVec = inputVec * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + nextVec); // 현재 위치에 벡터를 더한 위치로 이동
    }

    private void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }

    private void LateUpdate()
    {
        if (!GameManager.Instance.gameLive)
            return;

        anim.SetFloat("Speed", inputVec.magnitude); // 벡터의 크기만 전달

        if (inputVec.x != 0)
        {
            sr.flipX = inputVec.x < 0;
        }
    }
}