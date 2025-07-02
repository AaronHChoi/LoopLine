using System;
using UnityEngine;

public class TimeManager : MonoBehaviour, IDependencyInjectable, ISkipeable
{
    [SerializeField] float secondsPunishForSkip = 5f;
    GameSceneManager gameSceneManager;
    DialogueManager dialogueManager;

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
        }
        if (!IsTimePaused)
        {
            AdvanceTime();
        }
        
    }
    public void ResetLoopTime()
    {
        loopTime = DEFAULT_LOOP_TIME;
    }
    public void SetLoopTime(float newTime)
    {
        loopTime = newTime;
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
        }
    }

    public void PauseTime()
    {
        IsTimePaused = !IsTimePaused;
    }
    public void SkipDialogue()
    {
        loopTime -= secondsPunishForSkip;
    }

    public string returnTimeInMinutes()
    {
        int minutos = Mathf.FloorToInt(loopTime / 60f);
        int segundos = Mathf.FloorToInt(LoopTime % 60f);

        string resultado = string.Format("{0:00}:{1:00}", minutos, segundos);

        return resultado;
    }
}
public interface ISkipeable
{
    void SkipDialogue();
}