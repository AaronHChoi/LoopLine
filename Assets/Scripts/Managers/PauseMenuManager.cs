using DependencyInjection;
using Player;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour, IScreen
{
    bool isCursorVisible = false;
    [Header("Audio Settings")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private AudioMixer audioMixer;
    private Dictionary<AudioSource, float> MasterAudio;
    private Dictionary<AudioSource, float> sfxAudio;
    private Dictionary<AudioSource, float> MusicAudio;

    IPlayerStateController playerStateController;

    private void Awake()
    {
        playerStateController = InterfaceDependencyInjector.Instance.Resolve<IPlayerStateController>();
    }

    private void Start()
    {
        masterVolumeSlider.onValueChanged.AddListener(OnVolumeChangedMaster);
        sfxVolumeSlider.onValueChanged.AddListener(OnVolumeChangedSFX);
        musicVolumeSlider.onValueChanged.AddListener(OnVolumeChangedMusic);
        InitAudios();
    }
    //private void OnEnable()
    //{
    //    if (playerStateController != null)
    //    {
    //        playerStateController.OnPauseMenu += OnPauseGameMode;
    //    }
    //}
    //private void OnDisable()
    //{
    //    if (playerStateController != null)
    //    {
    //        playerStateController.OnPauseMenu -= OnPauseGameMode;
            
    //    }
    //}
    //public void OnPauseGameMode()
    //{
        
    //}

    public void Activate()
    {
        gameObject.SetActive(true);
        UpdateCursorState();
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        UpdateCursorState();
    }

    public void Free()
    {
        gameObject.SetActive(false);
        UpdateCursorState();
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

    private void OnVolumeChangedMusic(float value)
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
