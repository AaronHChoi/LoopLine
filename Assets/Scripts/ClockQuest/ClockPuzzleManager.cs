using System;
using UnityEngine;

public class ClockPuzzleManager : MonoBehaviour, IClockPuzzleManager
{
    [SerializeField] Clock clock;

    [SerializeField] int targetHour;
    [SerializeField] int targetMinute;
    [SerializeField] SoundData complete;
    [SerializeField] SingleDoorInteract doorInteract;

    bool firstTime = false;

    [SerializeField] Rigidbody rb;

    public event Action OnClockQuestFinished;

    private void OnEnable()
    {
        if(clock != null)
        {
            clock.OnExitClock += CheckTime;
        }

        if (doorInteract != null)
        {
            doorInteract.OnClockQuestOpenDoor += OpenDoorClockQuest;
        }
    }
    private void OnDisable()
    {
        if (clock != null)
        {
            clock.OnExitClock -= CheckTime;
        }
        if (doorInteract != null)
        {
            doorInteract.OnClockQuestOpenDoor -= OpenDoorClockQuest;
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

            OnClockQuestFinished?.Invoke();
        }
    }
    private void RevealObject()
    {
        rb.isKinematic = false;
        GameManager.Instance.SetCondition(GameCondition.IsClockQuestComplete, true);
        GameManager.Instance.SetCondition(GameCondition.TeleportAvailable, false);
    }

    private void OpenDoorClockQuest()
    {
        GameManager.Instance.SetCondition(GameCondition.ClockDoorOpen, true);
    }
}
public interface IClockPuzzleManager
{
    event Action OnClockQuestFinished;
}