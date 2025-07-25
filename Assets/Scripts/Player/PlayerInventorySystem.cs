using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;

public class PlayerInventorySystem : MonoBehaviour, IDependencyInjectable
{
    public static PlayerInventorySystem Instance { get; private set; }
    [Header("Inventory Settings")]
    [SerializeField] public List<ItemInteract> inventory = new List<ItemInteract>();
    [SerializeField] public Transform SpawnPosition;
    [SerializeField] public ItemInteract ItemInUse;

    FocusModeManager focusModeManager;
    PlayerController playerController;
    InventoryUI inventoryUI;
    DialogueManager dialogueManager;
    public delegate void InventoryChanged();
    public event InventoryChanged OnInventoryChanged;
    bool isCursorVisible = false;
    bool isUIActive = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        InjectDependencies(DependencyContainer.Instance);
        inventoryUI.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (inventoryUI.gameObject.activeInHierarchy == true)
        {
            playerController.characterController.enabled = false;
        }
        else
        {
            playerController.characterController.enabled = true;
        }
        UnityEngine.Debug.Log(ItemInUse);
        InputHandler();
    }

    private void InputHandler()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !dialogueManager.isDialogueActive && inventory.Count != 0)
        {
            UpdateCursorState();
            inventoryUI.gameObject.SetActive(!inventoryUI.gameObject.activeInHierarchy);
        }


    }

    void UpdateCursorState()
    {
        bool shouldShowCursor = !inventoryUI.gameObject.activeInHierarchy;
         
        if (isCursorVisible != shouldShowCursor)
        {
            playerController.SetCinemachineController(!shouldShowCursor);
            isCursorVisible = shouldShowCursor;
            Cursor.visible = isCursorVisible;
            Cursor.lockState = isCursorVisible ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }

    public void AddToInvetory(ItemInteract itemInteract)
    {
        if (CheckInventory(itemInteract) == false)
        {
            inventory.Add(itemInteract);
            OnInventoryChanged?.Invoke();
        }
        else
        {
            UnityEngine.Debug.Log("Already in inventory: " + itemInteract.name);
        }
    }
    public void RemoveFromInventory(ItemInteract itemInteract)
    {
        if (CheckInventory(itemInteract) == true)
        {
            inventory.Remove(itemInteract);
            OnInventoryChanged?.Invoke();
        }
    }
    public bool CheckInventory(ItemInteract itemInteract)
    {
        bool isInInventory = false;
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].id == itemInteract.id)
            {
                isInInventory = true;
            }
            else
            {
                isInInventory = false;
            }
        }
        return isInInventory;

    }

    public void ActivateNextItem(GameObject ItemToActivate, string itemIdRequierd)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].id == itemIdRequierd)
            {
                ItemToActivate.SetActive(true);
                return;
            }
        }
    }


    public void InjectDependencies(DependencyContainer provider)
    {
        focusModeManager = provider.FocusModeManager;
        inventoryUI = provider.InventoryUI;
        playerController = provider.PlayerController;
        dialogueManager = provider.DialogueManager;
    }

}
