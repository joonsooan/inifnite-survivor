using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float moveSpeed;

    private Rigidbody2D rb;
    private Vector2 inputVec;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 nextVec = inputVec * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + nextVec); // 현재 위치에 벡터를 더한 위치로 이동
    }

    private void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }
}