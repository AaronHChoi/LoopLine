using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemInventoryManager", menuName = "Scriptable Objects/ItemInventoryManager")]
public class ItemInventoryManager : ScriptableObject
{
    public List<ItemInfo> items = new List<ItemInfo>();
    public bool isFirstTime = true;

}
