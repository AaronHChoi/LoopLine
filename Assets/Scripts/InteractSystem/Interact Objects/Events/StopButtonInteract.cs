using UnityEngine;

public class StopButtonInteract : MonoBehaviour, IInteract, IDependencyInjectable
{
    [SerializeField] private string interactText = "";
    [SerializeField] private GameObject Crystal;
    [SerializeField] private ItemInteract Rock;

    PlayerInventorySystem inventory;
    ItemManager itemManager;
    InventoryUI inventoryUI;
    EventManager eventManager;

    private void Awake()
    {
        InjectDependencies(DependencyContainer.Instance);
    }

    private void Start()
    {
        foreach (var item in itemManager.items) 
        {
            if (item.id == "Rock") //asegurarse que el item de la roca tenga este id
            {
                Rock = item;
            }
        }
    }
    public void Interact()
    {
        if (Crystal.gameObject.activeSelf)
        {
            if (inventory.CheckInventory(Rock))
            {
                Crystal.gameObject.SetActive(false);
                inventory.RemoveFromInventory(Rock);
                inventoryUI.RemoveInventorySlot(Rock);
            }
        }
        else
        {
            eventManager.TrainEventStopTrain();
        }
    }

    void Update()
    {
        if (eventManager.stopTrain)
        {
            eventManager.StopedTimeForTrain -= Time.deltaTime;
            if(eventManager.StopedTimeForTrain < 0) eventManager.StopedTimeForTrain = 0;
        }       
    }

    public string GetInteractText()
    {
        return interactText;
    }

    public void InjectDependencies(DependencyContainer provider)
    {
        inventory = provider.PlayerInventorySystem;
        inventoryUI = provider.InventoryUI;
        eventManager = provider.EventManager;
        itemManager = provider.ItemManager;
    }
}
