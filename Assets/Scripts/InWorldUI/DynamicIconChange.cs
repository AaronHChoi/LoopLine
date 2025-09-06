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

        private ItemInteract _itemInteract;

        private void Awake()
        {
            // Find the ItemInteract script in the root of the parent object
            _itemInteract = GetComponentInParent<ItemInteract>();

            if (_itemInteract == null)
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
            if (_itemInteract == null || iconImage == null) return;

            // Switch icon depending on canBePicked
            iconImage.sprite = _itemInteract.canBePicked ? grabIcon : interactIcon;
        }
    }
}