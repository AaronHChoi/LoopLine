using Player;
using UnityEngine;

public class ItemInteract : MonoBehaviour, IDependencyInjectable, IItemGrabInteract
{
    [Header("Settings")]
    [SerializeField] private string interactText = "";
    public string id = "";
    public ItemInfo ItemData;
    public bool canBePicked = false;

    [Header("Item Inventory UI")]
    [SerializeField] private bool deactivateOnPickup = true;
    [SerializeField] public GameObject objectPrefab;
    [SerializeField] private GameObject itemToActivate;

    [Header("References")]
    PlayerInventorySystem playerInventorySystem;
    InventoryUI inventoryUI;
    ItemManager itemManager;
    PlayerStateController playerStateController;

    private void Awake()
    {
        InjectDependencies(DependencyContainer.Instance);
    }
    void Start()
    {
        id = ItemData.itemName;
        interactText = ItemData.itemName;
        if (objectPrefab == null && transform.childCount > 0)
        {
            objectPrefab = transform.GetChild(0).gameObject;
        }
        else
        {
            objectPrefab = gameObject;
        }
    }
    private void OnEnable()
    {
        if(playerStateController != null)
        {
            playerStateController.OnInteract += Interact;
        }
    }
    private void OnDisable()
    {
        if (playerStateController != null)
        {
            playerStateController.OnInteract -= Interact;
        }
    }
    public void Interact()
    {
        if (gameObject.tag == "Item" && canBePicked)
        {
            if (playerInventorySystem.ItemInUse == inventoryUI.HandItemUI || playerInventorySystem.ItemInUse == null)
            {
                if (deactivateOnPickup)
                {
                    gameObject.SetActive(false);
                    gameObject.layer = LayerMask.NameToLayer("Default");
                }
                if (playerInventorySystem.CheckInventory(this) == false)
                {
                    inventoryUI.AddInventorySlot(this);
                    playerInventorySystem.AddToInvetory(this);
                }
                
                if (itemToActivate != null && !string.IsNullOrEmpty(id))
                    playerInventorySystem.ActivateNextItem(itemToActivate, id);
            }
        }
    }
    public string GetInteractText()
    {
        if (interactText == null) return interactText = "";

        return interactText;
    }
    public void InjectDependencies(DependencyContainer provider)
    {
        playerInventorySystem = provider.PlayerInventorySystem;
        inventoryUI = provider.InventoryUI;
        itemManager = provider.ItemManager;
        playerStateController = provider.PlayerStateController;
    }
}