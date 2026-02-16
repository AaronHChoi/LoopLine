using System;
using System.Collections;
using UnityEngine;
using Core.Data;
using Core.EventBus;

public class ClockController : MonoBehaviour
{
    [Header("Hands")]
    [SerializeField] private Transform hourHand;
    [SerializeField] private Transform minuteHand;
    [SerializeField] private Transform secondHand;

    [Header("Speed Settings")]
    [SerializeField] private float baseSpeed = 150f;
    [SerializeField] private float eventFastForwardSpeed = 1600f;

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

    private bool isEventActive = false;

    private readonly TimeSpan targetTime = new TimeSpan(11, 20, 0);
    private const int ARBITRARY_YEAR = 1, ARBITRARY_MONTH = 1, ARBITRARY_DAY = 1;

    void Start()
    {
        SetupInitialTime();

        currentSpeed = baseSpeed;

        if (GameManager.Instance.GetCondition(GameCondition.IsClockFrozen))
        {
            SetTimeAndPause(11, 20, 0);
        }
    }
    private void OnEnable()
    {
        EventBus.Subscribe<ClockSyncEvent>(ForceTimeAndPause);
        EventBus.Subscribe<ClockFreezeEvent>(HandleFreeze);
        EventBus.Subscribe<ClockResumeEvent>(HandleResume);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe<ClockSyncEvent>(ForceTimeAndPause);
        EventBus.Unsubscribe<ClockFreezeEvent>(HandleFreeze);
        EventBus.Unsubscribe<ClockResumeEvent>(HandleResume);
    }
    void Update()
    {
        if (isEventActive)
        {
            return;
        }
        currentSpeed = baseSpeed;

        elapsed += Time.deltaTime * currentSpeed;
        if (elapsed >= 12 * 3600f) elapsed -= 12 * 3600f;

        if (tickSource != null)
        {
            tickSource.pitch = Mathf.Lerp(minPitch, maxPitch, Mathf.Clamp01(currentSpeed / baseSpeed));
            if (!tickSource.isPlaying)
            {
                tickSource.Play();
            }
        }

        UpdateClock(startTime.AddSeconds(elapsed));
        UpdateJitter();
    }
    private void SetupInitialTime()
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
    }
    [ContextMenu("Trigger 11:20 Event")]
    public void ForceTimeAndPause(ClockSyncEvent ev)
    {
        if (!isEventActive)
        {
            StartCoroutine(Sequence1120());
        }
    }
    IEnumerator Sequence1120()
    {
        isEventActive = true;

        DateTime now = startTime.AddSeconds(elapsed);
        TimeSpan current = now.TimeOfDay;
        double secondsToTarget = (targetTime - current).TotalSeconds;

        if (secondsToTarget < 0)
        {
            secondsToTarget += 12 * 3600;
        }

        float targertElapsed = elapsed + (float)secondsToTarget;
        currentSpeed = eventFastForwardSpeed;

        if (tickSource != null)
        {
            tickSource.pitch = maxPitch;
            if (!tickSource.isPlaying)
            {
                tickSource.Play();
            }
        }

        while (elapsed < targertElapsed)
        {
            elapsed += Time.deltaTime * currentSpeed;

            if (elapsed > targertElapsed)
            {
                elapsed = targertElapsed;
            }

            UpdateClock(startTime.AddSeconds(elapsed));
            UpdateJitter();
            yield return null;
        }

        currentSpeed = 0;
        if (tickSource != null)
        {
            tickSource.Stop();
        }

        UpdateJitter();

        yield return new WaitForSeconds(10f);

        float t = 0;

        while (t < 2f)
        {
            t += Time.deltaTime;
            currentSpeed = Mathf.Lerp(0, baseSpeed, t / 2f);

            if (tickSource != null)
            {
                if (!tickSource.isPlaying)
                {
                    tickSource.Play();
                }
                tickSource.pitch = Mathf.Lerp(minPitch, 1f, t / 2f);
            }

            elapsed += Time.deltaTime * currentSpeed;
            UpdateClock(startTime.AddSeconds(elapsed));
            UpdateJitter();
            yield return null;
        }

        currentSpeed = baseSpeed;
        isEventActive = false;
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

    public void SetTimeAndPause(int hours, int minutes, int seconds)
    {
        isEventActive = true;

        DateTime target = new DateTime(ARBITRARY_YEAR, ARBITRARY_MONTH, ARBITRARY_DAY, hours, minutes, seconds);
        double secondsToTarget = (target - startTime).TotalSeconds;

        if (secondsToTarget < 0)
        {
            secondsToTarget += 12 * 3600;
        }

        elapsed = (float)secondsToTarget;
        currentSpeed = 0;

        if (tickSource != null)
        {
            tickSource.Stop();
        }

        UpdateClock(startTime.AddSeconds(elapsed));
        UpdateJitter();
    }
    public void ResumeClock()
    {
        isEventActive = false;
        currentSpeed = baseSpeed;

        if (tickSource != null && !tickSource.isPlaying)
        {
            tickSource.Play();
        }
    }
    private void HandleFreeze(ClockFreezeEvent ev)
    {
        SetTimeAndPause(ev.hour, ev.minute, ev.second);
    }
    private void HandleResume(ClockResumeEvent ev)
    {
        ResumeClock();
    }
}