using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public bool isLeft;
    public SpriteRenderer sr;
    public Transform muzzle;

    private SpriteRenderer player;
    private Quaternion leftRot = Quaternion.Euler(0, 0, -30);
    private Quaternion leftRotReverse = Quaternion.Euler(0, 0, -150);
    private Vector3 rightPos = new Vector3(0.35f, -0.15f, 0f);
    private Vector3 rightPosReverse = new Vector3(-0.15f, -0.15f, 0f);
    private Vector3 muzzlePos = new Vector3(0.75f, 0.1f, 0f);
    private Vector3 muzzlePosReverse = new Vector3(-0.75f, 0.1f, 0f);

    private void Awake()
    {
        player = GetComponentsInParent<SpriteRenderer>()[1];
    }

    private void LateUpdate()
    {
        bool isReverse = player.flipX;

        if (isLeft) // 근접 무기
        {
            transform.localRotation = isReverse ? leftRotReverse : leftRot;
            sr.flipY = isReverse;
            sr.sortingOrder = isReverse ? 4 : 6;
        }
        else // 원거리 무기
        {
            transform.localPosition = isReverse ? rightPosReverse : rightPos;
            muzzle.localPosition = isReverse ? muzzlePosReverse : muzzlePos;
            sr.flipX = isReverse;
            sr.sortingOrder = isReverse ? 6 : 4;
        }
    }
}