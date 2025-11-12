using System;
using UnityEngine;

public class ClockPuzzleManager : MonoBehaviour, IClockPuzzleManager
{
    [SerializeField] Clock clock;

    [SerializeField] int targetHour;
    [SerializeField] int targetMinute;
    [SerializeField] SoundData complete;

    bool firstTime = false;

    [SerializeField] Rigidbody rb;

    public event Action OnPhotoQuestFinished;
    public event Action OnClockQuestFinished;

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

            OnPhotoQuestFinished?.Invoke();
            OnClockQuestFinished?.Invoke();
        }
    }
    private void RevealObject()
    {
        rb.isKinematic = false;
        GameManager.Instance.SetCondition(GameCondition.IsClockQuestComplete, true);
    }
}
public interface IClockPuzzleManager
{
    event Action OnPhotoQuestFinished;
    event Action OnClockQuestFinished;
}