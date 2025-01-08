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
            case 0: // ���� ����
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;

            default: // ���Ÿ� ����
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
        // 1. ���� ������ ���: ���� ���� ����
        // 2. ���Ÿ� ������ ���: ���� Ƚ�� ����
        this.count += count;

        if (id == 0) // ���� ����
            Batch();

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    public void Init(ItemData data)
    {   // data = Scriptable Object ������
        // �⺻ ����
        name = $"Weapon {data.itemId}";
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        // ���� ����
        id = data.itemId;
        damage = data.baseDamage;
        count = data.baseCount;

        for (int i = 0; i < GameManager.Instance.PoolManager.prefabs.Length; i++)
        {
            if (data.projectile == GameManager.Instance.PoolManager.prefabs[i])
            {
                prefabId = i; // �߻�ü ������ �ε��� ����
                break;
            }
        }

        switch (id)
        {
            case 0:
                speed = 150; // ���� ���� ȸ���ӵ�
                baseSpeed = speed;
                Batch();
                break;

            default:
                speed = 0.5f; // ���Ÿ� ���� �߻�ӵ�
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

            // ���� �� ��ȯ�Ǵ� ��ġ (�ð�������� �ϳ��� ������)
            Vector3 rotVec = Vector3.forward * 360 * i / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);
            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero); // -1 : Infinity Per
        }
    }

    private void Fire()
    {
        if (!player.scanner.nearestTarget) // nearestTarget�� null�̶��
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position; // ���� ����� �� ������Ʈ ��ġ
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        // �Ѿ� ����
        Transform bullet = GameManager.Instance.PoolManager.Get(prefabId).transform;

        bullet.position = player.hands[(int)ItemType.Range].muzzle.position;

        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage, count, dir);
    }
}