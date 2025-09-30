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
        
        public void SetState(NPCState state, Sprite portrait = null, string npcName = "")
        {
            currentState = state;

            // Always set portrait (blank, black, or NPC art provided by controller)
            portraitImage.sprite = portrait;
            portraitImage.color = Color.white;

            // Reset visuals
            nameText.text = "";
            outline.enabled = false;

            switch (state)
            {
                case NPCState.Locked:
                    button.interactable = false;
                    // Portrait will be set to blank sprite by controller
                    break;

                case NPCState.Unknown:
                    button.interactable = true;
                    // Portrait will be set to black sprite by controller; no name text shown
                    break;

                case NPCState.NoName:
                    button.interactable = true;
                    nameText.text = "???";
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
