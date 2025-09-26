using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class BlackRoomManager : MonoBehaviour
{
    [SerializeField] private List<Transform> RandomSpawnPoints = new List<Transform>();

    private void Start()
    {
        FindAllBlackRoomComponentInScene();
    }

    private void FindAllBlackRoomComponentInScene()
    {
        RandomSpawnPoints.Clear();

        BlackRoomComponent[] foundObjects = FindObjectsByType<BlackRoomComponent>(FindObjectsSortMode.None);

        foreach (BlackRoomComponent obj in foundObjects)
        {
            obj.transform.position = AssignRandomSpawner().position;
        }

    }

    private Transform AssignRandomSpawner()
    {
        if (RandomSpawnPoints.Count == 0) return null;

        int randomIndex = Random.Range(0, RandomSpawnPoints.Count);
        Transform chosenSpawnPoint = RandomSpawnPoints[randomIndex];

        RandomSpawnPoints.RemoveAt(randomIndex); 

        return chosenSpawnPoint;
    }
}
