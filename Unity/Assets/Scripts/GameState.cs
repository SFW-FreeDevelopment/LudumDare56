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

    public GameState()
    {
        SessionId = Guid.NewGuid().ToString();
        UserName = "test";
    }
}
