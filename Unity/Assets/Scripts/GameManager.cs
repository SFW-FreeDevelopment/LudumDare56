using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : GameSingleton<GameManager>
{
    private int algaeCount;
    private GameState gameState = new();

    void OnEnable()
    {
        EventManager.OnAlgaeCollected += AlgaeCollected;
        EventManager.OnCoinCollected += CollectCoin;
    }

    void OnDisable()
    {
        EventManager.OnAlgaeCollected -= AlgaeCollected;
        EventManager.OnCoinCollected -= CollectCoin;
    }

    private void CountAlgae()
    {
        GameObject[] algaeObjects = GameObject.FindGameObjectsWithTag("Algae");
        algaeCount = algaeObjects.Length;
    }

    public int GetAlgaeCount()
    {
        return algaeCount;
    }

    public int GetCoinCount()
    {
        return gameState.CoinsCollectedTotal;
    }

    private void Start()
    {
        CountAlgae();
    }

    private void AlgaeCollected()
    {
        gameState.AlgaeCollectedTotal++;
        gameState.AlgaeCollectedInLevel++;
        CheckLevelCompletion();
    }

    private void CollectCoin()
    {
        gameState.CoinsCollectedTotal++;
        gameState.CoinsCollectedInLevel++;
    }

    public int GetTotalAlgae()
    {
       return gameState.AlgaeCollectedTotal;
    }

    private void ResetCollectionCounts()
    {
        gameState.CoinsCollectedInLevel = 0;
        gameState.AlgaeCollectedInLevel = 0;
    }

    private void GoToNextLevel()
    {
        ResetCollectionCounts();
        gameState.CurrentLevel++;
        var level = gameState.CurrentLevel;
        var scene = "";
        if (level == 6 && gameState.CoinsCollectedTotal >= 10)
        {
            scene = "Crab Level";
        }
        else
        {
            scene = ConvertLevelEnumToString(level);
        }

        SceneManager.LoadScene(scene);
    }

    private void CheckLevelCompletion()
    {
        if (gameState.AlgaeCollectedInLevel == algaeCount)
        {
            EventManager.LevelCompletion();
        }
    }

    protected override void InitSingletonInstance()
    {
        Load();
    }

    private string ConvertLevelEnumToString(int level)
    {
        if (Enum.IsDefined(typeof(LevelEnums), level))
        {
            // Casting the integer to LevelEnums to retrieve the enum name
            var levelEnum = (LevelEnums)level;
            // Convert the enum name to the desired format
            return levelEnum.ToString().Replace("_", " "); // Replace underscore with space
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(level), "Invalid level value");
        }
    }

    public void Save()
    {
        var json = JsonConvert.SerializeObject(gameState);
        PlayerPrefs.SetString("GameState", json);
    }

    private void Load()
    {
        if (PlayerPrefs.HasKey("GameState"))
        {
            var json = PlayerPrefs.GetString("GameState");
            try
            {
                gameState = JsonConvert.DeserializeObject<GameState>(json) ?? new GameState();
            }
            catch
            {
                gameState = new GameState();
            }
        }
        else
        {
            gameState = new GameState();
        }
    }

}