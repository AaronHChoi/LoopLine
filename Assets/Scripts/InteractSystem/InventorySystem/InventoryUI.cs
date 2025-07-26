using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private List<UIInventoryItemSlot> inventorySlots = new List<UIInventoryItemSlot>();
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.WheelUp))
        {
            for (int i = 0; i < inventorySlots.Count; i++)
            {
                if (inventorySlots[i].gameObject.activeInHierarchy)
                {
                    inventorySlots[i].gameObject.SetActive(false);
                    if (i + 1 < inventorySlots.Count)
                    {
                        inventorySlots[i + 1].gameObject.SetActive(true);
                    }
                    else
                    {
                        inventorySlots[0].gameObject.SetActive(true);
                    }
                    break;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.WheelDown))
        {
            for (int i = inventorySlots.Count - 1; i >= 0; i--)
            {
                if (inventorySlots[i].gameObject.activeInHierarchy)
                {
                    inventorySlots[i].gameObject.SetActive(false);
                    if (i + 1 >= 0)
                    {
                        inventorySlots[i - 1].gameObject.SetActive(true);
                    }
                    else
                    {
                        inventorySlots[inventorySlots.Count - 1].gameObject.SetActive(true);
                    }
                    break;
                }
            }
        }
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
        inventorySlots.Add(slot);
    }

    public void RemoveInventorySlot(UIInventoryItemSlot item)
    {
        item.gameObject.SetActive(false);
    }
}

