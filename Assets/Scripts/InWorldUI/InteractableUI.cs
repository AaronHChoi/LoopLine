using UnityEngine;
using TMPro;

namespace InWorldUI
{
    public class InteractableUI : MonoBehaviour
    {
        private static readonly int Visible = Animator.StringToHash("Visible");
        [SerializeField] private TextMeshProUGUI promptText;
        [SerializeField] private Animator animator;

        void Awake()
        {
            if (animator == null)
                animator = GetComponentInChildren<Animator>();

            if (promptText == null)
                promptText = GetComponentInChildren<TextMeshProUGUI>();
        }

        public void SetMessage(string message)
        {
            if (promptText != null)
                promptText.text = message;
        }

        public void Show()
        {
            if (animator != null)
                animator.SetBool(Visible, true);
        }

        public void Hide()
        {
            if (animator != null)
                animator.SetBool(Visible, false);
        }
    }
}