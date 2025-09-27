using DependencyInjection;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BlackRoomManager : MonoBehaviour, IBlackRoomManager
{
    [SerializeField] private List<Transform> RandomSpawnPoints = new List<Transform>();
    [SerializeField] public List<BlackRoomComponent> blackRoomComponents { get; set; } = new List<BlackRoomComponent>();
    [SerializeField] public List<BlackRoomComponentSETTING> blackRoomComponentsSETTING { get; set; } = new List<BlackRoomComponentSETTING> ();
    [SerializeField] private GameObject BKDoor;
    public int ActiveBlackRoomComponent { get; set; } = 0;

    private void Awake()
    {    
        InstantiateBlackRoomComponents();
    }
    private void Start()
    {
        FindAllBlackRoomComponentInScene();
    }

    private void FindAllBlackRoomComponentInScene()
    {

        BlackRoomComponent[] foundObjects = FindObjectsByType<BlackRoomComponent>(FindObjectsSortMode.None);

        foreach (BlackRoomComponent obj in foundObjects)
        {
            obj.transform.position = AssignRandomSpawner().position;
            blackRoomComponents.Add(obj);
        }

    }

    private void InstantiateBlackRoomComponents()
    {
        for (int i = 0; i < blackRoomComponentsSETTING.Count; i++)
        {
            Quaternion spawnRotation = Quaternion.identity;
            if(blackRoomComponentsSETTING[i].CanSpawn)
                BlackRoomComponent.Instantiate(blackRoomComponentsSETTING[i].objectToSpawn, blackRoomComponentsSETTING[i].SpawnPosition, spawnRotation);

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

    public void SetBKDoorGameObject(bool setBkDoor)
    {
        BKDoor.SetActive(setBkDoor);
    }
}

public interface IBlackRoomManager
{
    List<BlackRoomComponent> blackRoomComponents { get; set;}
    void SetBKDoorGameObject(bool setBkDoor);
    int ActiveBlackRoomComponent { get; set; }
}
