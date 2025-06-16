using UnityEngine;
using UnityEngine.UI;

public class CrosshairFeedback : MonoBehaviour
{
    [SerializeField] private RawImage crosshairImage;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private float maxAngle = 15f;
    [SerializeField] private float detectionRadius = 1.5f;
    [SerializeField] private float detectionDistance = 2f;

    private Transform cameraTransform;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        SetCrosshairVisible(false);
    }

    void Update()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        RaycastHit[] hits = Physics.SphereCastAll(ray, detectionRadius, detectionDistance, interactableLayer);

        Transform closestTarget = null;
        float bestScore = 0f;

        foreach (var hit in hits)
        {
            Collider col = hit.collider;
            Vector3 boxCenter = col.bounds.center;
            Vector3 dirToTarget = (boxCenter - cameraTransform.position).normalized;
            float distance = Vector3.Distance(cameraTransform.position, boxCenter);

            if (Physics.Raycast(cameraTransform.position, dirToTarget, out RaycastHit blockHit, distance, ~0))
            {
                if (blockHit.collider != col)
                    continue;
            }

            float angle = Vector3.Angle(cameraTransform.forward, dirToTarget);
            if (angle > maxAngle || distance > detectionDistance) continue;

            float angleScore = 1f - (angle / maxAngle);
            float distanceScore = 1f - (distance / detectionDistance);
            float combinedScore = angleScore * distanceScore;

            if (combinedScore > bestScore)
            {
                bestScore = combinedScore;
                closestTarget = col.transform;
            }
        }


        if (closestTarget != null && bestScore > 0f)
        {
            SetCrosshairOpacity(bestScore);
            SetCrosshairVisible(true);
        }
        else
        {
            SetCrosshairVisible(false);
        }
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
}
