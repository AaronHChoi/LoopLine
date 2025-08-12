using System;
using System.Collections;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

namespace InWorldUI
{
    public class InteractableUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI promptText;
        [SerializeField] private Animator animator;
        [SerializeField] private CanvasGroup canvasGroup;
        private static readonly int State = Animator.StringToHash("State");
        
        [Tooltip("Cooldown time in seconds before toggling UI again")]
        [SerializeField] private float toggleCooldown = 0.15f;

        private Coroutine toggleRoutineVisible;
        private Coroutine toggleRoutineMarker;
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
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
                fadeCoroutine = null;
            }
            animator.SetInteger(State, 2);
        }

        public void ShowMarker()
        {
            animator.SetInteger(State, 1);
        }

        public void Hide()
        {
            animator.SetInteger(State, 0);
        }
        
        public void HidePromptWithDelay(float delay = 0.5f)
        {
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeOutPrompt(delay));
        }

        private void StartToggleRoutine(string paramName, bool state, ref Coroutine routine)
        {
            if (routine != null)
                StopCoroutine(routine);
            routine = StartCoroutine(ToggleBoolWithCooldown(paramName, state));
        }

        private IEnumerator ToggleBoolWithCooldown(string paramName, bool state)
        {
            yield return new WaitForSeconds(toggleCooldown);
            if (animator != null)
            {
                Debug.Log($"Setting Animator bool '{paramName}' to {state}");
                animator.SetBool(paramName, state);
            }
            else
            {
                Debug.LogWarning("Animator is null!");
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
