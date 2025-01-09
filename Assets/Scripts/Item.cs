using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Item : MonoBehaviour
{
    public ItemData data;
    public int level;
    public Weapon weapon;
    public Gear gear;

    private Image icon;
    private TMP_Text textLevel;
    private TMP_Text textName;
    private TMP_Text textDescription;

    private void Awake()
    {
        icon = GetComponentsInChildren<Image>()[1]; // 자식인 이미지 오브젝트 가져옴
        icon.sprite = data.itemIcon; // 자식 이미지의 스프라이트 설정
        TMP_Text[] texts = GetComponentsInChildren<TMP_Text>(); // 텍스트 여러개 생성위한 배열
        textLevel = texts[0];
        textName = texts[1];
        textDescription = texts[2];
        textName.text = data.itemName;
    }

    private void OnEnable()
    {
        textLevel.text = $"Lv.{level + 1}";

        switch (data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                textDescription.text = string.Format(
                    data.itemDescription, data.damages[level] * 100, data.counts[level]);
                break;

            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                textDescription.text = string.Format(
                    data.itemDescription, data.damages[level] * 100);
                break;

            default: // 체력 회복
                textDescription.text = string.Format(data.itemDescription);
                break;
        }
    }

    public void OnClick()
    {
        switch (data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                if (level == 0)
                {   // 처음이므로 무기 추가
                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.Init(data);
                }
                else
                {   // 데미지, 무기 개수 계산
                    float nextDamage = data.baseDamage;
                    int nextCount = 0;

                    nextDamage += data.baseDamage * data.damages[level];
                    nextCount += data.counts[level];

                    weapon.LevelUp(nextDamage, nextCount);
                }
                level++;
                break;

            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                if (level == 0)
                {   // 기어 추가 겸 초기 기어 레벨업
                    GameObject newGear = new GameObject();
                    gear = newGear.AddComponent<Gear>();
                    gear.Init(data);
                }
                else
                {   // 기어 레벨업
                    float nextRate = data.damages[level];
                    gear.LevelUp(nextRate);
                }
                level++;
                break;

            case ItemData.ItemType.Heal:
                GameManager.Instance.health = GameManager.Instance.maxHealth;
                break;
        }

        if (level == data.damages.Length) // 마지막 레벨에 도달했을 때
        {
            GetComponent<Button>().interactable = false;
        }
    }
}