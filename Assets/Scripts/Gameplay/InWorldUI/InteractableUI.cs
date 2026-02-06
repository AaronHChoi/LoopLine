using UnityEngine;
using TMPro;
using Core.UI;
using Core.DependencyInjection;

namespace InWorldUI
{
    public class InteractableUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI promptText;
        [SerializeField] private GameObject labelCanvas;   // Assign the canvas with the prompt/label
        [SerializeField] private GameObject markerCanvas;  // Assign the canvas with the marker

        IFadeInOutController fadeInOutLabel;
        IFadeInOutController fadeInOutMarker;

        void Awake()
        {
            var fadeControllers = GetComponentsInChildren<IFadeInOutController>();
            
            if (fadeControllers.Length >= 2)
            {
                fadeInOutLabel = fadeControllers[0];   // First canvas
                fadeInOutMarker = fadeControllers[1];  // Second canvas
            }

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
            if (fadeInOutLabel != null && fadeInOutLabel.gameObject != null)
            {
                var canvasTransform = fadeInOutLabel.gameObject.GetComponent<Canvas>().transform;
                if (canvasTransform != null)
                    canvasTransform.position = canvasTransform.position + canvasTransform.rotation * promptOffset;
            }
        }
        
        private void SetMessage(string message)
        {
            if (promptText != null)
                promptText.text = message;
        }
        
        public void ShowPrompt()
        {
            if (fadeInOutLabel != null)
                fadeInOutLabel.ForceFade(true);
        }
        
        public void HidePrompt()
        {
            if (fadeInOutLabel != null)
                fadeInOutLabel.ForceFade(false);
        }
        
        public void ShowMarker()
        {
            if (fadeInOutMarker != null)
                fadeInOutMarker.ForceFade(true);
        }
        
        public void HideMarker()
        {
            if (fadeInOutMarker != null)
                fadeInOutMarker.ForceFade(false);
        }
    }
}