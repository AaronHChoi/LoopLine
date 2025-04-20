using System.Collections.Generic;
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

    private void Update()
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