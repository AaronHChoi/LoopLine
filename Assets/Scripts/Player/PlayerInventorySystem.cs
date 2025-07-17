using UnityEngine;
using System.Collections.Generic;

public class PlayerInventorySystem : MonoBehaviour, IDependencyInjectable
{
    [Header("Inventory Settings")]
    [SerializeField] private List<ItemInteract> inventory = new List<ItemInteract>();

    FocusModeManager focusModeManager;

    public void AddToInvetory(ItemInteract itemInteract)
    {
        if (CheckInventory(itemInteract) == false)
        {
            inventory.Add(itemInteract);
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
    }

}
