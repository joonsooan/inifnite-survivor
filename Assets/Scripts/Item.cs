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
        icon = GetComponentsInChildren<Image>()[1]; // �ڽ��� �̹��� ������Ʈ ������
        icon.sprite = data.itemIcon; // �ڽ� �̹����� ��������Ʈ ����
        TMP_Text[] texts = GetComponentsInChildren<TMP_Text>(); // �ؽ�Ʈ ������ �������� �迭
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

            default: // ü�� ȸ��
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
                {   // ó���̹Ƿ� ���� �߰�
                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.Init(data);
                }
                else
                {   // ������, ���� ���� ���
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
                {   // ��� �߰� �� �ʱ� ��� ������
                    GameObject newGear = new GameObject();
                    gear = newGear.AddComponent<Gear>();
                    gear.Init(data);
                }
                else
                {   // ��� ������
                    float nextRate = data.damages[level];
                    gear.LevelUp(nextRate);
                }
                level++;
                break;

            case ItemData.ItemType.Heal:
                GameManager.Instance.health = GameManager.Instance.maxHealth;
                break;
        }

        if (level == data.damages.Length) // ������ ������ �������� ��
        {
            GetComponent<Button>().interactable = false;
        }
    }
}