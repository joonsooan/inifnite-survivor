using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Control")]
    public bool gameLive;

    public float gameTime;
    public float maxGameTime;

    [Header("Player Info")]
    public int health;

    public int maxHealth;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600 };

    [Header("Game Object")]
    public PoolManager PoolManager;

    public Player player;
    public LevelUp uiLevelUp;

    private void Awake()
    {
        Instance = this;
        maxGameTime = 100f;
        maxHealth = 100;
    }

    private void Start()
    {
        health = maxHealth;

        // юс╫ц
        uiLevelUp.Select(0);
    }

    private void Update()
    {
        if (!gameLive)
            return;

        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
        }
    }

    public void GetExp()
    {
        exp++;

        if (exp == nextExp[Mathf.Min(level, nextExp.Length - 1)])
        {
            level++;
            exp = 0;
            uiLevelUp.Show();
        }
    }

    public void Stop()
    {
        gameLive = false;
        Time.timeScale = 0;
    }

    public void Resume()
    {
        gameLive = true;
        Time.timeScale = 1;
    }
}