using UnityEngine;

public class ItemInteract : MonoBehaviour, IDependencyInjectable
{
    public string id = "";
    public ItemInfo ItemData;
    [Header("Item Inventory UI")]
    [SerializeField] private bool deactivateOnPickup = true;
    [SerializeField] public GameObject objectPrefab;
    [SerializeField] private GameObject itemToActivate;

    PlayerInventorySystem playerInventorySystem;
    InventoryUI inventoryUI;

    private void Awake()
    {
        InjectDependencies(DependencyContainer.Instance);
    }
    void Start()
    {
        id = gameObject.name;
        if(objectPrefab == null)
        {
            objectPrefab = gameObject;
        }
    }
    public void InjectDependencies(DependencyContainer provider)
    {
        playerInventorySystem = provider.PlayerInventorySystem;
        inventoryUI = provider.InventoryUI;
    }

    public void Interact()
    {
        if (gameObject.tag == "Item")
        {
            if (deactivateOnPickup)
            {
                gameObject.SetActive(false);
                gameObject.tag = "Untagged";
            }
            if (playerInventorySystem.CheckInventory(this) == false)
            {
                inventoryUI.AddInventorySlot(this);
            }
            playerInventorySystem.AddToInvetory(this);
                     
            if (itemToActivate != null && !string.IsNullOrEmpty(id))
                playerInventorySystem.ActivateNextItem(itemToActivate, id);
        }
    }

}
