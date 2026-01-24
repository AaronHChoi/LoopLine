using UnityEngine;
using System.Collections.Generic;

namespace InWorldUI
{
    [RequireComponent(typeof(Collider))]
    public class PlayerInteractMarkerPrompt : MonoBehaviour, IPlayerInteractMarkerPrompt
    {
        [SerializeField] private RaycastController rayController;
        [Space]
        [SerializeField] private bool isDetecting = true;
        [Space]
        [Header("Marker Settings")]
        [SerializeField] private LayerMask interactableLayer;

        private InteractableInWorld currentPrompt;
        private InteractableInWorld auxTargetInteractable;
        // Track interactables inside trigger area
        private readonly HashSet<InteractableInWorld> nearbyInteractables = new HashSet<InteractableInWorld>();

        public bool IsDetecting { get { return isDetecting; } set { isDetecting = value; } }

        private void Update()
        {
            if (!isDetecting) return;

            bool validTarget = false;
            if (rayController.FoundInteract && rayController.Target != null)
            {
                auxTargetInteractable = GetInteractableFromHierarchy(rayController.Target);
                // Only valid if target is inside trigger area
                validTarget = auxTargetInteractable != null && nearbyInteractables.Contains(auxTargetInteractable);
            }

            bool shouldClear =
                (!rayController.FoundInteract && currentPrompt != null) ||
                (rayController.FoundInteract && currentPrompt != null && !nearbyInteractables.Contains(currentPrompt)) ||
                (rayController.FoundInteract && auxTargetInteractable != currentPrompt);

            if (validTarget)
            {
                if (currentPrompt != auxTargetInteractable)
                {
                    currentPrompt?.HidePrompt();
                    currentPrompt?.ShowMarker();

                    currentPrompt = auxTargetInteractable;
                    currentPrompt.HideMarker();
                    currentPrompt.ShowPrompt();

                    auxTargetInteractable = null;
                }
            }
            else if (shouldClear)
            {
                currentPrompt?.HidePrompt();
                currentPrompt?.ShowMarker();
                currentPrompt = null;
            }
        }

        private void OnTriggerEnter(Collider other) 
        {
            if (!isDetecting) return;
            if (((1 << other.gameObject.layer) & interactableLayer) == 0) return;

            InteractableInWorld interactable = GetInteractableFromHierarchy(other.gameObject);
            if (interactable != null)
            {
                nearbyInteractables.Add(interactable);
                // Show marker when entering area
                interactable.ShowMarker();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!isDetecting) return;
            if (((1 << other.gameObject.layer) & interactableLayer) == 0) return;

            InteractableInWorld interactable = GetInteractableFromHierarchy(other.gameObject);
            if (interactable != null)
            {
                // Leaving area: hide prompt and show marker (or hide all)
                if (currentPrompt == interactable)
                {
                    currentPrompt.HidePrompt();
                    currentPrompt = null;
                }
                interactable.HideAll();
                nearbyInteractables.Remove(interactable);
            }
        }

        private InteractableInWorld GetInteractableFromHierarchy(GameObject target)
        {
            InteractableInWorld interactable = target.GetComponent<InteractableInWorld>();
            if (interactable != null) return interactable;
            interactable = target.GetComponentInParent<InteractableInWorld>();
            if (interactable != null) return interactable;
            interactable = target.GetComponentInChildren<InteractableInWorld>();
            return interactable;
        }

        #region DEPRECATED
        /*private bool IsInFOV(InteractableInWorld interactable, float fov)
        {
            Vector3 dirToUI = interactable.GetUIWorldPosition() - cam.transform.position;
            float angle = Vector3.Angle(cam.transform.forward, dirToUI);
            bool hasLOS = !Physics.Raycast(cam.transform.position, dirToUI.normalized, out RaycastHit hit, dirToUI.magnitude) || hit.collider.GetComponentInParent<InteractableInWorld>() == interactable;
            return angle < fov * 0.5f && hasLOS;
        }*/
        #endregion
    }
}

public interface IPlayerInteractMarkerPrompt
{
    bool IsDetecting { get; set; }
}
