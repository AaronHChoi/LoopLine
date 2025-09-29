using DependencyInjection;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BlackRoomManager : MonoBehaviour, IBlackRoomManager
{
    [SerializeField] private List<Transform> RandomSpawnPoints = new List<Transform>();
    [SerializeField] private List<Transform> RandomDoorSpawnPoints = new List<Transform>();
    [SerializeField] public List<BlackRoomComponent> blackRoomComponents { get; set; } = new List<BlackRoomComponent>();
    [SerializeField] public List<BlackRoomComponentSETTING> blackRoomComponentsSETTING = new List<BlackRoomComponentSETTING> ();
    [SerializeField] private GameObject BKDoor;
    public int ActiveBlackRoomComponent { get; set; } = 0;

    IPlayerController playerController;
    private void Awake()
    {   
        playerController = InterfaceDependencyInjector.Instance.Resolve<IPlayerController>();
        InstantiateBlackRoomComponents();
    }
    private void Start()
    {
        FindAllBlackRoomComponentInScene();
        AssignRandomDoorSpawnPoint();
    }
    private void Update()
    {
        GetClosestBKRC();
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

    private void GetClosestBKRC()
    {
        if (blackRoomComponents.Count == 0 || playerController == null) return;

        Vector3 playerPos = playerController.GetGameObject().transform.position;

        float minDistance = Mathf.Infinity;
        float maxDistance = 0f;

        foreach (BlackRoomComponent bkc in blackRoomComponents)
        {
            if (bkc == null) continue;
            float distance = Vector3.Distance(playerPos, bkc.transform.position);
            if (distance < minDistance) minDistance = distance;
            if (distance > maxDistance) maxDistance = distance;
        }

        if (Mathf.Approximately(maxDistance, minDistance)) maxDistance = minDistance + 1f;

        foreach (BlackRoomComponent bkc in blackRoomComponents)
        {
            if (bkc == null) continue;

            AudioSource audioSource = bkc.GetComponent<AudioSource>();
            if (audioSource == null) continue;

            float distance = Vector3.Distance(playerPos, bkc.transform.position);
            
            float t = Mathf.InverseLerp(maxDistance, minDistance, distance);
           
            audioSource.volume = Mathf.Lerp(0f, 1f, t);   
            audioSource.pitch = Mathf.Lerp(0.8f, 1.2f, t);
        }
    }

    private void AssignRandomDoorSpawnPoint()
    {
        if (RandomDoorSpawnPoints.Count != 0)
        {
            int randomIndex = Random.Range(0, RandomDoorSpawnPoints.Count);
            BKDoor.transform.position = RandomDoorSpawnPoints[randomIndex].position;
            BKDoor.transform.rotation = RandomDoorSpawnPoints[randomIndex].rotation;
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
