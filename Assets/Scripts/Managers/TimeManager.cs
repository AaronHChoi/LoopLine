
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour, IDependencyInjectable
{
    [SerializeField] public float LoopTime { get; private set; } = 360f;
    [SerializeField] float secondsPunishForSkip = 5f;

    public bool changeLoopTime = false;
    public bool AllowFastForward { get; private set; } = false;

    public int TimeMultiplier { get; private set; } = 4;

    private const int TIME_DEFAULT = 1;

    DialogueUI dialogueUI;
    Parallax parallax;
    private void Awake()
    {
        InjectDependencies(DependencyContainer.Instance);
    }
    public void InjectDependencies(DependencyContainer provider)
    {
        dialogueUI = provider.DialogueUI;
        parallax = provider.Parallax;
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
    public void SetLoopTimeToStopTrain()
    {
        LoopTime = 250f;
    }

    public void SetLoopTimeToBreakCrystal()
    {
        LoopTime = 70f;
    }
    private void TimeForward()
    {
        if (SceneManager.GetActiveScene().name == "Train" && AllowFastForward)
        {
            float speedMultiplier = TIME_DEFAULT;

            if (dialogueUI != null && dialogueUI.Dialogue != null)
            {
                speedMultiplier = dialogueUI.Dialogue.Skipeable && Input.GetKey(KeyCode.F) ? TimeMultiplier : TIME_DEFAULT;
            }

            AdjustGameSpeed(speedMultiplier);

            if (parallax != null)
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
            GameManager.Instance.LoadNextScene(GameManager.Instance.nextScene);
        }
    }
    public void SkipDialogue()
    {
        LoopTime -= secondsPunishForSkip;

        if(LoopTime < 0)
        {
            LoopTime = 0;
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
    public int GetTimeMultiplier()
    {
        return TimeMultiplier;
    }
    public bool GetAllowFastForward()
    {
        return AllowFastForward;
    }
}
