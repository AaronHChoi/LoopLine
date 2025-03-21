using UnityEngine;

public class FPS : MonoBehaviour
{
    [SerializeField] private int _fps = 60;

    void Start()
    {
        Application.targetFrameRate = _fps;
    }

}
