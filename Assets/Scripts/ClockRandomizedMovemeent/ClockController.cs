using UnityEngine;

public class ClockController : MonoBehaviour
{
    [Header("Hands")]
    [SerializeField] private Transform hourHand;
    [SerializeField] private Transform minuteHand;
    [SerializeField] private Transform secondHand;

    [Header("Speed Multiplier Range")]
    [SerializeField] private float minSpeed = 50f;   // much faster than real time
    [SerializeField] private float maxSpeed = 300f;

    private float timeSpeed;
    private float elapsed;
    private System.DateTime startTime;

    void Start()
    {
        int hour = Random.Range(0, 24);
        int minute = Random.Range(0, 60);
        int second = Random.Range(0, 60);
        startTime = new System.DateTime(1, 1, 1, hour, minute, second);
        timeSpeed = Random.Range(minSpeed, maxSpeed);
    }

    void Update()
    {
        elapsed += Time.deltaTime * timeSpeed;
        System.DateTime t = startTime.AddSeconds(elapsed);

        float hourAngle = (t.Hour % 12 + t.Minute / 60f) * 30f;
        float minuteAngle = (t.Minute + t.Second / 60f) * 6f;
        float secondAngle = t.Second * 6f;

        // rotate around local Z-axis (change axis if your mesh differs)
        hourHand.localRotation = Quaternion.Euler(0, 0, -hourAngle);
        minuteHand.localRotation = Quaternion.Euler(0, 0, -minuteAngle);
        secondHand.localRotation = Quaternion.Euler(0, 0, -secondAngle);
    }
}