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
    #endregion
    public Subject SubjectEventManager { get; private set; }
    public DialogueUI DialogueUI { get; private set; }
    public Parallax Parallax { get; private set; }
    
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        SubjectEventManager = FindFirstObjectByType<Subject>();
        DevelopmentManager = FindFirstObjectByType<DevelopmentManager>();
        UIManager = FindFirstObjectByType<UIManager>();
        DialogueUI = FindFirstObjectByType<DialogueUI>();
        Parallax = FindFirstObjectByType<Parallax>();
        TimeManager = FindFirstObjectByType<TimeManager>();
        GameSceneManager = FindFirstObjectByType<GameSceneManager>();
        GameManager = FindFirstObjectByType<GameManager>();
        DialogueManager = FindFirstObjectByType<DialogueManager>();
    }
}
