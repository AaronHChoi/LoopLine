using UnityEngine;

namespace InWorldUI
{
    [RequireComponent(typeof(Collider))]
    public class PlayerInteractMarkerPrompt : MonoBehaviour
    {
        [SerializeField] private RaycastController rayController;
        [Space]
        [SerializeField] private bool isDetecting = true;
        [Space]
        [Header("Marker Settings")]
        [SerializeField] private LayerMask interactableLayer;
        [Space]
        [Header("Prompt Settings")]
        [SerializeField, Range(0f,1f)] private float minScoreAllowed;

        private InteractableInWorld currentPrompt;
        private InteractableInWorld auxTargetInteractable;

        public bool IsDetecting { get { return isDetecting; } set { isDetecting = value; } }

        private void Update()
        {
            if (!isDetecting) return;

            bool validTarget =
                rayController.FoundInteract &&
                rayController.BestScore > minScoreAllowed &&
                rayController.Target.TryGetComponent(out auxTargetInteractable);

            bool shouldClear =
                (!rayController.FoundInteract && currentPrompt != null) ||
                (rayController.FoundInteract && rayController.BestScore < minScoreAllowed && rayController.Target == currentPrompt?.gameObject);

            if (validTarget)
            {
                if (currentPrompt != auxTargetInteractable)
                {
                    currentPrompt?.HidePrompt(); // hide old prompt
                    currentPrompt?.ShowMarker(); // show old prompt marker

                    currentPrompt = auxTargetInteractable;
                    currentPrompt.HideMarker(); // hide new prompt marker
                    currentPrompt.ShowPrompt(); // show new prompt

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
            if (isDetecting && ((1 << other.gameObject.layer) & interactableLayer) != 0)
            {
                if (other.TryGetComponent<InteractableInWorld>(out InteractableInWorld interactable))
                    interactable.ShowMarker();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (isDetecting && ((1 << other.gameObject.layer) & interactableLayer) != 0)
            {
                if (other.TryGetComponent<InteractableInWorld>(out InteractableInWorld interactable))
                {
                    interactable.HideAll();

                    if (currentPrompt == interactable)
                        currentPrompt = null;
                }
            }
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
