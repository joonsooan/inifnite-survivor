using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public ItemData.ItemType type;
    public float rate;

    public void Init(ItemData data)
    {   // data = Scriptable Object 데이터
        // 기본 세팅
        name = $"Gear {data.itemId}";
        // Parent를 플레이어 오브젝트로 설정
        transform.parent = GameManager.Instance.player.transform;
        transform.localPosition = Vector3.zero;

        // 변수 세팅
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
    {   // 무기 속도 증가
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        foreach (Weapon weapon in weapons)
        {
            switch (weapon.id)
            {
                case 0: // 근접 무기 회전속도 증가
                    weapon.speed = weapon.baseSpeed + (weapon.baseSpeed * rate);
                    break;

                default: // 원거리 무기 연사속도 증가
                    weapon.speed = weapon.baseSpeed * (1f - rate);
                    break;
            }
        }
    }

    private void SpeedUp()
    {   // 플레이어 이동속도 증가
        float speed = GameManager.Instance.player.baseMoveSpeed;
        GameManager.Instance.player.moveSpeed = speed + (speed * rate);
    }
}