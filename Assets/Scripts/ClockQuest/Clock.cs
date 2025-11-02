using System;
using DependencyInjection;
using Unity.Cinemachine;
using UnityEngine;

public class Clock : MonoBehaviour, IInteract, IClock
{
    public event Action OnExitClock;
    public event Action OnEnterClock;

    public Transform HandHour;
    public Transform HandMinute;

    public float rotationSpeed = 2f;
    public float lightChangeSpeed = 5f;

    private bool hourMode = true;
    private bool activeMode = false;

    private float[] hourAngle = { 0f, 30f, 60f, 90f, 120f, 150f, 180f, 210f, 240f, 270f, 300f, 330f };
    private float[] minuteAngle = { 0f, 30f, 60f, 90f, 120f, 150f, 180f, 210f, 240f, 270f, 300f, 330f };

    private int currentHourIndex = 0;
    private int currentMinuteIndex = 0;

    [SerializeField] Light minuteLight;
    [SerializeField] Light hourLight;

    [SerializeField] CinemachineCamera player;
    [SerializeField] CinemachineCamera clockZoom;

    [SerializeField] PlayerMovement playerMovement; //test
    [SerializeField] ICameraOrientation playerCamera;

    [SerializeField] SoundData clockSecondsData;
    [SerializeField] DialogueUI dialogueUI;
    private void Awake()
    {
        playerCamera = InterfaceDependencyInjector.Instance.Resolve<ICameraOrientation>();
    }
    private void Start()
    {
         SoundManager.Instance.CreateSound()
            .WithSoundData(clockSecondsData)
            .WithSoundPosition(transform.position)
            .Play();
    }
    void Update()
    {
        if (!activeMode)
        {
            UpdateLights(false);
            return;
        }

        UpdateLights(true);

        if (Input.GetKeyDown(KeyCode.E))
        {
            hourMode = !hourMode;
            UpdateLights(true);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            HandMove(-1);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            HandMove(1);
        }
    }
    private void LateUpdate()
    {
        RotationUpdate();
    }
    void UpdateLights(bool active)
    {
        float targetHourIntensity = 0f;
        float targetMinuteIntensity = 0f;

        if (active)
        {
            targetHourIntensity = hourMode ? 600f : 0f;
            targetMinuteIntensity = hourMode ? 0f : 600f;
        }

        hourLight.intensity = Mathf.Lerp(hourLight.intensity, targetHourIntensity, Time.deltaTime * lightChangeSpeed);
        minuteLight.intensity = Mathf.Lerp(minuteLight.intensity, targetMinuteIntensity, Time.deltaTime * lightChangeSpeed);
    }
    private void ActiveClock()
    {
        activeMode = !activeMode;

        UIManager.Instance.ShowClockTutorial(activeMode);

        if(activeMode)
        {
            playerMovement.CanMove = false;
            playerCamera.CanLook = false;
            clockZoom.gameObject.SetActive(true);
            clockZoom.Priority = 20;
            player.Priority = 10;
            OnEnterClock?.Invoke();
        }
        else
        {
            clockZoom.gameObject.SetActive(false);
            clockZoom.Priority = 10;
            player.Priority = 20;
            playerMovement.CanMove = true;
            playerCamera.CanLook = true;
            OnExitClock?.Invoke();
        }
    }
    void HandMove(int direction)
    {
        if (hourMode)
        {
            currentHourIndex += direction;

            if (currentHourIndex < 0) currentHourIndex = hourAngle.Length - 1;
            if (currentHourIndex >= hourAngle.Length) currentHourIndex = 0;
        }
        else
        {
            currentMinuteIndex += direction;

            if (currentMinuteIndex < 0) currentMinuteIndex = minuteAngle.Length - 1;
            if (currentMinuteIndex >= minuteAngle.Length) currentMinuteIndex = 0;
        }
    }
    void RotationUpdate()
    {
        float hourAngleTarget = hourAngle[currentHourIndex];
        Quaternion rotationTargetHour  = Quaternion.Euler(hourAngleTarget, 0f, 0f);
        HandHour.rotation = Quaternion.Lerp(HandHour.rotation, rotationTargetHour, Time.deltaTime * rotationSpeed);

        float minuteAngleTarget = minuteAngle[currentMinuteIndex];
        Quaternion rotationTargetMinute = Quaternion.Euler(minuteAngleTarget, 0f, 0f);
        HandMinute.rotation = Quaternion.Lerp(HandMinute.rotation, rotationTargetMinute, Time.deltaTime * rotationSpeed);
    }
    public void SetClockTime(int hora, int minuto)
    {
        hora = Mathf.Clamp(hora, 0, 11);
        minuto = Mathf.Clamp(minuto / 5, 0, 11);

        currentHourIndex = hora;
        currentMinuteIndex = minuto;
    }
    public string GetClockTime()
    {
        int hour = currentHourIndex;
        int minute = currentMinuteIndex * 5;

        return $"{hour}:{minute:00}";
    }
    public void Interact()
    {
        ActiveClock();
    }
    public string GetInteractText()
    {
        throw new System.NotImplementedException();
    }
}
public interface IClock
{
    event Action OnExitClock;
    event Action OnEnterClock;
}