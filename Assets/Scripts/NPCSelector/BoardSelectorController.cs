using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using Unity.Cinemachine;

namespace NPCSelector
{
    public class BoardSelectorController : MonoBehaviour
    {
        [Header("Cameras")]
        [SerializeField] private CinemachineCamera firstPersonCam;
        [SerializeField] private CinemachineCamera boardCam;

        [Header("UI")]
        [SerializeField] private GameObject selectorPanel;
        [SerializeField] private Transform slotContainer;
        [SerializeField] private NPCSlot slotPrefab;
        
        [Header("Sprites")]
        [SerializeField] private Sprite blankPortrait;      // Locked
        [SerializeField] private Sprite blackPortrait;      // Unknown
        [SerializeField] private Sprite[] npcPortraits;     // 1 per NPC

        [Header("Board Lighting")]
        [SerializeField] private Light boardLight;

        [Header("Config")]
        [SerializeField] private bool startInFirstPerson = true;

        // Events
        public UnityEvent<int> OnNPCSelected = new UnityEvent<int>();

        private List<NPCSlot> slots = new List<NPCSlot>();
        private bool inBoardView = false;
        private int selectedSlotId = -1; // track which slot is selected

        private void Start()
        {
            //test
            CreateSlots(4); // creates 4 test NPC slots
            //endtest
            
            // Ensure state is correct at start
            SetBoardView(false);

            if (startInFirstPerson)
                EnterFirstPerson();
            
            //test states
            UpdateNPCSlot(0, NPCState.Locked);
            UpdateNPCSlot(1, NPCState.Locked);
            UpdateNPCSlot(2, NPCState.Named, null, "");
            UpdateNPCSlot(3, NPCState.Named, null, "");
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
                ToggleBoardView();
        }

        /// Create slots dynamically for a given NPC count.
        public void CreateSlots(int npcCount)
        {
            foreach (Transform child in slotContainer)
                Destroy(child.gameObject);
            slots.Clear();
            selectedSlotId = -1;

            for (int i = 0; i < npcCount; i++)
            {
                var slot = Instantiate(slotPrefab, slotContainer);
                slot.npcId = i;
                slot.OnSlotClicked.AddListener(HandleSlotClick);
                slots.Add(slot);
            }
        }

        /// Update the state of one NPC slot (called by external manager).
        public void UpdateNPCSlot(int npcId, NPCState state, Sprite overridePortrait = null, string npcName = "")
        {
            if (npcId < 0 || npcId >= slots.Count) return;

            Sprite portraitToUse = overridePortrait;

            if (portraitToUse == null)
            {
                switch (state)
                {
                    case NPCState.Locked:
                        portraitToUse = blankPortrait;
                        break;
                    case NPCState.Unknown:
                        portraitToUse = blackPortrait;
                        break;
                    case NPCState.NoName:
                    case NPCState.Named:
                        if (npcId < npcPortraits.Length)
                            portraitToUse = npcPortraits[npcId];
                        else
                        {
                            portraitToUse = blankPortrait;
                            Debug.LogWarning($"NPCSelector: npcId {npcId} is out of bounds for npcPortraits (length {npcPortraits.Length}). Using blankPortrait as fallback.");
                        }
                        break;
                }
            }

            slots[npcId].SetState(state, portraitToUse, npcName);
        }

        public void ToggleBoardView()
        {
            SetBoardView(!inBoardView);
        }

        private void SetBoardView(bool active)
        {
            inBoardView = active;

            if (active)
            {
                EnterBoardView();
                ResetSelection();
                if (boardLight != null) boardLight.enabled = true;
            }
            else
            {
                EnterFirstPerson();
                if (boardLight != null) boardLight.enabled = false;
            }
        }

        private void EnterBoardView()
        {
            if (boardCam != null) boardCam.gameObject.SetActive(true);
            if (boardCam != null) boardCam.Priority = 20;
            if (firstPersonCam != null) firstPersonCam.Priority = 10;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void EnterFirstPerson()
        {
            if (boardCam != null) boardCam.gameObject.SetActive(false);
            if (boardCam != null) boardCam.Priority = 10;
            if (firstPersonCam != null) firstPersonCam.Priority = 20;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void HandleSlotClick(int npcId)
        {
            slots[npcId].SetSelected(true);
            OnNPCSelected.Invoke(npcId);

            // Exit board view (but keep portraits visible in next entry)
            SetBoardView(false);
        }
        
        private void ResetSelection()
        {
            selectedSlotId = -1;
            foreach (var slot in slots)
                slot.SetSelected(false);
        }
    }
}
