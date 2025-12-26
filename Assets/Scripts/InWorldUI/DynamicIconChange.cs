using UnityEngine;
using UnityEngine.UI;

namespace InWorldUI
{
    public class DynamicIconChange : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private Sprite grabIcon;
        [SerializeField] private Sprite interactIcon;

        ItemInteract item;
        IInteract interact;

        void OnEnable()
        {
            item = GetComponentInParent<ItemInteract>();
            interact = GetComponentInParent<IInteract>();

            if (!iconImage)
                iconImage = GetComponentInChildren<Image>();

            UpdateIcon();
        }

        void UpdateIcon()
        {
            if (item != null)
            {
                iconImage.sprite = item.canBePicked ? grabIcon : interactIcon;
                return;
            }

            if (interact != null)
            {
                iconImage.sprite = interactIcon;
            }
        }
    }
}