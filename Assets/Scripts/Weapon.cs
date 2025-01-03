using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

    private float timer;
    private Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;

            default: // 근접 무기가 아닐 때
                timer += Time.deltaTime;

                if (timer > speed)
                {
                    timer = 0f;
                    Fire();
                }
                break;
        }

        // .. Test Code
        if (Input.GetButtonDown("Jump"))
        {
            LevelUp(10, 1);
        }
    }

    public void LevelUp(float damage, int count)
    {   // 코드가 조금 야매 느낌이 있는데.. 일단은 이 상태로 진행하기
        // 현재 상황: 근접 무기, 원거리 무기 레벨업이 함께 작동함, 관통력도 함께 올라감
        this.damage = damage;
        this.count += count;

        if (id == 0)
            Batch();
    }

    public void Init()
    {
        switch (id)
        {
            case 0:
                speed = 150; // 근접 무기 회전속도
                Batch();
                break;

            default:
                speed = 0.2f; // 원거리 무기 발사속도
                break;
        }
    }

    private void Batch()
    {
        for (int i = 0; i < count; i++)
        {
            Transform bullet;

            if (i < transform.childCount)
            {
                bullet = transform.GetChild(i);
            }
            else
            {
                bullet = GameManager.Instance.PoolManager.Get(prefabId).transform;
                bullet.parent = transform;
            }

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            // 생성 시 소환되는 위치 (시계방향으로 하나씩 생성됨)
            Vector3 rotVec = Vector3.forward * 360 * i / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);
            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero); // -1 : Infinity Per
        }
    }

    private void Fire()
    {
        if (!player.scanner.nearestTarget) // nearestTarget이 null이라면
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position; // 가장 가까운 적 오브젝트 위치
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        // 총알 생성
        Transform bullet = GameManager.Instance.PoolManager.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage, count, dir);
    }
}