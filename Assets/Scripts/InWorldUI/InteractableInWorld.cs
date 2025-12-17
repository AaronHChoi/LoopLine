using UnityEngine;

namespace InWorldUI
{
    public class InteractableInWorld : MonoBehaviour
    {
        public string promptMessage = "Press E";
        public Vector3 uiOffset = new Vector3(0, 0f, 0);
        public Vector3 uiOffsetPrompt = new Vector3(0, 0f, 0);
        public float uiScale = 1f;

        private InteractableUI _uiInstance;
        private FadeState markerState;
        private FadeState promptState;


        void Start()
        {
            markerState = FadeState.FadeIn;
            promptState = FadeState.FadeIn;
            CreateUIInstance();
        }

        private void CreateUIInstance()
        {
            if (_uiInstance != null) return;

            if (InteractionUIManager.instance == null || InteractionUIManager.instance.uiPrefab == null)
            {
                Debug.LogWarning($"[Interactable] No InteractionUIManager or prefab on scene for {name}");
                return;
            }

            _uiInstance = Instantiate(
                InteractionUIManager.instance.uiPrefab,
                GetUIWorldPosition(),
                Quaternion.identity,
                transform // parent at creation to keep hierarchy clean
            );
            
            _uiInstance.transform.localScale = Vector3.one * uiScale;
            _uiInstance.Init(promptMessage, uiOffsetPrompt);
        }

        public Vector3 GetUIWorldPosition()
        {
            // Apply X and Z offset in local space
            Vector3 localOffset = new Vector3(uiOffset.x, 0, uiOffset.z);
            Vector3 basePosition = transform.TransformPoint(localOffset);
            
            // Apply Y offset towards camera (using billboard's forward direction)
            if (_uiInstance != null)
            {
                BillboardUI billboard = _uiInstance.GetComponent<BillboardUI>();
                if (billboard != null && Camera.main != null)
                {
                    // Get direction from this object to camera
                    Vector3 toCamera = (Camera.main.transform.position - transform.position).normalized;
                    basePosition += toCamera * uiOffset.y;
                }
                else
                {
                    // Fallback to world Y if billboard or camera not available
                    basePosition += Vector3.up * uiOffset.y;
                }
            }
            else
            {
                // Initial creation - use world Y temporarily
                basePosition += Vector3.up * uiOffset.y;
            }
            
            return basePosition;
        }

        public void ShowMarker()
        {
            if (markerState == FadeState.FadeIn)
            {
                if (_uiInstance == null || _uiInstance.gameObject == null) 
                    CreateUIInstance();
                if (_uiInstance == null) return;
                _uiInstance.ShowMarker();
                markerState = FadeState.FadeOut;
            }
        }

        public void ShowPrompt()
        {
            if (promptState == FadeState.FadeIn)
            {
                if (_uiInstance == null || _uiInstance.gameObject == null) 
                    CreateUIInstance();
                if (_uiInstance == null) return;

                _uiInstance.ShowPrompt();
                promptState = FadeState.FadeOut;
            }
        }

        public void HideMarker()
        {
            if (markerState == FadeState.FadeOut)
            {
                if (_uiInstance != null)
                    _uiInstance.HideMarker();
                markerState = FadeState.FadeIn;
            }
        }
        public void HidePrompt()
        {
            if (promptState == FadeState.FadeOut)
            {
                if (_uiInstance != null)
                    _uiInstance.HidePrompt();
                promptState = FadeState.FadeIn;
            }
            
        }
        public void HideAll()
        {
            HideMarker();
            HidePrompt();
        }
        
        void LateUpdate()
        {
            // Update UI position every frame to account for camera movement
            if (_uiInstance != null && Camera.main != null)
            {
                _uiInstance.transform.position = GetUIWorldPosition();
            }
        }
    }
}