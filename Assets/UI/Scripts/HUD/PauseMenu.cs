using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour
{
    public AudioMixer mainMixer;

    [Header("Slider References")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    // Audio Mixer Groups
    private const string MasterGroup = "MasterVolume";
    private const string MusicGroup = "MusicVolume";
    private const string SFXGroup = "SFXVolume";

    private void InitializeValues()
    {
        float masterVolume = PlayerPrefs.GetFloat(MasterGroup, 1f);
        float musicVolume = PlayerPrefs.GetFloat(MusicGroup, 1f);
        float sfxVolume = PlayerPrefs.GetFloat(SFXGroup, 1f);

        masterVolumeSlider.value = masterVolume;
        musicVolumeSlider.value = musicVolume;
        sfxVolumeSlider.value = sfxVolume;

        ConvertVolumeToDecibels(MasterGroup, masterVolume);
        ConvertVolumeToDecibels(MusicGroup, musicVolume);
        ConvertVolumeToDecibels(SFXGroup, sfxVolume);

        //masterVolumeValueText.text = $"{Mathf.RoundToInt(masterVolume * 100)}%";
       // musicVolumeValueText.text = $"{Mathf.RoundToInt(musicVolume * 100)}%";
       // sfxVolumeValueText.text = $"{Mathf.RoundToInt(sfxVolume * 100)}%";
    }

    private void Update()
    {
        SetMasterVolume(masterVolumeSlider.value);
        SetSFXVolume(sfxVolumeSlider.value);
        SetMusicVolume(musicVolumeSlider.value);
    }

    public void SetMasterVolume(float volume)
    {
        // Converted 0-1 slider to decibels
        PlayerPrefs.SetFloat(MasterGroup, volume);
        ConvertVolumeToDecibels(MasterGroup, volume);
      //  masterVolumeValueText.text = $"{Mathf.RoundToInt(volume * 100)}%";
    }

    public void SetMusicVolume(float volume)
    {
        // Converted 0-1 slider to decibels
        PlayerPrefs.SetFloat(MusicGroup, volume);
        ConvertVolumeToDecibels(MusicGroup, volume);
      //  musicVolumeValueText.text = $"{Mathf.RoundToInt(volume * 100)}%";
    }

    public void SetSFXVolume(float volume)
    {
        // Converted 0-1 slider to decibels
        PlayerPrefs.SetFloat(SFXGroup, volume);
        ConvertVolumeToDecibels(SFXGroup, volume);
      //  sfxVolumeValueText.text = $"{Mathf.RoundToInt(volume * 100)}%";
    }

    private void ConvertVolumeToDecibels(string audioMixerGroup, float volume)
    {
        if (mainMixer == null) return;

        float dB;
        volume = Mathf.Clamp(volume, 0.0001f, 2f);

        if (volume > 0.0001f)
        {
            dB = 20f * Mathf.Log10(volume);
        }
        else
        {
            dB = -80f; // Minimum dB value
        }

        // Set the volume in the AudioMixer
        mainMixer.SetFloat(audioMixerGroup, dB);
    }
}
