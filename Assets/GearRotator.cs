using System.Collections;
using UnityEngine;

public class GearRotator : MonoBehaviour, IGearRotator
{
    public enum RotationAxis { X, Y, Z }

    [System.Serializable]
    public class Gear
    {
        public Transform gearTransform;   // assign in Inspector
        public float rotationSpeed = 10f; // degrees per second
        public bool clockwise = true;     // direction toggle
        public RotationAxis axis = RotationAxis.X; // choose axis
    }

    [Header("Gear Settings")]
    [SerializeField] private Gear[] gears;

    private Coroutine rotationCoroutine;

    private void Start()
    {
        StartGears();
    }
    IEnumerator RotateRoutine()
    {
        while (true)
        {
            float dt = Time.deltaTime;

            for (int i = 0; i < gears.Length; i++)
            {
                Gear gear = gears[i];
                if (gear.gearTransform == null) continue;

                float dir = gear.clockwise ? 1f : -1f;
                Vector3 axisVector = GetAxisVector(gear.axis);

                gear.gearTransform.Rotate(axisVector, gear.rotationSpeed * dir * dt, Space.Self);
            }

            yield return null;
        }
    }
    public void StartGears()
    {
        if (rotationCoroutine == null)
        {
            rotationCoroutine = StartCoroutine(RotateRoutine());
        }
    }
    public void StopGears()
    {
        if (rotationCoroutine != null)
        {
            StopCoroutine(rotationCoroutine);
            rotationCoroutine = null;
        }
    }
    private Vector3 GetAxisVector(RotationAxis axis)
    {
        switch (axis)
        {
            case RotationAxis.X: return Vector3.right;
            case RotationAxis.Y: return Vector3.up;
            case RotationAxis.Z: return Vector3.forward;
            default: return Vector3.right;
        }
    }
}
public interface IGearRotator
{
    void StopGears();
}