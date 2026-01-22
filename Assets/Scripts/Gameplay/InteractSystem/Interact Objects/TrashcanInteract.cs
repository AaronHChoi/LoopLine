using UnityEngine;
using Core.DependencyInjection;
using Gameplay.Inventory;

public class TrashcanInteract : MonoBehaviour, IInteract
{
    [SerializeField] private string interactText;

    IInventoryUI inventory;

    void Start()
    {
        inventory = InterfaceDependencyInjector.Instance.Resolve<IInventoryUI>();
    }
    public void Interact()
    {
        inventory.RemoveUIInventoryLastSlot(inventory.inventorySlots[inventory.CurrentSlotIndex]);
    }
    public string GetInteractText()
    {
        return interactText;
    }
}