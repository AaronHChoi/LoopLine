using UnityEngine;
using UnityEngine.UI;

namespace InWorldUI
{
    public class DynamicIconChange : MonoBehaviour
    {
        [Header("Manual Icon Settings")]
        [SerializeField] private Image iconImage;
        [SerializeField] private Sprite grabIcon;
        [SerializeField] private Sprite interactIcon;

        [Tooltip("Toggle to choose which icon is shown")]
        [SerializeField] private bool showGrabIcon = true;

        void OnValidate()
        {
            if (!iconImage) return;
            iconImage.sprite = showGrabIcon ? grabIcon : interactIcon;
        }

        void Reset()
        {
            iconImage = GetComponentInChildren<Image>();
        }
    }
}