using UnityEngine;
using System;

public class ClockController : MonoBehaviour
{
    [Header("Hands")]
    [SerializeField] private Transform hourHand;
    [SerializeField] private Transform minuteHand;
    [SerializeField] private Transform secondHand;

    [Header("Speed Settings")]
    [SerializeField] private float minSpeed = 50f;
    [SerializeField] private float maxSpeed = 300f;

    [Header("Slowdown Settings")]
    [SerializeField] private bool enableSlowdown = true;
    [SerializeField] private float slowDistanceMinutes = 30f;
    [SerializeField] private float accelerationRate = 0.5f;
    [SerializeField] private float minSpeedFactor = 0.05f;
    [SerializeField] private float pauseDuration = 7f;
    [SerializeField] private float resumeSmoothTime = 3f;

    [Header("Failsafe Settings")]
    [SerializeField] private float slowdownTimeout = 60f;
    
    [Header("Audio")]
    [SerializeField] private AudioSource tickSource;       // assign in inspector
    [SerializeField] private float minPitch = 0.5f;        // slowest pitch
    [SerializeField] private float maxPitch = 2f;          // fastest pitch

    private float baseSpeed;
    private float currentSpeed;
    private float elapsed;
    private DateTime startTime;
    private float pauseTimer;
    private bool isPaused;
    private float resumeTimer;
    private float slowdownTimer;
    private bool isInSlowdown;

    private readonly TimeSpan targetTime = new TimeSpan(11, 40, 0);
    private const int ARBITRARY_YEAR = 1, ARBITRARY_MONTH = 1, ARBITRARY_DAY = 1;

    void Start()
    {
        int hour = UnityEngine.Random.Range(0, 24);
        int minute = UnityEngine.Random.Range(0, 60);
        int second = UnityEngine.Random.Range(0, 60);
        startTime = new DateTime(ARBITRARY_YEAR, ARBITRARY_MONTH, ARBITRARY_DAY, hour, minute, second);

        baseSpeed = UnityEngine.Random.Range(minSpeed, maxSpeed);
        currentSpeed = baseSpeed;
    }

    void Update()
    {
        if (enableSlowdown)
            UpdateWithSlowdown();
        else
            UpdateNormal();
    }

    void UpdateNormal()
    {
        elapsed += Time.deltaTime * baseSpeed;
        if (elapsed >= 12 * 3600f) elapsed -= 12 * 3600f;
        UpdateClock(startTime.AddSeconds(elapsed));
    }

    void UpdateWithSlowdown()
    {
        DateTime currentTime = startTime.AddSeconds(elapsed);
        TimeSpan current = currentTime.TimeOfDay;
        double diffMinutes = (targetTime - current).TotalMinutes;
        if (diffMinutes < -0.1) diffMinutes += 12 * 60;

        float targetSpeed = baseSpeed;

        // Enter slowdown zone
        if (!isPaused && diffMinutes <= slowDistanceMinutes && diffMinutes > 0f)
        {
            if (!isInSlowdown)
            {
                isInSlowdown = true;
                slowdownTimer = 0f;
                
            }

            slowdownTimer += Time.deltaTime;

            // Timeout fallback
            if (slowdownTimer >= slowdownTimeout)
            {
                isInSlowdown = false;
            }

            if (isInSlowdown)
            {
                float t = Mathf.Clamp01((float)(diffMinutes / slowDistanceMinutes));
                targetSpeed = Mathf.Lerp(minSpeedFactor * baseSpeed, baseSpeed, t * t);
                if (tickSource != null)
                {
                    // If paused, keep a minimal pitch for the tick
                    tickSource.pitch = Mathf.Lerp(minPitch, maxPitch, targetSpeed);

                    if (!tickSource.isPlaying)
                        tickSource.Play();
                }
            }
        }

        // Pause at target
        if (!isPaused && diffMinutes <= 0.1)
        {
            isPaused = true;
            isInSlowdown = false;
            pauseTimer = 0f;
            currentSpeed = 0f;
            targetSpeed = 0f;
        }

        // Handle pause
        if (isPaused)
        {
            pauseTimer += Time.deltaTime;
            targetSpeed = 0f;

            if (pauseTimer >= pauseDuration)
            {
                isPaused = false;
                resumeTimer = 0f;
            }
        }

        // Resume smoothly
        if (!isPaused && currentSpeed < baseSpeed)
        {
            resumeTimer += Time.deltaTime;
            float factor = Mathf.Clamp01(resumeTimer / resumeSmoothTime);
            targetSpeed = Mathf.Lerp(0f, baseSpeed, Mathf.SmoothStep(0f, 1f, factor));
        }

        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * accelerationRate);
        elapsed += Time.deltaTime * currentSpeed;

        if (elapsed >= 12 * 3600f) elapsed -= 12 * 3600f;

        UpdateClock(startTime.AddSeconds(elapsed));
        
        // Smooth interpolation
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * accelerationRate);
        elapsed += Time.deltaTime * currentSpeed;

        if (elapsed >= 12 * 3600f) elapsed -= 12 * 3600f;

        UpdateClock(startTime.AddSeconds(elapsed));
        
    }

    void UpdateClock(DateTime t)
    {
        float hourAngle = (t.Hour % 12 + t.Minute / 60f) * 30f;
        float minuteAngle = (t.Minute + t.Second / 60f) * 6f;
        float secondAngle = t.Second * 6f;

        hourHand.localRotation = Quaternion.Euler(0, 0, -hourAngle);
        minuteHand.localRotation = Quaternion.Euler(0, 0, -minuteAngle);
        secondHand.localRotation = Quaternion.Euler(0, 0, -secondAngle);
    }
}
