using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    public Settings settings;

    [Space]
    [Header("Audio")]
    public AudioMixer sfxMixer;
    public AudioMixer voiceMixer;
    public AudioMixer musicMixer;

    [Header("Controls")]
    public FirstPersonController[] fps;

    public void SetVideoSettings(VideoSettings videoSettings)
    {
        settings.video = videoSettings;
    }

    public void SetAudioSettings(AudioSettings audioSettings)
    {
        settings.audio = audioSettings;
    }

    public void SetControlsSettings(ControlsSettings controlsSettings)
    {
        settings.controls = controlsSettings;
    }

    public void SetGameplaySettings(GameplaySettings gameplaySettings)
    {
        settings.gameplay = gameplaySettings;
    }

    public void ApplySettings()
    {
        //Video


        //Audio
        AudioListener.volume = settings.audio.masterVolume;

        sfxMixer.SetFloat("Volume", Mathf.Log10(settings.audio.sfxVolume) * 20f);
        voiceMixer.SetFloat("Volume", Mathf.Log10(settings.audio.voiceVolume) * 20f);
        musicMixer.SetFloat("Volume", Mathf.Log10(settings.audio.musicVolume) * 20f);

        //Controls
        foreach (FirstPersonController f in fps)
        {
            f.RotationSpeed = settings.controls.sensitivity;
        }

        //Gameplay

    }
}

[System.Serializable]
public struct Settings
{
    public VideoSettings video;
    public AudioSettings audio;
    public ControlsSettings controls;
    public GameplaySettings gameplay;
}

[System.Serializable]
public class VideoSettings
{

}

[System.Serializable]
public class AudioSettings
{
    public float masterVolume;
    public float sfxVolume;
    public float voiceVolume;
    public float musicVolume;

    public AudioSettings()
    {

    }

    public AudioSettings(float masterVolume, float sfxVolume, float voiceVolume, float musicVolume)
    {
        this.masterVolume = masterVolume;
        this.sfxVolume = sfxVolume;
        this.voiceVolume = voiceVolume;
        this.musicVolume = musicVolume;
    }

    public AudioSettings Default()
    {
        return new AudioSettings(1f, 0f, 0f, 0f);
    }
}

[System.Serializable]
public class ControlsSettings
{
    public float sensitivity;

    public ControlsSettings()
    {

    }

    public ControlsSettings(float sensitivity)
    {
        this.sensitivity = sensitivity;
    }

    public ControlsSettings Default()
    {
        return new ControlsSettings(0.35f);
    }
}

[System.Serializable]
public class GameplaySettings
{
    public bool subtitles;

    public GameplaySettings()
    {

    }

    public GameplaySettings(bool subtitles)
    {
        this.subtitles = subtitles;
    }

    public GameplaySettings Default()
    {
        return new GameplaySettings(true);
    }
}
