using UnityEngine;

public class ItemInteract : MonoBehaviour, IDependencyInjectable
{
    public string id = "";
    [Header("Item To Activate")]
    [SerializeField] private GameObject itemToActivate;

    PlayerInventorySystem playerInventorySystem;

    private void Awake()
    {
        InjectDependencies(DependencyContainer.Instance);
    }
    void Start()
    {
        id = gameObject.name;
    }
    public void InjectDependencies(DependencyContainer provider)
    {
        playerInventorySystem = provider.PlayerInventorySystem;
    }

    public void Interact()
    {
        if (gameObject.tag == "Item")
        {
            playerInventorySystem.AddToInvetory(this);
            if (itemToActivate != null && !string.IsNullOrEmpty(id))
                playerInventorySystem.ActivateNextItem(itemToActivate, id);
        }
    }

}
