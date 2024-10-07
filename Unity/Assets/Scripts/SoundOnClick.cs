using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SoundOnClick : MonoBehaviour
{
    public SoundName soundName = SoundName.Click1;
    
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(PlaySound);
    }

    private void PlaySound()
    {
        Debug.Log($"Audio Manager is {AudioManager.Instance}");
        AudioManager.Instance.Play(soundName);
    }
}