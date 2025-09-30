using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

namespace NPCSelector
{
    public class NPCSlot : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Image portraitImage;
        [SerializeField] private Image overlayImage; // for ??? or greyscale overlay
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private Button button;
        [SerializeField] private Image outline;
        
        [Header("Config")]
        public int npcId; // unique ID set by BoardSelectorController
        private NPCState currentState;

        public UnityEvent<int> OnSlotClicked = new UnityEvent<int>();

        private void Awake()
        {
            if (button != null)
                button.onClick.AddListener(() => OnSlotClicked.Invoke(npcId));
        }
        
        /// Apply a visual state to this slot.
        
        public void SetState(NPCState state, Sprite portrait = null, string npcName = "")
        {
            currentState = state;

            // Default resets
            portraitImage.sprite = portrait;
            portraitImage.color = Color.white;
            nameText.text = "";
            overlayImage.enabled = false;
            outline.enabled = false;

            switch (state)
            {
                case NPCState.Locked:
                    button.interactable = false;
                    outline.enabled = false;
                    portraitImage.color = Color.grey;
                    break;

                case NPCState.Unknown:
                    button.interactable = true;
                    portraitImage.color = Color.black;
                    break;

                case NPCState.NoName:
                    button.interactable = true;
                    overlayImage.enabled = true; // Show ??? sprite or texture
                    break;

                case NPCState.Named:
                    button.interactable = true;
                    nameText.text = npcName;
                    break;
            }
        }
        
        public void SetSelected(bool selected)
        {
            outline.enabled = selected;
        }
    }

}