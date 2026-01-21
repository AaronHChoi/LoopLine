using UnityEngine;

public class LookAtNPC : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float lerpSpeed = 5f;
    [SerializeField] private float reachDistance = 1f;

    ICameraOrientation orientation;
    Transform camTransform;
    float targetPan, targetTilt;
    public bool isLooking;

    void Start()
    {
        // Get camera orientation interface and transform
        orientation = GetComponentInChildren<ICameraOrientation>();
        camTransform = (orientation as MonoBehaviour).transform;
    }
    void Update()
    {
        if (target != null && orientation != null && !isLooking)
        {
            // Calculate direction and desired pan/tilt from camera to target
            Vector3 direction = (target.position - camTransform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
            Vector3 euler = lookRotation.eulerAngles;

            targetPan = NormalizeAngle(euler.y);
            targetTilt = NormalizeAngle(euler.x);

            isLooking = true;
        }

        if (!isLooking) return;

        var (pan, tilt) = orientation.GetPanAndTilt();

        // Interpolate pan using DeltaAngle for smooth wrap-around
        float deltaPan = Mathf.DeltaAngle(pan, targetPan);
        pan += deltaPan * Time.deltaTime * lerpSpeed;
        pan = NormalizeAngle(pan);

        // Smooth tilt interpolation
        tilt = Mathf.Lerp(tilt, targetTilt, Time.deltaTime * lerpSpeed);

        orientation.SetPanAndTilt(pan, tilt);

        // Stop looking if the target is reached
        if (Mathf.Abs(Mathf.DeltaAngle(pan, targetPan)) < reachDistance &&
            Mathf.Abs(tilt - targetTilt) < reachDistance)
        {
            isLooking = false;
        }
    }
    // Keep angles within -180 to 180
    float NormalizeAngle(float angle)
    {
        while (angle > 180f) angle -= 360f;
        while (angle < -180f) angle += 360f;
        return angle;
    }
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
    public void ClearTarget()
    {
        target = null;
    }
}