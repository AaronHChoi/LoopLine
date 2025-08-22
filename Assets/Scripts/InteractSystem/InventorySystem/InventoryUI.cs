using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour, IDependencyInjectable
{
    [SerializeField] public List<UIInventoryItemSlot> inventorySlots = new List<UIInventoryItemSlot>();

    [SerializeField] public RawImage arrowImage; 
    [SerializeField] private Vector2 offset = new Vector2(50f, 0f);
    [SerializeField] public ItemInteract HandItemUI;
    [SerializeField] private float slotChangeCooldown = 0.5f; 
    private float lastSlotChangeTime = 0f;
    private int currentSlotIndex = 0;

    PlayerStateController playerStateController;
    private void Awake()
    {
        InjectDependencies(DependencyContainer.Instance);
    }
    private void Start()
    {
        PlayerInventorySystem.Instance.OnInventoryChanged += OnUpdateInventory;
        MoveArrowToSlot(inventorySlots[currentSlotIndex].transform as RectTransform);
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
        if (!DependencyContainer.Instance.PlayerStateController.IsInState(playerStateController.InventoryState)) return;

        float scroll = DependencyContainer.Instance.PlayerInputHandler.GetScrollValue();

        if (Time.time - lastSlotChangeTime < slotChangeCooldown)
        {
            return;
        }
            
        if (scroll > 0f) 
        {
            ChangeSlot(-1);
            lastSlotChangeTime = Time.time;
        }
        else if (scroll < 0f) 
        {
            ChangeSlot(1);
            lastSlotChangeTime = Time.time;
        }

        UpdateVisableSlots();
    }
    private void UpdateVisableSlots()
    {
        inventorySlots.RemoveAll(s => s == null);

        //int n = inventorySlots.Count;
        //if (n == 0) return;


        //currentSlotIndex = ((currentSlotIndex % n) + n) % n;

        //int previousIndex = (currentSlotIndex - 1 + n) % n;
        //int nextIndex = (currentSlotIndex + 1) % n;

        //for (int i = 0; i < n; i++)
        //{
        //    var slot = inventorySlots[i];
        //    if (slot == null) continue;

        //    bool visible = (i == previousIndex || i == currentSlotIndex || i == nextIndex);
        //    slot.gameObject.SetActive(visible);
        //}
    }
    public void ChangeSlot(int direction)
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

    public void InjectDependencies(DependencyContainer provider)
    {
        playerStateController = provider.PlayerStateController;
    }
}