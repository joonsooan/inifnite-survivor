using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ItemData;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;
    [HideInInspector] public float baseSpeed;

    private float timer;
    private Player player;

    private void Awake()
    {
        player = GameManager.Instance.player;
    }

    private void Update()
    {
        switch (id)
        {
            case 0: // 근접 무기
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;

            default: // 원거리 무기
                timer += Time.deltaTime;

                if (timer > speed)
                {
                    timer = 0f;
                    Fire();
                }
                break;
        }
    }

    public void LevelUp(float damage, int count)
    {
        this.damage = damage;
        // count
        // 1. 근접 무기인 경우: 무기 개수 증가
        // 2. 원거리 무기인 경우: 관통 횟수 증가
        this.count += count;

        if (id == 0) // 근접 무기
            Batch();

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    public void Init(ItemData data)
    {   // data = Scriptable Object 데이터
        // 기본 세팅
        name = $"Weapon {data.itemId}";
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        // 변수 세팅
        id = data.itemId;
        damage = data.baseDamage;
        count = data.baseCount;

        for (int i = 0; i < GameManager.Instance.PoolManager.prefabs.Length; i++)
        {
            if (data.projectile == GameManager.Instance.PoolManager.prefabs[i])
            {
                prefabId = i; // 발사체 프리팹 인덱스 설정
                break;
            }
        }

        switch (id)
        {
            case 0:
                speed = 150; // 근접 무기 회전속도
                baseSpeed = speed;
                Batch();
                break;

            default:
                speed = 0.5f; // 원거리 무기 발사속도
                baseSpeed = speed;
                break;
        }

        // Hand Set
        Hand hand = player.hands[(int)data.itemType];
        hand.sr.sprite = data.hand;
        hand.gameObject.SetActive(true);

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
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

        bullet.position = player.hands[(int)ItemType.Range].muzzle.position;

        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage, count, dir);
    }
}