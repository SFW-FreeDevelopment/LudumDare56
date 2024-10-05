using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager
{
    public static event UnityAction OnAlgaeCollected;
    public static event UnityAction OnCoinCollected;
    public static void AlgaeCollected() => OnAlgaeCollected?.Invoke();
    public static void CoinCollected() => OnCoinCollected?.Invoke();
}
