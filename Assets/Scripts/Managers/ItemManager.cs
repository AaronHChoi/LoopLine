using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemManager : MonoBehaviour
{
    [SerializeField] public List<ItemInteract> items = new List<ItemInteract>();

    private void Start()
    {
        FindAllItemsInScene();
    }

    private void FindAllItemsInScene()
    {
        items.Clear();

        ItemInteract[] foundObjects = FindObjectsByType<ItemInteract>(FindObjectsSortMode.None);

        foreach (ItemInteract obj in foundObjects)
        {
            items.Add(obj);
        }

    }
}
