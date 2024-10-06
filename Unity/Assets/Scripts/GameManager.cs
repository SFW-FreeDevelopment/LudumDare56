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

    private void ResetCollectionCounts()
    {
        gameState.CoinsCollectedInLevel = 0;
        gameState.AlgaeCollectedInLevel = 0;
    }

    private void GoToNextLevel()
    {
        gameState.CurrentLevel++;
        var level = gameState.CurrentLevel;
        var scene = ConvertLevelEnumToString(level);
        SceneManager.LoadScene(scene);
    }

    private void CheckLevelCompletion()
    {
        if (gameState.AlgaeCollectedInLevel >= 3)
        {
            ResetCollectionCounts();
            GoToNextLevel();
        }
    }

    protected override void InitSingletonInstance()
    {
        gameState = new GameState();
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

}