using Unity.Cinemachine.Samples;
using Unity.Cinemachine;
using UnityEngine;

public class DependencyContainer : MonoBehaviour
{
    public static DependencyContainer Instance { get; private set; }

    #region MANAGERS
    public GameManager GameManager { get; private set; }
    public UIManager UIManager { get; private set; }
    public DialogueManager DialogueManager { get; private set; }
    public TimeManager TimeManager { get; private set; }
    public GameSceneManager GameSceneManager { get; private set; }
    public DevelopmentManager DevelopmentManager { get; private set; }
    public Subject SubjectEventManager { get; private set; }
    public QuestionManager QuestionManager { get; private set; }
    public EventManager EventManager { get; private set; }
    public SoundManager SoundManager { get; private set; }
    public NoteBookManager NoteBookManager { get; private set; }
    #endregion

    #region PLAYER
    public PlayerController PlayerController { get; private set; }
    public PlayerInputHandler PlayerInputHandler { get; private set; }
    public PlayerCamera PlayerCamera { get; private set; }
    public PlayerView PlayerView { get; private set; }
    #endregion
    public DialogueUI DialogueUI { get; private set; }
    public Parallax Parallax { get; private set; }
    public CinemachineCamera CinemachineCamera { get; private set; }
    public CinemachinePOVExtension CinemachinePOVExtension { get; private set; }

    public FocusModeManager FocusModeManager { get; private set; }


    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        InitializeDependencies();
    }
    private void InitializeDependencies()
    {
        SubjectEventManager = FindAndValidate<Subject>();
        DevelopmentManager = FindAndValidate<DevelopmentManager>();
        UIManager = FindAndValidate<UIManager>();
        DialogueUI = FindAndValidate<DialogueUI>();
        Parallax = FindAndValidate<Parallax>();
        TimeManager = FindAndValidate<TimeManager>();
        GameSceneManager = FindAndValidate<GameSceneManager>();
        GameManager = FindAndValidate<GameManager>();
        DialogueManager = FindAndValidate<DialogueManager>();
        PlayerController = FindAndValidate<PlayerController>();
        QuestionManager = FindAndValidate<QuestionManager>();
        CinemachineCamera = FindAndValidate<CinemachineCamera>();
        CinemachinePOVExtension = FindAndValidate<CinemachinePOVExtension>();
        FocusModeManager = FindAndValidate<FocusModeManager>();
        PlayerInputHandler = FindAndValidate<PlayerInputHandler>();
        PlayerCamera = FindAndValidate<PlayerCamera>();
        PlayerView = FindAndValidate<PlayerView>();
        EventManager = FindAndValidate<EventManager>();
        SoundManager = FindAndValidate<SoundManager>();
        NoteBookManager = FindAndValidate<NoteBookManager>();
    }
    private T FindAndValidate<T>() where T : MonoBehaviour
    {
        T instance = FindFirstObjectByType<T>();

        return instance;
    }
}
