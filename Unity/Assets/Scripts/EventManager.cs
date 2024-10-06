using UnityEngine.Events;

public class EventManager
{
    public static event UnityAction OnAlgaeCollected;
    public static event UnityAction OnCoinCollected;
    public static event UnityAction OnVolumeAdjusted;
    public static event UnityAction OnColorChange;
    public static event UnityAction OnLevelCompletion;
    public static void AlgaeCollected() => OnAlgaeCollected?.Invoke();
    public static void CoinCollected() => OnCoinCollected?.Invoke();
    public static void VolumeAdjusted() => OnVolumeAdjusted?.Invoke();
    public static void ColorChange() => OnColorChange?.Invoke();
    public static void LevelCompletion() => OnLevelCompletion?.Invoke();
}
