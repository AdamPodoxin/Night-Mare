using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.PostProcessing;
using StarterAssets;

public class SettingsManager : MonoBehaviour
{
    public Settings settings;

    [Space]
    [Header("Video")]
    public PostProcessVolume[] normalVolumes;
    public PostProcessVolume[] nightmareVolumes;

    [Space]

    private Color darkest = new Color(0.047f, 0.047f, 0.047f, 1f);
    private Color brightest = new Color(0.236f, 0.236f, 0.236f, 1f);

    [Header("Audio")]
    public AudioMixer sfxMixer;
    public AudioMixer voiceMixer;
    public AudioMixer musicMixer;

    [Header("Controls")]
    public FirstPersonController[] fps;

    [Header("Gameplay")]
    public GameObject[] subtitles;
    public GameSubtitles gameSubtitles;

    public GameObject[] crosshairs;

    [Space]

    [SerializeField] private SettingsMenu settingsMenu;
    private DataManager dataManager;

    [Space]

    public float _normalBloomIntensity = 5f;
    public float _nightmareBloomIntensity = 10f;
    public float _motionBlurIntensity = 200f;

    private Light[] _lights;

    private void Awake()
    {
        dataManager = FindObjectOfType<DataManager>();

        _lights = FindObjectsOfType<Light>();

        settings = dataManager.GetSettings();
        ApplySettings();

        if (settingsMenu != null) settingsMenu.Populate();
    }

    public void SetVideoSettings(VideoSettings videoSettings) => settings.video = videoSettings;
    public void SetAudioSettings(AudioSettings audioSettings) => settings.audio = audioSettings;
    public void SetControlsSettings(ControlsSettings controlsSettings) => settings.controls = controlsSettings;
    public void SetGameplaySettings(GameplaySettings gameplaySettings) => settings.gameplay = gameplaySettings;

    private static LightShadows IndexToLightShadow(int i)
    {
        return i switch
        {
            0 => LightShadows.None,
            1 => LightShadows.Hard,
            2 => LightShadows.Soft,
            _ => LightShadows.None,
        };
    }

    private void ApplyVideo()
    {
        Screen.SetResolution(settings.video.width, settings.video.height, settings.video.fullscreen, settings.video.framerate);
        QualitySettings.SetQualityLevel(settings.video.qualityIndex);

        var shadowType = IndexToLightShadow(settings.video.shadowsIndex);
        foreach (Light light in _lights)
        {
            light.shadows = shadowType;
        }

        QualitySettings.vSyncCount = settings.video.vsync ? 1 : 0;

        Bloom bloom;

        if (normalVolumes.Length > 0)
        {
            foreach (PostProcessVolume ppv in normalVolumes)
            {
                ppv.profile.TryGetSettings(out bloom);
                bloom.intensity.value = settings.video.bloom ? _normalBloomIntensity : 0f;
            }
        }

        if (nightmareVolumes.Length > 0)
        {
            foreach (PostProcessVolume ppv in nightmareVolumes)
            {
                ppv.profile.TryGetSettings(out bloom);
                ppv.profile.TryGetSettings(out MotionBlur motionBlur);
                bloom.intensity.value = settings.video.bloom ? _nightmareBloomIntensity : 0f;
                motionBlur.shutterAngle.value = settings.video.motionBlur ? _motionBlurIntensity : 0f;
            }
        }

        RenderSettings.ambientLight = Color.Lerp(darkest, brightest, settings.video.brightness);
    }

    private void ApplyAudio()
    {
        AudioListener.volume = settings.audio.masterVolume;

        sfxMixer.SetFloat("Volume", Mathf.Log10(settings.audio.sfxVolume) * 20f);
        voiceMixer.SetFloat("Volume", Mathf.Log10(settings.audio.voiceVolume) * 20f);
        musicMixer.SetFloat("Volume", Mathf.Log10(settings.audio.musicVolume) * 20f);
    }

    private void ApplyControls()
    {
        foreach (FirstPersonController f in fps)
        {
            f.RotationSpeed = settings.controls.sensitivity;
        }
    }

    private void ApplyGameplay()
    {
        foreach (FirstPersonController f in fps)
        {
            f.RotationSpeed = settings.controls.sensitivity;
        }

        foreach (GameObject g in subtitles)
        {
            g.SetActive(settings.gameplay.subtitles);
        }

        if (gameSubtitles != null) gameSubtitles.UseSubtitles = settings.gameplay.subtitles;

        foreach (GameObject g in crosshairs)
        {
            g.SetActive(settings.gameplay.showCrosshair);
        }
    }

    public void ApplySettings()
    {
        ApplyVideo();
        ApplyAudio();
        ApplyControls();
        ApplyGameplay();

        if (settingsMenu != null) settingsMenu.Populate();
        SaveSettings();
    }

    public void UpdateBrightness(float brightness)
    {
        RenderSettings.ambientLight = Color.Lerp(darkest, brightest, brightness);
        settings.video.brightness = brightness;
    }

    public void Revert()
    {
        SetVideoSettings(VideoSettings.Default());
        SetAudioSettings(AudioSettings.Default());
        SetControlsSettings(ControlsSettings.Default());
        SetGameplaySettings(GameplaySettings.Default());

        ApplySettings();
    }

    public void SaveSettings()
    {
        dataManager.SetSettings(settings);
    }

    public void ResetProgress()
    {
        dataManager.ResetProgress();
    }
}

[System.Serializable]
public class Settings
{
    public VideoSettings video;
    public AudioSettings audio;
    public ControlsSettings controls;
    public GameplaySettings gameplay;

    public Settings(VideoSettings video, AudioSettings audio, ControlsSettings controls, GameplaySettings gameplay)
    {
        this.video = video;
        this.audio = audio;
        this.controls = controls;
        this.gameplay = gameplay;
    }

    public static Settings Default()
    {
        return new Settings(VideoSettings.Default(), AudioSettings.Default(), ControlsSettings.Default(), GameplaySettings.Default());
    }
}

[System.Serializable]
public class VideoSettings
{
    public int width;
    public int height;
    public bool fullscreen;
    public int qualityIndex;
    public int shadowsIndex;
    public int framerate;
    public bool vsync;
    public bool bloom;
    public bool motionBlur;
    public float brightness;

    public VideoSettings() { }
    public VideoSettings(int width, int height, bool fullscreen, int qualityIndex, int shadowsIndex, int framerate, bool vsync, bool bloom, bool motionBlur, float brightness)
    {
        this.width = width;
        this.height = height;
        this.fullscreen = fullscreen;
        this.qualityIndex = qualityIndex;
        this.shadowsIndex = shadowsIndex;
        this.framerate = framerate;
        this.vsync = vsync;
        this.bloom = bloom;
        this.motionBlur = motionBlur;
        this.brightness = brightness;
    }

    public static VideoSettings Default()
    {
        return new VideoSettings(Screen.currentResolution.width, Screen.currentResolution.height, true, QualitySettings.GetQualityLevel(), 2, 60, false, true, true, 0f);
    }
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

    public static AudioSettings Default()
    {
        return new AudioSettings(1f, 1f, 1f, 1f);
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

    public static ControlsSettings Default()
    {
        return new ControlsSettings(0.35f);
    }
}

[System.Serializable]
public class GameplaySettings
{
    public bool subtitles;
    public bool showCrosshair;

    public GameplaySettings()
    {

    }

    public GameplaySettings(bool subtitles, bool showCrosshair)
    {
        this.subtitles = subtitles;
        this.showCrosshair = showCrosshair;
    }

    public static GameplaySettings Default()
    {
        return new GameplaySettings(true, true);
    }
}
