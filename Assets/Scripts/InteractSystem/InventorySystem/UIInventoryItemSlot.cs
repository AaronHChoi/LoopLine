using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DependencyInjection;
public class UIInventoryItemSlot : MonoBehaviour, IDependencyInjectable
{
    [SerializeField] private TextMeshProUGUI itemNameLabel;
    [SerializeField] private Image itemImage;
    [SerializeField] private Button itemButton;
    public ItemInteract itemToSpawn { get; private set; }

    bool isActive = false;
    public bool IsActive
    {
        get => isActive;
        set
        {
            if (isActive == value) return;
            isActive = value;

            if (isActive)
                ActivateItem();
            else
                DeactivateItem();
        }
    }
    PlayerInventorySystem playerInventorySystem;
    PlayerStateController controller;

    private void Start()
    {
        InjectDependencies(DependencyContainer.Instance);
    }
    public void InjectDependencies(DependencyContainer provider)
    {
        playerInventorySystem = provider.PlayerContainer.PlayerInventorySystem;
        controller = provider.PlayerContainer.PlayerStateController;
    }
    public void Set(ItemInteract item)
    {
        itemImage.sprite = item.ItemData.itemIcon;
        itemNameLabel.text = item.ItemData.itemName;
        itemToSpawn = item;
    }
    void ActivateItem()
    {
        GameObject item = itemToSpawn.objectPrefab;

        if (item != null)
        {
            item.SetActive(true);
            item.transform.position = playerInventorySystem.SpawnPosition.position;
            item.transform.rotation = playerInventorySystem.SpawnPosition.rotation;
            item.transform.SetParent(playerInventorySystem.SpawnPosition);

            playerInventorySystem.ItemInUse = itemToSpawn;

            controller.ChangeState(controller.ObjectInHandState);
        }
    }
    void DeactivateItem()
    {
        itemToSpawn.objectPrefab.gameObject.SetActive(false);

        controller.ChangeState(controller.NormalState);
    }
    public void SpawnItem()
    {
        if (itemToSpawn.objectPrefab.gameObject.activeInHierarchy == false )
        {
            itemToSpawn.objectPrefab.gameObject.SetActive(true);
            itemToSpawn.objectPrefab.transform.position = playerInventorySystem.SpawnPosition.position;
            itemToSpawn.objectPrefab.transform.SetParent(playerInventorySystem.SpawnPosition);
            playerInventorySystem.ItemInUse = itemToSpawn;
        }
        else if (itemToSpawn.objectPrefab.gameObject.activeInHierarchy == true && playerInventorySystem.ItemInUse == itemToSpawn)
        {
            itemToSpawn.objectPrefab.gameObject.SetActive(false);
            playerInventorySystem.ItemInUse = null;
        }
    }
}