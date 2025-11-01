using UnityEngine;

public class GearRotator : MonoBehaviour
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

    void Update()
    {
        float dt = Time.deltaTime;

        for (int i = 0; i < gears.Length; i++)
        {
            if (gears[i].gearTransform == null) continue;

            float dir = gears[i].clockwise ? 1f : -1f;
            Vector3 axisVector = GetAxisVector(gears[i].axis);

            gears[i].gearTransform.Rotate(axisVector, gears[i].rotationSpeed * dir * dt, Space.Self);
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