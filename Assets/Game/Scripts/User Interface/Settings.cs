using System.IO;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterVolume;
    [SerializeField] private Slider musicVolume;
    [SerializeField] private Slider SFXVolume;
    [SerializeField] private Toggle masterMuteToggle;
    [SerializeField] private Toggle musicMuteToggle;
    [SerializeField] private Toggle sfxMuteToggle;

    private string filePath;

    private const float muteVolume = -80f;

    public void Awake()
    {
        string directory = Path.Combine(Application.persistentDataPath, "Settings");
        Directory.CreateDirectory(directory);

        filePath = Path.Combine(directory, "Settings.json");
        LoadSettings();
        ApplyAllVolumes();
    }

    public void ApplyAllVolumes()
    {
        SetMasterVolume();
        SetMusicVolume();
        SetSFXVolume();
    }

    public void Back()
    {
        Debug.Log("Loading Title Menu");
        SceneManager.LoadScene("TitleScreen");
    }

    //Volume Setters

    public void SetMasterVolume()
    {
        audioMixer.SetFloat("Master",  Mathf.Log10(masterVolume.value) * 20);
    }

    public void SetMusicVolume()
    {
        audioMixer.SetFloat("Music", Mathf.Log10(musicVolume.value) * 20);
    }

    public void SetSFXVolume ()
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(SFXVolume.value) * 20);
    }

    //Mute Toggles
    public void ToggleMasterMute()
    {
        if (masterMuteToggle.isOn)
            audioMixer.SetFloat("Master", muteVolume);
        else
            SetMasterVolume();
    }

    public void ToggleMusicMute()
    {
        if (musicMuteToggle.isOn)
            audioMixer.SetFloat("Music", muteVolume);
        else
            SetMusicVolume();
    }

    public void ToggleSFXMute()
    {
        if (sfxMuteToggle.isOn)
            audioMixer.SetFloat("SFX", muteVolume);
        else
            SetSFXVolume();
    }

    //Save and Load Settings

    public void SaveSettings()
    {
        SettingsData data = new SettingsData
        {
            masterVolume = masterVolume.value,
            musicVolume = musicVolume.value,
            SFXVolume = SFXVolume.value,

            masterMuted = masterMuteToggle.isOn,
            musicMuted = musicMuteToggle.isOn,
            sfxMuted = sfxMuteToggle.isOn
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
        Debug.Log("Volume Settings saved to: " + filePath);
    }

    public void LoadSettings()
    {
        if (File.Exists(filePath)){
            string json = File.ReadAllText(filePath); 
            SettingsData loaded = JsonUtility.FromJson<SettingsData>(json);

            masterVolume.value = loaded.masterVolume;
            musicVolume.value = loaded.musicVolume;
            SFXVolume.value = loaded.SFXVolume;

            masterMuteToggle.isOn = loaded.masterMuted;
            musicMuteToggle.isOn = loaded.musicMuted;
            sfxMuteToggle.isOn = loaded.sfxMuted;

            ToggleMasterMute();
            ToggleMusicMute();
            ToggleSFXMute();


            Debug.Log("Volume Settings Loaded!");
        } else
        {
            masterVolume.value = 1f;
            musicVolume.value = 1f;
            SFXVolume.value = 1f;

            masterMuteToggle.isOn = false;
            musicMuteToggle.isOn = false;
            sfxMuteToggle.isOn = false;

            Debug.Log("No settings file found");
        }
    }
}

//Settings Data because I don't really want to make a new script for it atm
[System.Serializable]
public class SettingsData
{
    public float masterVolume;
    public float musicVolume;
    public float SFXVolume;

    public bool masterMuted;
    public bool musicMuted;
    public bool sfxMuted;
}

