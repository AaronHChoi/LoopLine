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

        private float _timeSpeed;
        private float _elapsed;
        private System.DateTime _startTime;

        // Arbitrary date values since only the time component is used
        private const int ARBITRARY_YEAR = 1;
        private const int ARBITRARY_MONTH = 1;
        private const int ARBITRARY_DAY = 1;

        void Start()
        {
            int hour = Random.Range(0, 24);
            int minute = Random.Range(0, 60);
            int second = Random.Range(0, 60);
            // Only the time component matters; date is arbitrary
            _startTime = new System.DateTime(ARBITRARY_YEAR, ARBITRARY_MONTH, ARBITRARY_DAY, hour, minute, second);
            _timeSpeed = Random.Range(minSpeed, maxSpeed);
        }

        void Update()
        {
            _elapsed += Time.deltaTime * _timeSpeed;
            System.DateTime t = _startTime.AddSeconds(_elapsed);

            float hourAngle = (t.Hour % 12 + t.Minute / 60f) * 30f;
            float minuteAngle = (t.Minute + t.Second / 60f) * 6f;
            float secondAngle = t.Second * 6f;

            hourHand.localRotation = Quaternion.Euler(0, 0, -hourAngle);
            minuteHand.localRotation = Quaternion.Euler(0, 0, -minuteAngle);
            secondHand.localRotation = Quaternion.Euler(0, 0, -secondAngle);
        }
    }
}
