using UnityEngine;

[CreateAssetMenu(fileName = "Raycast", menuName = "Scriptable Object/New Raycast")]

public class RaycastSO : ScriptableObject
{
    [SerializeField] private RaycastType raycastType = RaycastType.SphereRaycast;
    [Space]
    [SerializeField] private LayerMask interactableLayer;
    [Space]
    [SerializeField] private float detectionRadius = 1f;
    [SerializeField] private float detectionDistance = 1f;
    [SerializeField] private float maxAngle = 45f;

    public RaycastType RaycastType => raycastType;
    public LayerMask InteractableLayer => interactableLayer;
    public float DetectionRadius => detectionRadius;
    public float DetectionDistance => detectionDistance;
    public float MaxAngle => maxAngle;
}

public enum RaycastType
{
    SphereRaycast
}