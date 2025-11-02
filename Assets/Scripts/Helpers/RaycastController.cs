using DependencyInjection;
using UnityEngine;

public class RaycastController : MonoBehaviour, IRaycast
{
    #region PROPERTIES
    [SerializeField] private RaycastSO raycastData;

    private Transform cameraTransform;
    private bool foundInteract;
    private GameObject target;

    public bool FoundInteract => foundInteract;
    public GameObject Target => target;
    #endregion

    #region UNITY_METHODS
    private void Start()
    {
        cameraTransform = InterfaceDependencyInjector.Instance.Resolve<IPlayerCamera>().GetCameraTransform();
    }
    private void Update()
    {
        foundInteract = GetTarget(cameraTransform, raycastData, out target);
    }
    private void OnDrawGizmos()
    {
        if (cameraTransform == null || raycastData == null) return;

        DrawRaycastGizmos();
    }
    #endregion

    #region PRIVATE_METHODS
    private bool GetTarget( Transform originTransform,
                            RaycastSO raycastData,
                        out GameObject target)
    {
        target = null;

        Ray ray = new Ray(originTransform.position, originTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, raycastData.DetectionDistance, raycastData.InteractableLayer))
        {
            target = hit.collider.gameObject;

            return true;
        }

        return false;
    }
    private void DrawRaycastGizmos()
    {
        Color rayColor = foundInteract ? Color.green : Color.red;
        Color hitColor = foundInteract ? Color.yellow : Color.magenta;

        Gizmos.color = rayColor;
        Vector3 rayStart = cameraTransform.position;
        Vector3 rayEnd = rayStart + cameraTransform.forward * raycastData.DetectionDistance;
        Gizmos.DrawLine(rayStart, rayEnd);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(rayStart, 0.05f);

        if (foundInteract && target != null)
        {
            Gizmos.color = hitColor;
            Vector3 targetCenter = target.GetComponent<Collider>().bounds.center;
            Gizmos.DrawLine(rayStart, targetCenter);

            Ray ray = new Ray(rayStart, cameraTransform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, raycastData.DetectionDistance, raycastData.InteractableLayer))
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(hit.point, 0.1f);

                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(hit.point, hit.point + hit.normal * 0.5f);
            }

            Gizmos.color = new Color(0, 1, 0, 0.3f);
            Gizmos.DrawWireCube(targetCenter, target.GetComponent<Collider>().bounds.size);
        }
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

interface IRaycast
{
    public bool FoundInteract { get; }
    public GameObject Target { get; }
}