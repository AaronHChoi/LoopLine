using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class PhotoMarkerManager : MonoBehaviour
    {
        [SerializeField] private PhotoMarker marker;
        [Tooltip("Manually assign up to 5 trackable objects here.")]
        public List<GameObject> photoTargets = new List<GameObject>();

        private Camera cachedMainCamera;

        void Start()
        {
            cachedMainCamera = Camera.main;
            if (cachedMainCamera == null)
            {
                Debug.LogWarning("PhotoMarkerManager: No camera tagged as MainCamera found in the scene.");
            }
        }

        void Update()
        {
            if (photoTargets.Count == 0 || marker == null || cachedMainCamera == null) return;

            Transform closest = null;
            float closestDist = Mathf.Infinity;
            Vector3 camPos = cachedMainCamera.transform.position;

            foreach (var obj in photoTargets)
            {
                if (obj == null) continue;
                float dist = Vector3.Distance(camPos, obj.transform.position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closest = obj.transform;
                }
            }

            marker.SetTarget(closest);
        }
    }

    public interface IPhotoMarkerManager
    {
        void ShowMarker();
        void HideMarker();
    }
}