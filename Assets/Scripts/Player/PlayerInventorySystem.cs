using UnityEngine;
using System.Collections.Generic;

public class PlayerInventorySystem : MonoBehaviour, IDependencyInjectable
{
    public static PlayerInventorySystem Instance { get; private set; }
    [Header("Inventory Settings")]
    [SerializeField] public List<ItemInteract> inventory = new List<ItemInteract>();

    FocusModeManager focusModeManager;
    [SerializeField] InventoryUI inventoryUI;
    public delegate void InventoryChanged();
    public event InventoryChanged OnInventoryChanged;

    void Start()
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
        InjectDependencies(DependencyContainer.Instance);
        inventoryUI.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            inventoryUI.gameObject.SetActive(!inventoryUI.gameObject.activeInHierarchy);
        }
    }

    public void AddToInvetory(ItemInteract itemInteract)
    {
        if (CheckInventory(itemInteract) == false)
        {
            inventory.Add(itemInteract);
            OnInventoryChanged?.Invoke();
        }
        else
        {
            Debug.Log("Already in inventory: " + itemInteract.name);
        }
    }
    public void RemoveFromInventory(ItemInteract itemInteract)
    {
        if (CheckInventory(itemInteract) == true)
        {
            inventory.Remove(itemInteract);
            OnInventoryChanged?.Invoke();
        }
    }
    public bool CheckInventory(ItemInteract itemInteract)
    {
        bool isInInventory = false;
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].id == itemInteract.id)
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

    public void ActivateNextItem(GameObject ItemToActivate, string itemIdRequierd)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].id == itemIdRequierd)
            {
                ItemToActivate.SetActive(true);
                return;
            }
        }
    }


    public void InjectDependencies(DependencyContainer provider)
    {
        focusModeManager = provider.FocusModeManager;
        inventoryUI = provider.InventoryUI;
    }

}
