using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public Settings settings;
}

[System.Serializable]
public struct Settings
{
    public VideoSettings videoSettings;
    public AudioSettings audioSettings;
    public ControlsSettings controlsSettings;
    public GameplaySettings gameplaySettings;
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

}

[System.Serializable]
public class GameplaySettings
{

}
