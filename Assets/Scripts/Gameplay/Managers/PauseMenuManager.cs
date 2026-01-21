using DependencyInjection;
using Player;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuManager : Singleton<PauseMenuManager>, IPauseMenuManager
{
    [Header("Audio Settings")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider bgmVolumeSlider;
    [SerializeField] private AudioMixer audioMixer;
    private Dictionary<AudioSource, float> MasterAudio;

    [SerializeField] private GameObject pauseMenu;

    IGameStateController gameStateController;
    IPlayerStateController playerStateController;
    IPlayerInputHandler inputHandler;
    IUIManager uiManager;
    protected override void Awake()
    {
        base.Awake();
        gameStateController = InterfaceDependencyInjector.Instance.Resolve<IGameStateController>();
        playerStateController = InterfaceDependencyInjector.Instance.Resolve<IPlayerStateController>();
        inputHandler = InterfaceDependencyInjector.Instance.Resolve<IPlayerInputHandler>();
        uiManager = InterfaceDependencyInjector.Instance.Resolve<IUIManager>();
    }

    private void Start()
    {
        pauseMenu = transform.GetChild(0).gameObject;
        masterVolumeSlider.onValueChanged.AddListener(OnVolumeChangedMaster);
        sfxVolumeSlider.onValueChanged.AddListener(OnVolumeChangedSFX);
        bgmVolumeSlider.onValueChanged.AddListener(OnVolumeChangedBgm);
        OnVolumeChangedMaster(1f);
        OnVolumeChangedBgm(1f);
        OnVolumeChangedSFX(1f);
        InitAudios();
       
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
    
    protected override void OnDestroy()
    {
        if (masterVolumeSlider != null)
            masterVolumeSlider.onValueChanged.RemoveListener(OnVolumeChangedMaster);
        if (sfxVolumeSlider != null)
            sfxVolumeSlider.onValueChanged.RemoveListener(OnVolumeChangedSFX);
    }

    public void LoadMenu()
    {
        //gameStateController.ChangeState(gameStateController.GameplayState);
        //gameStateController.GameplayState.Enter();
        //inputHandler.PauseMenuModePressed();
        GameManager.Instance.SetGameConditions();
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        SceneManager.LoadScene("01. MainMenu");
    }
    public GameObject PauseGameObject()
    {
        return pauseMenu;
    }

}

public interface IPauseMenuManager
{
    GameObject PauseGameObject();
}