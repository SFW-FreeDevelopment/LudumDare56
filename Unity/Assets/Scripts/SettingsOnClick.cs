using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UI;
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
        var value = ShrimpColor.Pink;

        switch (color)
        {
            case 0:
                value = ShrimpColor.Pink;
                break;
            case 1:
                value = ShrimpColor.Blue;
                break;
            case 2:
                value = ShrimpColor.Red;
                break;
            case 3:
                value = ShrimpColor.Green;
                break;
            case 4:
                value = ShrimpColor.Yellow;
                break;
        }

        SettingsManager.Instance.Settings.ShrimpColor = value;
        EventManager.ColorChange();
    }
}
