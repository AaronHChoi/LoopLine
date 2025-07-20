using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIInventoryItemSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameLabel;
    [SerializeField] private Sprite itemImage;
    public void Set(ItemInteract item)
    {
        itemImage = item.ItemData.itemIcon;
        itemNameLabel.text = item.ItemData.itemName;
    }
}
