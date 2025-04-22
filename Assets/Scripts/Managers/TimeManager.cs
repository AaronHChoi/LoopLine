using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour
{
    [SerializeField] public float LoopTime { get; private set; } = 360f;

    public bool changeLoopTime = false;
    public bool AllowFastForward = false;

    [SerializeField] private int timeMultiplier = 4;
    [SerializeField] DialogueUI dialogueUI;
    [SerializeField] Parallax parallax;

    private const int TIME_DEFAULT = 1;
    private void Awake()
    {
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
    public void SetLoopTimeToStopTrain()
    {
        LoopTime = 250f;
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
    public void AdjustGameSpeed(float speedMultiplier)
    {
        Time.timeScale = speedMultiplier;
    }
    public void AllowFastForwardMethod(bool enabled)
    {
        AllowFastForward = enabled;
    }
}
