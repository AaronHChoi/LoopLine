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
            
            //test
            // Set some test states
            UpdateNPCSlot(0, NPCState.Locked);
            UpdateNPCSlot(1, NPCState.Unknown);
            UpdateNPCSlot(2, NPCState.NoName);
            UpdateNPCSlot(3, NPCState.Named, null, "Alice");
            //endtest
        }
        
        //debug key to test function
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
                ToggleBoardView();
        }


        /// Create slots dynamically for a given NPC count.
        public void CreateSlots(int npcCount)
        {
            if (npcCount < 0)
            {
                Debug.LogWarning("CreateSlots called with negative npcCount: " + npcCount);
                return;
            }
            // Clear old
            foreach (Transform child in slotContainer)
                Destroy(child.gameObject);
            slots.Clear();
            selectedSlotId = -1;

            // Create new
            for (int i = 0; i < npcCount; i++)
            {
                var slot = Instantiate(slotPrefab, slotContainer);
                slot.npcId = i;
                slot.OnSlotClicked.AddListener(HandleSlotClick);
                slots.Add(slot);
            }
        }

        /// Update the state of one NPC slot (called by external manager).
        public void UpdateNPCSlot(int npcId, NPCState state, Sprite portrait = null, string npcName = "")
        {
            if (npcId < 0 || npcId >= slots.Count) return;
            slots[npcId].SetState(state, portrait, npcName);
        }

        /// Called when the board is interacted with (toggle between views).
        public void ToggleBoardView()
        {
            SetBoardView(!inBoardView);
        }

        private void SetBoardView(bool active)
        {
            inBoardView = active;

            // Show/hide panel depending on your design:
            // If you want slots to remain visible after selection, leave selectorPanel always active
            // selectorPanel.SetActive(active); // <-- remove this if you want slots always visible

            if (active)
            {
                EnterBoardView();
                ResetSelection();
                boardLight.enabled = true;
            }
            else
            {
                EnterFirstPerson();
                boardLight.enabled = false;
            }
        }

        private void EnterBoardView()
        {
            boardCam.gameObject.SetActive(true);
            if (boardCam != null) boardCam.Priority = 20;
            if (firstPersonCam != null) firstPersonCam.Priority = 10;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            // Optionally disable FPS controller input here
        }

        private void EnterFirstPerson()
        {
            boardCam.gameObject.SetActive(false);
            if (boardCam != null) boardCam.Priority = 10;
            if (firstPersonCam != null) firstPersonCam.Priority = 20;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            // Optionally enable FPS controller input here
        }

        private void HandleSlotClick(int npcId)
        {
            // Select slot visually (keep outline active)
            slots[npcId].SetSelected(true);

            // Fire event for external systems
            OnNPCSelected.Invoke(npcId);

            // Exit board view
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
