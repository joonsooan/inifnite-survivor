using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    public enum InfoType
    {
        Exp,
        Level,
        Kill,
        Time,
        Health
    }

    public InfoType type;

    private TMP_Text myText;
    private Slider mySlider;

    private void Awake()
    {
        myText = GetComponent<TMP_Text>();
        mySlider = GetComponent<Slider>();
    }

    private void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Exp:
                float currExp = GameManager.Instance.exp;
                float maxExp = GameManager.Instance.nextExp[GameManager.Instance.level];
                mySlider.value = currExp / maxExp;
                break;

            case InfoType.Level:
                myText.text = $"Lv.{GameManager.Instance.level:F0}";
                break;

            case InfoType.Kill:
                myText.text = $"{GameManager.Instance.kill:F0}";
                break;

            case InfoType.Time:
                float remainTime = GameManager.Instance.maxGameTime - GameManager.Instance.gameTime;
                int min = (int)(remainTime / 60);
                int sec = (int)(remainTime % 60);
                myText.text = $"{min:D2}:{sec:D2}";
                break;

            case InfoType.Health:
                float currHealth = GameManager.Instance.health;
                float maxHealth = GameManager.Instance.maxHealth;
                mySlider.value = currHealth / maxHealth;
                break;
        }
    }
}