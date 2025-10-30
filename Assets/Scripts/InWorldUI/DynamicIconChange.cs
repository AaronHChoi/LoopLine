using UnityEngine;
using UnityEngine.UI;

namespace InWorldUI
{
    public class DynamicIconChange : MonoBehaviour
    {
        [Header("UI Icon Settings")]
        [SerializeField] private Image iconImage;       // The UI Image component on the prefab
        [SerializeField] private Sprite grabIcon;       // Icon for when canBePicked = true
        [SerializeField] private Sprite interactIcon;   // Icon for when canBePicked = false

        private BaseItemInteract _baseInteract;

        private void Awake()
        {
            _baseInteract = GetComponentInParent<BaseItemInteract>();

            if (_baseInteract == null)
            {
                Debug.LogError($"[DynamicIconChange] No ItemInteract found in parent for {gameObject.name}");
            }

            if (iconImage == null)
            {
                Debug.LogError($"[DynamicIconChange] No Image assigned on {gameObject.name}");
            }
        }

        private void LateUpdate()
        {
            if (_baseInteract == null || iconImage == null) return;

            // Switch icon depending on canBePicked
            if (_baseInteract != null ) 
                iconImage.sprite = _baseInteract.canBePicked ? grabIcon : interactIcon;
        }
    }
}