using UnityEngine.Events;

public class EventManager
{
    public static event UnityAction OnAlgaeCollected;
    public static event UnityAction OnCoinCollected;
    public static event UnityAction OnVolumeAdjusted;
    public static void AlgaeCollected() => OnAlgaeCollected?.Invoke();
    public static void CoinCollected() => OnCoinCollected?.Invoke();
    public static void VolumeAdjusted() => OnVolumeAdjusted?.Invoke();
}
