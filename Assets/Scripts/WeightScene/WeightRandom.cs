using System.Collections.Generic;
using UnityEngine;

public class WeightRandom : MonoBehaviour
{  
    public static T GetRandomWeighted<T>(List<WeightedItem<T>> items)
    {
        float totalWeight = 0f;
        foreach (var i in items)
            totalWeight += i.weight;

        float randomValue = Random.value * totalWeight;
        float currentWeight = 0f;

        foreach (var i in items)
        {
            currentWeight += i.weight;
            if (randomValue <= currentWeight)
                return i.item;
        }

        return default;
    }
}

[System.Serializable]
public class WeightedItem<T>
{
    public T item;
    public float weight;
}
