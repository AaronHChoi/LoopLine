using UnityEngine;

namespace InWorldUI
{
    public class Interactable : MonoBehaviour
    {
        public string promptMessage = "Press E";
        public Vector3 uiOffset = new Vector3(0, 0f, 0);

        private InteractableUI _uiInstance;

        void Start()
        {
            CreateUIInstance();
        }

        private void CreateUIInstance()
        {
            if (_uiInstance != null) return;

            if (InteractionUIManager.Instance == null || InteractionUIManager.Instance.uiPrefab == null)
            {
                Debug.LogWarning($"[Interactable] No InteractionUIManager or prefab on scene for {name}");
                return;
            }

            _uiInstance = Instantiate(
                InteractionUIManager.Instance.uiPrefab,
                GetUIWorldPosition(),
                Quaternion.identity,
                transform // parent at creation to keep hierarchy clean
            );
            _uiInstance.SetMessage(promptMessage);
            _uiInstance.Hide(); // Start hidden
        }

        public Vector3 GetUIWorldPosition()
        {
            return transform.TransformPoint(uiOffset);
        }

        public void ShowMarker()
        {
            if (_uiInstance == null || _uiInstance.gameObject == null) 
                CreateUIInstance();
            if (_uiInstance == null) return;
            _uiInstance.ShowMarker();
        }

        public void ShowPrompt()
        {
            if (_uiInstance == null || _uiInstance.gameObject == null) 
                CreateUIInstance();
            if (_uiInstance == null) return;

            _uiInstance.SetMessage(promptMessage);
            _uiInstance.ShowPrompt();
        }

        public void HideUI()
        {
            if (_uiInstance != null)
                _uiInstance.Hide();
        }
        
        public void HidePromptWithDelay(float delay = 0.5f)
        {
            if (_uiInstance != null)
                _uiInstance.HidePromptWithDelay(delay);
            
        }

    }
}