using Core.Data;
using UnityEngine;

public class ParticleCinematicController : MonoBehaviour
{
    [SerializeField] ParticleSystem particleCinematic;
    [SerializeField] GameCondition condition;
    private const int MAX_PARTICLE_LIMIT = 1000;

    private void Start()
    {
        if (GameManager.Instance.GetCondition(condition))
        {
            ApplyParticleSettings();
        }
    }
    public void ApplyParticleSettings()
    {
        UpdateMaxParticles(MAX_PARTICLE_LIMIT);
    }
    private void UpdateMaxParticles(int maxParticles)
    {
        var mainModule = particleCinematic.main;
        mainModule.maxParticles = maxParticles;
    }
}