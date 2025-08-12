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
                else
                {
                    interactable.HideUI();
                }
                    
            }
        }

        private void UpdatePromptFromNearby()
        {
            Ray ray = new Ray(_cam.transform.position, _cam.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableLayer))
            {
                Interactable interactable = hit.collider.GetComponent<Interactable>();

                if (interactable != null && _nearbyInteractables.Contains(interactable))
                {
                    if (_currentPrompt != interactable)
                    {
                        if (_currentPrompt != null)
                            _currentPrompt.HidePromptWithDelay(0.5f);  // Use delayed hide here

                        _currentPrompt = interactable;
                        _currentPrompt.ShowPrompt();
                    }
                    return;
                }
            }

            // No prompt match
            if (_currentPrompt != null)
            {
                _currentPrompt.HidePromptWithDelay(0.5f);  // Use delayed hide here too
                _currentPrompt = null;
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
    }
}
