using System.Collections.Generic;
using UnityEngine;

public class PhotoDetectionZone : MonoBehaviour, ITogglePhotoDetection
{
    List<PhotoClue> cluesInZone = new List<PhotoClue>();
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
        if (clue != null && !cluesInZone.Contains(clue))
        {
            cluesInZone.Add(clue);
        }
    }
    void OnTriggerExit(Collider other)
    {
        PhotoClue clue = other.GetComponent<PhotoClue>();
        if (clue != null && cluesInZone.Contains(clue))
        {
            cluesInZone.Remove(clue);
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
    public PhotoClue GetClue()
    {
        return cluesInZone.Count > 0 ? cluesInZone[0] : null;
    }
}

public interface ITogglePhotoDetection
{
    void ToggleCollider(bool enabled);
}