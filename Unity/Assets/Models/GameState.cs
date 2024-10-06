using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    public string SessionId { get; set; }
    public string UserName { get; set; }
    public int CurrentLevel { get; set; }
    public int AlgaeCollectedInLevel { get; set; }
    public int AlgaeCollectedTotal { get; set; }
    public int CoinsCollectedInLevel { get; set; }
    public int CoinsCollectedTotal { get; set;}
    //public ShrimpColor ShrimpColor { get; set; }
    public DateTime StartTime { get; set; }
    public float Volume { get; set; }

    public GameState()
    {
        SessionId = Guid.NewGuid().ToString();
        UserName = "test";
        //ShrimpColor = SettingsManager.Instance.Settings.ShrimpColor;
        StartTime = DateTime.UtcNow;
        CurrentLevel = 1;
    }
}
