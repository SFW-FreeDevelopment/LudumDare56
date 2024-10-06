using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;


public class SettingsManager : GameSingleton<SettingsManager>
{
    public Settings Settings { get; private set; } = new();

    protected override void InitSingletonInstance()
    {
        Load();
    }

    public void Back()
    {
        Save();
        SceneManager.LoadScene("Main Menu");
    }

    public void AdjustVolume(float value)
    {
        Settings.MusicVolume = value;
    }
    
    public void Save()
    {
        var json = JsonConvert.SerializeObject(Settings);
        PlayerPrefs.SetString("Settings", json);
    }

    private void Load()
    {
        if (PlayerPrefs.HasKey("Settings"))
        {
            var json = PlayerPrefs.GetString("Settings");
            try
            {
                Settings = JsonConvert.DeserializeObject<Settings>(json) ?? new Settings();
            }
            catch
            {
                Settings = new Settings();
            }
        }
        else
        {
            Settings = new Settings();
        }
    }
}

