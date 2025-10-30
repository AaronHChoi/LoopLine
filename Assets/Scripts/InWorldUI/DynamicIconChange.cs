using UnityEngine;
using UnityEngine.UI;

namespace InWorldUI
{
    public class DynamicIconChange : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private Sprite grabIcon;
        [SerializeField] private Sprite interactIcon;

        BaseItemInteract item;

        void OnEnable()
        {
            if (item == null)
                item = GetComponentInParent<BaseItemInteract>();

            if (item == null)
            {
                Debug.LogWarning($"[DynamicIconChange] No BaseItemInteract found for {gameObject.name}");
                return;
            }

            if (!iconImage)
                iconImage = GetComponentInChildren<Image>();

            iconImage.sprite = item.canBePicked ? grabIcon : interactIcon;
        }
    }
}