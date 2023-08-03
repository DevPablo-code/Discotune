using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class SettingsMenu : MonoBehaviour
{
    public float defaultMusicVolume;
    public float defaultSFXVolume;
    public bool defaultAimAssistEnabled;

    public TextSlider musicVolumeSlider;
    public TextSlider sfxVolumeSlider;
    public Toggle aimAssistToggle;
    public Button applyButton;

    public AudioMixer audioMixer;

    private float _musicVolume;
    private float _sfxVolume;
    private bool _aimAssistEnabled;

    private bool _applied = false;

    void Start()
    {
        if(!(PlayerPrefs.HasKey("MusicVolume") && PlayerPrefs.HasKey("SFXVolume") && PlayerPrefs.HasKey("AimAssist")))
        {
            ResetToDefault();
            ApplyAndSaveSettings();
        }
        else 
        {
            PullSettings();
            ApplySettings();
        }
         UpdateUI();
    }

    public void Open() 
    {
        gameObject.SetActive(true);
    }
    public void Close()
    {
        gameObject.SetActive(false);
        if(!_applied) 
        {
            PullSettings();
            UpdateUI();
            _applied = true;
            OnApplied();
        }
    }

    private void UpdateUI() 
    {
        musicVolumeSlider.valueText.text = ((int)(_musicVolume * 100)).ToString();
        musicVolumeSlider.slider.SetValueWithoutNotify(_musicVolume);
        sfxVolumeSlider.valueText.text = ((int)(_sfxVolume * 100)).ToString();
        sfxVolumeSlider.slider.SetValueWithoutNotify(_sfxVolume);
        aimAssistToggle.SetIsOnWithoutNotify(_aimAssistEnabled);
        applyButton.interactable = !_applied;
    }
    private void ApplySettings()
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(_musicVolume) * 20);
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(_sfxVolume) * 20);
        _applied = true;
        OnApplied();
    }

    private void PullSettings() 
    {
        _musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        _sfxVolume = PlayerPrefs.GetFloat("SFXVolume");
        _aimAssistEnabled = PlayerPrefs.GetInt("AimAssist") > 0;
    }
    private void SaveSettings() 
    {
        PlayerPrefs.SetFloat("MusicVolume", _musicVolume);
        PlayerPrefs.SetFloat("SFXVolume", _sfxVolume);
        PlayerPrefs.SetInt("AimAssist", _aimAssistEnabled ? 1 : 0);
        PlayerPrefs.Save();
    }
    private void OnApplied() 
    {
        applyButton.interactable = false;
    }
    private void OnModified() 
    {
        if (_applied)
        {
            _applied = false;
            applyButton.interactable = true;
        }
    }

    public void ApplyAndSaveSettings() 
    {
        ApplySettings();
        SaveSettings();
    }

    public void ResetToDefault() 
    {
        OnModified();
        _musicVolume = defaultMusicVolume;
        _sfxVolume = defaultSFXVolume;
        _aimAssistEnabled = defaultAimAssistEnabled;
    }

    public void SetMusicVolume(float volume) 
    {
        OnModified();
        _musicVolume = volume;
        musicVolumeSlider.valueText.text = ((int)(_musicVolume * 100)).ToString();
    }

    public void SetSFXVolume(float volume) 
    {
        OnModified();
        _sfxVolume = volume;
        sfxVolumeSlider.valueText.text = ((int)(_sfxVolume * 100)).ToString();
    }

    public void SetAimAssistEnabled(bool enabled) 
    {
        OnModified();
        _aimAssistEnabled = enabled;
    }
}

