using UnityEngine;
using TMPro;

public class TrainAnnouncement : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI announcementText;
    [Range(0, 23)] public int startHour;
    [Range(0, 59)] public int startMinute;
    [SerializeField] private string nextStationName;

    [SerializeField] private float realTimeScale;

    [SerializeField] private float resetTextTimer;
    [SerializeField] private float textSpeed;

    private int currentTime;
    private float timeAccumulator;

    private float moveTimer = 0f;
    private Vector3 startPos;

    void Start()
    {
        currentTime = startHour * 60 + startMinute;
        startPos = announcementText.rectTransform.localPosition;

        UpdateAnnouncement();
    }

    void Update()
    {
        timeAccumulator += Time.deltaTime;
        moveTimer += Time.deltaTime;

        if (timeAccumulator >= realTimeScale)
        {
            currentTime++;
            timeAccumulator = 0f;
            UpdateAnnouncement();
        }

        announcementText.rectTransform.localPosition += Vector3.left * Time.deltaTime * textSpeed;

        if (moveTimer >= resetTextTimer)
        {
            announcementText.rectTransform.localPosition = startPos;
            moveTimer = 0f;
        }
    }

    void UpdateAnnouncement()
    {
        int hours = currentTime / 60 % 24;
        int minutes = currentTime % 60;
        string formattedTime = $"{hours:D2}:{minutes:D2}";
        string message = $"Tiempo actual: {formattedTime}, la próxima estación es {nextStationName}    ";
        announcementText.text = message;
    }
}
