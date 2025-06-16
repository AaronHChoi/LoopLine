using UnityEngine;

public class DisableGlassParticles : MonoBehaviour
{
    private ParticleSystem glassParticles;
    private bool timerStarted = false;
    private float timer = 0f;

    void Start()
    {
        glassParticles = GetComponent<ParticleSystem>();
    }

    void OnParticleCollision(GameObject other)
    {
        if (!timerStarted)
        {
            timerStarted = true;
            timer = 5f;
        }
    }

    void Update()
    {
        if (timerStarted)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                glassParticles.Pause();
                enabled = false;
            }
        }
    }
}
