using UnityEngine;

public class ClockPuzzleManager : MonoBehaviour
{
    [SerializeField] Clock clock;
    [SerializeField] GameObject doorHandlerItem;

    [SerializeField] int targetHour;
    [SerializeField] int targetMinute;
    private void Start()
    {
        if (GameManager.Instance.ClockQuest == true)
        {
            doorHandlerItem.SetActive(true);
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

        if (currenHour == targetHour && currentMinute == targetMinute)
        {
            RevealObject();
        }
    }
    private void RevealObject()
    {
        if (doorHandlerItem != null && !doorHandlerItem.activeInHierarchy && !GameManager.Instance.ClockQuest)
        {
            doorHandlerItem.SetActive(true);
            GameManager.Instance.ClockQuest = true;
        }
    }
}