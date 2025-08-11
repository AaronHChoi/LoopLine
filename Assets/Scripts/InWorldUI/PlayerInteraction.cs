using UnityEngine;

namespace InWorldUI
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] private float interactRange = 3f;
        [SerializeField] private LayerMask interactableLayer;
        private Interactable _currentInteractable;

        void Update()
        {
            CheckForInteractable();
        }

        private void CheckForInteractable()
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableLayer))
            {
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    if (_currentInteractable != interactable)
                    {
                        if (_currentInteractable != null)
                            _currentInteractable.HideUI();

                        _currentInteractable = interactable;
                        _currentInteractable.ShowUI(hit.collider.transform);
                    }
                    return;
                }
            }

            if (_currentInteractable != null)
            {
                _currentInteractable.HideUI();
                _currentInteractable = null;
            }
        }
    }
}