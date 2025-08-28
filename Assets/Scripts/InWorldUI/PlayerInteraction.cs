using System.Collections.Generic;
using UnityEngine;

namespace InWorldUI
{
    [RequireComponent(typeof(Collider))]
    public class PlayerInteraction : MonoBehaviour
    {
        [Header("Prompt Settings")]
        [SerializeField] public float interactRange = 3f;
        [SerializeField] private LayerMask interactableLayer;

        [Header("Marker Settings")]
        [SerializeField] private float promptFOV = 30f;

        private Camera _cam;
        private Interactable _currentPrompt;
        private HashSet<Interactable> _nearbyInteractables = new();

        private void Awake()
        {
            _cam = Camera.main;

            Collider col = GetComponent<Collider>();
            col.isTrigger = true;
        }

        private void Update()
        {
            UpdatePromptFromNearby();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (((1 << other.gameObject.layer) & interactableLayer) != 0)
            {
                Interactable interactable = other.GetComponent<Interactable>();
                if (interactable != null)
                {
                    _nearbyInteractables.Add(interactable);
                    interactable.ShowMarker();
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (((1 << other.gameObject.layer) & interactableLayer) != 0)
            {
                Interactable interactable = other.GetComponent<Interactable>();
                if (interactable != null)
                {
                    _nearbyInteractables.Remove(interactable);
                    interactable.HideMarker();

                    if (_currentPrompt == interactable)
                        _currentPrompt = null;
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

                if (CheckInteractInVision(interactable, promptFOV))
                {
                    float dist = GetInteractableRange(interactable);

                    if (dist <= interactRange && dist < closestDist)
                    {
                        bestInteractable = interactable;
                        closestDist = dist;
                    }
                }
            }

            // Hide prompt for old one if changed
            if (_currentPrompt != null && _currentPrompt != bestInteractable)
            {
                _currentPrompt.HidePrompt();
                _currentPrompt.ShowMarker();
            }

            if (bestInteractable != null)
            {
                _currentPrompt = bestInteractable;
                bestInteractable.HideMarker();
                bestInteractable.ShowPrompt();
            }
            else if (_currentPrompt != null)
            {
                _currentPrompt.HidePrompt();
                _currentPrompt.ShowMarker();
                _currentPrompt = null;
            }
        }

        private float GetInteractableRange(Interactable interactable)
        {
            Vector3 uiPos = interactable.GetUIWorldPosition();
            Vector3 dirToUI = uiPos - _cam.transform.position;
            float dist = dirToUI.magnitude;
            return dist;
        }
        private bool CheckInteractInVision(Interactable _interactable, float FOV)
        {
            bool isInFOV = false;
            if (_interactable == null) return isInFOV;

            Vector3 uiPos = _interactable.GetUIWorldPosition();
            Vector3 dirToUI = uiPos - _cam.transform.position;

            float angle = Vector3.Angle(_cam.transform.forward, dirToUI);
            bool inFOV = angle < FOV * 0.5f;
            bool hasLOS = HasLineOfSight(_interactable, uiPos);

            if (inFOV && hasLOS)
            {
                isInFOV = true;
            }
            else
            {
                isInFOV = false;
            }
            return isInFOV;
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