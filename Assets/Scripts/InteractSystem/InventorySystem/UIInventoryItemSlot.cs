using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIInventoryItemSlot : MonoBehaviour, IDependencyInjectable
{
    [SerializeField] private TextMeshProUGUI itemNameLabel;
    [SerializeField] private Sprite itemImage;
    [SerializeField] private UnityEngine.UI.Button itemButton;
    private ItemInteract itemToSpawn;

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
        itemImage = item.ItemData.itemIcon;
        itemNameLabel.text = item.ItemData.itemName;
        itemToSpawn = item;
    }

    private void Update()
    {
        
    }
    public void SpawnItem()
    {

        if (itemToSpawn.objectPrefab.gameObject.activeInHierarchy == false && playerInventorySystem.ItemInUse == null)
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
