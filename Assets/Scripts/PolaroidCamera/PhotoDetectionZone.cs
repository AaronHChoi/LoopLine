using System.Collections.Generic;
using UnityEngine;

public class PhotoDetectionZone : MonoBehaviour, ITogglePhotoDetection
{
    List<PhotoClue> cluesInZone = new List<PhotoClue>();
    List<BlackRoomComponent> blackRMComponentsInZone = new List<BlackRoomComponent>();
    BoxCollider boxCollider;

    public bool HasClues => cluesInZone.Count > 0;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.enabled = false;
    }
    void OnTriggerEnter(Collider other)
    {
        PhotoClue clue = other.GetComponent<PhotoClue>();
        BlackRoomComponent blackRMComponent = GetComponent<BlackRoomComponent>();
        if (clue != null && !cluesInZone.Contains(clue))
        {
            cluesInZone.Add(clue);
        }
        if (blackRMComponent != null && !blackRMComponentsInZone.Contains(blackRMComponent))
        {
            blackRMComponentsInZone.Add(blackRMComponent);
        }
    }
    void OnTriggerExit(Collider other)
    {
        PhotoClue clue = other.GetComponent<PhotoClue>();
        BlackRoomComponent blackRMComponent = GetComponent<BlackRoomComponent>();
        if (clue != null && cluesInZone.Contains(clue))
        {
            cluesInZone.Remove(clue);
        }
        if (blackRMComponent != null && !blackRMComponentsInZone.Contains(blackRMComponent))
        {
            blackRMComponentsInZone.Remove(blackRMComponent);
        }
    }
    public void ToggleCollider(bool enabled)
    {
        boxCollider.enabled = enabled;
    }
    public bool CheckIfAnyClue()
    {
        return cluesInZone.Count > 0;
    }
    public bool CheckIfAnyBlackRMComp()
    {
        return blackRMComponentsInZone.Count > 0;
    }
    public BlackRoomComponent GetBlackRMComp()
    {
        return blackRMComponentsInZone.Count > 0 ? blackRMComponentsInZone[0] : null;
    }
    public PhotoClue GetClue()
    {
        return cluesInZone.Count > 0 ? cluesInZone[0] : null;
    }
}

public interface ITogglePhotoDetection
{
    PhotoClue GetClue();
    BlackRoomComponent GetBlackRMComp();
    bool CheckIfAnyClue();
    bool CheckIfAnyBlackRMComp();
    void ToggleCollider(bool enabled);
}