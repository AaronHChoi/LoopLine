using UnityEngine;

public class ClockPuzzleManager : MonoBehaviour
{
    [SerializeField] Clock clock;
    [SerializeField] GameObject doorHandler;

    [SerializeField] int targetHour;
    [SerializeField] int targetMinute;

    private void Update()
    {
        CheckTime();
    }
    private void CheckTime()
    {
        string currentTime = clock.GetClockTime();
        string[] timeParts = currentTime.Split(':');

        int currenHour = int.Parse(timeParts[0]);
        int currentMinute = int.Parse(timeParts[1]);

        if (currenHour == targetHour && currentMinute == targetMinute)
        {
            RevealObject();
        }
    }
    private void RevealObject()
    {
        if (doorHandler != null && !doorHandler.activeInHierarchy)
        {
            doorHandler.SetActive(true);
        }
    }
}