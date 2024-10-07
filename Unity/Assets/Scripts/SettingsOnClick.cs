using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsOnClick : MonoBehaviour
{
    public Slider Slider;
    public TMP_Dropdown dropdown;
    // Start is called before the first frame update
    
    public void Back()
    {
        SettingsManager.Instance.Save();
        SceneManager.LoadScene("Main Menu");
    }

    public void VolumeAdjusted()
    {
        SettingsManager.Instance.Settings.MusicVolume = Slider.value;
        EventManager.VolumeAdjusted();
    }

    public void ColorChanged()
    {
        var color = dropdown.value;

        var value = color switch
        {
            0 => ShrimpColor.Pink,
            1 => ShrimpColor.Purple,
            2 => ShrimpColor.Yellow,
            3 => ShrimpColor.Green,
            4 => ShrimpColor.Magenta,
            5 => ShrimpColor.Blue,
            _ => ShrimpColor.Pink
        };

        SettingsManager.Instance.Settings.ShrimpColor = value;
        SettingsManager.Instance.Save();
        EventManager.ColorChange();
    }
}
