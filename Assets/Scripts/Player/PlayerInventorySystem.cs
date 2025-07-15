using UnityEngine;
using System.Collections.Generic;

public class PlayerInventorySystem : MonoBehaviour, IDependencyInjectable
{
    [Header("Inventory Settings")]
    [SerializeField] private List<DialogueSpeaker> inventory = new List<DialogueSpeaker>();

    FocusModeManager focusModeManager;

    public void AddToInvetory(DialogueSpeaker dialogueSpeaker)
    {
        if (CheckInventory(dialogueSpeaker) == false)
        {
            inventory.Add(dialogueSpeaker);
        }
        else
        {
            Debug.Log("Already in inventory: " + dialogueSpeaker.name);
        }
    }
    public void RemoveFromInventory(DialogueSpeaker dialogueSpeaker)
    {
        if (CheckInventory(dialogueSpeaker) == true)
        {
            inventory.Remove(dialogueSpeaker);
        }
    }
    public bool CheckInventory(DialogueSpeaker dialogueSpeaker)
    {
        bool isInInventory = false;
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].id == dialogueSpeaker.id)
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
