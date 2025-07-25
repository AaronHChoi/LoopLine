using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    private void Start()
    {
        PlayerInventorySystem.Instance.OnInventoryChanged += OnUpdateInventory;
    }
    private void OnUpdateInventory()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        DrawInventory();
    }

    public void DrawInventory()
    {
        foreach(ItemInteract item in PlayerInventorySystem.Instance.inventory)
        {
            AddInventorySlot(item);
        }
    }

    public void AddInventorySlot(ItemInteract item)
    {
        GameObject itemUI = Instantiate(item.ItemData.objectPrefab);
        itemUI.transform.SetParent(transform, false);

        UIInventoryItemSlot slot = itemUI.GetComponent<UIInventoryItemSlot>();
        slot.Set(item);

    }

    public void RemoveInventorySlot(UIInventoryItemSlot item)
    {
        item.gameObject.SetActive(false);
    }
}

