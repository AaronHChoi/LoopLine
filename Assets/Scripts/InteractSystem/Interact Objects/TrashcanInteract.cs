using DependencyInjection;
using UnityEngine;

public class TrashcanInteract : MonoBehaviour, IInteract
{
    [SerializeField] private string interactText;

    IInventoryUI inventory;

    private void Awake()
    {
        inventory = InterfaceDependencyInjector.Instance.Resolve<IInventoryUI>();
    }

    public void Interact()
    {
        inventory.RemoveUIInventoryLastSlot(inventory.inventorySlots[inventory.CurrentSlotIndex]);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public string GetInteractText()
    {
        return interactText;
    }
}
