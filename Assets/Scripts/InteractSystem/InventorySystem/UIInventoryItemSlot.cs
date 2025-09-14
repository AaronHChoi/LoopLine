using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DependencyInjection;
public class UIInventoryItemSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameLabel;
    [SerializeField] private Image itemImage;
    [SerializeField] private Button itemButton;
    [SerializeField] public string itemId;
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
    IInventoryUI inventorySystem;
    IPlayerStateController controller;

    private void Start()
    {
        inventorySystem = InterfaceDependencyInjector.Instance.Resolve<IInventoryUI>();
        controller = InterfaceDependencyInjector.Instance.Resolve<IPlayerStateController>();
    }
  
    public void Set(ItemInteract item)
    {
        itemImage.sprite = item.ItemData.itemIcon;
        itemNameLabel.text = item.ItemData.itemName;
        itemToSpawn = item;
        itemId = item.id;
    }
    void ActivateItem()
    {
        GameObject item = itemToSpawn.objectPrefab;

        if (item != null)
        {
            item.SetActive(true);
            item.transform.position = inventorySystem.GetSpawnPosition().position;
            item.transform.rotation = inventorySystem.GetSpawnPosition().rotation;
            item.transform.SetParent(inventorySystem.GetSpawnPosition());

            inventorySystem.ItemInUse = itemToSpawn;

            controller.ChangeState(controller.ObjectInHandState);
        }
    }
    void DeactivateItem()
    {
        itemToSpawn.objectPrefab.gameObject.SetActive(false);

        controller.ChangeState(controller.NormalState);
    }
   
}