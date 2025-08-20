using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryItemSlot : MonoBehaviour, IDependencyInjectable
{
    [SerializeField] private TextMeshProUGUI itemNameLabel;
    [SerializeField] private Image itemImage;
    [SerializeField] private UnityEngine.UI.Button itemButton;
    public ItemInteract itemToSpawn { get; private set; }
    public bool isActive = false;
    PlayerInventorySystem playerInventorySystem;
    InventoryUI inventoryUI;

    private void Start()
    {
        InjectDependencies(DependencyContainer.Instance);
    }
    public void InjectDependencies(DependencyContainer provider)
    {
        playerInventorySystem = provider.PlayerInventorySystem;
        inventoryUI = provider.InventoryUI;
    }
    public void Set(ItemInteract item)
    {
        itemImage.sprite = item.ItemData.itemIcon;
        itemNameLabel.text = item.ItemData.itemName;
        itemToSpawn = item;
    }
    private void Update()
    {
        if (isActive)
        {
            GameObject item = itemToSpawn.objectPrefab;

            item.SetActive(true);
            item.transform.position = playerInventorySystem.SpawnPosition.position;
            item.transform.rotation = playerInventorySystem.SpawnPosition.rotation;
            item.transform.SetParent(playerInventorySystem.SpawnPosition);

            playerInventorySystem.ItemInUse = itemToSpawn;
        }
        else 
        {
            itemToSpawn.objectPrefab.gameObject.SetActive(false);
        }
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