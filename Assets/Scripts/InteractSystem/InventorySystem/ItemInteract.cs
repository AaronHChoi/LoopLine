using UnityEngine;
using DependencyInjection;

public abstract class ItemInteract : MonoBehaviour, IItemGrabInteract
{
    [Header("Settings")]
    [SerializeField] private string interactText = "";
    public string id;
    public ItemInfo ItemData;
    public bool canBePicked = false;

    [Header("Item Inventory UI")]
    [SerializeField] private bool deactivateOnPickup = true;
    [SerializeField] public GameObject objectPrefab;

    [Header("References")]
    IInventoryUI inventoryUI;

    private void Awake()
    {
        inventoryUI = InterfaceDependencyInjector.Instance.Resolve<IInventoryUI>();
    }
    public virtual void Start()
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
    public virtual bool Interact()
    {
        bool isGrabbable = false;
        if (gameObject.tag == "Item" && canBePicked)
        {
            if (inventoryUI.ItemInUse == inventoryUI.HandItemUI || inventoryUI.ItemInUse == null)
            {
                if (deactivateOnPickup)
                {
                    gameObject.SetActive(false);
                    gameObject.layer = LayerMask.NameToLayer("Default");
                }
                if (inventoryUI.CheckInventory(this) == false)
                {
                    inventoryUI.AddInventorySlot(this);
                    InventoryManager.Instance.AddItemToInventory(ItemData);
                }
            }
            isGrabbable = true;
        }

        return isGrabbable;
    }
    public string GetInteractText()
    {
        if (interactText == null) return interactText = "";

        return interactText;
    }
}