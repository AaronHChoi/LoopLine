using System.Collections.Generic;
using UnityEngine;

namespace InWorldUI
{
    [RequireComponent(typeof(Collider))]
    public class PlayerInteraction : MonoBehaviour
    {
        [Header("Prompt Settings")]
        [SerializeField] private float interactRange = 3f;
        [SerializeField] private LayerMask interactableLayer;

        [Header("Marker Settings")]
        [SerializeField] private float markerFOV = 90f;
        [SerializeField] private float promptFOV = 30f;

        private Camera _cam;
        private Interactable _currentPrompt;
        private HashSet<Interactable> _nearbyInteractables = new();

        private void Awake()
        {
            _cam = Camera.main;

            // Ensure our detection collider is a trigger
            Collider col = GetComponent<Collider>();
            col.isTrigger = true;
        }

        private void Update()
        {
            UpdateNearbyMarkers();
            UpdatePromptFromNearby();
        }

        private void OnTriggerEnter(Collider other)
        {
            Interactable interactable = other.GetComponent<Interactable>();
            if (interactable != null)
                _nearbyInteractables.Add(interactable);
        }


        private void OnTriggerExit(Collider other)
        {
            if (((1 << other.gameObject.layer) & interactableLayer) != 0)
            {
                Interactable interactable = other.GetComponent<Interactable>();
                if (interactable != null)
                {
                    _nearbyInteractables.Remove(interactable);
                    interactable.HideUI();

                    if (_currentPrompt == interactable)
                        _currentPrompt = null;
                }
            }
        }

        /// <summary>
        /// Show/hide markers for interactables in range & FOV.
        /// </summary>
        private void UpdateNearbyMarkers()
        {
            foreach (var interactable in _nearbyInteractables)
            {
                if (interactable == null) continue;

                Vector3 uiPos = interactable.GetUIWorldPosition();
                Vector3 dirToUI = uiPos - _cam.transform.position;

                float angle = Vector3.Angle(_cam.transform.forward, dirToUI);
                bool inFOV = angle < markerFOV * 0.5f;
                bool hasLOS = HasLineOfSight(interactable, uiPos);

                if (inFOV && hasLOS && interactable != _currentPrompt)
                {
                    interactable.ShowMarker();
                }
                else if (interactable != _currentPrompt) // only hide if it's NOT the prompt
                {
                    interactable.HideUI();
                }
            }
        }

        private void UpdatePromptFromNearby()
        {
            Interactable bestInteractable = null;
            float closestDist = Mathf.Infinity;

            foreach (var interactable in _nearbyInteractables)
            {
                if (interactable == null) continue;

                Vector3 uiPos = interactable.GetUIWorldPosition();
                Vector3 dirToUI = uiPos - _cam.transform.position;
                float dist = dirToUI.magnitude;

                bool inFOV = Vector3.Angle(_cam.transform.forward, dirToUI) < promptFOV * 0.5f;
                bool hasLOS = HasLineOfSight(interactable, uiPos);

                if (inFOV && hasLOS && dist <= interactRange && dist < closestDist)
                {
                    bestInteractable = interactable;
                    closestDist = dist;
                }
            }

            // Hide prompt for old one if changed
            if (_currentPrompt != null && _currentPrompt != bestInteractable)
            {
                _currentPrompt.HideUI();
            }

            _currentPrompt = bestInteractable;

            foreach (var interactable in _nearbyInteractables)
            {
                if (interactable == null) continue;

                if (interactable == _currentPrompt)
                {
                    interactable.ShowPrompt(); // State = 2
                }
                else
                {
                    // Check if they should be marker instead
                    Vector3 uiPos = interactable.GetUIWorldPosition();
                    Vector3 dirToUI = uiPos - _cam.transform.position;

                    bool inFOV = Vector3.Angle(_cam.transform.forward, dirToUI) < markerFOV * 0.5f;
                    bool hasLOS = HasLineOfSight(interactable, uiPos);

                    if (inFOV && hasLOS)
                        interactable.ShowMarker(); // State = 1
                    else
                        interactable.HideUI();      // State = 0
                }
            }
        }

        private bool HasLineOfSight(Interactable interactable, Vector3 targetPos)
        {
            Vector3 origin = _cam.transform.position;
            Vector3 dir = targetPos - origin;
            float dist = dir.magnitude;
            dir.Normalize();

            if (Physics.Raycast(origin, dir, out RaycastHit hit, dist))
            {
                return hit.collider.GetComponentInParent<Interactable>() == interactable;
            }
            return true;
        }

        private void OnDrawGizmosSelected()
        {
            if (_cam == null) _cam = Camera.main;
            if (_cam == null) return;

            // Draw interact range sphere in blue
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(_cam.transform.position, interactRange);

            // Draw the trigger collider radius in green (if SphereCollider)
            Collider col = GetComponent<Collider>();
            if (col is SphereCollider sphereCol)
            {
                Gizmos.color = new Color(0, 1f, 0, 0.3f);
                Gizmos.DrawWireSphere(transform.position, sphereCol.radius);
            }

            // Draw marker FOV cone in yellow
            DrawFOVGizmo(_cam.transform.position, _cam.transform.forward, markerFOV, interactRange, Color.yellow);

            // Draw prompt FOV cone in cyan
            DrawFOVGizmo(_cam.transform.position, _cam.transform.forward, promptFOV, interactRange, Color.cyan);

            // Draw line to current prompt in red
            if (_currentPrompt != null)
            {
                Gizmos.color = Color.red;
                Vector3 uiPos = _currentPrompt.GetUIWorldPosition();
                Gizmos.DrawLine(_cam.transform.position, uiPos);
            }
        }

        private void DrawFOVGizmo(Vector3 origin, Vector3 forward, float fovDegrees, float length, Color color)
        {
            Gizmos.color = color;

            int stepCount = 30;
            float stepAngle = fovDegrees / stepCount;
            Quaternion leftRayRotation = Quaternion.AngleAxis(-fovDegrees * 0.5f, Vector3.up);
            Quaternion rightRayRotation = Quaternion.AngleAxis(fovDegrees * 0.5f, Vector3.up);

            Vector3 leftRayDirection = leftRayRotation * forward;
            Vector3 rightRayDirection = rightRayRotation * forward;

            Gizmos.DrawRay(origin, leftRayDirection * length);
            Gizmos.DrawRay(origin, rightRayDirection * length);

            // Draw arc between left and right ray
            Vector3 previousPoint = origin + leftRayDirection * length;
            for (int i = 1; i <= stepCount; i++)
            {
                float currentAngle = -fovDegrees * 0.5f + stepAngle * i;
                Vector3 nextDirection = Quaternion.AngleAxis(currentAngle, Vector3.up) * forward;
                Vector3 nextPoint = origin + nextDirection * length;
                Gizmos.DrawLine(previousPoint, nextPoint);
                previousPoint = nextPoint;
            }
        }
    }
}
