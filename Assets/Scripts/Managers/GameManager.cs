using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private ScreenManager screenManager;

    [SerializeField] public float LoopTime { get; private set; } = 360f;

    [SerializeField] private string nextScene;

    public bool changeLoopTime = false;
    public bool AllowFastForward = false;

    [SerializeField] private int timeMultiplier = 4;
    [SerializeField] DialogueUI dialogueUI;
    [SerializeField] Parallax parallax;
    
    private const int TIME_DEFAULT = 1;
    public ScreenManager ScreenManager => screenManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        dialogueUI = FindFirstObjectByType<DialogueUI>();
        parallax = FindFirstObjectByType<Parallax>();
    }

    private void Start()
    {
        LoopTime = 360f;
    }

    private void Update()
    {
        if (changeLoopTime && LoopTime >= 5f)
        {
            LoopTime = 5f;
        }

        TimeForward();
        AdvanceTime();
    }

    private void LoadNextScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    private void TimeForward()
    {
        if (SceneManager.GetActiveScene().name == "Train" && AllowFastForward)
        {
            float speedMultiplier = TIME_DEFAULT;

            if (dialogueUI != null && dialogueUI.Dialogue != null)
            {
                speedMultiplier = dialogueUI.Dialogue.ReUse && Input.GetKey(KeyCode.F) ? timeMultiplier : TIME_DEFAULT;
            }
           
            AdjustGameSpeed(speedMultiplier);

            if(parallax != null)
            {
                parallax.SetSpeedMultiplier(speedMultiplier);
            }
        }
        else
        {
            AdjustGameSpeed(TIME_DEFAULT);

            if (parallax != null)
            {
                parallax.SetSpeedMultiplier(TIME_DEFAULT);
            }
        }
    }

    private void AdvanceTime()
    {
        LoopTime -= Time.deltaTime * Time.timeScale;

        if (LoopTime <= 0)
        {
            LoopTime = 360f;
            LoadNextScene(nextScene);
        }
    } 
    public void AdjustGameSpeed(float speedMultiplier)
    {
        Time.timeScale = speedMultiplier;
    }
    public void AllowFastForwardMethod(bool enabled)
    {
        AllowFastForward = enabled;
    }
}
