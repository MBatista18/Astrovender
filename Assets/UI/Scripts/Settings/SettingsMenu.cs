using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer mainMixer;

    [Header("Parent References")]
    [SerializeField] private GameObject buttonsParent;
    [SerializeField] private GameObject settingsParent;

    [Header("Value References")]
    [SerializeField] private TextMeshProUGUI masterVolumeValueText;
    [SerializeField] private TextMeshProUGUI musicVolumeValueText;
    [SerializeField] private TextMeshProUGUI sfxVolumeValueText;

    private void Awake()
    {
        InitializeValues();
        buttonsParent.SetActive(true);
        settingsParent.SetActive(false);
    }

    public void OpenSettings()
    {
        InitializeValues();
        buttonsParent.SetActive(false);
        settingsParent.SetActive(true);
    }

    public void CloseSettings()
    {
        buttonsParent.SetActive(true);
        settingsParent.SetActive(false);
    }

    private void InitializeValues()
    {
        float masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        mainMixer.SetFloat("MasterVolume", Mathf.Log10(masterVolume) * 20f);
        mainMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20f);
        mainMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume) * 20f);

        masterVolumeValueText.text = $"{Mathf.RoundToInt(masterVolume * 100)}%";
        musicVolumeValueText.text = $"{Mathf.RoundToInt(musicVolume * 100)}%";
        sfxVolumeValueText.text = $"{Mathf.RoundToInt(sfxVolume * 100)}%";
    }

    public void SetMasterVolume(float volume)
    {
        // Converted 0-1 slider to decibels
        PlayerPrefs.SetFloat("MasterVolume", volume);
        mainMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20f);
        masterVolumeValueText.text = $"{Mathf.RoundToInt(volume * 100)}%";
    }

    public void SetMusicVolume(float volume)
    {
        // Converted 0-1 slider to decibels
        PlayerPrefs.SetFloat("MusicVolume", volume);
        mainMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20f);
        musicVolumeValueText.text = $"{Mathf.RoundToInt(volume * 100)}%";
    }

    public void SetSFXVolume(float volume)
    {
        // Converted 0-1 slider to decibels
        PlayerPrefs.SetFloat("SFXVolume", volume);
        mainMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20f);
        sfxVolumeValueText.text = $"{Mathf.RoundToInt(volume * 100)}%";
    }
}
