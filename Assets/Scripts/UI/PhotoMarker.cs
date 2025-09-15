using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

namespace UI
{
    public class PhotoMarker : MonoBehaviour, IPhotoMarker
    {
        [SerializeField] private Image markerImage;
        [SerializeField] private float minScale = 0.5f;
        [SerializeField] private float maxScale = 2f;
        [SerializeField] private float maxDistance = 20f;

        private Transform _target;

        void Awake()
        {
            if (markerImage == null)
                markerImage = GetComponent<Image>();

            SetAlpha(0f);
        }

        public void SetTarget(Transform t) => _target = t;

        public void Show() => SetAlpha(1f);
        public void Hide() => SetAlpha(0f);

        private void SetAlpha(float a)
        {
            if (markerImage == null) return;
            var c = markerImage.color;
            c.a = a;
            markerImage.color = c;
        }

        void Update()
        {
            if (_target == null || markerImage == null) return;

            Vector3 screenPos = Camera.main.WorldToScreenPoint(_target.position);

            if (screenPos.z <= 0)
            {
                markerImage.enabled = false;
                return;
            }

            markerImage.enabled = true;

            RectTransform canvasRect = markerImage.canvas.transform as RectTransform;
            Vector2 canvasPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect,
                screenPos,
                null, // null for Overlay canvas
                out canvasPos
            );

            markerImage.rectTransform.localPosition = canvasPos;

            float distance = Vector3.Distance(Camera.main.transform.position, _target.position);
            float t = Mathf.Clamp01(distance / maxDistance);
            float scale = Mathf.Lerp(minScale, maxScale, t);
            markerImage.rectTransform.localScale = Vector3.one * scale;
        }

        public void SetGameObjectEnable(bool State)
        {
            enabled = State;
        }
    }
}

public interface IPhotoMarker
{
    void SetGameObjectEnable(bool State);
}