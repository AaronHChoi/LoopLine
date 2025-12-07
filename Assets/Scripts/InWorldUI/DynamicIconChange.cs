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
        IInteract interact;

        void OnEnable()
        {
            if (item == null)
                item = GetComponentInParent<BaseItemInteract>();

            if (item == null)
            {
                interact = GetComponentInParent<IInteract>() ;
                if (interact == null)
                    return;
                if (!iconImage)
                    iconImage = GetComponentInChildren<Image>();
                iconImage.sprite = interactIcon;
                return;
            }

            if (!iconImage)
                iconImage = GetComponentInChildren<Image>();

            iconImage.sprite = item.canBePicked ? grabIcon : interactIcon;
        }
    }
}