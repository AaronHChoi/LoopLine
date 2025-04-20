using UnityEngine;

public class FPS : MonoBehaviour
{
    [SerializeField] private int _fps = 60;

    void Awake()
    {
        Application.targetFrameRate = _fps;
        QualitySettings.vSyncCount = 0; 
    }

}
