using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSettings : MonoBehaviour
{
    public AudioMixer myMixer;
    public Slider bgmSlider;
    public Slider sfxSlider;

    private void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume") && PlayerPrefs.HasKey("SFXVolume"))
        {
            LoadVolume();
        }
        else
        {
            bgmSlider.value = 0.2f; // Default 50%
            sfxSlider.value = 0.2f; // Default 50%
            SetMusicVolume();
            SetSFXVolume();
        }
    }

    public void SetMusicVolume()
    {
        float volume = bgmSlider.value;
        myMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        myMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    private void LoadVolume()
    {
        bgmSlider.value = PlayerPrefs.GetFloat("musicVolume", 0.2f); // Default to 50% if not found
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.2f);  // Default to 50% if not found
        SetMusicVolume();
        SetSFXVolume();
    }
}