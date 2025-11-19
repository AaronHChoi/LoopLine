using UnityEngine;
using System;

public class ClockController : MonoBehaviour
{
    [Header("Hands")]
    [SerializeField] private Transform hourHand;
    [SerializeField] private Transform minuteHand;
    [SerializeField] private Transform secondHand;

    [Header("Speed Settings")]
    [SerializeField] private float baseSpeed = 150f;
    [SerializeField] private float slowdownRangeMinutes = 20f;
    [SerializeField] private float slowdownStrength = 4f;
    [SerializeField] private float minSpeedFactor = 0.05f;

    [Header("Timing Settings")]
    [SerializeField] private bool enablePause = false;
    [SerializeField] private float pauseDuration = 5f;
    [SerializeField] private float resumeSmoothTime = 3f;

    [Header("Audio")]
    [SerializeField] private AudioSource tickSource;
    [SerializeField] private float minPitch = 0.5f;
    [SerializeField] private float maxPitch = 2f;

    [Header("Start Options")]
    [SerializeField] private bool startAt = false;

    [Header("Jitter Control")]
    [SerializeField] private ClockJitter jitterComponent;
    [SerializeField] private float defaultJitterAmount = 0.000482f;
    [SerializeField] private float defaultJitterSpeed = 0.00503f;
    [SerializeField] private float jitterSmooth = 2f;

    private float currentSpeed;
    private float elapsed;
    private DateTime startTime;
    private float pauseTimer;
    private bool isPaused;
    private float resumeTimer;
    private bool isResuming;

    private readonly TimeSpan targetTime = new TimeSpan(11, 20, 0);
    private const int ARBITRARY_YEAR = 1, ARBITRARY_MONTH = 1, ARBITRARY_DAY = 1;

    void Start()
    {
        if (!startAt)
        {
            int hour = UnityEngine.Random.Range(0, 12);
            int minute = UnityEngine.Random.Range(0, 60);
            int second = UnityEngine.Random.Range(0, 60);

            startTime = new DateTime(ARBITRARY_YEAR, ARBITRARY_MONTH, ARBITRARY_DAY, hour, minute, second);
        }
        else
        {
            startTime = new DateTime(ARBITRARY_YEAR, ARBITRARY_MONTH, ARBITRARY_DAY, 10, 40, 0);
        }

        currentSpeed = baseSpeed;
    }
    void Update()
    {
        if (isPaused)
        {
            pauseTimer += Time.deltaTime;
            if (pauseTimer >= pauseDuration)
            {
                isPaused = false;
                isResuming = true;
                resumeTimer = 0f;
            }

            UpdateClock(startTime.AddSeconds(elapsed));
            UpdateJitter();
            return;
        }

        DateTime currentTime = startTime.AddSeconds(elapsed);
        TimeSpan current = currentTime.TimeOfDay;

        double diffMinutes = (targetTime - current).TotalMinutes;
        if (diffMinutes < 0) diffMinutes += 12 * 60;

        float targetSpeed = baseSpeed;

        if (enablePause && diffMinutes <= slowdownRangeMinutes && diffMinutes > 0.1f && !isResuming)
        {
            float t = Mathf.Clamp01((float)(diffMinutes / slowdownRangeMinutes));
            t = Mathf.Pow(t, slowdownStrength);
            targetSpeed = Mathf.Lerp(baseSpeed * minSpeedFactor, baseSpeed, t);
        }

        if (enablePause && diffMinutes <= 0.1f && !isPaused && !isResuming)
        {
            isPaused = true;
            currentSpeed = 0f;
            pauseTimer = 0f;

            if (tickSource && tickSource.isPlaying)
                tickSource.Stop();

            UpdateJitter();
            return;
        }

        if (isResuming)
        {
            resumeTimer += Time.deltaTime;
            float factor = Mathf.Clamp01(resumeTimer / resumeSmoothTime);
            currentSpeed = Mathf.Lerp(0f, baseSpeed, Mathf.SmoothStep(0f, 1f, factor));

            if (factor >= 1f)
            {
                isResuming = false;
                currentSpeed = baseSpeed;
            }
        }
        else
        {
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * 2f);
        }

        elapsed += Time.deltaTime * currentSpeed;
        if (elapsed >= 12 * 3600f) elapsed -= 12 * 3600f;

        if (tickSource != null)
        {
            tickSource.pitch = Mathf.Lerp(minPitch, maxPitch, Mathf.Clamp01(currentSpeed / baseSpeed));
            if (!tickSource.isPlaying)
                tickSource.Play();
        }

        UpdateClock(startTime.AddSeconds(elapsed));
        UpdateJitter();
    }
    void UpdateClock(DateTime t)
    {
        float hourAngle = (t.Hour % 12 + t.Minute / 60f) * 30f;
        float minuteAngle = (t.Minute + t.Second / 60f) * 6f;
        float secondAngle = t.Second * 6f;

        Quaternion rotHour = Quaternion.Euler(0f, hourAngle, 0f);
        Quaternion rotMinute = Quaternion.Euler(0f, minuteAngle, 0f);
        Quaternion rotSecond = Quaternion.Euler(0f, secondAngle, 0f);

        hourHand.rotation = transform.rotation * rotHour;
        minuteHand.rotation = transform.rotation * rotMinute;
        secondHand.rotation = transform.rotation * rotSecond;
    }
    void UpdateJitter()
    {
        if (!jitterComponent) return;

        float speedFactor = Mathf.Clamp01(currentSpeed / baseSpeed);
        float targetAmount = Mathf.Lerp(0f, defaultJitterAmount, speedFactor);
        float targetSpeed = Mathf.Lerp(0f, defaultJitterSpeed, speedFactor);

        jitterComponent.jitterAmount = Mathf.Lerp(jitterComponent.jitterAmount, targetAmount, Time.deltaTime * jitterSmooth);
        jitterComponent.jitterSpeed = Mathf.Lerp(jitterComponent.jitterSpeed, targetSpeed, Time.deltaTime * jitterSmooth);
    }
}