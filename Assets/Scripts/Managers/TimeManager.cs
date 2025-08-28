using UnityEngine;

public class TimeManager : MonoBehaviour, IDependencyInjectable, ITimeProvider
{
    GameSceneManager gameSceneManager;
    DialogueManager dialogueManager;
    EventManager eventManager;

    float loopTime = 360f;
    private bool IsTimePaused = false;  
    public float LoopTime
    {
        get => loopTime;
        set => loopTime = value;
    }
    bool changeLoopTime = false;
    public bool ChangeLoopTime
    {
        get => changeLoopTime;
        set => changeLoopTime = value;
    }
    const float DEFAULT_LOOP_TIME = 360f;
    private void Awake()
    {
        InjectDependencies(DependencyContainer.Instance);
    }
    public void InjectDependencies(DependencyContainer provider)
    {
        gameSceneManager = provider.GameSceneManager;
        dialogueManager = provider.DialogueManager;
        eventManager = provider.EventManager;
    }
    private void Start()
    {
        ResetLoopTime();
    }
    private void Update()
    { 
        if (changeLoopTime && loopTime >= 5f)
        {
            LoopTime = 5f;

            ChangeLoopTime = false;
        }
        if (!IsTimePaused)
        {
            AdvanceTime();
        }
        
    }
    public void ResetLoopTime()
    {
        LoopTime = DEFAULT_LOOP_TIME;
    }
    public void SetLoopTime(float newTime)
    {
        LoopTime = newTime;
    }
    
    public void AddTime(float time)
    {
        LoopTime += time;
    }
    public void SetLoopTimeToStopTrain()
    {
        SetLoopTime(250f);
    }
    public void SetLoopTimeToBreakCrystal()
    {
        SetLoopTime(70f);
    }
    private void AdvanceTime()
    {
        loopTime -= Time.deltaTime * Time.timeScale;

        if (loopTime <= 0)
        {
            ResetLoopTime();
            gameSceneManager.LoadNextScene(GameManager.Instance.nextScene);
            dialogueManager.ResetAllQuestions();
            eventManager.InitializeDialogues();
        }
    }

    public void PauseTime(bool pause) => IsTimePaused = pause;
    
    public string returnTimeInMinutes()
    {
        int minutos = Mathf.FloorToInt(LoopTime / 60f);
        int segundos = Mathf.FloorToInt(LoopTime % 60f);

        string resultado = string.Format("{0:00}:{1:00}", minutos, segundos);

        return resultado;
    }
}
public interface ITimeProvider
{
    float LoopTime { get; set; }
    void PauseTime(bool pause);
    void AddTime(float time);
    bool ChangeLoopTime { get; set; }
    void SetLoopTimeToBreakCrystal();
}
public interface ITimeCuttable
{
    void SetLoopTimeToBreakCrystal();
}