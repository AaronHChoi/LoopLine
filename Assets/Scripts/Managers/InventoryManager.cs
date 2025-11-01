using DependencyInjection;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    IInventoryUI inventoryUI;

    [SerializeField] public ItemInventoryManager itemsInventoryManager;
    private static bool hasInitialized = false;

    protected override void Awake()
    {
        base.Awake();
        inventoryUI = InterfaceDependencyInjector.Instance.Resolve<IInventoryUI>();
        if (!hasInitialized)
        {
            if (itemsInventoryManager != null)
            {
                itemsInventoryManager.items.Clear();
            }
            hasInitialized = true;
        }
    }

    public void AddItemToInventory(ItemInfo item)
    {
        if (!itemsInventoryManager.items.Contains(item))
        {
            itemsInventoryManager.items.Add(item);
        }
    }

    public void RemoveItemFromInventory(ItemInfo item)
    {
        if (itemsInventoryManager.items.Contains(item))
        {
            itemsInventoryManager.items.Remove(item);
        }
    }

}
