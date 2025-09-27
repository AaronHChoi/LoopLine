using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "BlackRoomComponentSETTING", menuName = "Scriptable Objects/BlackRoomComponentSETTING")]
public class BlackRoomComponentSETTING : ScriptableObject
{
    public GameObject objectToSpawn;
    public Vector3 SpawnPosition;  

    public bool CanSpawn = false;

    public void SetSpawnPosition(GameObject objectToSpawn, Vector3 SpawnPosition)
    {
        objectToSpawn.transform.position = SpawnPosition;
    }

    public void ResetAllBKValues()
    {
        CanSpawn = false;
    }
}
