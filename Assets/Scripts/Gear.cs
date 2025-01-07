using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public ItemData.ItemType type;
    public float rate;

    public void Init(ItemData data)
    {   // data = Scriptable Object ������
        // �⺻ ����
        name = $"Gear {data.itemId}";
        // Parent�� �÷��̾� ������Ʈ�� ����
        transform.parent = GameManager.Instance.player.transform;
        transform.localPosition = Vector3.zero;

        // ���� ����
        type = data.itemType;
        rate = data.damages[0];
        ApplyGear();
    }

    public void LevelUp(float rate)
    {
        this.rate = rate;
        ApplyGear();
    }

    private void ApplyGear()
    {
        switch (type)
        {
            case ItemData.ItemType.Glove:
                RateUp();
                break;

            case ItemData.ItemType.Shoe:
                SpeedUp();
                break;
        }
    }

    private void RateUp()
    {   // ���� �ӵ� ����
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        foreach (Weapon weapon in weapons)
        {
            switch (weapon.id)
            {
                case 0: // ���� ���� ȸ���ӵ� ����
                    weapon.speed = weapon.baseSpeed + (weapon.baseSpeed * rate);
                    break;

                default: // ���Ÿ� ���� ����ӵ� ����
                    weapon.speed = weapon.baseSpeed * (1f - rate);
                    break;
            }
        }
    }

    private void SpeedUp()
    {   // �÷��̾� �̵��ӵ� ����
        float speed = GameManager.Instance.player.baseMoveSpeed;
        GameManager.Instance.player.moveSpeed = speed + (speed * rate);
    }
}