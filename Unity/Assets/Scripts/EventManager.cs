using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager
{
    public static event UnityAction OnAlgaeCollected;
    public static void AlgaeCollected() => OnAlgaeCollected?.Invoke();
}
