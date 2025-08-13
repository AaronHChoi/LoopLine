using System.Collections;
using UnityEngine;
using TMPro;

namespace InWorldUI
{
    public class InteractableUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI promptText;
        [SerializeField] private Animator animator;
        [SerializeField] private CanvasGroup canvasGroup;
        
        private static readonly int State = Animator.StringToHash("State");

        private Coroutine fadeCoroutine;

        void Awake()
        {
            if (animator == null)
                animator = GetComponentInChildren<Animator>();

            if (promptText == null)
                promptText = GetComponentInChildren<TextMeshProUGUI>();

            if (canvasGroup == null)
                canvasGroup = GetComponentInChildren<CanvasGroup>();
        }

        public void SetMessage(string message)
        {
            if (promptText != null)
                promptText.text = message;
        }

        public void ShowPrompt()
        {
            StopFade();
            animator.SetInteger(State, 2);
        }

        public void ShowMarker()
        {
            StopFade();
            animator.SetInteger(State, 1);
        }

        public void Hide()
        {
            StopFade();
            animator.SetInteger(State, 0);
        }
        
        public void HidePromptWithDelay(float delay = 0.5f)
        {
            StopFade();
            fadeCoroutine = StartCoroutine(FadeOutPrompt(delay));
        }

        private void StopFade()
        {
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
                fadeCoroutine = null;
            }
        }

        private IEnumerator FadeOutPrompt(float delay)
        {
            yield return new WaitForSeconds(delay);
            animator.SetInteger(State, 0);
            fadeCoroutine = null;
        }
    }
}