using DependencyInjection;
using Player;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour, IInventoryUI
{
    [SerializeField] public List<UIInventoryItemSlot> inventorySlots { get; } = new List<UIInventoryItemSlot>(); 
    
    [SerializeField] public RawImage arrowImage;
    [SerializeField] public ItemInteract ItemInUse { get; set; }

    [SerializeField] private BaseItemInteract handItemUI;
    [SerializeField] private Vector2 offset = new Vector2(50f, 0f);
    [SerializeField] private float slotChangeCooldown = 0.5f;
    [SerializeField] private int maxInventorySlots = 5;
    [SerializeField] private Transform spawnPosition;

    private float lastSlotChangeTime = 0f;
    private bool isInventoryOpen = false;
    public bool IsInventoryOpen
    {
        get => isInventoryOpen;
    }
    public BaseItemInteract HandItemUI { get { return handItemUI; } } 

    private int currentSlotIndex = 0;

    public int CurrentSlotIndex
    {
        get => currentSlotIndex;
    }
    public static InventoryUI Instance { get; private set; }

    FadeInOutController FadeController;
    CanvasGroup InventoryCanvasGroup;
    CanvasGroup ArrowCanvasGroup;
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
        arrowImage = GameObject.FindGameObjectWithTag("ArrowUI").GetComponent<RawImage>();
        handItemUI = GameObject.FindGameObjectWithTag("HandItem").GetComponent<BaseItemInteract>();
        InventoryCanvasGroup = GetComponent<CanvasGroup>();
        ArrowCanvasGroup = arrowImage.GetComponent<CanvasGroup>();
        FadeController = GetComponent<FadeInOutController>();
        ArrowFadeController = arrowImage.GetComponent<FadeInOutController>();
        RebuildInventoryFromManager();
        arrowImage.gameObject.SetActive(false);
        HideInventory();
        

    }
    private void Update()
    {
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
        if (inventorySlots.Count != 0 && !isInventoryOpen)
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
        if (inventorySlots.Count > 1)
        {          
            inventorySlots[currentSlotIndex].IsActive = false;
        }

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
        if (inventorySlots.Count < maxInventorySlots)
        {
            if (CheckInventory(item))
            {
                RemoveInventorySlot(item);
            }
            GameObject itemUI = Instantiate(item.ItemData.UIPrefab);
            itemUI.transform.SetParent(transform, false);

            UIInventoryItemSlot slot = itemUI.GetComponent<UIInventoryItemSlot>();
            slot.Set(item);            
            inventorySlots.Add(slot);
            MoveArrowToSlot(inventorySlots[currentSlotIndex].transform as RectTransform);
            
        }
              
    }
    public void RemoveInventorySlot(ItemInteract item)
    {
        if (CheckInventory(item))
        {
            UIInventoryItemSlot slotToRemove = inventorySlots[currentSlotIndex];

            if (currentSlotIndex > 0 && slotToRemove != null)
            {
                inventorySlots[currentSlotIndex].IsActive = false;
                inventorySlots[currentSlotIndex].gameObject.SetActive(false);
                slotToRemove.transform.parent = null;
                inventorySlots.Remove(slotToRemove);
                InventoryManager.Instance.RemoveItemFromInventory(item.ItemData);
                //if(InventoryManager.Instance.itemsInventoryManager.items.Count > 0)
                //    RebuildInventoryFromManager();
                currentSlotIndex--;
                inventorySlots[currentSlotIndex].IsActive = true;

                StartCoroutine(UpdateArrowNextFrame());
            }
            else
            {
                ResetArrowPosition();
            }
        }
   
    }
    public void RemoveUIInventoryLastSlot(UIInventoryItemSlot slotToRemove)
    {
         slotToRemove = inventorySlots[currentSlotIndex];

         if (currentSlotIndex > 0)
         {
             inventorySlots[currentSlotIndex].IsActive = false;
             inventorySlots[currentSlotIndex].gameObject.SetActive(false);
             inventorySlots.Remove(slotToRemove);
             slotToRemove.transform.parent = null;

             currentSlotIndex--;
             inventorySlots[currentSlotIndex].IsActive = true;

             StartCoroutine(UpdateArrowNextFrame());
         }
         else
         {
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
   
    public void AddHandItem()
    {
        AddInventorySlot(HandItemUI);
        ItemInUse = HandItemUI;
        currentSlotIndex = 0;
        inventorySlots[0].IsActive = true;
    }
    private void RebuildInventoryFromManager()
    {
        var manager = InventoryManager.Instance;
        if (manager == null || manager.itemsInventoryManager == null) return;
        AddHandItem();

        List<ItemInfo> savedItems = manager.itemsInventoryManager.items;

        if (savedItems.Count == 0)
        {
            Debug.Log("No hay ítems guardados en InventoryManager");
            return;
        }

        for (int i = 0; i < savedItems.Count; i++)
        {
            GameObject itemInstance = Instantiate(savedItems[i].itemPrefab);

            ItemInteract interactComponent = itemInstance.GetComponent<ItemInteract>();
            if (interactComponent == null)
                interactComponent = itemInstance.GetComponentInChildren<ItemInteract>(true);
            interactComponent.Start();
            interactComponent.Interact();
        }

    }
    private void ShowInventory()
    {
        if (InventoryCanvasGroup.alpha == 0)
        {
            InventoryCanvasGroup.alpha = 1;
            ArrowCanvasGroup.alpha = 1;
        }
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

    private IEnumerator UpdateArrowNextFrame()
    {
        yield return null; 
        MoveArrowToSlot(inventorySlots[currentSlotIndex].transform as RectTransform);
    }
}

public interface IInventoryUI
{
    public List<UIInventoryItemSlot> inventorySlots { get; }
    int CurrentSlotIndex { get; }
    bool IsInventoryOpen { get; }
    public BaseItemInteract HandItemUI { get; }
    public ItemInteract ItemInUse { get; set; }
    Transform GetSpawnPosition();
    void AddHandItem();
    void MoveArrowToSlot(RectTransform slotTransform);
    void AddInventorySlot(ItemInteract item);
    void RemoveInventorySlot(ItemInteract item);
    void RemoveUIInventoryLastSlot(UIInventoryItemSlot slotToRemove);
    bool CheckInventory(ItemInteract itemInteract);
}