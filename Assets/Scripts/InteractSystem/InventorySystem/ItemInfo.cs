using UnityEngine;
using UnityEngine.UIElements;
[CreateAssetMenu(fileName = "ItemInfo", menuName = "Scriptable Object/New Item")]
public class ItemInfo : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public GameObject UIPrefab;
    public bool isInHand = false;
    public GameObject itemPrefab;
}
