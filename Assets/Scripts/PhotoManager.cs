using System.Collections.Generic;
using UnityEngine;

public class PhotoManager : MonoBehaviour
{
    [SerializeField] List<Transform> photoPositions;

    [SerializeField] private List<Photo> movedPhotos = new List<Photo>();
    private List<Vector3> originalPositions = new List<Vector3>();

    public void PlacePhotos()
    {
        Photo[] allPhotos = Resources.FindObjectsOfTypeAll<Photo>();
        movedPhotos.Clear();
        originalPositions.Clear();

        List<Photo> validPhotos = new List<Photo>();

        foreach (Photo photo in allPhotos)
        {
            if (photo.gameObject.scene.IsValid() && photo.Texture != null)
            {
                validPhotos.Add(photo);
            }
        }

        validPhotos.Sort((a, b) => string.Compare(a.name, b.name));

        for (int i = 0; i < validPhotos.Count && i < photoPositions.Count; i++)
        {
            Photo photo = validPhotos[i];

            originalPositions.Add(photo.transform.position);
            movedPhotos.Add(photo);

            photo.transform.position = photoPositions[i].position;
        }
    }
    public void ResetPhotos()
    {
        for (int i = 0; i < movedPhotos.Count; i++)
        {
            if (movedPhotos[i] != null)
            {
                movedPhotos[i].transform.position = originalPositions[i];
            }
        }

        movedPhotos.Clear();
        originalPositions.Clear();
    }
}