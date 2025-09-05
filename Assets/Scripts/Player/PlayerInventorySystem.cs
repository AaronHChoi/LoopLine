using Player;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DependencyInjection;
public class PlayerInventorySystem : MonoBehaviour, IDependencyInjectable
{
    public static PlayerInventorySystem Instance { get; private set; }
    [Header("Inventory Settings")]
    [SerializeField] public List<ItemInteract> inventory = new List<ItemInteract>();
    [SerializeField] public Transform SpawnPosition;
    [SerializeField] public ItemInteract ItemInUse;

    PlayerController playerController;
    InventoryUI inventoryUI;
    DialogueManager dialogueManager;
    PlayerStateController controller;

    public delegate void InventoryChanged();
    public event InventoryChanged OnInventoryChanged;
    bool isCursorVisible = false;

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

        if (inventoryUI != null) 
        { 
            inventoryUI.gameObject.SetActive(false); 
        }
    }
    private void Start()
    {
        if (inventoryUI != null) 
        { 
            inventoryUI.AddInventorySlot(inventoryUI.HandItemUI);
            AddToInvetory(inventoryUI.HandItemUI);
            ItemInUse = inventoryUI.HandItemUI;
            inventoryUI.currentSlotIndex = 0;
            inventoryUI.inventorySlots[0].IsActive = true;
        }
    }
    private void OnEnable()
    {
        if (controller != null)
        {
            controller.OnOpenInventory += OpenInventory;
        }
    }
    private void OnDisable()
    {
        if (controller != null)
        {
            controller.OnOpenInventory -= OpenInventory;
        }
    }
    private void Update()
    {
        if(SceneManager.GetActiveScene().name == "04. Train")
        {
            if (ItemInUse == inventoryUI.HandItemUI)
            {
                if (inventoryUI.inventorySlots[0] != null)
                {
                    inventoryUI.MoveArrowToSlot(inventoryUI.inventorySlots[0].transform as RectTransform);
                }
                
            }

        }
    }
    private void OpenInventory()
    {
        if (!dialogueManager.isDialogueActive && inventory.Count != 0)
        {
            inventoryUI.gameObject.SetActive(!inventoryUI.gameObject.activeInHierarchy);
            inventoryUI.arrowImage.gameObject.SetActive(inventoryUI.gameObject.activeInHierarchy);
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
            itemInteract.objectPrefab.SetActive(false);
            ItemInUse = inventoryUI.HandItemUI;
            inventoryUI.inventorySlots[0].IsActive = true;
            inventoryUI.currentSlotIndex = 0;
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
        inventoryUI = provider.UIContainer.InventoryUI;
        playerController = provider.PlayerContainer.PlayerController;
        dialogueManager = provider.ManagerContainer.DialogueManager;
        controller = provider.PlayerContainer.PlayerStateController;
    }
}