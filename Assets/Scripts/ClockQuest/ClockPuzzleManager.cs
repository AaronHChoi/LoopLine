using SoundSystem;
using UnityEngine;

public class ClockPuzzleManager : MonoBehaviour
{
    [SerializeField] Clock clock;

    [SerializeField] int targetHour;
    [SerializeField] int targetMinute;
    [SerializeField] GenericMove doorHandlerBase;
    [SerializeField] SoundData complete;

    bool firstTime = false;

    private void Start()
    {
        if (GameManager.Instance.GetCondition(GameCondition.IsClockQuestComplete))
        {
            doorHandlerBase.Move();
        }
    }
    private void OnEnable()
    {
        if(clock != null)
        {
            clock.OnExitClock += CheckTime;
        }
    }
    private void OnDisable()
    {
        if (clock != null)
        {
            clock.OnExitClock -= CheckTime;
        }
    }
    public void CheckTime()
    {
        string currentTime = clock.GetClockTime();
        string[] timeParts = currentTime.Split(':');

        int currenHour = int.Parse(timeParts[0]);
        int currentMinute = int.Parse(timeParts[1]);

        if (currenHour == targetHour && currentMinute == targetMinute && !firstTime)
        {
            RevealObject();

            SoundManager.Instance.CreateSound()
                .WithSoundData(complete)
                .WithSoundPosition(transform.position)
                .Play();

            firstTime = true;
        }
    }
    private void RevealObject()
    {
        DelayUtility.Instance.Delay(1f, null);
        doorHandlerBase.Move();
        GameManager.Instance.SetCondition(GameCondition.IsClockQuestComplete, true);
    }
}