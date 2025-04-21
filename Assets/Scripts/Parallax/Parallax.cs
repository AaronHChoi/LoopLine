using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [System.Serializable]
    public class ParallaxLayer
    {
        public Transform layerTransform;
        public float parallaxEffect;
    }
    public List<ParallaxLayer> layers;
    public float parallaxSpeed = 1f; 
    public float teleportZ = -110f;
    public float offsetZ = 235f;

    float speedMultiplier = 1f;
    [SerializeField] private bool isStopped = false;
    [SerializeField] private bool isStopping = false;
    [SerializeField] private bool isResuming = false;
    [SerializeField] private float stopDuration = 2f;
    [SerializeField] private float resumeDuration = 2f;
    [SerializeField] private float stopTimer = 0f;
    [SerializeField] private float resumeTimer = 0f;

    private void Update()
    {
        if (Input.GetKey(KeyCode.T))
        {
            StopParallaxGradually(5f);
        }
        if (Input.GetKey(KeyCode.Y))
        {
            ResumeParallax(5f);
        }
        if (isStopping)
        {
            stopTimer += Time.deltaTime;
            speedMultiplier = Mathf.Lerp(1f, 0f, stopTimer / stopDuration);
            if (stopTimer >= stopDuration)
            {
                speedMultiplier = 0f;
                isStopping = true;
            }
        }
        if (isResuming)
        {
            resumeTimer += Time.deltaTime;
            speedMultiplier = Mathf.Lerp(0f, 1f, resumeTimer / resumeDuration);
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
        //if (isResuming || !isStopped) return;
        resumeDuration = duration;
        resumeTimer = 0f;
        isResuming = true;
    }
    private void TeleportLayer(ParallaxLayer layer, int index)
    {
        layer.layerTransform.position = new Vector3(
            layer.layerTransform.position.x,
            layer.layerTransform.position.y,
            layer.layerTransform.position.z + offsetZ
        );
        layers.RemoveAt(index);
        layers.Add(layer);
    }
    public void SetSpeedMultiplier(float multipler)
    {
        speedMultiplier = multipler;
    }
}