using UnityEngine;
using TMPro;
using Core.UI;
using Core.DependencyInjection;

namespace InWorldUI
{
    public class InteractableUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI promptText;

        IFadeInOutController fadeInOutLabel;
        IFadeInOutController fadeInOutMarker;

        void Awake()
        {
            fadeInOutLabel = InterfaceDependencyInjector.Instance.Resolve<IFadeInOutController>(FadeID.Canvas);
            fadeInOutMarker = InterfaceDependencyInjector.Instance.Resolve<IFadeInOutController>(FadeID.Canvas);

            if (promptText == null)
                promptText = GetComponentInChildren<TextMeshProUGUI>();
        }
        public void Init(string promptMessage, Vector3 promptOffset)
        {
            SetMessage(promptMessage);
            MovePromptOffset(promptOffset);
            
        }
        private void MovePromptOffset(Vector3 promptOffset)
        {
            var canvasTransform = fadeInOutLabel.gameObject.GetComponent<Canvas>().transform;
            canvasTransform.position = canvasTransform.position + canvasTransform.rotation * promptOffset;
        }
        private void SetMessage(string message)
        {
            if (promptText != null)
                promptText.text = message;
        }
        public void ShowPrompt()
        {
            fadeInOutLabel.ForceFade(true);
        }
        public void HidePrompt()
        {
            fadeInOutLabel.ForceFade(false);
        }
        public void ShowMarker()
        {
            fadeInOutMarker.ForceFade(true);
        }
        public void HideMarker()
        {
            fadeInOutMarker.ForceFade(false);
        }
    }
}