using UnityEngine;

namespace InWorldUI
{
    public class InteractableInWorld : MonoBehaviour
    {
        public string promptMessage = "Press E";
        public Vector3 uiOffset = new Vector3(0, 0f, 0);
        public Vector3 uiOffsetPrompt = new Vector3(0, 0f, 0);

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
            
            _uiInstance.Init(promptMessage, uiOffsetPrompt);
        }

        public Vector3 GetUIWorldPosition()
        {
            return transform.TransformPoint(uiOffset);
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
    }
}