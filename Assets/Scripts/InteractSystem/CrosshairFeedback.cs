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
        float closestAngle = maxAngle;

        foreach (var hit in hits)
        {
            Collider col = hit.collider;
            Vector3 closestPoint = col.ClosestPoint(cameraTransform.position);
            Vector3 dirToTarget = (closestPoint - cameraTransform.position).normalized;
            float angle = Vector3.Angle(cameraTransform.forward, dirToTarget);

            if (angle < closestAngle)
            {
                closestAngle = angle;
                closestTarget = col.transform;
            }
        }

        if (closestTarget != null)
        {
            float opacity = 1f - (closestAngle / maxAngle);
            SetCrosshairOpacity(opacity);
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
