using DependencyInjection;
using Player;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour, IInventoryUI
{
    [SerializeField] public List<UIInventoryItemSlot> inventorySlots = new List<UIInventoryItemSlot>();
    
    [SerializeField] public RawImage arrowImage;
    [SerializeField] public ItemInteract ItemInUse { get; set; }

    [SerializeField] private ItemInteract handItemUI;
    [SerializeField] private Vector2 offset = new Vector2(50f, 0f);
    [SerializeField] private float slotChangeCooldown = 0.5f;
    [SerializeField] private Transform spawnPosition;

    private float lastSlotChangeTime = 0f;
    private bool isInventoryOpen = false;
    public bool IsInventoryOpen
    {
        get => isInventoryOpen;
    }
    public ItemInteract HandItemUI { get { return handItemUI; } } 

    public int currentSlotIndex = 0;
    public static InventoryUI Instance { get; private set; }

    FadeInOutController FadeController;
    FadeInOutController ArrowFadeController;
    IPlayerStateController controller;
    IDialogueManager dialogueManager;
    IPlayerInputHandler inputHandler;

    #region MagicMethods
    private void Awake()
    {
        inputHandler = InterfaceDependencyInjector.Instance.Resolve<IPlayerInputHandler>();
        dialogueManager = InterfaceDependencyInjector.Instance.Resolve<IDialogueManager>();
        controller = InterfaceDependencyInjector.Instance.Resolve<IPlayerStateController>();
    }
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        FadeController = GetComponent<FadeInOutController>();
        ArrowFadeController = arrowImage.GetComponent<FadeInOutController>();
        HideInventory();
        AddInventorySlot(HandItemUI);
        ItemInUse = HandItemUI;
        currentSlotIndex = 0;
        inventorySlots[0].IsActive = true;
    }
    private void Update()
    {
        //if (!DependencyContainer.Instance.PlayerStateController.IsInState(playerStateController.InventoryState)) return;

        float scroll = inputHandler.GetScrollValue();

        if (Time.time - lastSlotChangeTime < slotChangeCooldown)
        {
            return;
        }

        if (isInventoryOpen)
        {
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
        }
    }
    private void OnEnable()
    {
        if (controller != null)
        {
            controller.OnOpenInventory += OpenInventory;
        }
    }
    private void OnDisable()
    {
        if (controller != null)
        {
            controller.OnOpenInventory -= OpenInventory;
        }
    }
    #endregion
    private void OpenInventory()
    {
        if (!dialogueManager.IsDialogueActive && inventorySlots.Count != 0 && !isInventoryOpen)
        {
            ShowInventory();
            MoveArrowToSlot(inventorySlots[currentSlotIndex].transform as RectTransform);
            arrowImage.gameObject.SetActive(true);
        }
        else if (isInventoryOpen)
        {
            HideInventory();
            arrowImage.gameObject.SetActive(false);
        }
    }
    public void ChangeSlot(int direction)
    {
        inventorySlots[currentSlotIndex].IsActive = false;

        currentSlotIndex += direction;

        if (currentSlotIndex < 0)
            currentSlotIndex = inventorySlots.Count - 1;
        else if (currentSlotIndex >= inventorySlots.Count)
            currentSlotIndex = 0;

        inventorySlots[currentSlotIndex].IsActive = true;

        if (currentSlotIndex == 0)
        {
            controller.ChangeState(controller.NormalState);
        }
        ArrowFadeController.ForceFade(true);
        MoveArrowToSlot(inventorySlots[currentSlotIndex].transform as RectTransform);
    }
    public void MoveArrowToSlot(RectTransform slotTransform)
    {
        Vector3 worldPos = slotTransform.TransformPoint(Vector3.zero);
        Vector3 localPos = arrowImage.rectTransform.parent.InverseTransformPoint(worldPos);

        arrowImage.rectTransform.localPosition = localPos + (Vector3)offset;
    }
    private void ResetArrowPosition()
    {
        inventorySlots[0].IsActive = true;
        ItemInUse = HandItemUI;
        currentSlotIndex = 0;
        MoveArrowToSlot(inventorySlots[currentSlotIndex].transform as RectTransform);
    }
    public void AddInventorySlot(ItemInteract item)
    {
        GameObject itemUI = Instantiate(item.ItemData.objectPrefab);
        itemUI.transform.SetParent(transform, false);

        UIInventoryItemSlot slot = itemUI.GetComponent<UIInventoryItemSlot>();
        slot.Set(item);
        inventorySlots.Add(slot);
        MoveArrowToSlot(inventorySlots[currentSlotIndex].transform as RectTransform);
    }
    public void RemoveInventorySlot(ItemInteract item)
    {
        if (CheckInventory(item) == true)
        {
            UIInventoryItemSlot slot = item.GetComponent<UIInventoryItemSlot>();
            inventorySlots.Remove(slot);
            ResetArrowPosition();
        }
    }
    public bool CheckInventory(ItemInteract itemInteract)
    {
        bool isInInventory = false;
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (inventorySlots[i].itemId == itemInteract.id)
            {
                isInInventory = true;
            }
            else
            {
                isInInventory = false;
            }
        }
        return isInInventory;
    }
    private void CheckArrowPosition()
    {
        if (SceneManager.GetActiveScene().name == "04. Train")
        {
            if (ItemInUse == HandItemUI)
            {
                if (inventorySlots[0] != null)
                {
                    MoveArrowToSlot(inventorySlots[0].transform as RectTransform);
                }

            }
        }
    }
    private void ShowInventory()
    {
        FadeController.ForceFade(true);
        isInventoryOpen = true;
    }
    private void HideInventory()
    {
        FadeController.ForceFade(false);
        isInventoryOpen = false;
    }

    public Transform GetSpawnPosition()
    {
        return spawnPosition;
    }
}

public interface IInventoryUI
{
    bool IsInventoryOpen { get; }
    public ItemInteract HandItemUI { get; }
    public ItemInteract ItemInUse { get; set; }
    Transform GetSpawnPosition();
    void AddInventorySlot(ItemInteract item);
    void RemoveInventorySlot(ItemInteract item);
    bool CheckInventory(ItemInteract itemInteract);
}