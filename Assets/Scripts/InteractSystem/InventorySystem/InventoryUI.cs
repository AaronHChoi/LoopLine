using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] public List<UIInventoryItemSlot> inventorySlots = new List<UIInventoryItemSlot>();
    [SerializeField] private UIInventoryItemSlot UIInventoryItemSlot;
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
        Debug.Log(UIInventoryItemSlot);
        if (gameObject.activeInHierarchy && PlayerInventorySystem.Instance.ItemInUse == null)
        {
            if (Input.GetKeyDown(KeyCode.WheelDown) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                inventorySlots[0].isActive = true;
            }
            
        }
        if (Input.GetKeyDown(KeyCode.WheelUp) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            for (int i = 0; i < inventorySlots.Count; i++)
            {
                if (inventorySlots[i].gameObject.activeInHierarchy)
                {
                    inventorySlots[i].isActive = false;
                    if (i + 1 < inventorySlots.Count)
                    {
                        inventorySlots[i + 1].isActive = true;
                    }
                    else
                    {
                        inventorySlots[0].isActive = true;
                    }
                    break;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.WheelDown) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            for (int i = inventorySlots.Count - 1; i >= 0; i--)
            {
                if (inventorySlots[i].gameObject.activeInHierarchy)
                {
                    inventorySlots[i].isActive = false;
                    if (i + 1 >= 0)
                    {
                        inventorySlots[i - 1].isActive = true;
                    }
                    else
                    {
                        inventorySlots[inventorySlots.Count - 1].isActive = true;
                    }
                    break;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            for (int i = inventorySlots.Count - 1; i >= 0; i--)
            {
                inventorySlots[i].isActive = false;
                PlayerInventorySystem.Instance.ItemInUse = null;
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

