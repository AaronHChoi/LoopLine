using UnityEngine;

public class Clock : MonoBehaviour
{
    public Transform HandHour;
    public Transform HandMinute;

    public float rotationSpeed = 2f;

    private bool hourMode = true; 
    private bool activeMode = false;

    private float[] hourAngle = { 0f, 30f, 60f, 90f, 120f, 150f, 180f, 210f, 240f, 270f, 300f, 330f };
    private float[] minuteAngle = { 0f, 30f, 60f, 90f, 120f, 150f, 180f, 210f, 240f, 270f, 300f, 330f };

    private int currentHourIndex = 0;
    private int currentMinuteIndex = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            activeMode = !activeMode;
        }

        if (!activeMode) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            hourMode = !hourMode;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            HandMove(-1);
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            HandMove(1); 
        }
    }
    private void LateUpdate()
    {
        RotationUpdate();
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
        Quaternion rotationTargetHour  = Quaternion.Euler(0f, 0f, -hourAngleTarget);
        HandHour.rotation = Quaternion.Lerp(HandHour.rotation, rotationTargetHour, Time.deltaTime * rotationSpeed);

        float minuteAngleTarget = minuteAngle[currentMinuteIndex];
        Quaternion rotationTargetMinute = Quaternion.Euler(0f, 0f, -minuteAngleTarget);
        HandMinute.rotation = Quaternion.Lerp(HandMinute.rotation, rotationTargetMinute, Time.deltaTime * rotationSpeed);
    }
    public void SetClockTime(int hora, int minuto)
    {
        hora = Mathf.Clamp(hora, 0, 11);
        minuto = Mathf.Clamp(minuto / 5, 0, 11);

        currentHourIndex = hora;
        currentMinuteIndex = minuto;
    }
}