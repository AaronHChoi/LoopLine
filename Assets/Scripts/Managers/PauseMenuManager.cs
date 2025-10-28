using DependencyInjection;
using Player;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseMenuManager : Singleton<PauseMenuManager>
{
    bool isCursorVisible = false;
    [Header("Audio Settings")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider bgmVolumeSlider;
    [SerializeField] private AudioMixer audioMixer;
    private Dictionary<AudioSource, float> MasterAudio;

    IGameStateController Controller;

    protected override void Awake()
    {
        base.Awake();
        Controller = InterfaceDependencyInjector.Instance.Resolve<IGameStateController>();
    }

    private void Start()
    {
        masterVolumeSlider.onValueChanged.AddListener(OnVolumeChangedMaster);
        sfxVolumeSlider.onValueChanged.AddListener(OnVolumeChangedSFX);
        bgmVolumeSlider.onValueChanged.AddListener(OnVolumeChangedBgm);
        OnVolumeChangedMaster(1f);
        OnVolumeChangedBgm(1f);
        OnVolumeChangedSFX(1f);
        InitAudios();
    }
    private void OnEnable()
    {
        Controller.OnPauseMenu += UseEventPauseMenu;
        UpdateCursorState();
    }
    private void OnDisable()
    {
        Controller.OnPauseMenu -= UseEventPauseMenu;
        UpdateCursorState();
    }

    public void UseEventPauseMenu()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
    private void InitAudios()
    {
        MasterAudio = new Dictionary<AudioSource, float>();

        var auxAudios = FindObjectsByType<AudioSource>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (var audio in auxAudios)
        {
            MasterAudio.Add(audio, audio.volume);
        }
    }
    private void OnVolumeChangedMaster(float value)
    {
        SetVolume("Master", value);
    }
    private void OnVolumeChangedSFX(float value)
    {
        SetVolume("SFX", value);
    }
    private void OnVolumeChangedBgm(float value)
    {
        SetVolume("BGM", value);
    }
    private void SetVolume(string parameterName, float value)
    {
        float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat(parameterName, dB);

        PlayerPrefs.SetFloat(parameterName, value);
    }
    void UpdateCursorState()
    {
        bool shouldShowCursor = gameObject.activeInHierarchy;

        if (isCursorVisible != shouldShowCursor)
        {
            isCursorVisible = shouldShowCursor;
            Cursor.visible = isCursorVisible;
            Cursor.lockState = isCursorVisible ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
    private void OnDestroy()
    {
        if (masterVolumeSlider != null)
            masterVolumeSlider.onValueChanged.RemoveListener(OnVolumeChangedMaster);
        if (sfxVolumeSlider != null)
            sfxVolumeSlider.onValueChanged.RemoveListener(OnVolumeChangedSFX);
    }
}