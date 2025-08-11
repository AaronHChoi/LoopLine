using UnityEngine;

namespace InWorldUI
{
    public class Interactable : MonoBehaviour
    {
        public string promptMessage = "Press E";
        public float interactDistance = 3f; // will add in world ui activation when near player
        public float interactYOffSet = 0f;
        private InteractableUI _uiInstance;

        public void ShowUI(Transform target)
        {
            if (InteractionUIManager.Instance == null || InteractionUIManager.Instance.uiPrefab == null)
            {
                Debug.LogError("No InteractionUIManager or prefab assigned!");
                return;
            }

            if (_uiInstance == null)
            {
                Vector3 spawnPos = target.position;

                _uiInstance = Instantiate(
                    InteractionUIManager.Instance.uiPrefab,
                    spawnPos,
                    Quaternion.identity
                );

                _uiInstance.transform.SetParent(target, true);
                _uiInstance.SetMessage(promptMessage);
            }
            _uiInstance.Show();
        }

        public void HideUI()
        {
            if (_uiInstance != null)
                _uiInstance.Hide();
        }
    }
}