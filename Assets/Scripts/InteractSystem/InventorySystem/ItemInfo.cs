using UnityEngine;
using UnityEngine.UIElements;
[CreateAssetMenu(fileName = "ItemInfo", menuName = "Scriptable Object/New Item")]
public class ItemInfo : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public GameObject objectPrefab;
    public bool isInHand = false;
}
