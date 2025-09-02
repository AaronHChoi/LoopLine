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
        [SerializeField] private float promptFOV = 30f;

        [SerializeField] private bool detectInteractables = true;

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
            if (detectInteractables)
            {
                RefreshInteractablesUI();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (((1 << other.gameObject.layer) & interactableLayer) != 0)
            {
                Interactable interactable = other.GetComponent<Interactable>();
                if (interactable != null)
                {
                    _nearbyInteractables.Add(interactable);
                    if (detectInteractables)
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
                    interactable.HideAll();

                    if (_currentPrompt == interactable)
                        _currentPrompt = null;
                }
            }
        }

        private void RefreshInteractablesUI()
        {
            Interactable closestInteractable = null;
            float closestDist = Mathf.Infinity;

            foreach (var interactable in _nearbyInteractables)
            {
                if (interactable == null) continue;

                float dist = Vector3.Distance(_cam.transform.position, interactable.GetUIWorldPosition());

                if (IsInFOV(interactable, promptFOV) && dist <= interactRange)
                {
                    if (dist < closestDist)
                    {
                        closestInteractable = interactable;
                        closestDist = dist;
                    }
                }
            }

            // Update all interactables
            foreach (var interactable in _nearbyInteractables)
            {
                if (interactable == null) continue;

                if (interactable == closestInteractable)
                {
                    if (_currentPrompt != interactable)
                    {
                        _currentPrompt?.HidePrompt();
                        _currentPrompt?.ShowMarker();
                        _currentPrompt = interactable;
                    }
                    interactable.HideMarker();
                    interactable.ShowPrompt();
                }
                else
                {
                    interactable.HidePrompt();
                    interactable.ShowMarker();
                }
            }

            if (closestInteractable == null)
            {
                _currentPrompt = null;
            }
        }

        private bool IsInFOV(Interactable interactable, float fov)
        {
            Vector3 dirToUI = interactable.GetUIWorldPosition() - _cam.transform.position;
            float angle = Vector3.Angle(_cam.transform.forward, dirToUI);
            bool hasLOS = !Physics.Raycast(_cam.transform.position, dirToUI.normalized, out RaycastHit hit, dirToUI.magnitude) || hit.collider.GetComponentInParent<Interactable>() == interactable;
            return angle < fov * 0.5f && hasLOS;
        }

        public void SetInteractableDetection(bool enabled)
        {
            detectInteractables = enabled;

            if (!enabled)
            {
                foreach (var interactable in _nearbyInteractables)
                {
                    interactable?.HideAll();
                }
                _currentPrompt = null;
            }
            else
            {
                RefreshInteractablesUI();
            }
        }
    }
}
