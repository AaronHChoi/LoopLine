using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour, IObserver, IDependencyInjectable
{
    [System.Serializable]
    public class ParallaxLayer
    {
        public Transform layerTransform;
        public float parallaxEffect;
        public string type;
    }
    Subject eventManager;
    public List<ParallaxLayer> layers;
    public float parallaxSpeed = 1f; 
    public float teleportZ = -110f;
    public float offsetZ = 235f;

    float speedMultiplier = 1f;
    [SerializeField] private bool isStopped = false;
    [SerializeField] private bool isStopping = false;
    [SerializeField] private bool isResuming = false;
    [SerializeField] private float stopDuration = 5f;
    [SerializeField] private float resumeDuration = 5f;
    [SerializeField] private float stopTimer = 0f;
    [SerializeField] private float resumeTimer = 0f;
    [SerializeField] private int layersTypeCount;

    float lerpA = 1f;
    float lerpB = 0f;
    private void Awake()
    {
        InjectDependencies(DependencyContainer.Instance);
    }
    public void InjectDependencies(DependencyContainer provider)
    {
        eventManager = provider.EventManager;
    }
    public void OnNotify(Events _event, string _id = null)
    {
        if (_event == Events.StopTrain)
            StopParallaxGradually(stopDuration);
        if (_event == Events.ResumeTrain)
            ResumeParallax(resumeDuration);
    }
    private void OnEnable()
    {
        eventManager.AddObserver(this);
    }
    private void OnDisable()
    {
        eventManager.RemoveObserver(this);
    }
    private void Update()
    {
        if (isStopping)
        {
            stopTimer += Time.deltaTime;
            UpdateSpeedMultiplier(lerpA, lerpB, stopTimer, stopDuration);
            if (stopTimer >= stopDuration)
            {
                speedMultiplier = 0f;
                isStopping = true;
            }
        }
        if (isResuming)
        {
            resumeTimer += Time.deltaTime;
            UpdateSpeedMultiplier(lerpB, lerpA, resumeTimer, resumeDuration);
            if(resumeTimer >= resumeDuration)
            {
                speedMultiplier = 1f;
                isResuming = false;
                isStopped = false;
                isStopping = false;
            }
        }
        if (!isStopped)
        {
            float frameDist = parallaxSpeed * Time.deltaTime * speedMultiplier;

            for (int i = 0; i < layers.Count; i++)
            {
                var layer = layers[i];
                if (layer.layerTransform == null) continue;

                float dist = frameDist * layer.parallaxEffect;
                layer.layerTransform.position -= new Vector3(0, 0, dist);

                if (layer.layerTransform.position.z <= teleportZ)
                {
                    TeleportLayer(layer, i);
                    i--;
                }
            }
        }
    }
    public void StopParallaxGradually(float duration)
    {
        if (isStopping || isStopped) return;
        stopDuration = duration;
        stopTimer = 0f;
        isStopping = true;
    }
    public void ResumeParallax(float duration)
    {
        resumeDuration = duration;
        resumeTimer = 0f;
        isResuming = true;
        isStopping = false;
    }
    private void TeleportLayer(ParallaxLayer layer, int index)
    {
        ParallaxLayer lastLayer = null;
        for (int i = layers.Count -1; i >= 0; i--)
        {
            if (layers[i].type == layer.type)
            {
                lastLayer = layers[i];
                break;
            }
        }
        //ParallaxLayer lastLayer = layers[layers.Count - 1];
        
        layer.layerTransform.position = new Vector3(
            lastLayer.layerTransform.position.x,
            lastLayer.layerTransform.position.y,
            lastLayer.layerTransform.position.z + offsetZ
        );

        layers.RemoveAt(index);
        layers.Add(layer);
    }
    
    private float UpdateSpeedMultiplier(float _a, float _b, float _timer, float _duration)
    {
        speedMultiplier = Mathf.Lerp(_a, _b, _timer / _duration);
        return speedMultiplier;
    }
    public void SetSpeedMultiplier(float multipler)
    {
        speedMultiplier = multipler;
    }
}