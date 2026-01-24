using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    [SerializeField] public ItemInventoryManager itemsInventoryManager;
    private static bool hasInitialized = false;

    protected override void Awake()
    {
        base.Awake();

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