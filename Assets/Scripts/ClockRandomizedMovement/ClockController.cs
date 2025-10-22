using System;
using UnityEngine;

namespace ClockRandomizedMovement
{
    public class ClockController : MonoBehaviour
    {
        [Header("Hands")]
        [SerializeField] private Transform hourHand;
        [SerializeField] private Transform minuteHand;
        [SerializeField] private Transform secondHand;

        [Header("Speed Multiplier Range")]
        [SerializeField] private float minSpeed = 5f;
        [SerializeField] private float maxSpeed = 100f;

        [SerializeField] bool enableStop = false;
        [SerializeField] int stopHour = 11;
        [SerializeField] int stopMinute = 40;
        [SerializeField] int stopSecond = 0;
        [SerializeField] float stopDuration = 10f;

        bool isStopped = false;
        float stopTimer = 0f;

        private float _timeSpeed;
        private float _elapsed;
        private System.DateTime _startTime;

        // Arbitrary date values since only the time component is used
        private const int ARBITRARY_YEAR = 1;
        private const int ARBITRARY_MONTH = 1;
        private const int ARBITRARY_DAY = 1;

        void Start()
        {
            int hour = UnityEngine.Random.Range(0, 24);
            int minute = UnityEngine.Random.Range(0, 60);
            int second = UnityEngine.Random.Range(0, 60);
            // Only the time component matters; date is arbitrary
            _startTime = new DateTime(ARBITRARY_YEAR, ARBITRARY_MONTH, ARBITRARY_DAY, hour, minute, second);
            _timeSpeed = UnityEngine.Random.Range(minSpeed, maxSpeed);
        }

        void Update()
        {
            if (isStopped)
            {
                HandleStopDuration();
                return;
            }

            _elapsed += Time.deltaTime * _timeSpeed;
            DateTime t = _startTime.AddSeconds(_elapsed);

            if(enableStop && ShouldStopAtTime(t))
            {
                isStopped = true;
                stopTimer = 0f;
                return;
            }

            UpdateClockHands(t);
        }
        private void UpdateClockHands(DateTime time)
        {
            float hourAngle = (time.Hour % 12 + time.Minute / 60f) * 30f;
            float minuteAngle = (time.Minute + time.Second / 60f) * 6f;
            float secondAngle = time.Second * 6f;

            hourHand.localRotation = Quaternion.Euler(0, 0, -hourAngle);
            minuteHand.localRotation = Quaternion.Euler(0, 0, -minuteAngle);
            secondHand.localRotation = Quaternion.Euler(0, 0, -secondAngle);
        }
        private void HandleStopDuration()
        {
            stopTimer += Time.deltaTime;

            if (stopTimer >= stopDuration)
            {
                isStopped = false;
                _elapsed += 1;
            }
        }
        private bool ShouldStopAtTime(DateTime time)
        {
            return time.Hour == stopHour && time.Minute == stopMinute && time.Second == stopSecond;
        }
    }
}
