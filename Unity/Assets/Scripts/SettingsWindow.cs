using TMPro;
using UnityEngine;

public class SettingsWindow : MonoBehaviour
{
    public TMP_Dropdown dropdown;

    private void OnEnable()
    {
            if (SettingsManager.Instance?.Settings != null)
                    dropdown.value = (int)SettingsManager.Instance.Settings.ShrimpColor;
    }
}
