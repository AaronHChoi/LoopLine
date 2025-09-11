using UnityEngine;

public class RaycastController : MonoBehaviour, IRaycastController
{
    #region PROPERTIES
    [SerializeField] private RaycastSO raycastData;

    private Transform cameraTransform;
    private bool foundInteract;
    private GameObject target;
    private float bestScore;

    public bool FoundInteract => foundInteract;
    public GameObject Target => target;
    public float BestScore => bestScore;
    #endregion

    #region UNITY_METHODS
    private void Awake()
    {
        cameraTransform = Camera.main.transform;
    }
    private void Update()
    {
        foundInteract = TryGetClosestTarget(cameraTransform, raycastData, out target, out bestScore);
    }
    #endregion

    #region PRIVATE_METHODS
    private bool TryGetClosestTarget(Transform originTransform, RaycastSO raycastData, out GameObject bestTarget, out float bestScore)
    {
        bestTarget = null;
        bestScore = 0f;

        Ray ray = new Ray(originTransform.position, originTransform.forward);
        RaycastHit[] hits = Physics.SphereCastAll(ray, raycastData.DetectionRadius, raycastData.DetectionDistance, raycastData.InteractableLayer);

        foreach (var hit in hits)
        {
            Collider col = hit.collider;

            Vector3 boxCenter = col.bounds.center;
            Vector3 dirToTarget = (boxCenter - originTransform.position).normalized;
            float distance = Vector3.Distance(originTransform.position, boxCenter);

            if (BlockedVision(originTransform, col, dirToTarget, distance))
                continue;

            float angle = Vector3.Angle(originTransform.forward, dirToTarget);
            if (angle > raycastData.MaxAngle || distance > raycastData.DetectionDistance) continue;

            float angleScore = 1f - (angle / raycastData.MaxAngle);
            float distanceScore = 1f - (distance / raycastData.DetectionDistance);
            float combinedScore = angleScore * distanceScore;

            if (combinedScore > bestScore)
            {
                bestScore = combinedScore;
                bestTarget = col.gameObject;
            }
        }

        return bestTarget != null && bestScore > 0f;
    }
    private bool BlockedVision(Transform originTransform, Collider targetCollider, Vector3 direction, float distance)
    {
        if (Physics.Raycast(originTransform.position, direction, out RaycastHit hit, distance, ~0))
        {
            return hit.collider != targetCollider;
        }
        return false;
    }
    #endregion
}

interface IRaycastController
{
    public bool FoundInteract { get; }
    public GameObject Target { get; }
    public float BestScore { get; }
}
