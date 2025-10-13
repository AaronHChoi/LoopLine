using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Transition
{
    public class ToMindPlace : MonoBehaviour
    {
        [Header("Transition Settings")]
        [SerializeField] private float transitionSpeed = 2f;

        [Header("Scene References")]
        [SerializeField] private Volume transitionVolume;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private CinemachineImpulseSource impulseSource;
        [SerializeField] private RawImage transitionImage;

        [Header("Camera Effect Settings")]
        [SerializeField] private float startFOV = 60f;
        [SerializeField] private float endFOV = 130f;

        private bool isActive = false;
        private float dissolveAmount = 0f;
        private float fov;
        private float volumeWeight = 0f;

        private void Start()
        {
            if (transitionVolume)
                transitionVolume.weight = 0f;

            if (mainCamera)
            {
                fov = startFOV;
                mainCamera.fieldOfView = fov;
            }

            if (transitionImage && transitionImage.material != null)
            {
                transitionImage.material.SetFloat("_DissolveAmount", dissolveAmount);
                transitionImage.gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            if (!isActive)
                CheckDebugKeys();
            
            if (!isActive) return;
            RunTransition();
        }
        
        private void CheckDebugKeys()
        {
            // Press T to trigger transition
            if (Input.GetKeyDown(KeyCode.T))
                Activate();

            // Optional: reset dissolve for testing
            if (Input.GetKeyDown(KeyCode.R))
            {
                dissolveAmount = 0f;
                volumeWeight = 0f;

                if (transitionVolume)
                    transitionVolume.weight = 0f;

                if (transitionImage && transitionImage.material != null)
                    transitionImage.material.SetFloat("_DissolveAmount", dissolveAmount);

                if (transitionImage)
                    transitionImage.gameObject.SetActive(false);

                if (mainCamera != null)
                    mainCamera.fieldOfView = startFOV;
            }
        }

        public void Activate()
        {
            if (isActive) return;
            isActive = true;

            if (transitionImage)
                transitionImage.gameObject.SetActive(true);

            // Screen shake
            if (impulseSource)
                impulseSource.GenerateImpulse();
        }

        private void RunTransition()
        {
            // Fade in post-process volume
            if (transitionVolume)
            {
                volumeWeight = Mathf.MoveTowards(transitionVolume.weight, 1f, Time.deltaTime * transitionSpeed);
                transitionVolume.weight = volumeWeight;
            }

            // Animate dissolve on the RawImage
            if (transitionImage && transitionImage.material != null)
            {
                dissolveAmount = Mathf.MoveTowards(dissolveAmount, 1f, Time.deltaTime * transitionSpeed * 0.5f);
                transitionImage.material.SetFloat("_DissolveAmount", dissolveAmount);
            }

            // Expand FOV
            if (mainCamera != null)
            {
                fov = Mathf.MoveTowards(fov, endFOV, Time.deltaTime * transitionSpeed * 20f);
                mainCamera.fieldOfView = fov;
            }

            // Stop when done
            if (Mathf.Approximately(dissolveAmount, 1f) && Mathf.Approximately(volumeWeight, 1f))
                isActive = false;
        }
    }
}
