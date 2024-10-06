using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class MusicVolume : MonoBehaviour
{
    public AudioSource audioSource;

    void OnEnable()
    {
        EventManager.OnVolumeAdjusted += AdjustVolume;
    }

    void OnDisable()
    {
        EventManager.OnVolumeAdjusted -= AdjustVolume;
    }

    private void AdjustVolume()
    {
        audioSource.volume = SettingsManager.Instance.volume;  
    }
}
