using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Transition
{
    public class InsideMindPlace : MonoBehaviour
    {
        [Header("Transition Settings")]
        [SerializeField] private float transitionSpeed = 2f;

        [Header("Scene References")]
        [SerializeField] private Volume transitionVolume;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private CinemachineImpulseSource impulseSource;
        [SerializeField] private RawImage transitionImage;

        [Header("Camera Effect Settings")]
        [SerializeField] private float startFOV = 130f;
        [SerializeField] private float endFOV = 60f;

        private bool isActive = false;
        private float dissolveAmount = 1f;
        private float fov;
        private float volumeWeight = 1f;

        private void Start()
        {
            if (transitionVolume)
                transitionVolume.weight = 1f;

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
            RunReverseTransition();
        }
        
        private void CheckDebugKeys()
        {
            // Press M to trigger reverse transition
            if (Input.GetKeyDown(KeyCode.M))
                Activate();

            // Optional: reset dissolve for testing
            if (Input.GetKeyDown(KeyCode.R))
            {
                dissolveAmount = 1f;
                volumeWeight = 1f;

                if (transitionVolume)
                    transitionVolume.weight = 1f;

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

        private void RunReverseTransition()
        {
            // Fade out post-process volume
            if (transitionVolume)
            {
                volumeWeight = Mathf.MoveTowards(transitionVolume.weight, 0f, Time.deltaTime * transitionSpeed);
                transitionVolume.weight = volumeWeight;
            }

            // Animate dissolve on the RawImage (bottom-to-top)
            if (transitionImage && transitionImage.material != null)
            {
                dissolveAmount = Mathf.MoveTowards(dissolveAmount, 0f, Time.deltaTime * transitionSpeed * 0.5f);
                transitionImage.material.SetFloat("_DissolveAmount", dissolveAmount);
            }

            // Collapse FOV
            if (mainCamera != null)
            {
                fov = Mathf.MoveTowards(fov, endFOV, Time.deltaTime * transitionSpeed * 20f);
                mainCamera.fieldOfView = fov;
            }

            // Stop when done
            if (Mathf.Approximately(dissolveAmount, 0f) && Mathf.Approximately(volumeWeight, 0f))
                isActive = false;
        }
    }
}
