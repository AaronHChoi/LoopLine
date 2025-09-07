using UnityEngine;
using UnityEngine.UI;

public class CrosshairFade : MonoBehaviour, ICrosshairFade
{
    [SerializeField] private RawImage crosshairImage;
    
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private float maxAngle = 15f;
    [SerializeField] private float detectionRadius = 1.5f;
    [SerializeField] private float detectionDistance = 2f;

    public bool IsVisible => isVisible;

    private bool isVisible = true;
    private Transform cameraTransform;
    private FadeInOutController crossFade;


    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        cameraTransform = Camera.main.transform;
        SetCrosshairVisible(false); // hide crosshair at start
        if (crosshairImage != null) crossFade = crosshairImage.GetComponent<FadeInOutController>();
    }

    void Update()
    {
        Transform target;
        float score;

        if (GetClosestTarget(out target, out score))
        {
            SetCrosshairOpacity(score); // fade based on angle + distance
            SetCrosshairVisible(true);
        }
        else
        {
            SetCrosshairVisible(false);
        }
    }

    private bool GetClosestTarget(out Transform bestTarget, out float bestScore)
    {
        bestTarget = null;
        bestScore = 0f;

        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        RaycastHit[] hits = Physics.SphereCastAll(ray, detectionRadius, detectionDistance, interactableLayer);

        foreach (var hit in hits)
        {
            Collider col = hit.collider;

            Vector3 boxCenter = col.bounds.center;
            Vector3 dirToTarget = (boxCenter - cameraTransform.position).normalized;
            float distance = Vector3.Distance(cameraTransform.position, boxCenter);

            if (BlockedVision(col, dirToTarget, distance))
                continue;

            float angle = Vector3.Angle(cameraTransform.forward, dirToTarget);
            if (angle > maxAngle || distance > detectionDistance) continue;

            float angleScore = 1f - (angle / maxAngle);
            float distanceScore = 1f - (distance / detectionDistance);
            float combinedScore = angleScore * distanceScore;

            if (combinedScore > bestScore)
            {
                bestScore = combinedScore;
                bestTarget = col.transform;
            }
        }

        return bestTarget != null && bestScore > 0f;
    }

    private bool BlockedVision(Collider targetCollider, Vector3 direction, float distance)
    {
        if (Physics.Raycast(cameraTransform.position, direction, out RaycastHit hit, distance, ~0))
        {
            return hit.collider != targetCollider;
        }
        return false;
    }

    private void SetCrosshairVisible(bool visible)
    {
        crosshairImage.enabled = visible;
    }

    private void SetCrosshairOpacity(float opacity)
    {
        Color color = crosshairImage.color;
        color.a = opacity;
        crosshairImage.color = color;
    }
    public void ShowCrosshair(bool show)
    {
        //Fades if it can and save it's state
        if (isVisible == show) return;
        else isVisible = show;

        if (crossFade == null) return;

        crossFade.ForceFade(!show);
    }
}

public interface ICrosshairFade
{
    public bool IsVisible { get; }
    void ShowCrosshair(bool show);

}