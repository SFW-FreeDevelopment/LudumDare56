using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsOnClick : MonoBehaviour
{
    public Slider Slider;
    // Start is called before the first frame update
    public void Back()
    {
        SettingsManager.Instance.Save();
        SceneManager.LoadScene("Main Menu");
    }

    public void VolumeAdjusted()
    {
        SettingsManager.Instance.volume = Slider.value;
        EventManager.VolumeAdjusted();
    }
}
