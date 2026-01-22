using UnityEngine;
using Core.DependencyInjection;
using Gameplay.Inventory;

namespace Gameplay.Items
{
    public abstract class ItemInteract : MonoBehaviour, IItemGrabInteractable
    {
        [Header("Settings")]
        [SerializeField] private string interactText = "";
        public string id;
        public ItemInfo ItemData;
        public bool canBePicked = false;

        [Header("Item Inventory UI")]
        [SerializeField] private bool deactivateOnPickup = true;
        [SerializeField] protected bool resetLayerOnPickup = true;
        [SerializeField] public GameObject objectPrefab;

        [Header("References")]
        IInventoryUI inventoryUI;

        protected virtual void Awake()
        {
        }
        public virtual void Start()
        {
            inventoryUI = InterfaceDependencyInjector.Instance.Resolve<IInventoryUI>();

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
                        if (resetLayerOnPickup)
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
}