using UnityEngine;

namespace InWorldUI
{
    public class InteractionUIManager : MonoBehaviour
    {
        public static InteractionUIManager instance;
        public InteractableUI uiPrefab;

        void Awake()
        {
            instance = this;
        }
    }
}