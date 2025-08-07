using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] public List<UIInventoryItemSlot> inventorySlots = new List<UIInventoryItemSlot>();
    [SerializeField] private UIInventoryItemSlot UIInventoryItemSlot;
    [SerializeField] public RawImage arrowImage; 
    [SerializeField] private Vector2 offset = new Vector2(50f, 0f);
    private int currentSlotIndex = 0;
    private Vector3 ArrowOriginalPosition = new Vector3(-2000f, 0f, 0f);
    private void Start()
    {
        PlayerInventorySystem.Instance.OnInventoryChanged += OnUpdateInventory;
        //MoveArrowToSlot(inventorySlots[currentSlotIndex].transform as RectTransform);
        arrowImage.transform.position = ArrowOriginalPosition;
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
        float scroll = Input.mouseScrollDelta.y;

        //if (gameObject.activeInHierarchy && PlayerInventorySystem.Instance.ItemInUse == null)
        //{
        //    ChangeSlot(1);
        //    MoveArrowToSlot(inventorySlots[currentSlotIndex].transform as RectTransform);
        //}

        if (inventorySlots.Count > 0)
        {
            for (int i = 0; i < inventorySlots.Count; i++)
            {
                int previousIndex = (currentSlotIndex - 1 + inventorySlots.Count) % inventorySlots.Count;
                int nextIndex = (currentSlotIndex + 1) % inventorySlots.Count;

                if (i == previousIndex || i == currentSlotIndex || i == nextIndex)
                {
                    inventorySlots[i].gameObject.SetActive(true);
                }
                else
                {
                    inventorySlots[i].gameObject.SetActive(false);
                }
            }
        }
        if (scroll > 0f) 
        {
            ChangeSlot(-1);
        }
        else if (scroll < 0f) 
        {
            ChangeSlot(1);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            for (int i = inventorySlots.Count - 1; i >= 0; i--)
            {
                inventorySlots[i].isActive = false;
                PlayerInventorySystem.Instance.ItemInUse = null;
                arrowImage.transform.position = ArrowOriginalPosition;
            }
        }
    }

    private void ChangeSlot(int direction)
    {
        inventorySlots[currentSlotIndex].isActive = false;

        currentSlotIndex += direction;

        if (currentSlotIndex < 0)
            currentSlotIndex = inventorySlots.Count - 1;
        else if (currentSlotIndex >= inventorySlots.Count)
            currentSlotIndex = 0;

        inventorySlots[currentSlotIndex].isActive = true;

        
        

        MoveArrowToSlot(inventorySlots[currentSlotIndex].transform as RectTransform);
    }

    private void MoveArrowToSlot(RectTransform slotTransform)
    {
        Vector3 worldPos = slotTransform.TransformPoint(Vector3.zero);
        Vector3 localPos = arrowImage.rectTransform.parent.InverseTransformPoint(worldPos);

        arrowImage.rectTransform.localPosition = localPos + (Vector3)offset;
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

