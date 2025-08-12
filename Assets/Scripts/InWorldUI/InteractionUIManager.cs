using UnityEngine;

namespace InWorldUI
{
    public class InteractionUIManager : MonoBehaviour
    {
        public static InteractionUIManager Instance;
        public InteractableUI uiPrefab;

        void Awake()
        {
            Instance = this;
        }
    }
}